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
    // 時が止まっているときはダメージをあげる
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
        // 敵が剣に攻撃されたらダメージを処理する
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
        // 敵が魔法に攻撃されたらダメージを処理する
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
    // 攻撃受けたとき赤くなる
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
    // 会心の攻撃の時黄色く光る
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

    
}
