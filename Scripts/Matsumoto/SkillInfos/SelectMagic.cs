using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectMagic : MonoBehaviour
{

	/*
	 * SelectMagicUIとPauseタグ(Pauseスクリプト入り)が必須
	 * */

	public bool selectingMagicFlag;		//魔法選択中
	Pause pauseFlag;					//ポーズ判定取得
	[SerializeField]
	GameObject selectMagicUI;           //魔法UI
	[SerializeField] InputManager inputManager;
	[SerializeField]
	ManaBar manaBar;
	public int waitTime = 50;			//魔法選択画面までの長押し時間
	bool doSkillFlag;					//trueで発動,falseで選択判定
	bool pushingFlag;               //現在スキルを選択中か
	public bool selectingSkill;
	bool reserveStopChoiceSkill;		//スキル選択停止予約


	void Start()
	{
		selectingMagicFlag = false;
		doSkillFlag = false;
		selectingSkill = false;
		reserveStopChoiceSkill = false;
		pauseFlag = GameObject.FindWithTag("Pause").transform.GetComponent<Pause>();

	}

	
	void Update()
	{
		if (pauseFlag.pauseFlag || GetComponent<PlayerController>().GetEventTime())//ポーズ中はキー入力を受け付けない
		{
			return;
		}
		//スキルキーが押された場合
		if (inputManager.inputCheck[(int)KeyBindings.KeyBindNumber.magic].buttonFlag)
		{
			if (!pushingFlag)
			{
				doSkillFlag = true;
				pushingFlag = true;
			}
		}
		//スキル選択キーが押された場合
		else if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.selectSkill].buttonDownFlag)
		{
			if (!pushingFlag)
			{
				SelectSkill();
				selectingSkill = true;
				pushingFlag = true;
			}
		}
		else
		{
			pushingFlag = false;
		}

		//スキル選択中にスキル選択キーが離されたら中断
		if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.selectSkill].buttonUpFlag && selectingSkill)
		{
			reserveStopChoiceSkill = true;//スキル中断予約
		}
		if(reserveStopChoiceSkill && !selectMagicUI.GetComponent<SelectMagicUI>().TurningFlag())//回り終わっているときにスキル中断
		{
			reserveStopChoiceSkill = false;
			StopSelectSkill();
		}
	}
	private void FixedUpdate()
	{
		//スキル発動判定
		if (doSkillFlag)
		{
			doSkill();
			doSkillFlag = false;
		}
	}

	//IEnumerator PushSkillCoroutine(float waittime)//1waittimeでfixedtime1回分(0.02秒)
	//{
	//	for (int countTime = 0; countTime < waittime; countTime++)
	//	{
	//		if (!Input.GetKey(skillKey))//時間待ち中にスキルボタンが離されたら
	//		{
	//			doSkillFlag = true;
	//			yield break;
	//		}
	//		yield return new WaitForFixedUpdate();//fixedupdate1回待つ
	//	}
	//	//一定時間スキルボタン押しっぱなしの場合
	//	doSkillFlag = false;
	//	finishCoroutineFlag = true;
	//}

	//スキル選択
	void SelectSkill()
	{
		Time.timeScale = 0;
		selectingMagicFlag = true;//外部に魔法選択ということを伝えるため
		Debug.Log("selectingSkill");
	}

	//スキル選択の中断
	void StopSelectSkill()
	{
		Time.timeScale = 1;
		selectingSkill = false;
		selectingMagicFlag = false;//外部用
		manaBar.updateScaleBarFlag = true;
		Debug.Log("stopped hold A");
	}

	//スキル発動
	void doSkill()
	{
		Debug.Log("Spelled");
		GetComponent<MagicController>().SwitchMagic();
	}

	public bool GetSelectFlag()
	{
		return selectingMagicFlag;
	}
}
