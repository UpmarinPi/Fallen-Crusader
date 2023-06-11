using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamage : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    Rigidbody2D rbody2d;
    int damage = 0;
    int scaleX = 0;
    float stun = 0;
    Vector2 knockback;    
    bool playerHurtFlag = false;
    bool invincibleFlag = false;
    public bool inversFlag { get; set; } = false;
    Coroutine coroutine;
    PlayerController playerController;
	GameObject Player;
    IDamageEnemy iDamage;


    void Start()
    {
		Player = transform.parent.gameObject;
        spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        rbody2d = transform.parent.GetComponent<Rigidbody2D>();
        playerController = transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
		// if player is hit by an enemy attack process damage
		// プレイヤーが敵の攻撃受けたとき、ダメージを処理する
        if((iDamage = collision.gameObject.GetComponent<IDamageEnemy>()) != null)
        {
			// if invincible or hit by spike, ignore incoming damage
			// 無敵やとげに当たったら攻撃を無視する
            if(invincibleFlag)
                return;
			if (collision.gameObject.CompareTag("SpikeTag"))
				return;

			iDamage.GetDamage(out damage, out stun, out knockback, out scaleX);
            if(stun == 0 || knockback == Vector2.zero)
                return;
            playerController.PlayerHP -= damage;
            playerHurtFlag = true;
            StunTime = stun;
            coroutine = null;            
            coroutine = StartCoroutine(DamageFlash());
	        rbody2d.velocity = new Vector2(0, 0);
            KnockbackBig(knockback.x, knockback.y, scaleX);
            invincibleFlag = true;

        }

		// boss exclusive damage script
		// ボス専用ダメージ処理
		if (!invincibleFlag)
		{
			if(collision.gameObject.CompareTag("BossWave"))
			{
				int scaleX = 1;
				if(Player.transform.position.x - collision.transform.position.x > 0)
				{
					scaleX = 1;
				}
				else
				{
					scaleX = -1;
				}
				playerController.PlayerHP -= 30;
				playerHurtFlag = true;
				StunTime = 0.5f;
				coroutine = null;
				coroutine = StartCoroutine(DamageFlash());
				rbody2d.velocity = new Vector2(0, 0);
				KnockbackBig(3.5f, 3.5f, scaleX);

			}
			if(collision.gameObject.CompareTag("BossFall"))
			{
				int scaleX = 1;
				if (Player.transform.position.x - collision.transform.position.x > 0)
				{
					scaleX = 1;
				}
				else
				{
					scaleX = -1;
				}
				playerController.PlayerHP -= 20;
				playerHurtFlag = true;
				StunTime = 0.3f;
				coroutine = null;
				coroutine = StartCoroutine(DamageFlash());
				rbody2d.velocity = new Vector2(0, 0);
				KnockbackBig(2.5f, 2.5f, scaleX);

			}
		}


    }

	private void OnTriggerStay2D(Collider2D collision)
	{
		if(!invincibleFlag)
		{

			if ((iDamage = collision.gameObject.GetComponent<IDamageEnemy>()) != null)
			{
				// only accept damage if its spike and not invincible
				// 無敵でなければとげの攻撃のみ受け取る
				if (invincibleFlag)
					return;
				if (!collision.gameObject.CompareTag("SpikeTag"))                
					return;     

				iDamage.GetDamage(out damage, out stun, out knockback, out scaleX);
				if (stun == 0 || knockback == Vector2.zero)
					return;
				playerController.PlayerHP -= damage;
				playerHurtFlag = true;
				StunTime = stun;
				coroutine = null;
				coroutine = StartCoroutine(DamageFlash());
				if (scaleX == 10)
				{
					// top and bottom spikes will hit player in opposite direction
					// 上下向きのとげはプレイヤーを反対方向へ飛ばす
					if (rbody2d.velocity.x > 0)
					{
						// if moving right, knockback left
						// 右へ動いてたら左へ飛ばす
						scaleX = -1;
					}
					else if (rbody2d.velocity.x < 0)
					{
						scaleX = 1;
					}
					else
					{
						scaleX = 1;
					}

				}
				rbody2d.velocity = new Vector2(0, 0);
				KnockbackBig(knockback.x, knockback.y, scaleX);
				invincibleFlag = true;

			}

			if (collision.gameObject.CompareTag("BossRoot"))
			{
				int scaleX = 1;
				if(Player.transform.position.x - collision.transform.position.x > 0)
				{
					scaleX = 1;
				}
				else
				{
					scaleX = -1;
				}
				playerController.PlayerHP -= 25;
				playerHurtFlag = true;
				StunTime = 0.5f;
				coroutine = null;
				coroutine = StartCoroutine(DamageFlash());
				rbody2d.velocity = new Vector2(0, 0);
				KnockbackBig(3.5f, 3.5f, scaleX);
				
			}
		}
	}

	// on taking damage flash red, when invincible be slightly transparent
	// ダメージ受けたら赤くなって、無敵の時は透過する
	IEnumerator DamageFlash()
    {
        invincibleFlag = true;
        yield return new WaitForSeconds(0.3f);
		// flash red
        // 赤くなる
        for (int i = 0; i < 2; i++)
        {
            spriteRenderer.color = new Color(1f, 0f, 0f, 1f);
            yield return new WaitForSeconds(0.05f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.05f);
        }
		// slightly transparent
        // 透過する
        for (int i = 0; i < 4; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(0.075f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.075f);
        }
        invincibleFlag = false;
    }

    IEnumerator InvincibleFlash()
	{
        for (int i = 0; i < 3; i++)
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f);
            yield return new WaitForSeconds(0.15f);
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(0.15f);
        }
    }

	// function to check if player got hit
    // プレイヤーが攻撃されたかをみる関数
    // 宣言と取得ができる関数
    public bool GetPlayerHit
    {
        get
		{
            return playerHurtFlag;
        }
        set
		{
            playerHurtFlag = value;
		}
    }
    public float StunTime
    {
        get; set;
    }

	// function to manually set invincibility
	// 無敵時間を設定する関数
    public void SetInvincible(bool Flag)
	{
        invincibleFlag = Flag;
        if (invincibleFlag == true)
		{
            StartCoroutine(InvincibleFlash());
		}
	}

	// knockback function
	// ノックバックの関数
    private void KnockbackBig(float knockbackPowerX, float knockbackPowerY, int scaleX)
    {
        coroutine = null;
        if(scaleX > 0)
        {
			playerController.FindInvers(-1f);
            rbody2d.AddForce(new Vector2(knockbackPowerX, knockbackPowerY), ForceMode2D.Impulse);
            //Debug.Log("right");
        }
        else
        {
			playerController.FindInvers(1f);
            rbody2d.AddForce(new Vector2(-knockbackPowerX, knockbackPowerY), ForceMode2D.Impulse);
            //Debug.Log("left");

        }
        this.transform.parent.transform.localScale = new Vector3(-scaleX, 1, 1);
    }

}
