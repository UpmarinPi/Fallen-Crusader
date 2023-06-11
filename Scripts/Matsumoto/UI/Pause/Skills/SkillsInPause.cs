using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum SkillEnum//�X�L���̎��
{
	Magic = 0,
	Passive = 1
}
[System.Serializable]
public class SelectingSkillInPause
{
	public SkillEnum skill;//�X�L��
	public int num;//���Ԗڂ̃X�L����
}
[Serializable]
public class PassiveUIList
{
	public List<Passives> passives = new List<Passives>();//���@�̎�ނƁA���@UI�̃I�u�W�F�N�g
	public List<GameObject> passiveUI = new List<GameObject>();

	//magicUI���ɂ���component3�BGetComponent��update���ł��Ȃ��悤�Ăяo���Ă���
	public List<RectTransform> passiveUIRT = new List<RectTransform>();
	public List<Image> passiveImage = new List<Image>();
	public List<Canvas> passiveCanvas = new List<Canvas>();
}
public class SkillsInPause : MonoBehaviour
{

	//�Ȃ���SerializeField�����Ȃ��ƃG���[���o�邽�߂��邾�����ĉB���Ă�B�v����
	[SerializeField, HideInInspector]
	MagicUIList magicUIList;
	[SerializeField]
	SelectMagicUI selectMagicUI;

	[SerializeField, HideInInspector]
	PassiveUIList passiveUIList;
	[SerializeField]
	PassiveList passiveList;

	public static int select;//���ݑI�𒆂̃X�L��
	bool pushingFlag;
	//�㉺�ɉ񂷂�����
	bool turnUpFlag;
	bool turnDownFlag;

	//�v���C���[�]�X
	[SerializeField] Transform player;
	[SerializeField] InputManager inputManager;

	[HideInInspector]
	public int countSkills;//���@�̌����擾
	[SerializeField] Pause pause;

	[SerializeField]
	private MagicList magicList;
	Sprite sprite;

	[SerializeField] EditData editData;
	PlayerDataClass data;

	RectTransform thisPos;

	//���̂Ƃ���g��Ȃ�
	//Image tabImage;
	[SerializeField]
	MagicInfoUI magicInfo;//���@�����Ɏg�p����I�u�W�F�N�g

	//���肵�Ă��Ȃ��X�L���̃A�C�R��
	[SerializeField] private Sprite missingMagicIconImage;
	[SerializeField] private Sprite missingPassiveIconImage;

	[SerializeField] private SkillInfoInPause skillInfoInPause;
	[SerializeField] private SkillVideoController skillVideoController;

	float angle;
	float baseAngle;//�A�C�R�����m�̊p�x
	float turnAngle = 0.0f;//�A�C�R�����񂷍ۂ̕ϐ�
	[SerializeField] [Tooltip("�A�C�R�����񂷑���")] float baseTurnAngle = 10;
	float baseTurnAngleRadians;//��̊p�x�̌ʓx�\�L
	[SerializeField] float slope;
	float slopeRadians;
	int turnCount;//������̃X�L�b�v���J�E���g

	[SerializeField]
	float radius;   //���S����̋���


	[SerializeField]
	int countSkillBetweenMagics = 1;//���@�̊ԂɃX�L���������ꍞ�ނ�


	//�X�L�����񂷍ۂɕK�v�ȕϐ�
	bool foundSkillFlag;//����ς݃X�L���𔭌������Ƃ�true
	int nowCountSkill;//���݂̃��[�v�łǂ̃X�L�������Ă��邩
	int passiveNumber;//�p�b�V�u�̔ԍ�
	public SelectingSkillInPause selectingSkillInPause;//���݃|�[�Y���łǂ̃X�L����I�����Ă��邩
	public void SetSelectingSkillInPause(SelectingSkillInPause _selectingSkillInPause)
	{
		selectingSkillInPause = _selectingSkillInPause;
	}
	public void SetSelectingSkillInPause(SkillEnum _skill, int _num)
	{
		selectingSkillInPause.skill = _skill;
		selectingSkillInPause.num = _num;
	}
	public SelectingSkillInPause GetSelectingSkillInPause()
	{
		return selectingSkillInPause;
	}

