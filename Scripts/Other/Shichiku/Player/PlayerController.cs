using System.Collections;
using UnityEngine.Events;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

// イベント時のフラグ用クラス
class EVENTFLAG
{
	public bool flag = false;
	public bool endFlag = true;
	public bool onceFlag = false;
	public bool coroutineFlag = false;
	public bool inputFlag = false;
}

class TRAINING
{
	public int hp;
	public float mp;
	public Vector3 position;
	public bool flag = false;
}
public class PlayerController : MonoBehaviour
{
	#region
	enum JumpState
	{
		CanTwice = 0,
		CanOnce = 1,
		CanNone = 2,
		CanAirOnce = 3,
		CanAirNone = 4
	}

	[SerializeField] BGMController BGM;
	[SerializeField] SEController SE;
	
	[Header("トレーニングモードの初期位置")]
	[SerializeField]Vector3 trainingPosition;

	[Header("エフェクトプレハブ")]
	[SerializeField] List<GameObject> effectPrefab = new List<GameObject>();

	[Header("エフェクト位置")]
	[SerializeField]Vector3 effectPosition;

	[Header("アニメーション")]
	public string idleAnim = "";
	public string walkAnim = "";
	//public string runAnim = "";
	public string dashAnim = "";
	public string fallAnim = "";
	public string hurtAnim = "";
	public string magicAnim = "";

	[Header("落ちた判定の位置")]
	public float fallPoint;

	[Header("セーブ")]
	[SerializeField] private EditData editData;

	[SerializeField]
	InputManager inputManager;

	[Header("ジャンプ攻撃中を位置を固定するか")]
	[SerializeField] bool jumpAttackStopFlag = false;

	PlayerDataClass dataClass;
	PlayerAfterDash playerAfterDash;

	string nowAnim = "";
	Animator anim;
	Animator chestAnim;
	GameObject swordCollision;
	GameObject hitBox;
	Rigidbody2D rbody2d;
	Vector2 velocity2d;
	Vector2 scale2d;
	
	Vector2 checkPoint;
	Collider2D chestCollision;
	public float powerY{ get; private set; }
	//float jumpmulti = 1;
	int effectTime = 60;
	Vector3 secondPos;
	Vector3 thirdPos;
	int playerHP = 500;
	const int maxPlayerHP = 500;

	SpriteRenderer playerRenderer;

	Pause pause;
	SelectMagic selectMagic;
	[Header("イベントスクリプト")]
	[SerializeField] EventController eventScript;

	bool deathFlag = false;
	bool overFlag = false;

	bool getKeyFlag = false;
	bool keydownFlag = false;
	bool fallFlag = false;
	bool playerStunnedFlag = false;
	static bool stopFlag = false;

	/**** イベントS関係  ****/
	int eventNumber;
	int videoNumber;

	public bool eventFlag = false;
	/****                ****/


	/**** ダッシュ関係  ****/
	bool canDashFlag = false;
	public bool DashFlag { get; private set; } = false;
	/****               ****/

	/**** 攻撃関係  ****/
	bool attackFlag = false;
	bool nextAttackFlag = false;
	bool jumpAttackFlag = false;

	int attackNumber = 0;
	public bool JumpMagicFlag { get; private set; } = false;
	/****           ****/

	/**** ジャンプ関係  ****/
	bool groundFlag = false;
	bool jumpFlag = false;
	bool jumpButtonFlag = false;
	bool doubleJumpOnceFlag = false;
	bool fallingFlag = false;
	float jumpPower = 120;

	JumpState jumptime = JumpState.CanTwice;
	/****               ****/

	/**** ワープ関係  ****/
	bool warpFlag = false;

	Vector3 warpPos;
	/****             ****/

	EVENTFLAG[] events = new EVENTFLAG[9];

	// トレーニングモードの控え値
	TRAINING training = new TRAINING();

	Coroutine coroutine, coroutineWaitTime, coroutineGetKeyOff, coroutineSpaceKeyUp;
	PlayerDamage playerDamage;
	MagicController magicController;

	readonly Vector3 InitPosition = new Vector3(-30f, -9.9f, 0f);
	// ジャンプ力
	const float maxJumpPower = 120;
	const float jumpPowerMinus = 12;
	// 歩くスピード
	const float walkSpeed = 5.0f;
	// 走るスピード
	//const float runSpeed = 7.5f;
	// 回避スピード
	const float dashSpeed = 19f;
	// 着地アニメーションの時間
	const float groundWaitTime = 1.0f / 18.0f;
	// Attack1アニメーションの時間
	const float attack1Time = 0.29f; //0.37f
									 // Attack2アニメーションの時間
	const float attack2Time = 0.17f; //0.26f
									 // Attack3アニメーションの時間
	const float attack3Time = 0.48f; //0.59f

	const float attackFinishTime = 0.17f;
	// エフェクトのクールダウン
	const int maxEffectTime = 15;

	// ボスの強制移動位置(X軸)
	private float bossPosX;
	[Header("スタートの強制移動位置(X軸)")]
	[SerializeField]
	private float startPosX;

	enum AttackNumber
	{
		Attack1 = 0,
		Attack2 = 1,
		Attack3 = 2
	};

	enum EffectNumber
	{
		Jump = 0,
		Air,
		Run,
		Dash,
		AirDash
	};

	enum SelectEvent
	{
		Talk = 0,
		Read,
		Watch,
		Boss,
		Kinomi,
		Ending,
		Begining,
		Special,
		Goal
	};

