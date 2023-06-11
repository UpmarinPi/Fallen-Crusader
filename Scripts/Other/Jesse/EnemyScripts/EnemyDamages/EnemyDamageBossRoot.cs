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
        // 時が止まっているときはダメージをあげる
        if (MagicController.freezeFlag)
        {
            buff = 1.5f;
        }
        else
        {
            buff = 1f;
        }
        // increase damage if weakness
        // 弱点だったらダメージをあげる 
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
        // 敵が剣に攻撃されたらダメージを処理する
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
        // 敵が魔法に攻撃されたらダメージを処理する
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
    // 攻撃受けたとき赤くなる
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
    // 会心の攻撃の時黄色く光る
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
    // 剣の攻撃の無効か解除
    void InvokeSword()
    {
        swordFlag = false;
    }

    // remove magic attack invincibility
    // 魔法の攻撃の無効か解除
    void InvokeMagic()
    {
        magicFlag = false;
    }

    // spawn damage effect
    // ダメージエフェクトを召喚
    void DamageEffect(Vector3 pos)
    {
        GameObject effect = Instantiate(hitEffect) as GameObject;
        effect.transform.position = pos;
    }

    // function to set if object is weakness
    // オブジェクトが弱点か設定する関数
    public bool IsWeakness
    {
        set
        {
            weaknessFlag = value;
        }
    }
}
