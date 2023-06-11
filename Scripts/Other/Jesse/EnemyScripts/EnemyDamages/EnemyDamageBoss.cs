using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageBoss: MonoBehaviour
{
    [SerializeField] GameObject hitEffect;

    SpriteRenderer spriteRenderer, spriteRenderer2;
    int damage;
    int scaleX = 0;
    float buff = 1f;
    float stun;
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
        spriteRenderer = transform.gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer2 = transform.parent.GetComponent<SpriteRenderer>();
        bossController = transform.parent.GetComponent<BossController>();
    }

    // when time is stopped increase damage
    // �����~�܂��Ă���Ƃ��̓_���[�W��������
    private void FixedUpdate()
    {
        if (MagicController.freezeFlag)
        {
            buff = 1.5f;
        }
        else
        {
            buff = 1f;
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // if enemy is hit by sword process damage
        // �G�����ɍU�����ꂽ��_���[�W����������
        if((sword = collision.gameObject.GetComponent<IDamageSword>()) != null)
        {            
            if(swordFlag)
			{
                return;
			}
            int mpSteal = 0;
            bool critFlag = false;

            sword.GetDamage(out damage, out mpSteal, out stun, out knockback, out scaleX, out critFlag);

            DamageEffect(transform.position);
            magicController.SetManaSteal(mpSteal);
            bossController.DamageHP(Mathf.CeilToInt(damage * buff));
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
        if((magic = collision.gameObject.GetComponent<IDamageMagic>()) != null)
        {
			if(magicFlag)
			{
                return;
			}

            magic.GetDamage(out damage, out stun, out knockback, out scaleX);
            DamageEffect(transform.position);
            bossController.DamageHP(Mathf.CeilToInt(damage * buff));
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
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        spriteRenderer2.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        spriteRenderer2.color = Color.white;
        yield return new WaitForSeconds(0.05f);
    }
    // critical hit flash yellow
    // ��S�̍U���̎����F������
    IEnumerator CritFlash()
    {
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = Color.yellow;
        spriteRenderer2.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        spriteRenderer2.color = Color.white;
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

    
}