	#endregion
	// all variables
	// スタート関数
	private void Start()
	{
		// Update関数のフレームレートを約60に設定
		Application.targetFrameRate = 60;

		// ロード
		Debug.Log(editData.data.savePosition);
		transform.position = editData.data.savePosition;

		// 取得
		pause = GameObject.FindWithTag("Pause").transform.GetComponent<Pause>();
		playerAfterDash = this.GetComponent<PlayerAfterDash>();
		selectMagic = GetComponent<SelectMagic>();
		anim = GetComponent<Animator>();
		swordCollision = transform.Find("SwordCollision").gameObject;
		magicController = GetComponent<MagicController>();
		hitBox = transform.Find("HitCollision").gameObject;
		playerDamage = hitBox.GetComponent<PlayerDamage>();
		hitBox = hitBox.transform.GetChild(0).gameObject;
		rbody2d = GetComponent<Rigidbody2D>();
		velocity2d = rbody2d.velocity;
		playerRenderer = GetComponent<SpriteRenderer>();
		scale2d = transform.localScale;
		groundFlag = true;
		canDashFlag = true;

		for(int i = 0; i < 9; i++)
        {
			events[i] = new EVENTFLAG();
        }

		jumpPower = maxJumpPower;
		events[(int) SelectEvent.Begining].flag = IsInitData();
		if (events[(int) SelectEvent.Begining].flag)
		{
			eventScript.Starting();
			eventFlag = true;
		}
		else
		{
			BGM.PlayBGM();
			StartCoroutine(BGM.FadeInPlay());
		}
	}
	// 更新関数
	private void Update()
	{
		// 死んだら終了
		if(deathFlag)
		{
			jumpButtonFlag = false;
			return;
		}

		// ポーズ状態になったら完全停止
		if (pause.pauseFlag)
		{
			jumpButtonFlag = false;
			return;
		}

		// ビギニングイベントが始まった時
		if (events[(int) SelectEvent.Begining].flag)
		{
			if (!groundFlag)
			{
				JumpMove();
			}
			if (!events[(int) SelectEvent.Begining].coroutineFlag)
			{
				velocity2d = rbody2d.velocity;
				velocity2d.x = 0;
				rbody2d.velocity = velocity2d;

				if (!attackFlag && !DashFlag && groundFlag)
				{
					events[(int) SelectEvent.Begining].coroutineFlag = true;
					anim.SetBool("JumpState", false);
					nowAnim = walkAnim;
					StartCoroutine(Walking(startPosX));
				}
			}
			if (nowAnim == idleAnim)
			{
				GameEventFunction(ref events[(int) SelectEvent.Begining], SelectEvent.Begining, BeginningEnd);
			}
			eventFlag = true;
			return;
		}

		if (selectMagic.selectingMagicFlag)
		{
			nowAnim = idleAnim;
			jumpButtonFlag = false;
			return;
		}

		eventFlag = Events();

		if (eventFlag && !training.flag)
		{
			jumpButtonFlag = false;
			return;
		}
		if(stopFlag)
		{
			jumpButtonFlag = false;
			return;
		}

		// プレイヤーが攻撃されたら bool を true
		if (playerDamage.GetPlayerHit && !playerStunnedFlag)
		{
			playerStunnedFlag = true;
			playerDamage.GetPlayerHit = false;
		}
		if (playerHP > maxPlayerHP)
		{
			playerHP = maxPlayerHP;
		}
		if (playerHP <= 0 && !deathFlag)
		{
			//Time.timeScale = 0;
			deathFlag = true;
			StartCoroutine(PlayerDeath());
		}
		// プレーやが攻撃されたとき
		if (playerStunnedFlag)
		{
			//groundFlag = false;
			// プレイやーのダメージアニメーション

			if(playerDamage.StunTime > 0)
			{
				playerDamage.StunTime = 0;
				coroutine = StartCoroutine(PlayerDamage(playerDamage.StunTime));
			}

			jumpButtonFlag = false;
			return;
		}
		if (magicController.GetPlayerCasting())
		{
			if (GetGrounded())
			{
				nowAnim = magicAnim; // casting anim
				rbody2d.velocity = new Vector2(0f, 0f);
			}
			else if(GetJumpMagic())
			{
				JumpMagicFlag = false;
				anim.SetTrigger("MagicState");
				Vector3 vel = rbody2d.velocity;
				vel.x = 0;
				rbody2d.velocity = vel;
				rbody2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			}
			jumpButtonFlag = false;
			return;
		}

		if (transform.position.y < fallPoint)
		{
			rbody2d.velocity = Vector2.zero;
			transform.position = checkPoint;
			fallFlag = true;
			nowAnim = idleAnim;
			StartCoroutine(Revival());
		}

		if(fallFlag)
		{
			jumpButtonFlag = false;
			return;
		}

		if (fallingFlag && !DashFlag)
		{
			nowAnim = fallAnim;
		}

		Inputs();
	}

	private void FixedUpdate()
	{
		if (playerHP < 100 && training.flag)
        {
			playerHP += 100;
        }
		if(jumpButtonFlag)
		{
			switch(jumptime)
			{
				case JumpState.CanTwice:
				case JumpState.CanOnce:
				case JumpState.CanAirOnce:
					rbody2d.AddForce(new Vector2(0, jumpPower), ForceMode2D.Force);
					jumpPower -= jumpPowerMinus;
					if(jumpPower <= 0)
						jumpPower = 0;
					break;
				case JumpState.CanNone:
				case JumpState.CanAirNone:
					break;
			}
		}
		// アニメーションの切り替え
		if ((!attackFlag && groundFlag) || DashFlag)
		{
			anim.Play(nowAnim);
		}
		if (!canDashFlag)
		{
			canDashFlag = playerAfterDash.DashCoolDown();
		}
	}

	public bool GetJumpMagic()
	{
		return JumpMagicFlag && editData.jumpMagicFlag;
	}

	public bool GetEventTime()
	{
		if(stopFlag)
		{
			return true;
		}
		if(training.flag)
		{
			return false;
		}
		return eventFlag;
	}

	bool IsInitData()
	{
		return editData.data.savePosition == InitPosition;
	}

