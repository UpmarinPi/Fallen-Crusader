using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JungleFruit : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;

    SpriteRenderer spriteRenderer;
    int damage = 0;
    int scaleX = 0;
    [SerializeField] float fruitHP = 200;
    [SerializeField] bool spawnFlag = false;
    [SerializeField] string idleAnim;
    float buff = 1f;
    float stun = 0;
    bool deadFlag = false;
    bool startFlag = false;
    bool wasfrozen = false;
    bool swordFlag = false;
    bool magicFlag = false;
    Vector2 knockback;

    GameObject Player;
    Coroutine coroutine;
    MagicController magicController;
    BattleZone battleZone;
    PlayerFinder playerFinder;

    MahoSE SE;

    IDamageSword sword;
    IDamageMagic magic;

    void Start()
    {
        playerFinder = new PlayerFinder();

        Player = playerFinder.GetPlayer();
        SE = GetComponent<MahoSE>();
        magicController = Player.GetComponent<MagicController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spawnFlag)
        {
            battleZone = transform.Find("EnemySpawner").gameObject.GetComponent<BattleZone>();
        }
        else
        {
            Destroy(transform.GetChild(0).gameObject);
        }
    }


    void FixedUpdate()
    {
        if (fruitHP <= 0)
        {
            // if fruit spawns enemies wait until enemies are dead to disappear
            // 木の実が敵を出現するものだったら、敵が消えた後消す
            if (spawnFlag)
            {
                deadFlag = true;
                if (battleZone.CanDestroy() && !startFlag)
                {
                    startFlag = true;
                    StartCoroutine(Death());
                }
            }
            else
            {
                if (!startFlag)
                {
                    deadFlag = true;
                    startFlag = true;
                    StartCoroutine(Death());
                }
            }
        }

        // if time is stopped increase damage taken
        // 時が止まってたら攻撃力上げる
        if (MagicController.freezeFlag)
        {
            buff = 1.5f;

            if (!wasfrozen)
			{
                wasfrozen = true;
                GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 1f);
                GetComponent<Animator>().Play(null);
			}
        }
        else if (wasfrozen && !MagicController.freezeFlag)
        {
            if (wasfrozen)
            {
                wasfrozen = false;
                GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
                GetComponent<Animator>().Play(idleAnim);
            }
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
        if ((sword = collision.gameObject.GetComponent<IDamageSword>()) != null)
        {

            if(swordFlag)
                return;

            int mpSteal = 0;
            bool critFlag = false;
            
            sword.GetDamage(out damage, out mpSteal, out stun, out knockback, out scaleX, out critFlag);
            DamageEffect(transform.position);
            magicController.SetManaSteal(mpSteal); 
            fruitHP -= Mathf.CeilToInt(damage * buff);
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

            if(magicFlag)
                return;
			magic.GetDamage(out damage, out stun, out knockback, out scaleX);

            DamageEffect(transform.position);
            fruitHP -= Mathf.CeilToInt(damage * buff);
            coroutine = null;
            coroutine = StartCoroutine(DamageFlash());
            magicFlag = true;
            Invoke("InvokeMagic", 0.4f);

		}

    }

    // function to check if dead
    // 死んでるか確認する関数
    public bool getDead()
    {
        return deadFlag;
    }

    // get the hp of the fruit
    // 木の実のHPを取得する
    public float fruitGetHP()
	{
        return fruitHP;
	}

    // enemy death function
    // 敵死亡関数
    IEnumerator Death()
    {
        Destroy(GetComponent<BoxCollider2D>());
        float decrement = 0.05f;
        while (spriteRenderer.material.GetFloat("_Fade") > 0)
        {
            spriteRenderer.material.SetFloat("_Fade", spriteRenderer.material.GetFloat("_Fade") - decrement);
            decrement = 0.005f + (decrement * 0.8f);
            yield return new WaitForSeconds(decrement);
        }
    }

    // flash red when taking damage
    // 攻撃受けたとき赤くなる
    IEnumerator DamageFlash()
    {
        SE.DamageSE();
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.05f);
    }

    // critical hit flash yellow
    // 会心の攻撃の時黄色く光る
    IEnumerator CritFlash()
    {
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.05f);
    }

    // remove sword immunity 
    // 剣攻撃の無力化をとる
    void InvokeSword()
    {
        swordFlag = false;
    }

    // remove magic immunity 
    // 魔法攻撃の無力化をとる
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