	[SerializeField]
	float showSize = 1f;
	[SerializeField]
	float hideSize = 0.8f;

	//���s�ɂ��X�L���̊e�傫�����v�Z
	float maxZ;//�ő��Z���W�B������minSize�Ƃ���
	float minZ;//�ŏ���Z���W�B������1�Ƃ���(��O�̂��̂قǑ傫�����AZ���������قǎ�O�ɂȂ邽��)
	[SerializeField] float minSize;//��ԏ��������̂��ǂꂮ�炢�̑傫���ɂ��邩
	float baseZ = 0;//�萔�B��̎O�̒l�����肳�ꂽ�Ƃ��Ɍ��܂�
	void SetBaseZ()
	{
		slopeRadians = 2.0f * Mathf.PI * slope / 360.0f;
		minZ = radius * Mathf.Cos(slopeRadians);
		maxZ = -radius * Mathf.Cos(slopeRadians);
		baseZ = (maxZ - minZ * minSize) / (1 - minSize);
	}

	void Start()
	{
		data = editData.data;

		//player = GameObject.Find("Player").transform;

		countSkills = magicList.GetMagicCount() * (countSkillBetweenMagics + 1);//�X�L���̐����J�E���g�B���@ x (�Ԃɓ���X�L�� + 1)
		thisPos = this.GetComponent<RectTransform>();

		baseAngle = 2.0f * Mathf.PI / countSkills;//�ʓx�@ "1/�� * 2��"

		baseTurnAngleRadians = 2.0f * Mathf.PI * baseTurnAngle / 360.0f;
		pushingFlag = false;
		turnCount = 1;
		select = 0;
		//���@������
		for(int i = 0; i < magicList.GetMagicCount(); i++)
		{
			magicUIList.magics.Add(magicList.GetMagicListOne(i));
			magicUIList.magicUI.Add(transform.GetChild(i).gameObject);
			magicUIList.magicUIRT.Add(magicUIList.magicUI[i].GetComponent<RectTransform>());
			magicUIList.magicImage.Add(magicUIList.magicUI[i].transform.GetComponent<Image>());
			magicUIList.magicCanvas.Add(magicUIList.magicUI[i].transform.GetComponent<Canvas>());
			if(data.magicFlag[i])
			{
				magicUIList.magics[i].UseFlag = true;
				sprite = magicUIList.magics[i].GetIconImage();
				magicUIList.magicImage[i].sprite = sprite;
			}
			else
			{
				magicUIList.magics[i].UseFlag = false;
				sprite = missingMagicIconImage;
				magicUIList.magicImage[i].sprite = sprite;
			}
		}
		//�p�b�V�u������
		for(int i = 0; i < passiveList.GetPassiveCount(); i++)
		{
			passiveUIList.passives.Add(passiveList.GetPassiveListOne(i));
			passiveUIList.passiveUI.Add(transform.GetChild(magicList.GetMagicCount() + i).gameObject);
			passiveUIList.passiveUIRT.Add(passiveUIList.passiveUI[i].GetComponent<RectTransform>());
			passiveUIList.passiveImage.Add(passiveUIList.passiveUI[i].transform.GetComponent<Image>());
			passiveUIList.passiveCanvas.Add(passiveUIList.passiveUI[i].transform.GetComponent<Canvas>());
			if(data.passiveFlag[i])
			{
				passiveUIList.passives[i].UseFlag = true;
				sprite = passiveUIList.passives[i].GetIconImage();
				passiveUIList.passiveImage[i].sprite = sprite;
			}
			else
			{
				passiveUIList.passives[i].UseFlag = false;
				sprite = missingPassiveIconImage;
				passiveUIList.passiveImage[i].sprite = sprite;
			}
		}
		SetBaseZ();
	}


