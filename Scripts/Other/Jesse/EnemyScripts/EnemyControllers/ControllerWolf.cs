using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerWolf : MonoBehaviour
{

	#region
	[Header("�A�j���[�V����")]
	[SerializeField] string walkAnim = "";
	[SerializeField] string runAnim = "";
	[SerializeField] string attackAnim = "";
	[SerializeField] string jumpAttackAnim = "";
	[SerializeField] string howlAnim = "";
	[SerializeField] string hurtAnim = "";
	string nowAnim = "";

	int direction = 1;
	int scale = 1;
	int count = 0;
	int enemyHP = 100;
	readonly int maxCount = 300;
	[SerializeField] float runningSpeed = 6f;
	[SerializeField] LayerMask Patrol;
	public float walkSpeed = 0; 
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
	bool runFlag = false;

	GameObject Player;
	GameObject hitbox;

	EnemyDamageWolf enemyDamage;
	EnemyGroundCheck enemyGroundCheck;
	EnemyAttack enemyAttack;
	EnemyAttack enemyJumpAttack;
	DetectFollow detectFollow;
	EnemyFlip enemyFlip;
	EnemyInterfaceWolf wolfInterface;
	PlayerFinder playerFinder;

	Coroutine coroutine, coroutineWaitTime;

	MahoSE SE;


	public enum attackName
	{
		Slash = 1,
		Run,
		JumpSlash
	}


	#endregion
	// variable declaration
	// �ϐ��錾

	private void Start()
	{
		
		playerFinder = new PlayerFinder();

		anim = GetComponent<Animator>();
		rbody2d = GetComponent<Rigidbody2D>();
		enemyGroundCheck = transform.Find("GroundCheck").gameObject.GetComponent<EnemyGroundCheck>();
		enemyAttack = transform.Find("AttackRange").gameObject.GetComponent<EnemyAttack>();
		enemyJumpAttack = transform.Find("JumpRange").gameObject.GetComponent<EnemyAttack>();
		hitbox = transform.Find("HitCollision").gameObject;
		enemyDamage = hitbox.GetComponent<EnemyDamageWolf>();
		enemyFlip = transform.Find("WallCheck").gameObject.GetComponent<EnemyFlip>();
		detectFollow = transform.Find("Detection").GetComponent<DetectFollow>();
		wolfInterface = transform.Find("AttackCollision").GetComponent<EnemyInterfaceWolf>();
		SE = GetComponent<MahoSE>();
		Player = playerFinder.GetPlayer();
		rbody2d.gravityScale = 1;
		rbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;

	}

	private void FixedUpdate()
	{
		// if too far from parent object destroy self
		// �e�I�u�W�F�N�g���痣�ꂷ����Ə�����
		if (transform.parent != null)
		{
			if(transform.parent.name == "enemyHolder")
			{
				if(Mathf.Abs(transform.parent.transform.position.y - transform.position.y) > 10)
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
			if(!enemyStunnedFlag)
			{
				enemyStunnedFlag = true;
			}
			enemyDamage.ResetEnemyHit();
		}

		// set bool to true if hp is below 0
		// HP ���O�ȉ��}��� bool �� true
		if (enemyHP <= 0)
		{
			EnemyDeadFlag = true;
		}
		// if falling or stunned dont move or attack
		// �G���X�^����Ԃ������Ă���Ƃ��͂ق��̃X�N���v�g�����s���Ȃ�
		if (enemyStunnedFlag || !enemyGroundCheck.GetEnemyGroundedFlag())
		{
			walkSpeed = 0;
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

			direction = (int)transform.localScale.x;
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
		// �p�g���[���̃X�N���v�g// �p�g���[���̃X�N���v�g
		if (!detectFollow.GetPlayerDetect())
		{
			
			vx = walkSpeed * transform.localScale.x;

			// flip on collision with wall or ledge
			// �R��ǂƂԂ������甽�]
			if (enemyFlip.GetWallCollide())
			{
				vx = -vx;
				enemyFlip.SetWallCollide(false);
			}
			nowAnim = walkAnim;
			rbody2d.velocity = new Vector2(vx, rbody2d.velocity.y);
			
			if(vx > 0)
			{
				direction = 1;
			}
			else if(vx < 0)
			{
				direction = -1;
			}
			transform.localScale = new Vector3(scale * direction, scale, 1);
		}

		// player following script
		// �v���C���[�ǂ��X�N���v�g
		if (detectFollow.GetPlayerDetect())
		{
			// if already attacking dont move or attack again
			// �U������������s�����Ȃ�
			if (attackingFlag)
			{
				return;
			}
			
			Vector2 player = Player.transform.position;
			Vector2 enemy = transform.position;
			Vector2 dir = (player - enemy).normalized;

			// after some time change attack distance, so enemies dont stack on each other
			// ��莞�Ԍ�U���J�n������ς���A����œG���d�Ȃ�ɂ����Ȃ�
			count++;
			if(count > maxCount)
			{
				count = 0;
				random = Random.Range(-0.4f, 0.2f);
			}

			// based on which side the player is on, change attack distance
			// �v���C���[�����鑤�ɂ���čU��������ς���
			if (dir.x > 0)
			{
				direction = 1;
				player.x -= 0.5f + random;
			}
			else
			{
				direction = -1;
				player.x += 0.5f + random;
			}


			// if player is not in front of enemy then turn around
			// �v���C���[���G�̌����Ă�������ɂ��Ȃ�������A���]����
			if ((direction) != transform.localScale.x && !turnFlag)
			{
				turnFlag = true;
				Invoke("InvokeTurn", 0.5f);
			}

			// if player is far away start running
			// �v���C���[�������Ƃ�����
			if(Mathf.Abs(player.x - enemy.x) > 5f)
			{
				runFlag = true;
			}
			else
			{
				runFlag = false;
				nowAnim = walkAnim;
			}


			// if still in attack animation and able to attack, revert to running animation
			// �܂��A�j���[�V�����̓r���ōU���\�ɂȂ��Ă��瑖��A�j���[�V�����ɐ؂�ւ��
			if (nowAnim == attackAnim && !cantAttackFlag)
			{
				nowAnim = null;
				anim.Play(nowAnim);
			}
			// if player is in attack range attack, if attacking dont move
			// �v���C���[���͈͓��ɂ�����U���A�U�����Ă�����ǂ�Ȃ�
			if (enemyAttack.AttackRange || nowAnim == attackAnim)
			{
				if(!cantAttackFlag && !turnFlag)
				{
					walkSpeed = 0;
					coroutine = StartCoroutine(EnemyAttack());
					return;
				}
			}


			// walk or run towards the player, if in front stop
			// �v���C���[�̕����֕���������A��O��������~�܂�
			if (enemyAttack.AttackRange)
			{
				rbody2d.velocity = new Vector2(0, 0);
				anim.Play(walkAnim);
				anim.Play(null);

			}
			else if (runFlag)
			{
				walkSpeed = 0;
				coroutine = StartCoroutine(JumpAttack());
				return;
			}
			else
			{
				Vector2 rayPos = transform.position;
				rayPos.x += 1 * transform.localScale.x;
				Debug.DrawLine(transform.position, rayPos);
				bool rayHitFlag = Physics2D.Linecast(transform.position, rayPos, Patrol);

				// if there is an invisible wall in front then stop, other wise walk towards player
				// ��O�Ɍ����Ȃ��ǂ�����Ύ~�܂�, �Ȃ���΃v���C���[�̕����֕���
				if (enemyFlip.GetWallCollide() && rayHitFlag)
				{
					rbody2d.velocity = new Vector2(0, rbody2d.velocity.y);
					nowAnim = null;
				}
				else
				{
					nowAnim = walkAnim;
					dir = (player - enemy).normalized;
					if (dir.x < 0.3f && dir.x > 0)
					{
						dir.x = 0.3f;
					}
					else if (dir.x > -0.3f && dir.x < 0)
					{
						dir.x = -0.3f;
					}
					vx = Mathf.Abs(dir.x) * transform.localScale.x * walkSpeed;
					rbody2d.velocity = new Vector2(walkSpeed * transform.localScale.x, rbody2d.velocity.y);
				}

			}
		}

		// if attack is done, start cooldown for attack
		// �U�����I�������A�U���̃N�[���_�E�����n�߂�
		if (animationEndFlag)
		{
			if(coroutineWaitTime != null)
			{
				StopCoroutine(coroutineWaitTime);
			}
			animationEndFlag = false;
			coroutineWaitTime = StartCoroutine(AttackCooldown(0.8f));
		}
		anim.Play(nowAnim);
	}

	// attack sound effect function
	// �U�����֐�
	void AttackSE()
	{
		SE.AttackSE();
	}

	// function to stop moving
	// �����̂��~�߂�֐�
	void Stop()
	{
		rbody2d.velocity = new Vector2(0, 0);
	}

	// function to change attack type
	// �U����ނ�ς���֐�
	public void ChangeAttack(attackName x)
	{
		wolfInterface.setAttack((int) x);
	}

	// turn functon
	// ���]�֐�
	void InvokeTurn()
	{
		turnFlag = false;
		this.transform.localScale = new Vector3(direction*scale, scale, 1);
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
	IEnumerator EnemyAttack()
	{
		cantAttackFlag = true;
		rbody2d.velocity = new Vector2(0, 0);
		attackingFlag = true;
		anim.Play(howlAnim);
		
		yield return null;
		yield return new WaitForAnimation(anim, 0);
		float distance = Player.transform.position.x - transform.position.x;
		if (distance > 0)
			transform.localScale = new Vector3(1f, scale, 1f);
		else 
			transform.localScale = new Vector3(-1f, scale, 1f);
		direction = (int)transform.localScale.x;
		anim.Play(attackAnim);		
		yield return null;
		yield return new WaitForAnimation(anim, 0);
		enemyAttack.AttackRange = false;
		attackingFlag = false;
		animationEndFlag = true;
	}

	// enemy jump attack function
	// �W�����v�U���֐�
	IEnumerator JumpAttack()
	{
		cantAttackFlag = true;
		attackingFlag = true;
		rbody2d.velocity = new Vector2(0, 0);
		anim.Play(howlAnim);
		yield return null;
		yield return new WaitForAnimation(anim, 0);
		anim.Play(runAnim);
		rbody2d.velocity = new Vector2(runningSpeed * transform.localScale.x, 0f);
		int tick = 120;
		bool wallFlag = false;
		// check if player is in attack range or collided with wall
		// �v���C���[���U���͈͓����ǂƐڐG�������m�F����
		while(tick > 0)
		{
			tick--;
			if (enemyJumpAttack.AttackRange)
			{
				break;
			}
			if(enemyFlip.GetWallCollide())
			{
				wallFlag = true;
				break;
			}
			yield return new WaitForFixedUpdate();
		}
		// if collided wih wall or a set time passed stop attack
		// �ǂƐڐG�������莞�ԉ߂�����U�����~
		if (tick <= 0 || wallFlag)
		{
			nowAnim = walkAnim;
			anim.Play(null);
			rbody2d.velocity = new Vector2(0, 0);
			animationEndFlag = true;
			attackingFlag = false;
			yield break;
		}
		anim.Play(jumpAttackAnim);

		Vector2 launchVelocity;
		float time = 0.6f;
		
		float distance = Player.transform.position.x - (transform.position.x + 0.5f * transform.localScale.x);
		float gravity = Physics2D.gravity.y;


		float launchHorizontal = 0;
		launchHorizontal = distance / (time);
		float launchVertical = 0;
		launchVertical = -gravity * (time / 2f);

		// calculate jump direction and attack player
		// �W�����v�������v�Z���ăv���C���[���U��
		launchVelocity = new Vector2(launchHorizontal, launchVertical);
		rbody2d.velocity = launchVelocity;

	
		yield return null;
		yield return new WaitForAnimation(anim, 0);
		detectFollow.keepDetect();
		attackingFlag = false;
		animationEndFlag = true;
	}

	// enemy attack cooldown
	// �G�̍U���N�[���_�E��
	IEnumerator AttackCooldown(float waitTime)
	{
		cantAttackFlag = true;
		yield return new WaitForSeconds(waitTime);
		cantAttackFlag = false;
	}

	// enemy damage function
	// �G�̃_���[�W�֐�
	IEnumerator EnemyDamage(float wait)
	{
		detectFollow.keepDetect();
		CancelInvoke("InvokeTurn");
		anim.Play(null);
		attackingFlag = false;
		cantAttackFlag = true;
		nowAnim = hurtAnim;
		anim.Play(hurtAnim);
		SE.DamageSE();
		yield return new WaitForSeconds(0.05f);
		yield return new WaitForAnimation(anim, 0);
		rbody2d.velocity = new Vector2(rbody2d.velocity.x / 2, rbody2d.velocity.y);
		if(EnemyDeadFlag)
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
	// �G���S�֐�
	IEnumerator Death()
	{
		Destroy(hitbox);
		SpriteRenderer sprite;
		sprite = GetComponent<SpriteRenderer>();
		while(sprite.color != new Color(1f, 1f, 1f, 0f))
		{
			sprite.color = new Color(1f, 1f, 1f, sprite.color.a - 0.05f);
			yield return new WaitForSeconds(0.05f);
		}
		if(sprite.color == new Color(1f, 1f, 1f, 0f))
		{
			Destroy(gameObject);
		}
	}

}
