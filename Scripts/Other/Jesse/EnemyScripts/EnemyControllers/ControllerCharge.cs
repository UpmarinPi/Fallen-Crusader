using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerCharge : MonoBehaviour
{
	#region
	[Header("�A�j���[�V����")]
	[SerializeField] string runAnim = "";
	[SerializeField] string hurtAnim = "";
	[SerializeField] string chargeAnim = "";
	[SerializeField] string brakeAnim = "";
	string nowAnim = "";


	float scale = 1;
	int enemyHP = 20;
	[SerializeField] float patrolSpeed = 3;
	[SerializeField] float chaseSpeed = 4;
	[SerializeField] float prepareTime;
	[SerializeField] float chargeTime;
	[SerializeField] float waitTime;
	[SerializeField] LayerMask Patrol;
	float vx = 0;

	Animator anim;
	Rigidbody2D rbody2d;

	
	bool enemyStunnedFlag = false;
	bool canAttackFlag = true;
	bool EnemyDeadFlag = false;
	bool attackingFlag = false;
	bool coroutineFlag = false;
	bool wasFrozenFlag = false;
	bool wallHitFlag = false;
	bool turnFlag = false;


	GameObject Player;
	GameObject attackHitbox;

	EnemyDamageCharge enemyDamage;
	EnemyGroundCheck enemyGroundCheck;
	DetectFollow detectFollow;
	EnemyFlip enemyFlip;
	EnemyAttack enemyAttack;
	EnemyAttack attackRange;
	BoxCollider2D hitbox;
	PlayerFinder playerFinder;
	MahoSE SE;

	Coroutine coroutine;
	Coroutine attackCoroutine;

	#endregion
	// variable declaration
	// �ϐ��錾

	private void Start()
	{

		playerFinder = new PlayerFinder();

		anim = GetComponent<Animator>();
		rbody2d = GetComponent<Rigidbody2D>();
		enemyGroundCheck = transform.Find("GroundCheck").gameObject.GetComponent<EnemyGroundCheck>();
		hitbox = transform.Find("HitCollision").gameObject.GetComponent<BoxCollider2D>();
		enemyDamage = transform.Find("HitCollision").GetComponent<EnemyDamageCharge>();
		enemyFlip = transform.Find("WallCheck").gameObject.GetComponent<EnemyFlip>();
		detectFollow = transform.Find("Detection").GetComponent<DetectFollow>();
		attackHitbox = transform.Find("AttackCollision").GetChild(0).gameObject;
		enemyAttack = transform.Find("AttackCollision").GetComponent<EnemyAttack>();
		attackRange = transform.Find("AttackRange").gameObject.GetComponent<EnemyAttack>();
		SE = transform.GetComponent<MahoSE>();
		Player = playerFinder.GetPlayer();
		rbody2d.gravityScale = 1;
		rbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		attackHitbox.SetActive(true);

	}

	private void FixedUpdate()
	{
		// if too far from parent object destroy self
		// �e�I�u�W�F�N�g���痣�ꂷ����Ə�����
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
		// ����ȉ��������������
		if (transform.position.y < -100)
		{
			Destroy(this.gameObject);
		}
		// if enemy takes damage set bool to true
		// �G���_���[�W��������� bool �� true
		if (enemyDamage.GetEnemyHit())
		{
			if (!enemyStunnedFlag)
			{
				enemyStunnedFlag = true;
			}
			enemyDamage.ResetEnemyHit();
		}
		
		// if colliding with wall while charging, set bool to true
		// �ːi���Ă鎞�ǂƏՓ˂����� bool �� true
		if (attackHitbox.activeSelf && enemyFlip.GetWallCollide() && !enemyFlip.GetCollider().CompareTag("PatrolPoint"))
		{
			wallHitFlag = true;
			enemyFlip.SetWallCollide(false);
		}

		// set bool to true if hp is below 0
		// HP ���O�ȉ��}��� bool �� true
		if (enemyHP <= 0)
		{
			EnemyDeadFlag = true;
		}
		// if falling or stunned dont move or attack
		// �G���X�^����Ԃ������Ă���Ƃ��͂ق��̃X�N���v�g�����s���Ȃ�
		if (enemyStunnedFlag || !enemyGroundCheck.GetEnemyGroundedFlag() || enemyAttack.getChargeHit || wallHitFlag/* || freezeFlag */)
		{
			attackHitbox.SetActive(false);

			// if already stunned, end stun function
			// �X�^����Ԃł���΃X�^���𒆒f����
			if (coroutine != null && enemyDamage.StunTime > 0)
			{
				StopCoroutine(coroutine);
			}

			// call enemy stun function
			// �G�̃X�^���֐����Ă�
			if (enemyStunnedFlag && enemyDamage.StunTime > 0)
			{
				coroutine = StartCoroutine(EnemyDamage(enemyDamage.StunTime));
				enemyDamage.StunTime = 0;
			}
			// end attack
			// �U�����f
			if ((attackCoroutine != null && attackingFlag) || (enemyAttack.getChargeHit || wallHitFlag))
			{
				StopCoroutine(attackCoroutine);
				coroutineFlag = false;
			} 
			attackingFlag = false;
			canAttackFlag = true;
			attackRange.AttackRange = false;

			// if collided with player recieve knockback
			// �v���C���[�ƂԂ�������m�b�N�o�b�N
			if (enemyAttack.getChargeHit || wallHitFlag)
			{
				wallHitFlag = false;
				enemyAttack.getChargeHit = false;
				enemyStunnedFlag = true;
				rbody2d.velocity = new Vector2(0f, rbody2d.velocity.y);
				if (transform.localScale.x > 0)
				{
					rbody2d.AddForce(new Vector2(-3f, 2f), ForceMode2D.Impulse);
				}
				else
				{
					rbody2d.AddForce(new Vector2(3f, 2f), ForceMode2D.Impulse);
				}
				coroutine = StartCoroutine(EnemyDamage(0.6f));
				coroutineFlag = true;
			}

			
			return;
		}
		// if time is frozen dont move or attack
		// ���Ԃ��~�߂��Ă���΍s�����Ȃ�
		if (MagicController.freezeFlag)
		{
			wasFrozenFlag = true;
			GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 1f);
			rbody2d.velocity = new Vector2(0f, rbody2d.velocity.y);
			anim.Play(null);
			return;
		}
		else if(!MagicController.freezeFlag && wasFrozenFlag)
		{
			wasFrozenFlag = false;
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
		}


		// patrol script
		// �p�g���[���̃X�N���v�g
		if (!detectFollow.GetPlayerDetect())
		{
			// flip on collision with wall or ledge
			// �R��ǂƂԂ������甽�]
			if (enemyFlip.GetWallCollide())
			{
				patrolSpeed = -patrolSpeed;
				enemyFlip.SetWallCollide(false);
			}
			nowAnim = runAnim;
			rbody2d.velocity = new Vector2(patrolSpeed, rbody2d.velocity.y);
			
			if (patrolSpeed > 0)
			{
				scale = 1f;
			}
			else if (patrolSpeed < 0)
			{
				scale = -1f;
			}
			this.transform.localScale = new Vector3(scale, 1f, 1f);
		}


		// player following script
		// �v���C���[�ǂ��X�N���v�g
		if (detectFollow.GetPlayerDetect())
		{
			// if already attacking dont move or attack again
			// �U������������s�����Ȃ�
			if (coroutineFlag)
			{				
				return;
			}

			
			nowAnim = runAnim;

			Vector2 player = Player.transform.position;
			Vector2 enemy = transform.position;
			Vector2 dir = (player - enemy).normalized;

			// based on which side the player is on, change attack distance
			// �v���C���[�����鑤�ɂ���čU��������ς���
			if (dir.x > 0)
			{
				scale = 1f;
				player.x -= 0.2f;
			}
			else
			{
				scale = -1f;
				player.x += 0.2f;
			}

			// if player is not in front of enemy then turn around
			// �v���C���[���G�̌����Ă�������ɂ��Ȃ�������A���]����
			if (scale != transform.localScale.x && !turnFlag)
			{
				turnFlag = true;
				Invoke("InvokeTurn", 0.5f);
			}

			// if player is in attack range attack, if attacking dont move
			// �v���C���[���͈͓��ɂ�����U���A�U�����Ă�����ǂ�Ȃ�
			if (attackRange.AttackRange && !turnFlag && canAttackFlag && detectFollow.CanSeePlayer())
			{
				attackRange.AttackRange = false;
				rbody2d.velocity = new Vector2(0, 0);
				attackCoroutine = StartCoroutine(Charge(prepareTime, chargeTime, waitTime));
				coroutineFlag = true;

				return;
			}


			dir = (player - enemy).normalized;
			if (dir.x < 0.3f && dir.x > 0)
			{
				dir.x = 0.3f;
			}
			else if(dir.x > -0.3f && dir.x < 0)
			{
				dir.x = -0.3f;
			}

			Vector2 rayPos  = transform.position;
			rayPos.x += 1 * transform.localScale.x;
			Debug.DrawLine(transform.position, rayPos);
			bool rayHitPos = Physics2D.Linecast(transform.position, rayPos, Patrol);

			// if there is an invisible wall in front then stop, other wise walk towards player
			// ��O�Ɍ����Ȃ��ǂ�����Ύ~�܂�, �Ȃ���΃v���C���[�̕����֕���
			if (enemyFlip.GetWallCollide() && rayHitPos)
			{
				rbody2d.velocity = new Vector2(0, rbody2d.velocity.y);
			}
			else
			{
				vx = transform.localScale.x * chaseSpeed;
				rbody2d.velocity = new Vector2(vx, rbody2d.velocity.y);
				if(vx > 0.2)
				{
					patrolSpeed = 3;
				}
				else if(vx < -0.2)
				{
					patrolSpeed = -3;
				}
			}


		}
	}

	// when not attacking start animation
	// �U�����ĂȂ��Ƃ��A�j���[�V�����n�߂�
	private void Update()
	{
		if (!coroutineFlag)
        {
			anim.Play(nowAnim);
		}
	}

	// turn functon
	// ���]�֐�
	void InvokeTurn()
	{
		turnFlag = false;
		this.transform.localScale = new Vector3(scale, 1, 1);
		canAttackFlag = false;
		Invoke("TurnAttackCooldown", 0.2f);
	}

	// enemy attack cooldown
	// �G�̍U���N�[���_�E��
	void TurnAttackCooldown()
	{
		canAttackFlag = true;
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

	// enemy attack function
	// �G�̍U���֐�
	IEnumerator Charge(float prepareTime, float chargeTime,  float waitTime)
	{
		attackingFlag = true;
		canAttackFlag = false;
		nowAnim = chargeAnim;
		anim.Play(nowAnim);
		yield return null;
		yield return new WaitForAnimation(anim, 0);
		nowAnim = runAnim;
		anim.Play(nowAnim);
		attackHitbox.SetActive(true);
		for (int i = 0; i < 20; i++)
		{
			rbody2d.velocity = new Vector2(rbody2d.velocity.x + (chaseSpeed/10f) * scale, rbody2d.velocity.y);
			yield return new WaitForFixedUpdate();
		}
		yield return new WaitForSeconds(chargeTime);
		nowAnim = brakeAnim;
		anim.Play(nowAnim);
		for (int i = 0; i < 20; i++)
		{
			float tempVelocity = rbody2d.velocity.x;
			if (rbody2d.velocity.x == 0)
			{
				break;
			}
			rbody2d.velocity = new Vector2(rbody2d.velocity.x - tempVelocity*0.05f, rbody2d.velocity.y);

			yield return new WaitForSeconds(0.01f);
		}
		rbody2d.velocity = new Vector2(0, rbody2d.velocity.y);
		attackingFlag = false;
		attackHitbox.SetActive(false);
		yield return new WaitForSeconds(waitTime - 0.2f);
		canAttackFlag = true;
		coroutineFlag = false;
		
	}

	// enemy damage function
	// �G�̃_���[�W�֐�
	IEnumerator EnemyDamage(float wait)
	{
		detectFollow.keepDetect();
		CancelInvoke("InvokeTurn");
		nowAnim = hurtAnim;
		anim.Play(nowAnim);
		SE.DamageSE();
		yield return new WaitForSeconds(0.05f);
		yield return new WaitForAnimation(anim, 0);
		rbody2d.velocity = new Vector2(rbody2d.velocity.x/2, rbody2d.velocity.y);
		if (EnemyDeadFlag)
		{
			StartCoroutine(Death());
			coroutineFlag = true;
		}
		else
		{
			yield return new WaitForSeconds(wait);
			enemyStunnedFlag = false;
			coroutineFlag = false;
			turnFlag = false;
		}
	}

	// enemy death function
	// �G���S�֐�
	IEnumerator Death()
	{
		SE.DeathSE();
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
			coroutineFlag = false;
			Destroy(gameObject);
		}

	}

}
