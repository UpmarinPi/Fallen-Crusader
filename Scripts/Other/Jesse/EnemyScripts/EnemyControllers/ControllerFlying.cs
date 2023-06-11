using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerFlying : MonoBehaviour
{

	#region
	[Header("�A�j���[�V����")]
	[SerializeField] string flyAnim = "";
	[SerializeField] string attackPrepAnim = "";
	[SerializeField] string attackAnim = "";
	[SerializeField] string BrakeInitAnim = "";
	[SerializeField] string BrakeLoopAnim = "";
	[SerializeField] string hurtAnim = "";
	string nowAnim = "";

	int scale = 1;
	int enemyHP = 2;
	[SerializeField] float patrolDistance = 6;
	[SerializeField] float patrolSpeed = 3;
	[SerializeField] float chaseSpeed = 5;
	float speed = 0;

	Animator anim;
	Rigidbody2D rbody2d;

	bool EnemyDeadFlag = false;
	bool attackingFlag = false;
	bool stunFlag = false;
	bool endFlag = false;
	bool wasFrozenFlag = false;

	GameObject Player;
	GameObject AttackBox;
	Vector2 origin;
	Vector3 playerPos;

	EnemyDamageFlying enemyDamage;
	EnemyAttack enemyAttack;
	DetectFollow detectFollow;
	EnemyFlip enemyFlip;
	BoxCollider2D hitbox;
	PlayerFinder playerFinder;

	MahoSE SE;

	Coroutine coroutine;

	#endregion
	// variable declaration
	// �ϐ��錾

	private void Start()
	{
		playerFinder = new PlayerFinder();

		anim = GetComponent<Animator>();
		rbody2d = GetComponent<Rigidbody2D>();
		AttackBox = transform.Find("AttackCollision").gameObject;
		enemyAttack = transform.Find("AttackRange").gameObject.GetComponent<EnemyAttack>();
		hitbox = transform.Find("HitCollision").GetComponent<BoxCollider2D>();
		enemyDamage = transform.Find("HitCollision").GetComponent<EnemyDamageFlying>();
		detectFollow = transform.Find("Detection").GetComponent<DetectFollow>();
		enemyFlip = transform.Find("WallCheck").GetComponent<EnemyFlip>();
		Player = playerFinder.GetPlayer();
		SE = GetComponent<MahoSE>();
		rbody2d.gravityScale = 0;
		rbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		origin = transform.position;
		speed = patrolSpeed;
		AttackBox.SetActive(false);
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

		// set bool to true if hp is below 0
		// HP ���O�ȉ��}��� bool �� true
		if (enemyHP <= 0)
		{
			EnemyDeadFlag = true;
		}
		// if  stunned dont move or attack
		// �G���X�^����Ԃ̂Ƃ��͂ق��̃X�N���v�g�����s���Ȃ�
		if (enemyDamage.StunTime > 0 && !attackingFlag && !EnemyDeadFlag)
		{
			stunFlag = true;
			coroutine = StartCoroutine(EnemyDamage(enemyDamage.StunTime));
			enemyDamage.StunTime = 0;
		}
		else if (enemyDamage.StunTime > 0 && attackingFlag && !EnemyDeadFlag)
		{
			enemyDamage.StunTime = 0;
		}
		// if enemy is dead call damage function
		// �G������ł���Ƃ��̓_���[�W�֐����Ă�
		if(EnemyDeadFlag && !endFlag)
		{
			endFlag = true;
			if(coroutine != null)
				StopAllCoroutines();

			coroutine = StartCoroutine(EnemyDamage(0.0f));

			attackingFlag = false;
			return;
		}
		// if time is frozen dont move or attack
		// ���Ԃ��~�܂��Ă���΍s�����Ȃ�
		else if (MagicController.freezeFlag) // ���Ɩڏ���
		{
			wasFrozenFlag = true;
			GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 1f);
			rbody2d.velocity = new Vector2(0f, 0f);
			anim.Play(null);
			return;
		}
		else if(!MagicController.freezeFlag && wasFrozenFlag)
		{
			wasFrozenFlag = false;
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f);
			
		}

		// if stunned or dead dont execute code
		// �X�^����Ԃ�����ł���Ƃ��͍s�����Ȃ�
		if(EnemyDeadFlag || stunFlag)
			return;

		// if already attacking dont move or attack again
		// �U������������s�����Ȃ�
		if (attackingFlag)
		{
			return;
		}

		// if can detect player move faster
		// �v���C���[�����G�ł����瑁������
		if(detectFollow.GetPlayerDetect())
		{
			speed = scale * chaseSpeed;
		}
		else
		{
			speed = scale * patrolSpeed;
		}

		// flip on collision with wall
		// �ǂƂԂ������甽�]
		if (enemyFlip.GetWallCollide())
		{
			enemyFlip.SetWallCollide(false);
			speed = -speed;
		}

		// after moving a certain distance from their origin turn around
		// ���S�_�����苗�����ꂽ�甽�]����
		if(transform.position.x > origin.x + patrolDistance)
		{
			// flip left
			// ���։�]
			if(detectFollow.GetPlayerDetect())
				speed = -chaseSpeed;
			else 
				speed = -patrolSpeed;
		}
		else if (transform.position.x < origin.x - patrolDistance)
		{
			// flip right
			// �E�։�]
			if(detectFollow.GetPlayerDetect())
				speed = chaseSpeed;
			else
				speed = patrolSpeed;
		}

		nowAnim = flyAnim;
		rbody2d.velocity = new Vector2(speed, rbody2d.velocity.y);
		// face movement direction
		// �ړ������֌���
		if(speed > 0)
		{
			scale = 1;
		}
		else if(speed < 0)
		{
			scale = -1;
		}
		this.transform.localScale = new Vector3(scale, 1, 1);


		// if player is in attack range attack
		// �v���C���[���͈͓��ɂ�����U��
		if (enemyAttack.AttackRange && detectFollow.CanSeePlayer())
		{
			playerPos = Player.transform.position;
			coroutine = StartCoroutine(EnemyAttack(playerPos));
			return;		
		}
		
	}

	// when not attacking start animation
	// �U�����ĂȂ��Ƃ��A�j���[�V�����n�߂�
	private void Update()
	{
		if(!attackingFlag)
		{
			anim.Play(nowAnim);
		}
	}

	// coroutine cancelling function
	// �R�[���[�`�����f�֐�
	public void stopCoroutineFunc()
	{
		if (coroutine != null)
		{
			StopCoroutine(coroutine);
		}
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

	// enemy attack func
	// �G�̍U��
	IEnumerator EnemyAttack(Vector3 playerPos)
	{
		attackingFlag = true;
		rbody2d.velocity = new Vector2(0, 0);
		playerPos.z = 0f;
		Vector3 originalPos = playerPos;
		Vector3 selfPos = transform.position;
		playerPos.x = playerPos.x - selfPos.x;
		playerPos.y = playerPos.y - (selfPos.y + 1f);

		// rotate and face player 
		// ��]���ăv���C���[�Ɍ���
		float angle = Mathf.Atan2(playerPos.y, playerPos.x) * Mathf.Rad2Deg;
		if(transform.localScale.x < 0)
			angle -= 180;
		anim.Play(attackPrepAnim);
		
		// rise up 
		// �㏸����
		while (selfPos.y + 1f > transform.position.y)
		{
			rbody2d.velocity = new Vector2(0, 1f);
			yield return null;
		}
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
		anim.Play(attackAnim);
		AttackBox.SetActive(true);
		// dive attack
		// �}�~���U��
		while(transform.position.y > originalPos.y || rbody2d.velocity.y > -0.5f)
		{
			rbody2d.velocity = transform.TransformDirection(scale * 15, 0, 0);
			yield return null;
		}
		AttackBox.SetActive(false);
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		rbody2d.velocity = new Vector2(10f * scale, 0);
		anim.Play(BrakeInitAnim);
		yield return null;
		yield return new WaitForAnimation(anim, 0);
		anim.Play(BrakeLoopAnim);
		// braking loop
		// �����̃��[�v
		while(Mathf.Abs(rbody2d.velocity.x) > 1f)
		{
			rbody2d.velocity = new Vector2(rbody2d.velocity.x - 0.25f*(scale), 0);
			yield return null;
		}
		anim.Play(flyAnim);
		rbody2d.velocity = new Vector2(0, 0);
		yield return new WaitForSeconds(1f);
		// return to sky
		// �󒆂ɖ߂�
		while(transform.position.y < origin.y)
		{
			rbody2d.velocity = new Vector2(scale * 2, 5f);
			yield return null;
		}

		rbody2d.velocity = new Vector2(0, 0);
		attackingFlag = false;
	}


	// enemy damage function
	// �G�̃_���[�W�֐�
	IEnumerator EnemyDamage(float wait)
	{
		detectFollow.keepDetect();
		attackingFlag = false;
		nowAnim = hurtAnim;
		anim.Play(nowAnim);
		yield return new WaitForSeconds(0.05f);
		yield return new WaitForAnimation(anim, 0);
		if(EnemyDeadFlag)
		{
			StartCoroutine(Death());
		}
		else
		{
			rbody2d.velocity = new Vector2(rbody2d.velocity.x / 2, rbody2d.velocity.y);
			yield return new WaitForSeconds(wait);
			nowAnim = flyAnim;
			stunFlag = false;
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
			yield return new WaitForSeconds(0.025f);
		}
		if(sprite.color == new Color(1f, 1f, 1f, 0f))
		{
			Destroy(gameObject);
		}

	}
}
