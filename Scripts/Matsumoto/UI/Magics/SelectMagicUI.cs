using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*　追記
 * 魔法の選択UIです。グルグル回ります。
 * 円周上の点をsin,cosで取って、邪魔にならないよう中心点の移動も入れています
 * また手に入れていない魔法は飛ばすよう計算しています
 * ・半径
 * ・出し入れ速度
 * ・回る速度
 * ・出した時と入れた時、それぞれの中心点の場所
 * ・選択中以外の魔法の大きさ、色の変化
 * がインスペクター上で変更できます。
 */
[Serializable]
public class MagicUIList
{
	public List<Magics> magics = new List<Magics>();//魔法の種類と、魔法UIのオブジェクト
	public List<GameObject> magicUI = new List<GameObject>();

	//magicUI内にあるcomponent3つ。GetComponentをupdate内でしないよう呼び出しておく
	public List<RectTransform> magicUIRT = new List<RectTransform>();
	public List<Image> magicImage = new List<Image>();
	public List<Canvas> magicCanvas = new List<Canvas>();
}

public class SelectMagicUI : MonoBehaviour
{
	//なぜかSerializeFieldをつけないとエラーが出るためつけるだけつけて隠してる。要検証
	[HideInInspector, SerializeField]
	MagicUIList magicUIList;
	public static int select;//現在選択中の魔法
	bool pushingFlag;
	//上下に回すか判定
	bool turnUpFlag;
	bool turnDownFlag;

	//プレイヤー云々
	[SerializeField] Transform player;
	[SerializeField] InputManager inputManager;

	SelectMagic selectMagic;

	[HideInInspector]
	public int countMagics;//魔法の個数を取得
	[SerializeField] Pause pause;

	[SerializeField]
	private MagicList magicList;
	private Image iconImage;
	Sprite sprite;

	[SerializeField] EditData editData;
	PlayerDataClass data;

	float angle;
	float baseAngle;//アイコン同士の角度
	float turnAngle = 0.0f;//アイコンを回す際の変数
	[SerializeField] [Tooltip("アイコンを回す速さ")] int baseTurnAngle = 10;
	float baseTurnAngleRadians;//上の角度の弧度表記
	int turnCount;//未入手のスキップをカウント

	[SerializeField]
	GameObject movePlace;
	RectTransform thisPos;
	RectTransform showPos;
	Vector2 hideOriginalPos;
	Vector2 showOriginalPos;
	bool changePosFlag;

	RectTransform magicFrame;

	//今のところ使わない
	//Image tabImage;


	bool openFlag;//開けたときに一回だけ発動

	[SerializeField]
	public float r;//魔法UIの最大半径
	[SerializeField]
	float rSpeed;//半径が出現するときの速さ
	float radius;//実際にはこれが半径になる。(開いてるモーション途中など)
				 //半径が変更される際の最大との割合でpositionも変更している

	[SerializeField]
	float hideSize = 0.8f;

	[SerializeField]
	MagicInfoUI magicInfo;//魔法説明に使用するオブジェクト

	//入手していない魔法のアイコン
	[SerializeField] private Sprite missingIconImage;

	void Start()
	{
		selectMagic = player.GetComponent<SelectMagic>();
		data = editData.data;

		//player = GameObject.Find("Player").transform;

		countMagics = magicList.GetMagicCount();//魔法の数をカウント
		thisPos = this.GetComponent<RectTransform>();
		showPos = movePlace.GetComponent<RectTransform>();

		hideOriginalPos = thisPos.anchoredPosition;
		showOriginalPos = showPos.anchoredPosition;

		baseAngle = 2.0f * Mathf.PI / countMagics;//弧度法 "1/個数 * 2π"
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
			else//魔法の次のchildをフレームに登録、/*そのもう一つ先をtab文字のアイコンに登録*/
			{
				magicFrame = transform.GetChild(i).gameObject.GetComponent<RectTransform>();
				//tabImage = transform.GetChild(i + 1).gameObject.GetComponent<Image>();
			}
		}
	}


	void Update()
	{
		if(pause.pauseFlag)//ポーズ中はキー入力を受け付けない
		{
			return;
		}
		if(changePosFlag)//開く動作時の移動、回転
		{
			thisPos.anchoredPosition = new Vector2(Mathf.Lerp(hideOriginalPos.x, showOriginalPos.x, radius / r), Mathf.Lerp(hideOriginalPos.y, showOriginalPos.y, radius / r));
			magicFrame.rotation = Quaternion.Euler(0f, 0f, radius * -90.0f / r);
		}
		if(!selectMagic.selectingMagicFlag)
		{
			//閉じるときのアイコン表示処理
			CloseMagicUI(rSpeed);
			openFlag = false;
		}
		else
		{
			if(!openFlag)
			{
				//開き始めるときのアイコン表示処理
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
						if(select + turnCount >= countMagics)//一番上にいるなら一番下に移動、それ以外なら上に移動
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
						if(select - turnCount < 0)//一番下にいるなら(略
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
	//魔法選択をやめて魔法UIを閉じる動き
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
	//魔法選択を始めるときの動き
	void OpenMagicUI(float _openSpeed, float _radiusSize, ref float _radius)//_radiusが_radiusSizeになるまで_openSpeedを加算し続ける
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

	//アイコン表示処理
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

	//円周上の点を見つける
	public Vector2 AroundCirclePos(float r, float angle)//r = 半径, angle = 角度(弧度法)
	{
		float xPos = r * Mathf.Cos(angle);
		float yPos = r * Mathf.Sin(angle);

		return new Vector2(xPos, yPos);
	}

	//アイコンの回し途中ならtrue,回ってないならfalse 
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
