using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
	[SerializeField] GameObject magicSlashes;
	[SerializeField] GameObject magicExplosion;
	[SerializeField] GameObject magicHeal;
	[SerializeField] GameObject magicTheWorld;
	[SerializeField] GameObject magicFinisher;
	[SerializeField] SelectMagicUI selectMagicUI;
	[SerializeField] float timeOffsetY;
	[SerializeField] float shootOffsetX;
	[SerializeField] float healOffsetX;
	[SerializeField] float healOffsetY;

	public float mana = 0;
	public float maxMana = 100f;
	int count = 0;
	readonly int maxCount = 120;
	int countMagics;
	[SerializeField] MagicList magicList;
	List<int> needMana = new List<int>();
	public int GetNeedMana(int magicNumber)
	{
		return needMana[magicNumber];
	}

	[SerializeField] float magicSlashesSpeed = 10f;
	[SerializeField] float magicExplosionSpeed = 8f;

	bool leftFlag = false;
	bool castingFlag = false;
	bool decreaseFlag = false;
	bool magicFlag = false;
	bool testTrainingFlag = false;
	public static bool freezeFlag = false;
	int hitCount = 0;
	int decreaseCount = 0;

	PlayerController playerController;
	CameraShakeManager camManager;
	CameraShake CamShake;
	[SerializeField] SEController SE;

	Coroutine coroutine;

	private void Awake()
	{
		countMagics = magicList.GetMagicCount();
		for (int i = 0; i < countMagics; i++)//全魔法の名前と情報を取得
		{
			needMana.Add(magicList.GetMagicLists()[i].GetNeedMana());
		}
	}
	private void Start()
	{
		camManager = new CameraShakeManager();
		CamShake = camManager.GetShakeScript();
		freezeFlag = false;
		playerController = GetComponent<PlayerController>();
		
	}

	private void FixedUpdate()
	{
		// MP recovery
		// MP回復
		mana += hitCount;
		hitCount = 0;


		if (!playerController.GetEventTime() && !testTrainingFlag)
		{
			// count when MP isnt decreasing
			// MP減ってないときカウントする
			if (!decreaseFlag)
			{
				count++;
				decreaseCount = 0;
			}
			else
			{
				decreaseCount++;
			}

			// on sword attack reset MP decreasing counter
			// 攻撃したときはMP減るカウントリセット
			if (playerController.GetPlayerAttacking() || magicFlag)
			{
				magicFlag = false;
				count = 0;
				decreaseFlag = false;
			}
			
			// start decrease MP
			// MPが減り始める
			if (count == maxCount)
			{
				decreaseFlag = true;
				count = 0;
			}

			// set MP max and min
			// MP 最小値と最高値
			if (mana < 0)
			{
				mana = 0;
			}
			else if (mana > maxMana)
			{
				mana = maxMana;
			}

			// MP decreasing speed
			// MP減るスピード
			if (decreaseFlag && decreaseCount % 50 == 0 && mana != 0)
			{
				mana--;
			}
		}

		// training mode MP Bar process
		// トレーニングモードのMPバー処理
		if (testTrainingFlag)
		{
			// count when MP isnt decreasing
			// MP減ってないときカウントする
			if(!decreaseFlag)
			{
				count++;
				decreaseCount = 0;
			}
			else
			{
				decreaseCount++;
			}

			// start increase MP
			// MPが増え始める
			if(count == maxCount/4)
			{
				decreaseFlag = true;
				count = 0;
			}

			// set MP max and min
			// MP 最小値と最高値
			if(mana < 0)
			{
				mana = 0;
			}
			else if (mana > maxMana)
			{
				mana = maxMana;
			}

			// MP increasing speed
			// MP増えるスピード
			if(decreaseFlag && decreaseCount % 2 == 0 && mana != 100)
			{
				mana += 2;
			}

		}

	}


	void Update()
	{
		// determine direction
		// 向きを決める
		if (transform.localScale.x > 0)
		{
			leftFlag = false;
		}
		else
		{
			leftFlag = true;
		}

	}

	// 魔法打つ
	public void SwitchMagic()
	{
		// if player is on the ground and not attacking, cast magic
		// プレイヤーが地面にいて、攻撃してなければ魔法発動
		if ((playerController.GetGrounded() || playerController.GetJumpMagic()) && !playerController.GetPlayerAttacking())
		{
			int _needMana = needMana[selectMagicUI.GetSelectMagic];

			// if magic effect is active cant cast
			// 魔法効果中であれば魔法放てない
			if (selectMagicUI.GetSelectMagic == 3 && freezeFlag)
			{
				return;
			}
			if (selectMagicUI.GetSelectMagic == 4)
            {
				return;
            }

			// if player has enough mana, cast magic
			// プレイヤーが必要MPあれば魔法放つ
			if (!castingFlag && mana >= _needMana)
			{
				magicFlag = true;
				mana -= _needMana;
				castingFlag = true;
				switch (selectMagicUI.GetSelectMagic)
				{
					case 0:
						StartCoroutine(MagicHeal());
						break;
					case 1:
						StartCoroutine(MagicSlashes());
						break;
					case 2:
						StartCoroutine(MagicExplosion());
						break;
					case 3:
						StartCoroutine(FreezeAll());						
						break;
					case 4:
						break;
					default:
						Debug.Log("magic error");
						break;
				}
			}

		}
	}

	public bool CanFinisher()
    {
		if ((playerController.GetGrounded() || playerController.GetJumpMagic()))
		{
			int _needMana = needMana[selectMagicUI.GetSelectMagic];

			if (mana >= _needMana)
			{
				if (selectMagicUI.GetSelectMagic == 4)
                {
					return true;
                }
			}
		}
		return false;
	}
	public void Finisher()
    {
		int _needMana = needMana[selectMagicUI.GetSelectMagic];
		magicFlag = true;
		mana -= _needMana;
		StartCoroutine(MagicFinisher());
	}

	// function to set mana absorption
	// MP回復
	public void SetManaSteal(int stolenMana)
	{
		hitCount += stolenMana;
	}
	
	// function to inform player is casting magic
	// プレイヤーが魔法発動中を知らせる関数
	public bool GetPlayerCasting()
	{
		return castingFlag;
	}

	// function to set training mode
	// トレーニングモードを操作する関数
	public bool training
	{
		get
		{
			return testTrainingFlag;
		}

		set
		{
			testTrainingFlag = value;
			decreaseFlag = false;
			count = 0;
			decreaseCount = 0;
		}
	}
	
	// time stop magic function
	// 時止め魔法関数
	IEnumerator FreezeAll()
	{
		freezeFlag = true;
		Vector3 newPos = this.transform.position;

		GameObject TheWorldPrefab = Instantiate(magicTheWorld) as GameObject;
		newPos.z = -5;
		TheWorldPrefab.transform.position = newPos;
		CamShake.ShakeCamera(0.8f, 0.1f);
		for (int i = 0; i < 10; i++)
		{
			newPos.y += 0.1f;
			TheWorldPrefab.transform.position = newPos;
			yield return new WaitForSeconds(0.025f);
		}
		yield return new WaitForSeconds(0.25f);
		castingFlag = false;
		Animator anim = TheWorldPrefab.GetComponent<Animator>();
		SpriteRenderer sprite = TheWorldPrefab.GetComponent<SpriteRenderer>();
		SE.MagicSE(5);
		yield return new WaitForAnimation(anim, 0);
		for (int i = 0; i < 10; i++)
		{
			sprite.color -= new Color(0, 0, 0, -0.5f);
			yield return new WaitForSeconds(0.05f);
		}
		Destroy(TheWorldPrefab);
		yield return new WaitForSeconds(7f);
		freezeFlag = false;
	}

	// magic slashes function
	// 魔法斬撃関数
	IEnumerator MagicSlashes()
	{
		yield return new WaitForSeconds(0.1f);
		CamShake.ShakeCamera(0.2f, 0.1f);
		Vector3 newPos = this.transform.position;
		if (leftFlag)
		{
			newPos.x -= shootOffsetX;
		}
		else
		{
			newPos.x += shootOffsetX;
		}
		newPos.z = -5;

		GameObject magicSlashesPrefab = Instantiate(magicSlashes) as GameObject;
		magicSlashesPrefab.transform.position = newPos;

		SE.MagicSE(1);
		Rigidbody2D rbody = magicSlashesPrefab.GetComponent<Rigidbody2D>();

		Vector2 scale = magicSlashesPrefab.transform.localScale;
		scale = new Vector2(scale.x * transform.localScale.x, scale.y);
		magicSlashesPrefab.transform.localScale = scale;
		rbody.AddForce(new Vector2(magicSlashesSpeed * transform.localScale.x, rbody.velocity.y), ForceMode2D.Impulse);
		SpriteRenderer sprite = magicSlashesPrefab.GetComponent<SpriteRenderer>();
		sprite.color = new Color(1f, 1f, 1f, 0f);
		for (int i = 0; i < 10; i++)
		{
			if (magicSlashesPrefab != null)
			{
				sprite.color += new Color(0f, 0f, 0f, 0.1f);
				yield return new WaitForSeconds(0.01f);
			}
			else if (magicSlashesPrefab == null)
			{
				break;
			}
		}
		yield return new WaitForSeconds(0.3f);
		castingFlag = false;
	}

	// magic explosion function
	// 魔法爆発関数
	IEnumerator MagicExplosion()
	{
		yield return new WaitForSeconds(0.1f);
		CamShake.ShakeCamera(0.2f, 0.1f);
		Vector3 newPos = this.transform.position;
		if (leftFlag)
		{
			newPos.x -= shootOffsetX + 0.1f;
		}
		else
		{
			newPos.x += shootOffsetX;
		}
		newPos.z = -5;

		GameObject manaBombPrefab = Instantiate(magicExplosion) as GameObject;
		manaBombPrefab.transform.position = newPos;
		Rigidbody2D rbody = manaBombPrefab.GetComponent<Rigidbody2D>();

		Vector2 scale = manaBombPrefab.transform.localScale;
		scale = new Vector2(scale.x * transform.localScale.x, scale.y);
		manaBombPrefab.transform.localScale = scale;
		rbody.AddForce(new Vector2(magicExplosionSpeed * transform.localScale.x , rbody.velocity.y), ForceMode2D.Impulse);
		manaBombPrefab.tag = "MagicExplosion";
		yield return new WaitForSeconds(0.4f);
		castingFlag = false;
	}

	// magic heal function
	// 魔法回復関数
	IEnumerator MagicHeal()
	{
		// send a bool to player controller
		// should have an extra one to freeze player in place and be mid animation during
		yield return new WaitForSeconds(0.1f);
		Vector3 newPos = this.transform.position;
		if (leftFlag)
		{
			newPos.x -= healOffsetX;
		}
		else
		{
			newPos.x += healOffsetX;
		}

		GameObject manaHealPrefab = Instantiate(magicHeal) as GameObject;
		SE.MagicSE(0);
		newPos.y += healOffsetY;
		newPos.z = -5;
		manaHealPrefab.transform.position = newPos;
		Vector2 scale = manaHealPrefab.transform.localScale;
		scale = new Vector2(scale.x * transform.localScale.x, scale.y);
		manaHealPrefab.transform.localScale = scale;

		yield return new WaitForSeconds(0.5f);
		castingFlag = false;

	}

	// unfinished magic
	// 制作途中の魔法
	IEnumerator MagicFinisher()
    {
		Vector3 newPos = this.transform.position;
		if (leftFlag)
		{
			newPos.x -= 2.5f;
		}
		else
		{
			newPos.x += 1f;
		}
		newPos.y += 0.95f;
		newPos.z = -5;
		Instantiate(magicFinisher, newPos, Quaternion.identity);
		yield return new WaitForSeconds(0.1f);
		CamShake.ShakeCamera(0.2f, 0.2f);
	}

}
