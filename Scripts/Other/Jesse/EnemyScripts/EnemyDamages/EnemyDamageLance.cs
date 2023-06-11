using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageLance : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rbody2d;
    bool hurtFlag = false;
    bool swordFlag = false;
    bool magicFlag = false;
    int damage = 0;
    int scaleX = 0;
    int direction = 0;
    float buff = 1f;
    float stun = 0;
    Vector2 knockback = new Vector2(0, 0);

    Collider2D hitbox;
    GameObject Player;
    Coroutine coroutine, blockCoroutine;
    MagicController magicController;
    ControllerLance enemyController;
    PlayerFinder playerFinder;

    IDamageSword sword;
    IDamageMagic magic;


    void Start()
    {
        playerFinder = new PlayerFinder();
        Player = playerFinder.GetPlayer();
        magicController = Player.GetComponent<MagicController>();
        spriteRenderer = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        rbody2d = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        enemyController = transform.parent.gameObject.GetComponent<ControllerLance>();
        GetComponent<BoxCollider2D>().enabled = true;
        hitbox = GetComponent<Collider2D>();

    }

    // when time is stopped increase damage
    // 時が止まっているときはダメージをあげる
    private void FixedUpdate()
    {
        if(MagicController.freezeFlag)
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
        //Debug.Log(hitbox.enabled);
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
            magicController.SetManaSteal(mpSteal + 1); 
            // check if stunned or blocking
            // スタン状態か、ブロック状態確認
            if (!enemyController.GetStunned())
			{
                direction = (int)transform.parent.localScale.x;
			}
            else if(enemyController.GetStunned() && !enemyController.GetGuardStun())
			{
                direction = 0;
			}

            // determine type of knockback
            // ノックバックの種類を決める（スタンかブロック）
            if (direction == -scaleX && !MagicController.freezeFlag)
			{             
                BlockCount--;
                knockback = new Vector2(knockback.x / 3, knockback.y / 3);
                enemyController.EnemyHP -= Mathf.CeilToInt((damage/2) * buff);
                if (blockCoroutine != null)
				{
                    StopCoroutine(blockCoroutine);
                }
                blockCoroutine = StartCoroutine(ResetBlock());
                StunTime = stun;
			}
            else
			{
                BlockCount = 7;
                enemyController.EnemyHP -= Mathf.CeilToInt(damage * buff);
                StunTime = stun + 0.15f;
			}


            hurtFlag = true;
            coroutine = null;

            if(critFlag)
                coroutine = StartCoroutine(CritFlash()); // 赤く光る            
            else
                coroutine = StartCoroutine(DamageFlash());

            rbody2d.velocity = new Vector2(0, 0);
            KnockbackBig(knockback.x, knockback.y, scaleX);
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

            BlockCount = 7;

            enemyController.EnemyHP -= Mathf.CeilToInt(damage * buff);
            StunTime = stun + 0.3f;
            hurtFlag = true;
            coroutine = null;
            coroutine = StartCoroutine(DamageFlash());
            rbody2d.velocity = new Vector2(0, 0);
            // set knockback direction for explosion
            // 爆発のノックバック方向
            if (collision.gameObject.CompareTag("MagicExplosion"))
            {
                if(collision.gameObject.transform.position.x < transform.position.x)
                {
                    scaleX = 1;
                }
                else
                {
                    scaleX = -1;
                }
            }
            KnockbackBig(knockback.x, knockback.y, scaleX);
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

    // reset block count
    // ブロック数をリセット
    IEnumerator ResetBlock()
	{
        yield return new WaitForSeconds(1f);
        BlockCount = 7;
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

    // function to check and set stun time
    // スタン時間の取得と設定をする関数
    public float StunTime
    {
        get; set;
    }

    // function to retrieve if hit
    // 攻撃受けたかを取得する関数
    public bool GetEnemyHit()
    {
        return hurtFlag;
    }
    // function to reset hit bool
    // 攻撃されたフラグを false にする関数
    public void ResetEnemyHit()
    {
        hurtFlag = false;
    }
    

    // function to retrieve or set block count
    // ブロック数の取得と設定する関数
    public int BlockCount
    {
        get; set;
    } = 7;

    // knockback function
    // ノックバック関数
    private void KnockbackBig(float knockbackPowerX, float knockbackPowerY, int scaleX)
    {
        coroutine = null;
        if(scaleX > 0)
        {
            rbody2d.AddForce(new Vector2(knockbackPowerX, knockbackPowerY), ForceMode2D.Impulse);
        }
        else
        {
            rbody2d.AddForce(new Vector2(-knockbackPowerX, knockbackPowerY), ForceMode2D.Impulse);

        }
        this.transform.parent.transform.localScale = new Vector3(-scaleX, 1.15f, 1);
    }
}
