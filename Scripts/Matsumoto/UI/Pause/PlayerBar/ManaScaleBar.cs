using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaScaleBar : MonoBehaviour
{
	//���@�o�[�I�u�W�F�N�g
	[SerializeField] GameObject magicBar;
	RectTransform RTmagicBar;
	//�o�[�̕�
	float barWidth;
	ManaBar manaBar;
	//�o�[�̌��_�B�}�i0�̂Ƃ��̏ꏊ
	Vector2 originPos;
	//���݂̃o�[�̏ꏊ�Bx�̂�
	float currentPosX;

	RectTransform myRT;

	private void Awake()
	{
	}
	private void Start()
	{
		myRT = this.gameObject.GetComponent<RectTransform>();
		RTmagicBar = magicBar.GetComponent<RectTransform>();
		manaBar = magicBar.GetComponent<ManaBar>();
		barWidth = RTmagicBar.sizeDelta.x;
		//magicBar�̈�ԍ��̍��W�����߂�B(�^��(0) - (�o�[�̕� / 2), �^��(0))
		originPos = new Vector2(barWidth / 2.0f * -1, 0.5f);
	}
	public void ChangeScaleBarPos(float _maxValue, int _needMana)
	{
		//���݃o�[���Wx���v�Z�B 0�̎�pos + (�� / �ő� * �K�v�}�i)
		currentPosX = originPos.x + (barWidth / _maxValue * _needMana);
		myRT.anchoredPosition = new Vector2(currentPosX, originPos.y);
	}
}
