using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Opening : MonoBehaviour
{
	/*
     * Originalつきは元々あった位置。色々動い理変更したあと最終的にそこにたどり着くためのもの
     */

	[SerializeField]
	GameObject titleBack;

	[HeaderAttribute("\"Crusader\"はfallenの子オブジェクトに設定すること")]
	[SerializeField]
	GameObject fallen;//"Crusader"はfallenの子オブジェクトに設定すること
	Image fallenImage;
	Sprite fallenOriginalSprite;
	[SerializeField]//白色のfallen
	Sprite whiteFallen;
	RectTransform fallenPos;
	Vector2 fallenOriginalPos;

	GameObject crusader;
	Image crusaderImage;
	Sprite crusaderOriginalSprite;
	[SerializeField]//白色のcrusader
	Sprite whiteCrusader;

	[SpaceAttribute]
	[SerializeField]
	GameObject fallenSword;
	RectTransform swordPos;
	[SerializeField]
	float swordX;
	[SerializeField]
	float swordSpeed;

	//剣の動きに参考するいろいろ
	[SerializeField]
	GameObject swordTyoten;
	Vector2 tyoten;
	float a;

	float swordScale;
	bool increaseSwordScaleFlag;
	[SerializeField]
	float turningSwordSpeed;
	[SerializeField]
	float turningSwordSlerp = 0.5f;

	[SpaceAttribute]
	[SerializeField]
	GameObject Continue;
	RectTransform ContinuePos;
	Vector2 ContinueOriginalPos;
	Vector2 ContinueKariPos;

	[SerializeField]
	GameObject NewGame;
	RectTransform NewGamePos;
	Vector2 NewGameOriginalPos;
	Vector2 NewGameKariPos;

	[SerializeField]
	GameObject Exit;
	RectTransform ExitPos;
	Vector2 ExitOriginalPos;
	Vector2 ExitKariPos;

	[SpaceAttribute]
	[SerializeField]
	float delayMenuTime;
	float totalDelay;
	[SerializeField]
	float menuSpeed;
	[SerializeField]
	float hideMenuX = 1500;
	float continueSlerp;
	float newGameSlerp;
	float exitSlerp;

	[SpaceAttribute]
	[SerializeField]
	GameObject particle;

	[HideInInspector]
	public bool canMove;

	[SerializeField]
	float startDelay;
	float countStartDelay;

	bool startFlag;
	bool swordMoveFlag;
	bool normalScreenFlag;
	bool finishSlideMenuFlag;

	private void Start()
	{
		canMove = false;

		//fallen crusaderロゴの色々読み込み
		//crusader読み込み
		crusader = fallen.transform.GetChild(0).gameObject;

		//fallen crusader元画像保存
		fallenImage = fallen.GetComponent<Image>();
		fallenOriginalSprite = fallenImage.sprite;
		crusaderImage = crusader.GetComponent<Image>();
		crusaderOriginalSprite = crusaderImage.sprite;

		//fallenの場所読み込み
		fallenPos = fallen.GetComponent<RectTransform>();
		fallenOriginalPos = fallenPos.anchoredPosition;

		//fallenCrusaderのロゴを変更
		fallenImage.sprite = whiteFallen;
		crusaderImage.sprite = whiteCrusader;

		//swordの設定いじり
		swordPos = fallenSword.GetComponent<RectTransform>();

		tyoten = swordTyoten.GetComponent<RectTransform>().anchoredPosition;
		a = (fallenOriginalPos.y - tyoten.y) / Mathf.Pow(fallenOriginalPos.x - tyoten.x, 2);

		swordScale = 1.0f;

		swordPos.anchoredPosition = NijiFunctionMove(swordX, a, tyoten);

		//選択の色々読み込み
		//ポジションいじり用
		ContinuePos = Continue.transform.GetComponent<RectTransform>();
		NewGamePos = NewGame.transform.GetComponent<RectTransform>();
		ExitPos = Exit.transform.GetComponent<RectTransform>();

		//それぞれの場所を保存
		ContinueOriginalPos = ContinuePos.anchoredPosition;
		NewGameOriginalPos = NewGamePos.anchoredPosition;
		ExitOriginalPos = ExitPos.anchoredPosition;

		ContinueKariPos = new Vector2(hideMenuX, ContinueOriginalPos.y);
		NewGameKariPos = new Vector2(hideMenuX, NewGameOriginalPos.y);
		ExitKariPos = new Vector2(hideMenuX, ExitOriginalPos.y);

		//演出のため捌ける
		ContinuePos.anchoredPosition = ContinueKariPos;
		NewGamePos.anchoredPosition = NewGameKariPos;
		ExitPos.anchoredPosition = ExitKariPos;

		//背景・パーティクル不可視化
		titleBack.SetActive(false);
		particle.SetActive(false);

		countStartDelay = 0.0f;

		//bool初期化
		swordMoveFlag = false;
		normalScreenFlag = false;
		finishSlideMenuFlag = false;
		startFlag = true;
	}


	private void Update()
	{
		if (startFlag)
		{
			countStartDelay += Time.fixedDeltaTime;
			if(countStartDelay >= startDelay)
			{
				startFlag = false;
				swordMoveFlag = true;
			}
		}
		if (normalScreenFlag)
		{

			totalDelay += Time.fixedDeltaTime;

			if (totalDelay >= delayMenuTime)
				continueSlerp += menuSpeed;
			ContinueKariPos = MoveMenu(hideMenuX, ContinueOriginalPos, continueSlerp);
			if (totalDelay >= delayMenuTime * 2)
			{
				newGameSlerp += menuSpeed;
				NewGameKariPos = MoveMenu(hideMenuX, NewGameOriginalPos, newGameSlerp);
			}
			if (totalDelay >= delayMenuTime * 3)
			{
				exitSlerp += menuSpeed;
				ExitKariPos = MoveMenu(hideMenuX, ExitOriginalPos, exitSlerp);
			}

			if (exitSlerp >= 1.0f && !finishSlideMenuFlag)
			{
				this.GetComponent<TitleStart>().finishOpening = true;
				finishSlideMenuFlag = true;
			}
		}
	}

	private void FixedUpdate()
	{
		if (swordMoveFlag)
		{
			swordX -= swordSpeed;
			swordPos.anchoredPosition = NijiFunctionMove(swordX, a, tyoten);

			if (increaseSwordScaleFlag)
			{
				swordScale += turningSwordSpeed;
			}
			else
			{
				swordScale -= turningSwordSpeed;
			}
			if (swordScale >= 1.0f)
			{
				increaseSwordScaleFlag = false;
				swordScale = 1.0f;
			}
			else if (swordScale <= 0.0f)
			{
				increaseSwordScaleFlag = true;
				swordScale = 0.0f;
			}

			swordPos.localScale = new Vector2(Vector3.Slerp(new Vector3(0, 0), new Vector3(turningSwordSlerp, 0), swordScale).x / turningSwordSlerp, 1);

			if (swordPos.anchoredPosition.x <= fallenOriginalPos.x)
			{
				swordPos.anchoredPosition = new Vector2(fallenOriginalPos.x, fallenOriginalPos.y);
				titleBack.SetActive(true);
				particle.SetActive(true);
				fallenSword.SetActive(false);
				fallenImage.sprite = fallenOriginalSprite;
				crusaderImage.sprite = crusaderOriginalSprite;
				normalScreenFlag = true;
				swordMoveFlag = false;
			}
		}
		if (normalScreenFlag)
		{
			ContinuePos.anchoredPosition = ContinueKariPos;
			NewGamePos.anchoredPosition = NewGameKariPos;
			ExitPos.anchoredPosition = ExitKariPos;
		}
	}

	Vector2 NijiFunctionMove(float x, float a, Vector2 pq)
	{
		return new Vector2(x, a * Mathf.Pow(x - pq.x, 2) + pq.y);
	}

	Vector2 MoveMenu(float firstPosX, Vector2 originalPos, float slerp)
	{
		return new Vector2(Vector3.Slerp(new Vector2(firstPosX, originalPos.y), new Vector2(originalPos.x, originalPos.y), slerp).x, originalPos.y);
	}
}
