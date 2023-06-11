using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerMouse : MonoBehaviour
{

	#region
	[Header("アニメーション")]
	[SerializeField] string runAnim = "";
	[SerializeField] string hurtAnim = "";
	[SerializeField] string attackAnim = "";
	[SerializeField] string tackleAnim = "";
	string nowAnim = "";

	int scale = 1;
	int count = 0;
	int enemyHP = 30;
	readonly int maxCount = 300;
	[SerializeField] float patrolSpeed = 3;
	[SerializeField] float chaseSpeed = 4.5f;
	[SerializeField] LayerMask Patrol;
	float vx = 0;
	float random = 0;


	Animator anim;
	Rigidbody2D rbody2d;


	bool turnFlag = false;
	bool enemyStunnedFlag = false;
	bool EnemyDeadFlag = false;
	bool cantAttackFlag = false;
	bool attackingFlag = false;
	bool animationEndFlag = false;
	bool wasFrozenFlag = false;
	bool jumpFlag = false;

	GameObject Player;
	GameObject hitbox;

	EnemyDamageMouse enemyDamage;
	EnemyGroundCheck enemyGroundCheck;
	EnemyAttack enemyAttack;
	EnemyAttack tackleAttack;
	EnemyInterfaceMouse interfaceMouse;
	DetectFollow detectFollow;
	EnemyFlip enemyFlip;
	PlayerFinder playerFinder;

	Coroutine coroutine, coroutineWaitTime;

	MahoSE SE;


	public enum attackName
	{
		Tail = 1,
		Tackle
	}

	#endregion
	// variable declaration
	// 変数宣言

	private void Start()
	{
		
		playerFinder = new PlayerFinder();

		anim = GetComponent<Animator>();
		rbody2d = GetComponent<Rigidbody2D>();
		enemyGroundCheck = transform.Find("GroundCheck").gameObject.GetComponent<EnemyGroundCheck>();
		enemyAttack = transform.Find("AttackRange").gameObject.GetComponent<EnemyAttack>();
		tackleAttack = transform.Find("JumpRange").gameObject.GetComponent<EnemyAttack>();
		hitbox = transform.Find("HitCollision").gameObject;
		interfaceMouse = transform.Find("AttackCollision").gameObject.GetComponent<EnemyInterfaceMouse>();
		enemyDamage = hitbox.GetComponent<EnemyDamageMouse>();
		enemyFlip = transform.Find("WallCheck").gameObject.GetComponent<EnemyFlip>();
		detectFollow = transform.Find("Detection").GetComponent<DetectFollow>();
		SE = GetComponent<MahoSE>();
		Player = playerFinder.GetPlayer();
		rbody2d.gravityScale = 1;
		rbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;

	}

	private void FixedUpdate()
	{
		// if too far from parent object destroy self
		// 親オブジェクトから離れすぎると消える
		if (transform.parent != null)
		{
			if (transform.parent.name == "enemyHolder")
			{
				if (Mathf.Abs(transform.parent.transform.position.y - transform.position.y) > 10)
				{
					Destroy(gameObject);
				}
			}
		}
		// if below this point destroy
		// これ以下落ちたら消える
		if (transform.position.y < -100)
		{
			Destroy(this.gameObject);
		}
		// if enemy takes damage set bool to true
		// 敵がダメージくらったら bool を true
		if (enemyDamage.GetEnemyHit())
		{
			if (!enemyStunnedFlag)
			{
				enemyStunnedFlag = true;
			}
			enemyDamage.ResetEnemyHit();
		}

		// set bool to true if hp is below 0
		// HP が０以下枝れば bool を true
		if (enemyHP <= 0)
		{
			EnemyDeadFlag = true;
		}
		// if falling or stunned dont move or attack
		// 敵がスタン状態か落ちているときはほかのスクリプトを実行しない
		if (enemyStunnedFlag || !enemyGroundCheck.GetEnemyGroundedFlag())
		{
			// if already stunned, end stun function
			// スタン状態であればスタンを中断する
			if (coroutine != null && enemyDamage.StunTime > 0)
			{
				StopCoroutine(coroutine);
			}

			// call enemy stun function
			// 敵のスタン関数を呼ぶ
			if (enemyStunnedFlag && enemyDamage.StunTime > 0)
			{
				coroutine = StartCoroutine(EnemyDamage(enemyDamage.StunTime));
				enemyDamage.StunTime = 0;
			}

			interfaceMouse.tackleFlag = false;
			jumpFlag = false;
			attackingFlag = false;
			animationEndFlag = true;
			return;
		}
		// if time is frozen dont move or attack
		// 時間が止めっていれば行動しない
		if (MagicController.freezeFlag)
		{
			wasFrozenFlag = true;
			GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 1f);
			rbody2d.velocity = new Vector2(0f, rbody2d.velocity.y);
			anim.Play(null);
			return;
		}
		else if (!MagicController.freezeFlag && wasFrozenFlag)
		{
			wasFrozenFlag = false;
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
		}

		// patrol script
		// パトロールのスクリプト
		if (!detectFollow.GetPlayerDetect())
		{
			// flip on collision with wall or ledge
			// 崖や壁とぶつかったら反転
			if (enemyFlip.GetWallCollide())
			{
				patrolSpeed = -patrolSpeed;
				enemyFlip.SetWallCollide(false);
			}
			nowAnim = runAnim;
			rbody2d.velocity = new Vector2(patrolSpeed, rbody2d.velocity.y);
			
			if (patrolSpeed > 0)
			{
				scale = 1;
			}
			else if (patrolSpeed < 0)
			{
				scale = -1;
			}
			this.transform.localScale = new Vector3(scale, 1, 1);
		}

		// player following script
		// プレイヤー追うスクリプト
		if (detectFollow.GetPlayerDetect())
		{
			// if already attacking dont move or attack again
			// 攻撃中だったら行動しない
			if (attackingFlag)
			{
				return;
			}

			Vector2 player = Player.transform.position;
			Vector2 enemy = transform.position;
			Vector2 dir = (player - enemy).normalized;

			// after some time change attack distance, so enemies dont stack on each other
			// 一定時間後攻撃開始距離を変える、これで敵が重なりにくくなる
			count++;
			if (count > maxCount)
			{
				count = 0;
				random = Random.Range(-0.4f, 0.2f);
			}


			int attackRand = Random.Range(0, 100);
			float distance = 0;

			// determine whether to do tackle attack or tail attack
			// タックル攻撃か尻尾攻撃するか判断する
			if (attackRand > 70)
			{
				distance = 0.3f;
			}

			if (dir.x > 0)
			{
				scale = 1;
				player.x -= 0.5f + distance + random;
			}
			else
			{
				scale = -1;
				player.x += 0.5f + distance + random;
			}

			// if player is not in front of enemy then turn around
			// プレイヤーが敵の向いている方向にいなかったら、反転する
			if (scale != transform.localScale.x && !turnFlag)
			{
				turnFlag = true;
				Invoke("InvokeTurn", 0.5f);
			}

			// if still in attack animation and able to attack, revert to running animation
			// まだアニメーションの途中で攻撃可能になってたら走るアニメーションに切り替わる
			if (nowAnim == attackAnim && !cantAttackFlag)
			{
				nowAnim = null;
				anim.Play(nowAnim);
			}

			// tackle attack function call
			// タックル攻撃関数よびだし
			if (tackleAttack.AttackRange && attackRand > 70)
			{
				if(!cantAttackFlag && !turnFlag)
				{
					coroutine = StartCoroutine(EnemyTackleAttack());
					return;
				}
			}
			// if player is in attack range attack, if attacking dont move
			// プレイヤーが範囲内にいたら攻撃、攻撃していたら追わない
			if (enemyAttack.AttackRange || nowAnim == attackAnim)
			{
				if (!cantAttackFlag && !turnFlag)
				{
					coroutine = StartCoroutine(EnemyTailAttack());
					return;
				}
			}

			nowAnim = runAnim;

			// walk towards the player, if in front stop
			// プレイヤーの方向へ歩く、手前だったら止まる
			if (enemyAttack.AttackRange)
			{
				rbody2d.velocity = new Vector2(0, 0);
				anim.Play(runAnim);
				anim.Play(null);
			}
			else
			{
				Vector2 rayPos = transform.position;
				rayPos.x += 1 * transform.localScale.x;
				Debug.DrawLine(transform.position, rayPos);
				bool rayHitFlag = Physics2D.Linecast(transform.position, rayPos, Patrol);

				// if there is an invisible wall in front then stop, other wise walk towards player
				// 手前に見えない壁があれば止まる, なければプレイヤーの方向へ歩く
				if (enemyFlip.GetWallCollide() && rayHitFlag)
				{
					rbody2d.velocity = new Vector2(0, rbody2d.velocity.y);
				}
				else
				{
					vx = transform.localScale.x * chaseSpeed;
					rbody2d.velocity = new Vector2(vx, rbody2d.velocity.y);
					if (vx > 0.2)
					{
						patrolSpeed = 3;
					}
					else if (vx < -0.2)
					{
						patrolSpeed = -3;
					}
				}

			}
		}

		// if attack is done, start cooldown for attack
		// 攻撃が終わったら、攻撃のクールダウンを始める
		if (animationEndFlag)
		{
			if (coroutineWaitTime != null)
			{
				StopCoroutine(coroutineWaitTime);
			}
			animationEndFlag = false;
			coroutineWaitTime = StartCoroutine(AttackCooldown(0.5f));
		}
		anim.Play(nowAnim);
	}

	// turn functon
	// 反転関数
	void InvokeTurn()
	{
		turnFlag = false;
		this.transform.localScale = new Vector3(scale, 1, 1);
	}

	// tackle attack jump function
	// タックル攻撃のジャンプ関数
	void JumpFunction()
    {
		jumpFlag = true;
    }

	// function to change attack type
	// 攻撃種類を変える関数
	public void ChangeAttack(attackName x)
	{
		interfaceMouse.SetAttack((int) x);
	}

	public int EnemyHP
	{
		get
		{
			return enemyHP;
		}
		set
		{
			enemyHP = value;
		}
	}

	// enemy tail attack function
	// 敵の尻尾攻撃関数
	IEnumerator EnemyTailAttack()
	{
		cantAttackFlag = true;
		rbody2d.velocity = new Vector2(0, 0);
		attackingFlag = true;
		yield return new WaitForSeconds(0.1f);
		nowAnim = attackAnim;
		anim.Play(nowAnim);
		SE.AttackSE();
		yield return null;
		yield return new WaitForAnimation(anim, 0);
		enemyAttack.AttackRange = false;
		attackingFlag = false;
		animationEndFlag = true;
	}

	// enemy tackle attack function
	// 敵のタックル攻撃関数
	IEnumerator EnemyTackleAttack()
	{
		cantAttackFlag = true;
		rbody2d.velocity = new Vector2(0, 0);
		attackingFlag = true;
		yield return new WaitForSeconds(0.2f);
		nowAnim = tackleAnim;
		anim.Play(nowAnim);
		while(!jumpFlag)
		{
			yield return null;
		}
		jumpFlag = false;
		rbody2d.velocity = new Vector2(2.5f * transform.localScale.x, 2f);
		while(!interfaceMouse.tackleFlag)
		{
			yield return null;
		}
		rbody2d.velocity = new Vector2(0, 0);
		yield return null;
		yield return new WaitForAnimation(anim, 0);
		interfaceMouse.tackleFlag = false;
		enemyAttack.AttackRange = false;
		attackingFlag = false;
		animationEndFlag = true;
	}

	// enemy attack cooldown
	// 敵の攻撃クールダウン
	IEnumerator AttackCooldown(float waitTime)
	{
		cantAttackFlag = true;
		yield return new WaitForSeconds(waitTime);
		cantAttackFlag = false;
	}

	// enemy damage function
	// 敵のダメージ関数
	IEnumerator EnemyDamage(float wait)
	{
		detectFollow.keepDetect();
		CancelInvoke("InvokeTurn");
		attackingFlag = false;
		cantAttackFlag = true;
		nowAnim = hurtAnim;
		anim.Play(nowAnim);
		SE.DamageSE();
		yield return new WaitForSeconds(0.05f);
		yield return new WaitForAnimation(anim, 0);
		rbody2d.velocity = new Vector2(rbody2d.velocity.x / 2, rbody2d.velocity.y);
		if (EnemyDeadFlag)
		{
			StartCoroutine(Death());
		}
		else
		{
			yield return new WaitForSeconds(wait);
			enemyStunnedFlag = false;
			animationEndFlag = true;
			turnFlag = false;
		}
	}

	// enemy death function
	// 敵死亡関数
	IEnumerator Death()
	{
		Destroy(hitbox);
		SpriteRenderer sprite;
		sprite = GetComponent<SpriteRenderer>();
		while (sprite.color != new Color(1f, 1f, 1f, 0f))
		{
			sprite.color = new Color(1f, 1f, 1f, sprite.color.a - 0.05f);
			yield return new WaitForSeconds(0.05f);
		}
		if (sprite.color == new Color(1f, 1f, 1f, 0f))
		{
			Destroy(gameObject);
		}
	}


}