	bool Events()
	{
		/*イベント*/
		// チュートリアル状態になったら(会話バージョン)
		if (events[(int)SelectEvent.Talk].flag)
		{
			GameEventFunction(ref events[(int) SelectEvent.Talk], SelectEvent.Talk, TalkEnd);
			return true;
		}
		// チュートリアル状態になったら(看板バージョン)
		if (events[(int) SelectEvent.Read].flag)
		{
			GameEventFunction(ref events[(int) SelectEvent.Read], SelectEvent.Read, ReadEnd);
			return true;
		}
		// チュートリアル状態になったら(宝箱バージョン)
		if (events[(int) SelectEvent.Watch].flag)
		{
			GameEventFunction(ref events[(int) SelectEvent.Watch], SelectEvent.Watch, WatchEnd);
			return true;
		}
		// チュートリアル状態になったら(特殊バージョン)
		if (events[(int) SelectEvent.Special].flag)
		{
			GameEventFunction(ref events[(int) SelectEvent.Special], SelectEvent.Special, SpecialEnd);
			return true;
		}
		// イベント状態になったら(ボスバージョン)
		if (events[(int) SelectEvent.Boss].flag)
		{
			if (!groundFlag)
			{
				JumpMove();
			}
			if (!events[(int) SelectEvent.Boss].coroutineFlag)
			{
				velocity2d = rbody2d.velocity;
				velocity2d.x = 0;
				rbody2d.velocity = velocity2d;

				if (!attackFlag && !DashFlag && groundFlag)
				{
					events[(int) SelectEvent.Boss].coroutineFlag = true;
					anim.SetBool("JumpState", false);
					nowAnim = walkAnim;
					StartCoroutine(Walking(bossPosX));
				}
			}
			if (nowAnim == idleAnim)
			{
				GameEventFunction(ref events[(int) SelectEvent.Boss], SelectEvent.Boss, BossEnd);
			}
			return true;
		}
		// イベント状態になったら(木の実バージョン)
		if (events[(int) SelectEvent.Kinomi].flag)
		{
			GameEventFunction(ref events[(int) SelectEvent.Kinomi], SelectEvent.Kinomi, KinomiEnd);
			return true;
		}
		// エンディングイベント状態になったら
		if (events[(int) SelectEvent.Ending].flag)
		{
			GameEventFunction(ref events[(int) SelectEvent.Ending], SelectEvent.Ending, null);
			return true;
		}
		// イベント状態になったら(ステージクリアバージョン)
		if (events[(int) SelectEvent.Goal].flag)
		{
			GameEventFunction(ref events[(int) SelectEvent.Goal], SelectEvent.Goal, GoalEnd);
			return true;
		}

		return false;
		/************/
	}
	public void FindInvers(float scaleX)
	{
		if (transform.localScale.x != scaleX)
		{
			for (int i = 0; i < transform.childCount; i++)
			{
				if (transform.GetChild(i).GetComponent<IPlayerInvers>() != null)
				{
					Vector3 childScale = transform.GetChild(i).localScale;
					childScale.x *= -1f;
					transform.GetChild(i).localScale = childScale;
				}
			}
		}
	}

