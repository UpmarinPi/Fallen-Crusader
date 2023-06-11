using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoInPause : MonoBehaviour
{
	Text nameBox;
	Text infoBox;//�q�I�u�W�F�N�g�Ɏw�肵���A���������I�u�W�F�N�g

	[SerializeField]
	List<string> nameTextOfMagic = new List<string>();//���@�̖��O���X�g
	[SerializeField]
	List<string> nameTextOfPassive = new List<string>();//�p�b�V�u�̖��O���X�g
	[SerializeField]
	List<string> infoTextOfMagic = new List<string>();//���@�̏�񃊃X�g
	[SerializeField]
	List<string> infoTextOfPassive = new List<string>();//�p�b�V�u�̏�񃊃X�g

	[SerializeField]//���ݑI�𒆂̃X�L��
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

		for(int i = 0; i < countMagics; i++)//�S���@�̖��O�Ə����擾
		{
			nameTextOfMagic.Add(magicList.GetMagicLists()[i].GetName());
			infoTextOfMagic.Add(eventController.DecodingText(magicList.GetMagicLists()[i].GetInformation()));
		}
		for(int i = 0; i < countPassives; i++)//�S�p�b�V�u�̖��O�Ə����擾
		{
			nameTextOfPassive.Add(passiveList.GetPassiveLists()[i].GetName());
			infoTextOfPassive.Add(eventController.DecodingText(passiveList.GetPassiveLists()[i].GetInformation()));
		}
	}
	
	void Update()
	{
		if(infoFlag)
		{
			//boxPosX = Mathf.Lerp(boxPos.anchoredPosition.x, showBoxOriginalPos.x, moveSpeed + Time.deltaTime);	//X�𓮂����Ƃ��ɒǉ�
			boxPosY = Mathf.Lerp(boxPos.anchoredPosition.y, showBoxOriginalPos.y, moveSpeed * Time.unscaledDeltaTime);


			switch(skillsInPause.GetSelectingSkillInPause().skill)//�|�[�Y�I�𒆂̃X�L����ǂݍ���
			{
				case SkillEnum.Magic://���@�I����
					nameBox.text = nameTextOfMagic[skillsInPause.GetSelectingSkillInPause().num];
					infoBox.text = infoTextOfMagic[skillsInPause.GetSelectingSkillInPause().num];
					break;
				case SkillEnum.Passive://�p�b�V�u�I����
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
		if(boxPos == null)//������
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
