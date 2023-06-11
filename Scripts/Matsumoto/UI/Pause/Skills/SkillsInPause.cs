using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum SkillEnum//スキルの種類
{
	Magic = 0,
	Passive = 1
}
[System.Serializable]
public class SelectingSkillInPause
{
	public SkillEnum skill;//スキル
	public int num;//何番目のスキルか
}
[Serializable]
public class PassiveUIList
{
	public List<Passives> passives = new List<Passives>();//魔法の種類と、魔法UIのオブジェクト
	public List<GameObject> passiveUI = new List<GameObject>();

	//magicUI内にあるcomponent3つ。GetComponentをupdate内でしないよう呼び出しておく
	public List<RectTransform> passiveUIRT = new List<RectTransform>();
	public List<Image> passiveImage = new List<Image>();
	public List<Canvas> passiveCanvas = new List<Canvas>();
}
public class SkillsInPause : MonoBehaviour
{

	//なぜかSerializeFieldをつけないとエラーが出るためつけるだけつけて隠してる。要検証
	[SerializeField, HideInInspector]
	MagicUIList magicUIList;
	[SerializeField]
	SelectMagicUI selectMagicUI;

	[SerializeField, HideInInspector]
	PassiveUIList passiveUIList;
	[SerializeField]
	PassiveList passiveList;

	public static int select;//現在選択中のスキル
	bool pushingFlag;
	//上下に回すか判定
	bool turnUpFlag;
	bool turnDownFlag;

	//プレイヤー云々
	[SerializeField] Transform player;
	[SerializeField] InputManager inputManager;

	[HideInInspector]
	public int countSkills;//魔法の個数を取得
	[SerializeField] Pause pause;

	[SerializeField]
	private MagicList magicList;
	Sprite sprite;

	[SerializeField] EditData editData;
	PlayerDataClass data;

	RectTransform thisPos;

	//今のところ使わない
	//Image tabImage;
	[SerializeField]
	MagicInfoUI magicInfo;//魔法説明に使用するオブジェクト

	//入手していないスキルのアイコン
	[SerializeField] private Sprite missingMagicIconImage;
	[SerializeField] private Sprite missingPassiveIconImage;

	[SerializeField] private SkillInfoInPause skillInfoInPause;
	[SerializeField] private SkillVideoController skillVideoController;

	float angle;
	float baseAngle;//アイコン同士の角度
	float turnAngle = 0.0f;//アイコンを回す際の変数
	[SerializeField] [Tooltip("アイコンを回す速さ")] float baseTurnAngle = 10;
	float baseTurnAngleRadians;//上の角度の弧度表記
	[SerializeField] float slope;
	float slopeRadians;
	int turnCount;//未入手のスキップをカウント

	[SerializeField]
	float radius;   //中心からの距離


	[SerializeField]
	int countSkillBetweenMagics = 1;//魔法の間にスキルを何個入れ込むか


	//スキルを回す際に必要な変数
	bool foundSkillFlag;//入手済みスキルを発見したときtrue
	int nowCountSkill;//現在のループでどのスキルを見ているか
	int passiveNumber;//パッシブの番号
	public SelectingSkillInPause selectingSkillInPause;//現在ポーズ中でどのスキルを選択しているか
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

	//奥行によるスキルの各大きさを計算
	float maxZ;//最大のZ座標。ここをminSizeとする
	float minZ;//最小のZ座標。ここを1とする(手前のものほど大きくし、Zが小さいほど手前になるため)
	[SerializeField] float minSize;//一番小さいものをどれぐらいの大きさにするか
	float baseZ = 0;//定数。上の三つの値が決定されたときに決まる
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

		countSkills = magicList.GetMagicCount() * (countSkillBetweenMagics + 1);//スキルの数をカウント。魔法 x (間に入るスキル + 1)
		thisPos = this.GetComponent<RectTransform>();

		baseAngle = 2.0f * Mathf.PI / countSkills;//弧度法 "1/個数 * 2π"

		baseTurnAngleRadians = 2.0f * Mathf.PI * baseTurnAngle / 360.0f;
		pushingFlag = false;
		turnCount = 1;
		select = 0;
		//魔法初期化
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
		//パッシブ初期化
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
		if(!pause.pauseFlag || !pause.moveFlag)//ポーズ中以外はキー入力を受け付けない
		{
			return;
		}
		skillInfoInPause.SetInfoFlag = true;//ポーズ中ならば表示は続ける
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
		//passive版
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
		//↑↓入力管理
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

		//回す処理
		if(turnUpFlag)
		{
			turnAngle += baseTurnAngleRadians;//角度を大きく
			if(turnAngle >= baseAngle * (float) turnCount)//角度が次のスキルのところまで来たら
			{
				nowCountSkill = (select + turnCount) % countSkills;//魔法とスキル含め丸何個分回ったか
				foundSkillFlag = SearchUseFlag(nowCountSkill);//次のスキルが入手済みかどうか見る
				if(foundSkillFlag)//入手済みスキルが選択できたら
				{
					turnAngle = 0.0f;
					turnUpFlag = false;
					if(select + turnCount >= countSkills)//一番上にいるなら一番下に移動、それ以外なら上に移動
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
				nowCountSkill = (select + countSkills - turnCount) % countSkills;//魔法とスキル含め丸何個分回ったか
				foundSkillFlag = SearchUseFlag(nowCountSkill);
				if(foundSkillFlag)
				{
					turnAngle = 0.0f;
					turnDownFlag = false;
					if(select - turnCount < 0)//一番下にいるなら(略
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
		if(_nowCountSkill % (countSkillBetweenMagics + 1) == 0)//選択したものが魔法の場合
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
		else//パッシブの場合
		{
			//選択した場所が何番目のpassiveかを求める。選択した場所 - (選択した場所/3(切り捨て) + 1)//1番目に0のパッシブが入るため、1多めに減算
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
			if(i % (countSkillBetweenMagics + 1) == 0)//ここが真なら魔法選択
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
	public Vector3 AroundCirclePosInPause(float _r, float _angle)//r = 半径, angle = 角度(弧度法), slope = 傾き(弧度法)
	{
		float cosAngle = Mathf.Cos(_angle);
		float xPos = _r * cosAngle * Mathf.Sin(slopeRadians);
		float yPos = _r * Mathf.Sin(_angle);
		float zPos = _r * cosAngle * Mathf.Cos(slopeRadians);

		return new Vector3(xPos, yPos, zPos);
	}
	public Vector2 CircleSizeInPause(float _zPos, float _size)//円の大きさを変更する関数
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