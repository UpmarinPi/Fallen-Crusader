using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Opening : MonoBehaviour
{
	/*
     * Original���͌��X�������ʒu�B�F�X�������ύX�������ƍŏI�I�ɂ����ɂ��ǂ蒅�����߂̂���
     */

	[SerializeField]
	GameObject titleBack;

	[HeaderAttribute("\"Crusader\"��fallen�̎q�I�u�W�F�N�g�ɐݒ肷�邱��")]
	[SerializeField]
	GameObject fallen;//"Crusader"��fallen�̎q�I�u�W�F�N�g�ɐݒ肷�邱��
	Image fallenImage;
	Sprite fallenOriginalSprite;
	[SerializeField]//���F��fallen
	Sprite whiteFallen;
	RectTransform fallenPos;
	Vector2 fallenOriginalPos;

	GameObject crusader;
	Image crusaderImage;
	Sprite crusaderOriginalSprite;
	[SerializeField]//���F��crusader
	Sprite whiteCrusader;

	[SpaceAttribute]
	[SerializeField]
	GameObject fallenSword;
	RectTransform swordPos;
	[SerializeField]
	float swordX;
	[SerializeField]
	float swordSpeed;

	//���̓����ɎQ�l���邢�낢��
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

		//fallen crusader���S�̐F�X�ǂݍ���
		//crusader�ǂݍ���
		crusader = fallen.transform.GetChild(0).gameObject;

		//fallen crusader���摜�ۑ�
		fallenImage = fallen.GetComponent<Image>();
		fallenOriginalSprite = fallenImage.sprite;
		crusaderImage = crusader.GetComponent<Image>();
		crusaderOriginalSprite = crusaderImage.sprite;

		//fallen�̏ꏊ�ǂݍ���
		fallenPos = fallen.GetComponent<RectTransform>();
		fallenOriginalPos = fallenPos.anchoredPosition;

		//fallenCrusader�̃��S��ύX
		fallenImage.sprite = whiteFallen;
		crusaderImage.sprite = whiteCrusader;

		//sword�̐ݒ肢����
		swordPos = fallenSword.GetComponent<RectTransform>();

		tyoten = swordTyoten.GetComponent<RectTransform>().anchoredPosition;
		a = (fallenOriginalPos.y - tyoten.y) / Mathf.Pow(fallenOriginalPos.x - tyoten.x, 2);

		swordScale = 1.0f;

		swordPos.anchoredPosition = NijiFunctionMove(swordX, a, tyoten);

		//�I���̐F�X�ǂݍ���
		//�|�W�V����������p
		ContinuePos = Continue.transform.GetComponent<RectTransform>();
		NewGamePos = NewGame.transform.GetComponent<RectTransform>();
		ExitPos = Exit.transform.GetComponent<RectTransform>();

		//���ꂼ��̏ꏊ��ۑ�
		ContinueOriginalPos = ContinuePos.anchoredPosition;
		NewGameOriginalPos = NewGamePos.anchoredPosition;
		ExitOriginalPos = ExitPos.anchoredPosition;

		ContinueKariPos = new Vector2(hideMenuX, ContinueOriginalPos.y);
		NewGameKariPos = new Vector2(hideMenuX, NewGameOriginalPos.y);
		ExitKariPos = new Vector2(hideMenuX, ExitOriginalPos.y);

		//���o�̂��ߎJ����
		ContinuePos.anchoredPosition = ContinueKariPos;
		NewGamePos.anchoredPosition = NewGameKariPos;
		ExitPos.anchoredPosition = ExitKariPos;

		//�w�i�E�p�[�e�B�N���s����
		titleBack.SetActive(false);
		particle.SetActive(false);

		countStartDelay = 0.0f;

		//bool������
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
