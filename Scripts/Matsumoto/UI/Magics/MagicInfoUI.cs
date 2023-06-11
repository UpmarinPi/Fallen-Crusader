using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicInfoUI : MonoBehaviour
{
	Text nameBox;
	Text infoBox;//�q�I�u�W�F�N�g�Ɏw�肵���A���������I�u�W�F�N�g
	
	[SerializeField]
	List<string> nameText = new List<string>();//���@�̖��O���X�g
	[SerializeField]
	List<string> infoText = new List<string>();//���@�̏�񃊃X�g

	[SerializeField]//���ݑI�𒆂̖��@
	SelectMagicUI selectMagicUI;

	RectTransform boxPos;
	[SerializeField]
	float hideBoxY = 500.0f;
	Vector2 showBoxOriginalPos;
	float boxPosX;
	float boxPosY;

	[SerializeField]
	float moveSpeed = 5.0f;

	[SerializeField]
	private MagicList magicList;
	public int countMagics;

	[SerializeField]
	private bool infoFlag;

	public bool SetInfoFlag
	{
		set { infoFlag = value; }
	}

	[SerializeField]
	private EventController eventController;

	void Start()
	{
		nameBox = this.transform.GetChild(0).gameObject.GetComponent<Text>();
		infoBox = this.transform.GetChild(0).GetChild(0).gameObject.GetComponent<Text>();

		countMagics = magicList.GetMagicCount();

		boxPos = this.GetComponent<RectTransform>();
		showBoxOriginalPos = boxPos.anchoredPosition;
		boxPosX = boxPos.anchoredPosition.x;
		boxPos.anchoredPosition = showBoxOriginalPos + new Vector2(0, hideBoxY);

		for (int i = 0; i < countMagics; i++)//�S���@�̖��O�Ə����擾
		{
			nameText.Add(magicList.GetMagicLists()[i].GetName());
			infoText.Add(eventController.DecodingText(magicList.GetMagicLists()[i].GetInformation()));
		}
	}

	
	void Update()
	{
		if (infoFlag)
		{
			//boxPosX = Mathf.Lerp(boxPos.anchoredPosition.x, showBoxOriginalPos.x, moveSpeed + Time.deltaTime);	//X�𓮂����Ƃ��ɒǉ�
			boxPosY = Mathf.Lerp(boxPos.anchoredPosition.y, showBoxOriginalPos.y, moveSpeed * Time.unscaledDeltaTime);
			nameBox.text = nameText[selectMagicUI.GetSelectMagic];
			infoBox.text = infoText[selectMagicUI.GetSelectMagic];
		}
		else
		{
			//boxPosX = Mathf.Lerp(boxPos.anchoredPosition.x, showBoxOriginalPos.x, moveSpeed + Time.deltaTime);
			boxPosY = Mathf.Lerp(boxPos.anchoredPosition.y, showBoxOriginalPos.y + hideBoxY, moveSpeed * Time.unscaledDeltaTime);
		}
		boxPos.anchoredPosition = new Vector2(boxPosX, boxPosY);
	}
}