	void Update()
	{
		if(!pause.pauseFlag || !pause.moveFlag)//�|�[�Y���ȊO�̓L�[���͂��󂯕t���Ȃ�
		{
			return;
		}
		skillInfoInPause.SetInfoFlag = true;//�|�[�Y���Ȃ�Ε\���͑�����
		SetBaseZ();
		//tabImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
		for(int i = 0; i < magicList.GetMagicCount(); i++)
		{
			if(data.magicFlag[i])
			{
				selectMagicUI.UnlockMagic(ref magicUIList, i);
			}
			else
			{
				selectMagicUI.LockMagic(ref magicUIList, i, missingMagicIconImage);
			}
		}
		//passive��
		for(int i = 0; i < passiveList.GetPassiveLists().Count; i++)
		{
			if(data.passiveFlag[i])
			{
				UnlockPassive(ref passiveUIList, i);
			}
			else
			{
				LockPassive(ref passiveUIList, i, missingPassiveIconImage);
			}
		}
		//�������͊Ǘ�
		if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.up].buttonFlag)
		{
			if(!turnUpFlag && !turnDownFlag && !pushingFlag)
			{
				turnUpFlag = true;
				pushingFlag = true;
			}
		}
		else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.down].buttonFlag)
		{
			if(!turnUpFlag && !turnDownFlag && !pushingFlag)
			{
				turnDownFlag = true;
				pushingFlag = true;
			}
		}
		else
		{
			pushingFlag = false;
		}

		//�񂷏���
		if(turnUpFlag)
		{
			turnAngle += baseTurnAngleRadians;//�p�x��傫��
			if(turnAngle >= baseAngle * (float) turnCount)//�p�x�����̃X�L���̂Ƃ���܂ŗ�����
			{
				nowCountSkill = (select + turnCount) % countSkills;//���@�ƃX�L���܂ߊۉ����������
				foundSkillFlag = SearchUseFlag(nowCountSkill);//���̃X�L��������ς݂��ǂ�������
				if(foundSkillFlag)//����ς݃X�L�����I���ł�����
				{
					turnAngle = 0.0f;
					turnUpFlag = false;
					if(select + turnCount >= countSkills)//��ԏ�ɂ���Ȃ��ԉ��Ɉړ��A����ȊO�Ȃ��Ɉړ�
					{
						select += turnCount - countSkills;
					}
					else
					{
						select += turnCount;
					}
					turnCount = 1;
				}
			}
		}
		if(turnDownFlag)
		{
			turnAngle -= baseTurnAngleRadians;
			if(turnAngle <= -baseAngle * (float) turnCount)
			{
				nowCountSkill = (select + countSkills - turnCount) % countSkills;//���@�ƃX�L���܂ߊۉ����������
				foundSkillFlag = SearchUseFlag(nowCountSkill);
				if(foundSkillFlag)
				{
					turnAngle = 0.0f;
					turnDownFlag = false;
					if(select - turnCount < 0)//��ԉ��ɂ���Ȃ�(��
					{
						select = countSkills - (select + turnCount);
					}
					else
					{
						select -= turnCount;
					}
					turnCount = 1;
				}
			}
		}
		DisplaySkillUI(ref magicUIList, ref passiveUIList, showSize, hideSize, select, radius, turnAngle);
	}
	bool SearchUseFlag(int _nowCountSkill)
	{
		//Debug.LogWarning("nowCountSkill : " + _nowCountSkill);
		if(_nowCountSkill % (countSkillBetweenMagics + 1) == 0)//�I���������̂����@�̏ꍇ
		{
			if(magicUIList.magics[_nowCountSkill / (countSkillBetweenMagics + 1) % countSkills].UseFlag)
			{
				return true;
			}
			else
			{
				turnCount++;
				return false;
			}
		}
		else//�p�b�V�u�̏ꍇ
		{
			//�I�������ꏊ�����Ԗڂ�passive�������߂�B�I�������ꏊ - (�I�������ꏊ/3(�؂�̂�) + 1)//1�Ԗڂ�0�̃p�b�V�u�����邽�߁A1���߂Ɍ��Z
			passiveNumber = _nowCountSkill - ((_nowCountSkill - _nowCountSkill % (countSkillBetweenMagics + 1)) / (countSkillBetweenMagics + 1) + 1);
			if(passiveNumber < passiveList.GetPassiveLists().Count && passiveList.GetPassiveListOne(passiveNumber).UseFlag)
			{
				return true;
			}
			else
			{
				turnCount++;
				return false;
			}
		}
	}
	public void DisplaySkillUI(ref MagicUIList _magicUIList, ref PassiveUIList _passiveUIList, float _showSize, float _hideSize, int _select, float _radius, float _turnAngle)
	{
		for(int i = 0; i < countSkills; i++)
		{
			angle = baseAngle * (_select - i) + _turnAngle;
			if(i % (countSkillBetweenMagics + 1) == 0)//�������^�Ȃ疂�@�I��
			{
				int mNum = i / (countSkillBetweenMagics + 1);

				Vector3 pos = AroundCirclePosInPause(_radius, angle);
				_magicUIList.magicUIRT[mNum].localPosition = pos;
				//Debug.LogWarning("[m] " + i + " / (" + countSkillBetweenMagics + " + 1) = " + mNum);
				if(i == _select)
				{
					SetSelectingSkillInPause(SkillEnum.Magic, mNum);
					_magicUIList.magicImage[mNum].color = new Color32(255, 255, 255, 255);
					_magicUIList.magicUIRT[mNum].localScale = CircleSizeInPause(pos.z, _showSize);
				}
				else
				{
					_magicUIList.magicImage[mNum].color = new Color32(127, 127, 127, 255);
					_magicUIList.magicUIRT[mNum].localScale = CircleSizeInPause(pos.z, _hideSize);
				}
			}
			else
			{
				int pNum = i - ((i - i % (countSkillBetweenMagics + 1)) / (countSkillBetweenMagics + 1) + 1);
				//Debug.Log("[p] " + i + " - ((" + i + " - " + i + " % (" + countSkillBetweenMagics + " + 1)) / (" + countSkillBetweenMagics + "+ 1) + 1) = " + pNum);
				if(pNum < passiveList.GetPassiveLists().Count)
				{
					Vector3 pos = AroundCirclePosInPause(_radius, angle);
					_passiveUIList.passiveUIRT[pNum].localPosition = pos;
					if(i == _select)
					{
						SetSelectingSkillInPause(SkillEnum.Passive, pNum);
						_passiveUIList.passiveImage[pNum].color = new Color32(255, 255, 255, 255);
						_passiveUIList.passiveUIRT[pNum].localScale = CircleSizeInPause(pos.z, _showSize);
					}
					else
					{
						_passiveUIList.passiveImage[pNum].color = new Color32(127, 127, 127, 255);
						_passiveUIList.passiveUIRT[pNum].localScale = CircleSizeInPause(pos.z, _hideSize);
					}
				}
			}
			//Debug.Log("r = " + r + ", countMagics = " + countMagics + ", i + select = " + (i + select));

		}
	}
	public Vector3 AroundCirclePosInPause(float _r, float _angle)//r = ���a, angle = �p�x(�ʓx�@), slope = �X��(�ʓx�@)
	{
		float cosAngle = Mathf.Cos(_angle);
		float xPos = _r * cosAngle * Mathf.Sin(slopeRadians);
		float yPos = _r * Mathf.Sin(_angle);
		float zPos = _r * cosAngle * Mathf.Cos(slopeRadians);

		return new Vector3(xPos, yPos, zPos);
	}
	public Vector2 CircleSizeInPause(float _zPos, float _size)//�~�̑傫����ύX����֐�
	{
		float realSize = (_zPos - baseZ) / (minZ - baseZ) * _size;
		return new Vector2(realSize, realSize);

	}
	void UnlockPassive(ref PassiveUIList _passiveUIList, int passiveNumber)
	{
		_passiveUIList.passives[passiveNumber].UseFlag = true;
		sprite = _passiveUIList.passives[passiveNumber].GetIconImage();
		_passiveUIList.passiveImage[passiveNumber].sprite = sprite;
	}
	void LockPassive(ref PassiveUIList _passiveUIList, int passiveNumber, Sprite _missingIconImage)
	{
		_passiveUIList.passives[passiveNumber].UseFlag = false;
		sprite = _missingIconImage;
		_passiveUIList.passiveImage[passiveNumber].sprite = sprite;
	}
}