	void Inputs()
	{
		// Searchキーで看板や宝箱を見る
		if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.up].buttonDownFlag || inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.search].buttonDownFlag)
		{
			if (events[(int) SelectEvent.Read].inputFlag)
			{
				events[(int) SelectEvent.Read].flag = true;
			}
			else if (events[(int) SelectEvent.Watch].inputFlag)
			{
				SetMagicData(videoNumber);
				chestAnim.SetBool("GetBook", true);
				events[(int) SelectEvent.Watch].flag = true;
				if (chestCollision != null)
                {
					Destroy(chestCollision);
					chestCollision = null;
                }
			}
			else if (events[(int) SelectEvent.Special].inputFlag)
			{
				events[(int) SelectEvent.Special].flag = true;
			}
			if(warpFlag)
            {
				warpFlag = false;
				StartCoroutine(eventScript.WarpTime(warpPos));
            }
		}

		// 右キーで右に進む
		if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.right].buttonFlag && !attackFlag && !DashFlag)
		{
			FindInvers(1f);
			scale2d.x = 1f;
			transform.localScale = scale2d;
			playerRenderer.flipX = false;
			if(!fallingFlag && !jumpFlag)
			{
				nowAnim = walkAnim;
				CreateRunEffect(new Vector3(effectPosition.x, effectPosition.y, 0));
			}
			velocity2d = rbody2d.velocity;
			velocity2d.x = walkSpeed;
			rbody2d.velocity = velocity2d;
		}
		// 左キーで左に進む
		else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.left].buttonFlag && !attackFlag && !DashFlag)
		{
			FindInvers(-1f);
			scale2d.x = -1f;
			transform.localScale = scale2d;
			if(!fallingFlag && !jumpFlag)
			{
				nowAnim = walkAnim;
				CreateRunEffect(new Vector3(-effectPosition.x, effectPosition.y, 0));
			}
			velocity2d = rbody2d.velocity;
			velocity2d.x = -walkSpeed;
			rbody2d.velocity = velocity2d;
		}
		else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.right].buttonFlag && !jumpAttackStopFlag && !GetGrounded() && !DashFlag)
		{
			velocity2d = rbody2d.velocity;
			velocity2d.x = walkSpeed;
			rbody2d.velocity = velocity2d;
		}		
		else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.left].buttonFlag && !jumpAttackStopFlag && !GetGrounded() && !DashFlag)
		{
			velocity2d = rbody2d.velocity;
			velocity2d.x = -walkSpeed;
			rbody2d.velocity = velocity2d;
		}
		else if(!DashFlag && !fallingFlag)
		{
			if(!fallingFlag)
				nowAnim = idleAnim;
			velocity2d = rbody2d.velocity;
			velocity2d.x = 0;
			rbody2d.velocity = velocity2d;
		}

		// Attackキーでアタック
		if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.attack].buttonDownFlag && !attackFlag && GetGrounded() && !DashFlag)
		{
			swordCollision.tag = "PlayerAttack1";
			anim.SetTrigger("AttackState");
			coroutineWaitTime = StartCoroutine(WaitAttackAnimation(attack1Time));
		}
		else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.attack].buttonDownFlag && !attackFlag && jumpAttackFlag && editData.jumpAttackFlag && !DashFlag)
		{
			jumpAttackFlag = false;
			if(attackNumber <= 0)
			{
				swordCollision.tag = "PlayerAttack1";
				anim.SetTrigger("AttackState");
				if(jumpAttackStopFlag)
					rbody2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
				coroutineWaitTime = StartCoroutine(WaitAttackAnimation(attack1Time));
			}
		}
		// 連続でAttackキーを押すとコンボ
		else if (inputManager.inputCheck[(int)KeyBindings.KeyBindNumber.attack].buttonDownFlag && attackFlag && GetGrounded() && !DashFlag)
		{
			if (attackNumber <= 1)
			{
				nextAttackFlag = true;
			}
			else if(attackNumber <= 2 && magicController.CanFinisher())
            {
				nextAttackFlag = true;
            }
		}
		

		// Dashキーで回避
		if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.dash].buttonDownFlag && !DashFlag && canDashFlag)
		{
			if (attackFlag)
			{
				StopCoroutine(coroutineWaitTime);
				attackNumber = 0;
				anim.SetInteger("AttackNumber", attackNumber);
				attackFlag = false;
			}
			DashFlag = true;
			nowAnim = dashAnim;
			anim.Play(nowAnim);
			GameObject dashEffect;

			if(GetGrounded())
			{
				if(scale2d.x > 0)
				{
					dashEffect = Instantiate(effectPrefab[(int) EffectNumber.Dash], transform.position + new Vector3(-effectPrefab[(int) EffectNumber.Dash].transform.position.x, effectPrefab[(int) EffectNumber.Dash].transform.position.y, 0), Quaternion.identity);
					Vector2 effectScale = dashEffect.transform.localScale;
				}
				else
				{
					dashEffect = Instantiate(effectPrefab[(int) EffectNumber.Dash], transform.position + new Vector3(effectPrefab[(int) EffectNumber.Dash].transform.position.x, effectPrefab[(int) EffectNumber.Dash].transform.position.y, 0), Quaternion.identity);
					Vector2 effectScale = dashEffect.transform.localScale;
				}

				dashEffect.transform.localScale = new Vector2(-scale2d.x, scale2d.y);
				velocity2d = rbody2d.velocity;
			}
			else
			{
				if(scale2d.x > 0)
				{
					dashEffect = Instantiate(effectPrefab[(int) EffectNumber.AirDash], transform.position + new Vector3(-effectPrefab[(int) EffectNumber.AirDash].transform.position.x, effectPrefab[(int) EffectNumber.AirDash].transform.position.y, 0), effectPrefab[(int) EffectNumber.AirDash].transform.rotation);
					Vector2 effectScale = dashEffect.transform.localScale;
				}
				else
				{
					dashEffect = Instantiate(effectPrefab[(int) EffectNumber.AirDash], transform.position + new Vector3(effectPrefab[(int) EffectNumber.AirDash].transform.position.x, effectPrefab[(int) EffectNumber.AirDash].transform.position.y, 0), effectPrefab[(int) EffectNumber.AirDash].transform.rotation);
					Vector2 effectScale = dashEffect.transform.localScale;
				}

				dashEffect.transform.localScale = new Vector2(scale2d.x, -scale2d.x);
				velocity2d = rbody2d.velocity;
			}

			

			if (transform.localScale.x > 0)
			{
				velocity2d.x = dashSpeed;
			}
			else if (transform.localScale.x < 0)
			{
				velocity2d.x = -dashSpeed;
			}
			velocity2d.y = 0;
			rbody2d.velocity = velocity2d;
			//↓これはビット演算子 ORと同じ意味
			rbody2d.constraints = RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
			if(editData.dashAttackFlag)
			{
				hitBox.tag = "DashSlash";
				hitBox.SetActive(true);
			}
			else
			{
				hitBox.tag = "Untagged";
			}
			gameObject.layer = 18;
			playerDamage.SetInvincible(true);
			Invoke(nameof(DashOff), 0.2f);
		}

		JumpMove();

		// 入力しなければアイドル状態
		if (!Input.anyKey && (!(Mathf.Abs(Input.GetAxisRaw("Horizontal")) > 0f) && !(Mathf.Abs(Input.GetAxisRaw("JujiHorizontal")) > 0f)) && !DashFlag && !fallingFlag)
		{
			velocity2d = rbody2d.velocity;
			velocity2d.x = 0;
			rbody2d.velocity = velocity2d;
			if (!attackFlag && groundFlag)
				nowAnim = idleAnim;
		}
	}

	void MagicAnimEnd()
	{
		rbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	void GroundAnimEnd()
	{
		groundFlag = true;
	}

	// ジャンプ用のプログラム
	void JumpMove()
	{
		if(jumpFlag)
        {
			// 飛んでいるときY軸の速度を取得しBrendTree(アニメーション)に代入
			powerY = rbody2d.velocity.y;
			anim.SetFloat("JumpPower", powerY);
		}

		// ジャンプボタンが押された直後
		if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.jump].buttonDownFlag && !attackFlag && !keydownFlag)
		{
			getKeyFlag = true;
			coroutineGetKeyOff = StartCoroutine(GetKeyOff());
			keydownFlag = true;
			if (coroutine != null)
			{
				StopCoroutine(coroutine);
				anim.SetBool("IsGround", false);
			}
			// ジャンプ　0は地面からのジャンプ、1はジャンプ中のジャンプ、2はジャンプができない状態、3はジャンプをせずに落下中のジャンプ、4は3のジャンプできない状態
			switch (jumptime)
			{
				case JumpState.CanTwice:
					//jumpPower = maxJumpPower;
					jumpFlag = true;
					groundFlag = false;
					jumpAttackFlag = true;
					JumpMagicFlag = true;
					anim.SetBool("JumpState", true);
					SEJ();
					Instantiate(effectPrefab[(int) EffectNumber.Jump], transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
					break;
				case JumpState.CanOnce:
					if (!doubleJumpOnceFlag)
					{
						StopVelocityY();
						doubleJumpOnceFlag = true;
					}
					anim.SetBool("JumpState", true);
					SEJ();
					Instantiate(effectPrefab[(int) EffectNumber.Air], transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
					break;
				case JumpState.CanAirOnce:
					if (!jumpFlag)
					{
						StopVelocityY();
					}
					jumpFlag = true;
					groundFlag = false;
					jumpAttackFlag = true;
					JumpMagicFlag = true;
					anim.SetBool("JumpState", true);
					SEJ();
					Instantiate(effectPrefab[(int) EffectNumber.Air], transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
					break;
				case JumpState.CanNone:
					break;
				case JumpState.CanAirNone:
					break;
			}
		}

		// ジャンプボタンが押されている時
		if ((inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.jump].buttonFlag || getKeyFlag) && !attackFlag && keydownFlag)
		{
			jumpButtonFlag = true;

			// ジャンプ　0は地面からのジャンプ、1はジャンプ中のジャンプ、2はジャンプができない状態、3はジャンプをせずに落下中のジャンプ、4は3のジャンプできない状態
			switch (jumptime)
			{
				case JumpState.CanTwice:
					jumpFlag = true;
					groundFlag = false;
					anim.SetBool("JumpState", true);
					break;
				case JumpState.CanOnce:
					if (!doubleJumpOnceFlag)
					{
						StopVelocityY();
						doubleJumpOnceFlag = true;
					}
					anim.SetBool("JumpState", true);
					break;
				case JumpState.CanAirOnce:
					if (!jumpFlag)
					{
						StopVelocityY();
					}
					jumpFlag = true;
					groundFlag = false;
					anim.SetBool("JumpState", true);
					break;
				case JumpState.CanNone:
					break;
				case JumpState.CanAirNone:
					break;
			}
		}
		// ジャンプボタンを離した直後
		if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.jump].buttonUpFlag && keydownFlag)
		{
			jumpButtonFlag = false;
			keydownFlag = false;
			jumpPower = maxJumpPower;
			// ジャンプした後ジャンプカウンタを上昇
			if (jumptime == JumpState.CanTwice || jumptime == JumpState.CanOnce || jumptime == JumpState.CanAirOnce)
			{
				jumptime++;
			}
		}
	}
	// RunEffectを生成
	void CreateRunEffect(Vector3 effectPos)
	{
		if (effectTime > maxEffectTime)
		{
			effectTime = 0;
			GameObject runEffect = Instantiate(effectPrefab[(int) EffectNumber.Run], transform.position + effectPos, Quaternion.identity);
			runEffect.transform.localScale = scale2d;
		}
		effectTime++;
	}

	// 回避後の状態
	void DashOff()
	{
		Invoke("DashInvincible", 0.25f);
		hitBox.tag = "Player";
		hitBox.SetActive(false);
		gameObject.layer = 8;
		rbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
		velocity2d = rbody2d.velocity;
		velocity2d.x = 0;
		velocity2d.y = 0;
		rbody2d.velocity = velocity2d;
		DashFlag = false;
		if(jumpFlag && !groundFlag /*&& jumptime == 0*/ && !anim.GetBool("JumpState"))
		{
			groundFlag = true;
			jumpFlag = false;
			jumptime = JumpState.CanTwice;
			//anim.SetBool("JumpState", false);
			//anim.SetBool("IsGround", true);
			//coroutine = StartCoroutine(JumpOff());
		}
		nowAnim = idleAnim;
		if(playerDamage.inversFlag)
		{
			if(scale2d.x > 0)
			{
				FindInvers(-1f);
				scale2d.x = -1f;
				transform.localScale = scale2d;
			}
			else if (scale2d.x < 0)
			{
				FindInvers(1f);
				scale2d.x = 1f;
				transform.localScale = scale2d;
			}
		}
		canDashFlag = false;
	}

	void DashInvincible()
	{
		playerDamage.SetInvincible(false);
	}

	/*イベント*/
	void TalkEnd(bool value)
	{
		events[(int) SelectEvent.Talk].endFlag = value;
	}

	void ReadEnd(bool value)
	{
		events[(int) SelectEvent.Read].endFlag = value;
	}

	void WatchEnd(bool value)
	{
		events[(int) SelectEvent.Watch].endFlag = value;
		chestAnim.SetBool("GetBook", false);
	}

	void SpecialEnd(bool value)
	{
		events[(int) SelectEvent.Special].endFlag = value;
	}

	void BossEnd(bool value)
	{
		events[(int) SelectEvent.Boss].endFlag = value;
	}

	void KinomiEnd(bool value)
	{
		events[(int) SelectEvent.Kinomi].endFlag = value;
	}

	void BeginningEnd(bool value)
	{
		events[(int) SelectEvent.Begining].endFlag = value;
	}

	void GoalEnd(bool value)
	{
		events[(int)SelectEvent.Goal].endFlag = value;
		//nowAnim = walkAnim;
		//StartCoroutine(Walking(210, false));
	}

	// トレーニングモード
	public void TrainingMode(bool value)
	{
		if(value)
        {
			TrainingOnMode();
        }
		else
        {
			TrainingOffMode();
        }
	}

	// トレーニングONモード(ステータス値を一時的に保持)
	void TrainingOnMode()
    {
		training.hp = playerHP;
		training.mp = magicController.mana;
		training.position = transform.position;
		training.flag = true;

		Warp(trainingPosition, false);

		playerHP = maxPlayerHP;
		magicController.mana = 100;

		magicController.training = true;
	}
	// トレーニングOFFモード(一時的に保持した値をステータス値に戻す)
	void TrainingOffMode()
    {
		magicController.training = false;

		training.flag = false;
		playerHP = training.hp;
		magicController.mana = training.mp;

		Warp(training.position, false);

		velocity2d = rbody2d.velocity;
		velocity2d.x = 0;
		rbody2d.velocity = velocity2d;
	}

	// ゲームイベントが起こった時のプレイヤーの行動
	void GameEventFunction(ref EVENTFLAG gameEvent, SelectEvent paturn, UnityAction<bool> TimeEnd)
	{
		// ジャンプ中にゲームイベントが入ったらジャンプが終わるまで待つ
		if (!groundFlag)
			JumpMove();
		if (!gameEvent.onceFlag)
		{
			velocity2d = rbody2d.velocity;
			velocity2d.x = 0;
			rbody2d.velocity = velocity2d;

			if (!attackFlag && !DashFlag && groundFlag)
			{
				anim.SetBool("JumpState", false);
				nowAnim = idleAnim;
				gameEvent.endFlag = false;
				gameEvent.onceFlag = true;
				switch (paturn)
				{
					case SelectEvent.Talk:
						StartCoroutine(eventScript.TalkTime(eventNumber, 0.5f, TimeEnd));
						break;
					case SelectEvent.Read:
						StartCoroutine(eventScript.ReadTime(eventNumber, 0.5f, TimeEnd));
						break;
					case SelectEvent.Watch:
						StartCoroutine(eventScript.WatchTime(eventNumber, videoNumber, 0.5f, TimeEnd, TrainingMode));
						break;
					case SelectEvent.Boss:
						StartCoroutine(eventScript.BossTime(eventNumber, TimeEnd));
						break;
					case SelectEvent.Kinomi:
						StartCoroutine(eventScript.KinomiTime(eventNumber, 0.5f, TimeEnd));
						break;
					case SelectEvent.Ending:
						StartCoroutine(eventScript.EndingTime());
						break;
					case SelectEvent.Begining:
						StartCoroutine(eventScript.BeginningTime(TimeEnd));
						break;
					case SelectEvent.Special:
						StartCoroutine(eventScript.SpecialTime(eventNumber, videoNumber, TimeEnd, TrainingMode));
						break;
					case SelectEvent.Goal:
						StartCoroutine(eventScript.GoalTime(eventNumber, secondPos, thirdPos, TimeEnd));
						break;
				}
			}
		}
		else if (gameEvent.onceFlag && gameEvent.endFlag && !attackFlag && !DashFlag)
		{
			gameEvent.onceFlag = false;
			gameEvent.flag = false;
		}
		return;
	}

	void StopVelocityY()
	{
		velocity2d = rbody2d.velocity;
		velocity2d.y = 0;
		rbody2d.velocity = velocity2d;
	}

	IEnumerator GetKeyOff()
	{
		yield return new WaitForSeconds(0.1f);
		getKeyFlag = false;
	}

	/* 強制的に動かす */
	public IEnumerator Walking(float posX)
	{

		//bool cameraFlag = false;
		if (startPosX != posX)
		{
			StartCoroutine(eventScript.CameraMoving());
		}
		while (transform.position.x <= posX)
		{
			velocity2d = rbody2d.velocity;
			velocity2d.x = walkSpeed;
			rbody2d.velocity = velocity2d;
			CreateRunEffect(new Vector3(effectPosition.x, effectPosition.y, 0));
			yield return null;
		}
		velocity2d = rbody2d.velocity;
		velocity2d.x = 0;
		rbody2d.velocity = velocity2d;
		nowAnim = idleAnim;
	}

	// 歩かせる
	public IEnumerator Walking(float posX, bool flag)
	{
		while(transform.position.x <= posX)
		{
			velocity2d = rbody2d.velocity;
			velocity2d.x = walkSpeed;
			rbody2d.velocity = velocity2d;
			CreateRunEffect(new Vector3(effectPosition.x, effectPosition.y, 0));
			yield return null;
		}
		velocity2d = rbody2d.velocity;
		velocity2d.x = 0;
		rbody2d.velocity = velocity2d;
		nowAnim = idleAnim;

		events[(int) SelectEvent.Goal].endFlag = flag;
	}

	// 動きを止める
	static public void Stopping()
	{
		stopFlag = true;
	}

	// 移動用関数
	public void Warp(Vector3 pos, bool saveFlag)
	{
		transform.position = pos;
		if(saveFlag)
			PosSave();
	}
	/*****        *****/

	public void PosSave()
	{
		checkPoint = transform.position;
		SetPositionData = transform.position;
		editData.Save(editData.data);
	}

	public void SaveNextScenePosition()
	{

	}

	void SetMagicData(int magicNumber)
	{
		if (magicNumber == 4)
		{
			editData.dashAttackFlag = true;
			editData.dashAttackFlag = true;
			return;
		}
		editData.data.magicFlag[magicNumber] = true;
		editData.data.magicFlag[magicNumber] = true;
	}

	Vector3 SetPositionData
	{
		set { editData.data.savePosition = value; }
	}

	public void EndingEvent()
	{
		events[(int) SelectEvent.Ending].flag = true;
	}

	IEnumerator PlayerDeath()
	{
		BGM.StopBGM();
		Time.timeScale = 0.1f;

		yield return new WaitForSeconds(0.1f);

		Time.timeScale = 1;
		SE.SystemSE(1);

		GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;

		transform.GetChild(3).gameObject.SetActive(true);
		Vector3 size = transform.GetChild(3).localScale;
		Light2D light = transform.GetChild(3).GetComponent<Light2D>();

		for (float i = 0; i <= 1; i += 0.02f)
		{
			size = Vector3.Lerp(new Vector3(0, 0, 0), new Vector3(4, 4, 1), i);
			light.pointLightOuterRadius = Mathf.Lerp(0, 0.8f, i);
			transform.GetChild(3).localScale = size;
			yield return new WaitForSeconds(0.01f);
		}
		GetComponent<SpriteRenderer>().enabled = false;
		GetComponent<BoxCollider2D>().enabled = false;
		transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
		yield return new WaitForSeconds(0.1f);
		for (float i = 1; i >= 0; i -= 0.02f)
		{
			size = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(4f, 4f, 1), i);
			light.pointLightOuterRadius = Mathf.Lerp(0f, 0.8f, i);
			transform.GetChild(3).localScale = size;
			yield return new WaitForSeconds(0.01f);
		}

		overFlag = true;
	}

	public bool IsDeath
	{
		get { return overFlag; }
	}

	// アタックアニメーションの待ち時間
	IEnumerator WaitAttackAnimation(float waitTime)
	{
		attackFlag = true;
		
		if(jumpAttackStopFlag || GetGrounded())
		{
			velocity2d = rbody2d.velocity;
			velocity2d.x = 0;
			rbody2d.velocity = velocity2d;
		}

		yield return new WaitForSeconds(waitTime);

		// 2回目の攻撃
		if (nextAttackFlag && attackNumber == 0)
		{
			attackNumber++;
			nextAttackFlag = false;
			swordCollision.tag = "PlayerAttack2";
			anim.SetInteger("AttackNumber", attackNumber);
			yield return WaitAttackAnimation(attack2Time);
		}
		// 3回目の攻撃
		else if (nextAttackFlag && attackNumber == 1)
		{
			attackNumber++;
			nextAttackFlag = false;
			int random = Random.Range(0, 10);
			if (random == 9)
			{
				swordCollision.tag = "PlayerCrit1";
			}
			else
			{
				swordCollision.tag = "PlayerAttack3";
			}
			anim.SetInteger("AttackNumber", attackNumber);
			yield return WaitAttackAnimation(attack3Time);
		}
		// フィニッシャー攻撃
		else if (nextAttackFlag && attackNumber == 2 && magicController.CanFinisher())
        {
			attackNumber++;
			nextAttackFlag = false;
			anim.SetInteger("AttackNumber", attackNumber);
			magicController.Finisher();
			yield return WaitAttackAnimation(attackFinishTime);
        }
		else
		{
			switch (attackNumber)
			{
				case 0:
					yield return new WaitForSeconds(0.08f);
					break;
				case 1:
					yield return new WaitForSeconds(0.07f);
					break;
				case 2:
					yield return new WaitForSeconds(0.1f);
					break;
				case 3:
					yield return new WaitForSeconds(0.07f);
					break;
			}

			swordCollision.tag = "PlayerAttack1";
			attackNumber = 0;
			anim.SetInteger("AttackNumber", attackNumber);
			attackFlag = false;
		}
		rbody2d.constraints = RigidbodyConstraints2D.FreezeRotation;
	}

	void AttackEnd()
	{
		attackFlag = false;
	}

	// ジャンプ入力の最大時間
	/*IEnumerator InputTime()
	{
		yield return new WaitForSeconds(0.01f);
		for (int i = 1; i < 10; i++)
		{
			// スペースキーから放れたらコルーチンを終了
			if (keyupFlag)
			{
				yield break;
			}
			yield return new WaitForSeconds(0.01f);
		}
		inputFlag = true;
	}*/

	// 着地アニメーションの待ち時間
	IEnumerator JumpOff()
	{
		jumpAttackFlag = false;
		JumpMagicFlag = false;
		yield return new WaitForSeconds(groundWaitTime);
		groundFlag = true;
		anim.SetBool("IsGround", false);
	}

	IEnumerator Revival()
	{
		yield return new WaitForSeconds(0.5f);

		fallFlag = false;
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{

		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("CheckPoint"))
		{
			checkPoint = collision.transform.position;
			SetPositionData = collision.transform.position; // + new Vector3(0, 2, 0);
															// セーブ
			editData.Save(editData.data);
		}

		var warpZone = collision.transform.GetComponent<IWarpZone>();
		if(warpZone != null)
		{
			warpFlag = true;
			warpPos = warpZone.GetWarpPos();
		}

		var eventPointTag = collision.gameObject.GetComponent<EventPointTag>();
		if(eventPointTag == null) return;

		if (eventPointTag.ComparePointTag(EventPointTag.TagType.Talk) && !events[(int) SelectEvent.Talk].flag)
		{
			eventNumber = collision.gameObject.GetComponent<EventNumber>().GetSetEventNumber;
			events[(int) SelectEvent.Talk].flag = true;
			Destroy(collision.gameObject);
		}
		if (eventPointTag.ComparePointTag(EventPointTag.TagType.Read) && !events[(int) SelectEvent.Read].flag)
		{
			events[(int) SelectEvent.Read].inputFlag = true;
			eventNumber = collision.gameObject.GetComponent<EventNumber>().GetSetEventNumber;
		}
		if (eventPointTag.ComparePointTag(EventPointTag.TagType.Watch) && !events[(int) SelectEvent.Watch].flag)
		{
			events[(int) SelectEvent.Watch].inputFlag = true;
			eventNumber = collision.gameObject.GetComponent<EventNumber>().GetSetEventNumber;
			videoNumber = collision.gameObject.GetComponent<EventNumber>().GetSetVideoNumber;
			chestAnim = collision.GetComponent<Animator>();
			chestCollision = collision;
		}
		if (eventPointTag.ComparePointTag(EventPointTag.TagType.Special) && !events[(int) SelectEvent.Special].flag)
		{
			events[(int) SelectEvent.Special].inputFlag = true;
			eventNumber = collision.gameObject.GetComponent<EventNumber>().GetSetEventNumber;
			videoNumber = collision.gameObject.GetComponent<EventNumber>().GetSetVideoNumber;
		}
		if (eventPointTag.ComparePointTag(EventPointTag.TagType.Kinomi) && !events[(int) SelectEvent.Kinomi].flag)
		{
			events[(int) SelectEvent.Kinomi].flag = true;
			eventNumber = collision.gameObject.GetComponent<EventNumber>().GetSetEventNumber;
			Destroy(collision.gameObject);
		}
		if (eventPointTag.ComparePointTag(EventPointTag.TagType.Boss) && !events[(int) SelectEvent.Boss].flag)
		{
			StartCoroutine(BGM.FadeOutPlay());
			eventNumber = collision.gameObject.GetComponent<EventNumber>().GetSetEventNumber;
			bossPosX = collision.gameObject.GetComponent<EventNumber>().GetSetSecondPos.x;
			events[(int) SelectEvent.Boss].flag = true;
			Destroy(collision.gameObject);
		}
		if(eventPointTag.ComparePointTag(EventPointTag.TagType.Goal) && !events[(int) SelectEvent.Goal].flag)
		{
			eventNumber = collision.gameObject.GetComponent<EventNumber>().GetSetEventNumber;
			secondPos = collision.gameObject.GetComponent<EventNumber>().GetSetSecondPos;
			thirdPos = collision.gameObject.GetComponent<EventNumber>().GetSetThirdPos;
			events[(int) SelectEvent.Goal].flag = true;
			Destroy(collision.gameObject);
		}
		// 着地時の処理
		/*if (collision.gameObject.CompareTag("Ground") && !groundFlag)
		{

		}*/
	}
	public void GroundEnter()
	{
		/*if(!groundFlag)
        {
			Debug.Log(gameObject);
			jumptime = 0;
			groundFlag = true;
			anim.SetBool("JumpState", false);
			anim.SetBool("IsGround", true);
			coroutine = StartCoroutine(JumpOff());
		}*/

		if (jumpFlag)
		{
			jumpFlag = false;
			doubleJumpOnceFlag = false;
			anim.SetBool("JumpState", false);
			anim.SetBool("IsGround", true);
			//Instantiate(effectPrefab[0], transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
			coroutine = StartCoroutine(JumpOff());
		}
		if (fallingFlag)
		{
			groundFlag = false;
			anim.SetBool("IsGround", true);
			//Instantiate(effectPrefab[0], transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
			coroutine = StartCoroutine(JumpOff());
			jumptime = JumpState.CanTwice;
		}
		if (inputManager.inputCheck[(int)KeyBindings.KeyBindNumber.jump].buttonFlag)
		{
			jumptime = JumpState.CanAirNone;
			StartCoroutine(WaitSpaceKeyUp());
		}
		else
		{
			jumptime = JumpState.CanTwice;
		}
	}

	public void GroundStay()
	{
		if (jumptime == JumpState.CanNone || jumptime == JumpState.CanAirNone)
		{
			if (coroutineSpaceKeyUp == null)
			{
				coroutineSpaceKeyUp = StartCoroutine(WaitSpaceKeyUp());
			}
		}
		anim.SetBool("JumpState", false);
		fallingFlag = false;
	}

	public void GroundExit()
	{
		if (!jumpFlag)
		{
			fallingFlag = true;
			jumptime = JumpState.CanAirOnce;
		}
	}

	IEnumerator WaitSpaceKeyUp()
	{
		while (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.jump].buttonFlag)
		{
			yield return null;
		}
		yield return null;
		jumptime = JumpState.CanTwice;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		var eventPointTag = collision.gameObject.GetComponent<EventPointTag>();
		if(eventPointTag == null) return;

		var warpZone = collision.transform.GetComponent<IWarpZone>();
		if (warpZone != null)
		{
			warpFlag = false;
		}

		if (eventPointTag.ComparePointTag(EventPointTag.TagType.Read))
		{
			events[(int) SelectEvent.Read].inputFlag = false;
		}
		if (eventPointTag.ComparePointTag(EventPointTag.TagType.Watch))
		{
			events[(int) SelectEvent.Watch].inputFlag = false;
		}
		if (eventPointTag.ComparePointTag(EventPointTag.TagType.Special))
		{
			events[(int) SelectEvent.Special].inputFlag = false;
		}
	}

	public bool GetPlayerAttacking()
	{
		return attackFlag;
	}

	//地面についてるか判定する
	public bool GetGrounded()
	{
		return !fallingFlag && groundFlag;
	}

	public int PlayerHP
	{
		get
		{
			return playerHP;
		}
		set
		{
			playerHP = value;
		}
	}

	// ダメージアニメーション

	IEnumerator PlayerDamage(float wait)
	{
		nowAnim = hurtAnim;
		anim.Play(nowAnim);
		yield return null;
		yield return new WaitForAnimation(anim, 0);
		yield return new WaitForSeconds(wait);
		playerStunnedFlag = false;
		nowAnim = idleAnim;
	}


	// SE
	public void SEAttack(int number)
    {
		SE.PlayerAttackSE(number);
    }
	public void SEMagic(int number)
    {
		SE.MagicSE(number);
    }
	public void SEOther(int number)
    {
		SE.PlayerSE(number);
	}


	public void SEA1()
	{
		SE.PlayerAttackSE(0);
	}

	public void SEA2()
	{
		SE.PlayerAttackSE(0);
	}

	public void SEA3()
	{
		SE.PlayerAttackSE(0);
	}

	public void SED()
	{
		SE.PlayerSE(0);
	}

	public void SEH1()
	{
		SE.PlayerSE(1);
	}


	public void SER()
	{
		SE.PlayerSE(2);
	}
	public void SEJ()
	{
		SE.PlayerSE(3);
	}

	public void SEG()
	{
		SE.PlayerSE(4);
	}

	public void SEM1()
	{
		SE.MagicSE(0);
	}

	public void SEM2()
	{
		SE.MagicSE(1);
	}

	public void SEM3_1()
	{
		SE.MagicSE(2);
	}

	public void SEM3_2()
	{
		SE.MagicSE(3);
	}

	public void SEM4()
	{
		SE.MagicSE(4);
	}
}
