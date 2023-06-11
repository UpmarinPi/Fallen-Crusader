using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageBossRoot: MonoBehaviour
{
    [SerializeField] GameObject hitEffect;

    SpriteRenderer spriteRenderer;
    int damage;
    int scaleX = 0;
    float buff = 1f;
    float debuff = 1f;
    float stun;
    bool weaknessFlag = false;
    bool swordFlag = false;
    bool magicFlag = false;
    Vector2 knockback;

    GameObject Player;
    Coroutine coroutine;
    MagicController magicController;
    BossController bossController;
    PlayerFinder playerFinder;

    IDamageSword sword;
    IDamageMagic magic;


    void Start()
    {
        playerFinder = new PlayerFinder();

        Player = playerFinder.GetPlayer();
        magicController = Player.GetComponent<MagicController>();
        spriteRenderer = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        bossController = transform.parent.parent.parent.GetComponent<BossController>();
    }

    private void FixedUpdate()
    {
        // when time is stopped increase damage
        // �����~�܂��Ă���Ƃ��̓_���[�W��������
        if (MagicController.freezeFlag)
        {
            buff = 1.5f;
        }
        else
        {
            buff = 1f;
        }
        // increase damage if weakness
        // ��_��������_���[�W�������� 
        if (weaknessFlag)
        {
            debuff = 2f;
        }
        else
        {
            debuff = 1f;
        }
    }
    


    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if enemy is hit by sword process damage
        // �G�����ɍU�����ꂽ��_���[�W����������
        if ((sword = collision.gameObject.GetComponent<IDamageSword>()) != null)
        {            
            if (swordFlag)
			{
                return;
			}
            int mpSteal = 0;
            bool critFlag = false;

            sword.GetDamage(out damage, out mpSteal, out stun, out knockback, out scaleX, out critFlag);

            DamageEffect(transform.position);
            magicController.SetManaSteal(mpSteal);
            bossController.DamageShield(Mathf.CeilToInt(damage * buff * debuff));
            coroutine = null;

            if(critFlag)
                coroutine = StartCoroutine(CritFlash());       
            else
                coroutine = StartCoroutine(DamageFlash());
            swordFlag = true;
            Invoke("InvokeSword", 0.2f);

        }

        // if enemy is hit by magic process damage
        // �G�����@�ɍU�����ꂽ��_���[�W����������
        if ((magic = collision.gameObject.GetComponent<IDamageMagic>()) != null)
        {
            if (magicFlag)
			{
                return;
			}

            magic.GetDamage(out damage, out stun, out knockback, out scaleX);
            DamageEffect(transform.position);
            bossController.DamageShield(Mathf.CeilToInt(damage * buff * debuff));
            coroutine = null;
            coroutine = StartCoroutine(DamageFlash());
            magicFlag = true;
            Invoke("InvokeMagic", 0.4f);

        }
        
    }

    // flash red when taking damage
    // �U���󂯂��Ƃ��Ԃ��Ȃ�
    IEnumerator DamageFlash()
    {
        Color rootColor = spriteRenderer.color;
        yield return new WaitForSeconds(0.1f);
        bossController.DamageSE();
        spriteRenderer.color = Color.blue;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = rootColor;
        yield return new WaitForSeconds(0.05f);
    }
    // critical hit flash yellow
    // ��S�̍U���̎����F������
    IEnumerator CritFlash()
    {
        Color rootColor = spriteRenderer.color;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = rootColor;
        yield return new WaitForSeconds(0.05f);
    }

    // remove sword attack invincibility
    // ���̍U���̖���������
    void InvokeSword()
    {
        swordFlag = false;
    }

    // remove magic attack invincibility
    // ���@�̍U���̖���������
    void InvokeMagic()
    {
        magicFlag = false;
    }

    // spawn damage effect
    // �_���[�W�G�t�F�N�g������
    void DamageEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(hitEffect) as GameObject;
        effect.transform.position = pos;
    }

    // function to set if object is weakness
    // �I�u�W�F�N�g����_���ݒ肷��֐�
    public bool IsWeakness
    {
        set
        {
            weaknessFlag = value;
        }
    }
}
