using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*�@�ǋL
 * ���@�̑I��UI�ł��B�O���O�����܂��B
 * �~����̓_��sin,cos�Ŏ���āA�ז��ɂȂ�Ȃ��悤���S�_�̈ړ�������Ă��܂�
 * �܂���ɓ���Ă��Ȃ����@�͔�΂��悤�v�Z���Ă��܂�
 * �E���a
 * �E�o�����ꑬ�x
 * �E��鑬�x
 * �E�o�������Ɠ��ꂽ���A���ꂼ��̒��S�_�̏ꏊ
 * �E�I�𒆈ȊO�̖��@�̑傫���A�F�̕ω�
 * ���C���X�y�N�^�[��ŕύX�ł��܂��B
 */
[Serializable]
public class MagicUIList
{
	public List<Magics> magics = new List<Magics>();//���@�̎�ނƁA���@UI�̃I�u�W�F�N�g
	public List<GameObject> magicUI = new List<GameObject>();

	//magicUI���ɂ���component3�BGetComponent��update���ł��Ȃ��悤�Ăяo���Ă���
	public List<RectTransform> magicUIRT = new List<RectTransform>();
	public List<Image> magicImage = new List<Image>();
	public List<Canvas> magicCanvas = new List<Canvas>();
}

public class SelectMagicUI : MonoBehaviour
{
	//�Ȃ���SerializeField�����Ȃ��ƃG���[���o�邽�߂��邾�����ĉB���Ă�B�v����
	[HideInInspector, SerializeField]
	MagicUIList magicUIList;
	public static int select;//���ݑI�𒆂̖��@
	bool pushingFlag;
	//�㉺�ɉ񂷂�����
	bool turnUpFlag;
	bool turnDownFlag;

	//�v���C���[�]�X
	[SerializeField] Transform player;
	[SerializeField] InputManager inputManager;

	SelectMagic selectMagic;

	[HideInInspector]
	public int countMagics;//���@�̌����擾
	[SerializeField] Pause pause;

	[SerializeField]
	private MagicList magicList;
	private Image iconImage;
	Sprite sprite;

	[SerializeField] EditData editData;
	PlayerDataClass data;

	float angle;
	float baseAngle;//�A�C�R�����m�̊p�x
	float turnAngle = 0.0f;//�A�C�R�����񂷍ۂ̕ϐ�
	[SerializeField] [Tooltip("�A�C�R�����񂷑���")] int baseTurnAngle = 10;
	float baseTurnAngleRadians;//��̊p�x�̌ʓx�\�L
	int turnCount;//������̃X�L�b�v���J�E���g

	[SerializeField]
	GameObject movePlace;
	RectTransform thisPos;
	RectTransform showPos;
	Vector2 hideOriginalPos;
	Vector2 showOriginalPos;
	bool changePosFlag;

	RectTransform magicFrame;

	//���̂Ƃ���g��Ȃ�
	//Image tabImage;


	bool openFlag;//�J�����Ƃ��Ɉ�񂾂�����

	[SerializeField]
	public float r;//���@UI�̍ő唼�a
	[SerializeField]
	float rSpeed;//���a���o������Ƃ��̑���
	float radius;//���ۂɂ͂��ꂪ���a�ɂȂ�B(�J���Ă郂�[�V�����r���Ȃ�)
				 //���a���ύX�����ۂ̍ő�Ƃ̊�����position���ύX���Ă���

	[SerializeField]
	float hideSize = 0.8f;

	[SerializeField]
	MagicInfoUI magicInfo;//���@�����Ɏg�p����I�u�W�F�N�g

	//���肵�Ă��Ȃ����@�̃A�C�R��
	[SerializeField] private Sprite missingIconImage;

	void Start()
	{
		selectMagic = player.GetComponent<SelectMagic>();
		data = editData.data;

		//player = GameObject.Find("Player").transform;

		countMagics = magicList.GetMagicCount();//���@�̐����J�E���g
		thisPos = this.GetComponent<RectTransform>();
		showPos = movePlace.GetComponent<RectTransform>();

		hideOriginalPos = thisPos.anchoredPosition;
		showOriginalPos = showPos.anchoredPosition;

		baseAngle = 2.0f * Mathf.PI / countMagics;//�ʓx�@ "1/�� * 2��"
		baseTurnAngleRadians = 2.0f * Mathf.PI * baseTurnAngle / 360.0f;
		pushingFlag = false;
		radius = 0;
		turnCount = 1;
		select = 0;

		for(int i = 0; i < countMagics + 1; i++)
		{
			if(i < countMagics)
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
					sprite = missingIconImage;
					magicUIList.magicImage[i].sprite = sprite;
				}
			}
			else//���@�̎���child���t���[���ɓo�^�A/*���̂�������tab�����̃A�C�R���ɓo�^*/
			{
				magicFrame = transform.GetChild(i).gameObject.GetComponent<RectTransform>();
				//tabImage = transform.GetChild(i + 1).gameObject.GetComponent<Image>();
			}
		}
	}


	void Update()
	{
		if(pause.pauseFlag)//�|�[�Y���̓L�[���͂��󂯕t���Ȃ�
		{
			return;
		}
		if(changePosFlag)//�J�����쎞�̈ړ��A��]
		{
			thisPos.anchoredPosition = new Vector2(Mathf.Lerp(hideOriginalPos.x, showOriginalPos.x, radius / r), Mathf.Lerp(hideOriginalPos.y, showOriginalPos.y, radius / r));
			magicFrame.rotation = Quaternion.Euler(0f, 0f, radius * -90.0f / r);
		}
		if(!selectMagic.selectingMagicFlag)
		{
			//����Ƃ��̃A�C�R���\������
			CloseMagicUI(rSpeed);
			openFlag = false;
		}
		else
		{
			if(!openFlag)
			{
				//�J���n�߂�Ƃ��̃A�C�R���\������
				magicInfo.SetInfoFlag = true;
				openFlag = true;
				//tabImage.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
				for(int i = 0; i < countMagics; i++)
				{
					if(data.magicFlag[i])
					{
						UnlockMagic(ref magicUIList, i);
					}
					else
					{
						LockMagic(ref magicUIList, i, missingIconImage);
					}
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
				turnAngle += baseTurnAngleRadians;
				if(turnAngle >= baseAngle * (float) turnCount)
				{
					if(!magicUIList.magics[(select + turnCount) % countMagics].UseFlag)
					{
						turnCount++;
					}
					else
					{
						turnAngle = 0.0f;
						turnUpFlag = false;
						if(select + turnCount >= countMagics)//��ԏ�ɂ���Ȃ��ԉ��Ɉړ��A����ȊO�Ȃ��Ɉړ�
						{
							select += turnCount - countMagics;
						}
						else
						{
							select += turnCount;
						}
						turnCount = 1;
						Debug.Log("nowSelecting " + select);
					}
				}
			}
			if(turnDownFlag)
			{
				turnAngle -= baseTurnAngleRadians;
				if(turnAngle <= -baseAngle * (float) turnCount)
				{
					if(!magicUIList.magics[(select + countMagics - turnCount) % countMagics].UseFlag)
					{
						turnCount++;
					}
					else
					{
						turnAngle = 0.0f;
						turnDownFlag = false;
						if(select - turnCount < 0)//��ԉ��ɂ���Ȃ�(��
						{
							select = countMagics - (select + turnCount);
						}
						else
						{
							select -= turnCount;
						}
						turnCount = 1;
						Debug.Log("nowSelecting " + select);
					}
				}
			}
			OpenMagicUI(rSpeed, r, ref radius);
			DisplayMagicUI(ref magicUIList, hideSize, select, radius, ref angle, turnAngle);
		}
	}
	//���@�I������߂Ė��@UI����铮��
	void CloseMagicUI(float closeSpeed)
	{
		magicInfo.SetInfoFlag = false;
		if(radius > 0)
		{
			radius -= closeSpeed;
			changePosFlag = true;
		}
		else
		{
			radius = 0;
			//tabImage.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
			changePosFlag = false;
		}

		for(int i = 0; i < countMagics; i++)
		{
			angle = baseAngle * (select - i) + turnAngle;
			if(i == select)
			{
				magicUIList.magicCanvas[i].sortingOrder = 1;
			}
			else
			{
				magicUIList.magicCanvas[i].sortingOrder = 0;
			}
			//Debug.Log("r = " + r + ", countMagics = " + countMagics + ", i + select = " + (i + select));
			magicUIList.magicUIRT[i].localPosition = AroundCirclePos(radius, angle);
		}
	}
	//���@�I�����n�߂�Ƃ��̓���
	void OpenMagicUI(float _openSpeed, float _radiusSize, ref float _radius)//_radius��_radiusSize�ɂȂ�܂�_openSpeed�����Z��������
	{
		if(_radius < _radiusSize)
		{
			_radius += _openSpeed;
			changePosFlag = true;
		}
		else
		{
			_radius = _radiusSize;
			changePosFlag = false;
		}
	}

	//�A�C�R���\������
	public void DisplayMagicUI(ref MagicUIList _magicUIList, float _hideSize, int _select, float _radius, ref float _angle, float _turnAngle)
	{
		for(int i = 0; i < countMagics; i++)
		{
			_angle = baseAngle * (_select - i) + _turnAngle;
			if(i == _select)
			{
				_magicUIList.magicCanvas[i].sortingOrder = 2;
				_magicUIList.magicImage[i].color = new Color32(255, 255, 255, 255);
				_magicUIList.magicUIRT[i].localScale = Vector2.one;
			}
			else
			{
				_magicUIList.magicCanvas[i].sortingOrder = 1;
				_magicUIList.magicImage[i].color = new Color32(127, 127, 127, 255);
				_magicUIList.magicUIRT[i].localScale = new Vector2(_hideSize, _hideSize);
			}
			//Debug.Log("r = " + r + ", countMagics = " + countMagics + ", i + select = " + (i + select));
			_magicUIList.magicUIRT[i].localPosition = AroundCirclePos(_radius, _angle);
		}
	}

	//�~����̓_��������
	public Vector2 AroundCirclePos(float r, float angle)//r = ���a, angle = �p�x(�ʓx�@)
	{
		float xPos = r * Mathf.Cos(angle);
		float yPos = r * Mathf.Sin(angle);

		return new Vector2(xPos, yPos);
	}

	//�A�C�R���̉񂵓r���Ȃ�true,����ĂȂ��Ȃ�false 
	public bool TurningFlag()
	{
		if(!turnUpFlag && !turnDownFlag)
			return false;
		else
			return true;
	}

	public int GetSelectMagic
	{
		get
		{
			return select;
		}
	}
	public void UnlockMagic(ref MagicUIList _magicUIList, int magicNumber)
	{
		_magicUIList.magics[magicNumber].UseFlag = true;
		sprite = _magicUIList.magics[magicNumber].GetIconImage();
		_magicUIList.magicImage[magicNumber].sprite = sprite;
	}
	public void LockMagic(ref MagicUIList _magicUIList, int magicNumber, Sprite _missingIconImage)
	{
		_magicUIList.magics[magicNumber].UseFlag = false;
		sprite = _missingIconImage;
		_magicUIList.magicImage[magicNumber].sprite = sprite;
	}
}
