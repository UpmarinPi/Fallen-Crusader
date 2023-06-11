using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoInPause : MonoBehaviour
{
	Text nameBox;
	Text infoBox;//子オブジェクトに指定した、情報を書くオブジェクト

	[SerializeField]
	List<string> nameTextOfMagic = new List<string>();//魔法の名前リスト
	[SerializeField]
	List<string> nameTextOfPassive = new List<string>();//パッシブの名前リスト
	[SerializeField]
	List<string> infoTextOfMagic = new List<string>();//魔法の情報リスト
	[SerializeField]
	List<string> infoTextOfPassive = new List<string>();//パッシブの情報リスト

	[SerializeField]//現在選択中のスキル
	SkillsInPause skillsInPause;


	RectTransform boxPos;
	[SerializeField]
	public float hideBoxY = 500.0f;
	Vector2 showBoxOriginalPos;
	float boxPosX;
	public float boxPosY;

	[SerializeField]
	float moveSpeed = 5.0f;

	[SerializeField]
	private MagicList magicList;
	public int countMagics;

	[SerializeField]
	private PassiveList passiveList;
	public int countPassives;

	[SerializeField]
	private bool infoFlag;

	public bool SetInfoFlag
	{
		set
		{
			infoFlag = value;
		}
	}

	[SerializeField]
	private EventController eventController;

	void Start()
	{
		nameBox = this.transform.GetChild(0).gameObject.GetComponent<Text>();
		infoBox = this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();

		countMagics = magicList.GetMagicCount();
		countPassives = passiveList.GetPassiveCount();

		for(int i = 0; i < countMagics; i++)//全魔法の名前と情報を取得
		{
			nameTextOfMagic.Add(magicList.GetMagicLists()[i].GetName());
			infoTextOfMagic.Add(eventController.DecodingText(magicList.GetMagicLists()[i].GetInformation()));
		}
		for(int i = 0; i < countPassives; i++)//全パッシブの名前と情報を取得
		{
			nameTextOfPassive.Add(passiveList.GetPassiveLists()[i].GetName());
			infoTextOfPassive.Add(eventController.DecodingText(passiveList.GetPassiveLists()[i].GetInformation()));
		}
	}
	
	void Update()
	{
		if(infoFlag)
		{
			//boxPosX = Mathf.Lerp(boxPos.anchoredPosition.x, showBoxOriginalPos.x, moveSpeed + Time.deltaTime);	//Xを動かすときに追加
			boxPosY = Mathf.Lerp(boxPos.anchoredPosition.y, showBoxOriginalPos.y, moveSpeed * Time.unscaledDeltaTime);


			switch(skillsInPause.GetSelectingSkillInPause().skill)//ポーズ選択中のスキルを読み込み
			{
				case SkillEnum.Magic://魔法選択時
					nameBox.text = nameTextOfMagic[skillsInPause.GetSelectingSkillInPause().num];
					infoBox.text = infoTextOfMagic[skillsInPause.GetSelectingSkillInPause().num];
					break;
				case SkillEnum.Passive://パッシブ選択時
					nameBox.text = nameTextOfPassive[skillsInPause.GetSelectingSkillInPause().num];
					infoBox.text = infoTextOfPassive[skillsInPause.GetSelectingSkillInPause().num];
					break;
				default:
					nameBox.text = null;
					infoBox.text = null;
					break;
			}
		}
		boxPos.anchoredPosition = new Vector2(boxPosX, boxPosY);
	}
	public void Active()
	{
		if(boxPos == null)//初期化
		{
			boxPos = this.GetComponent<RectTransform>();
			showBoxOriginalPos = boxPos.anchoredPosition;
			boxPosX = boxPos.anchoredPosition.x;
			boxPos.anchoredPosition = showBoxOriginalPos + new Vector2(0, hideBoxY);
		}
		boxPos.anchoredPosition = showBoxOriginalPos + new Vector2(0, hideBoxY);
		infoFlag = true;
	}
}
