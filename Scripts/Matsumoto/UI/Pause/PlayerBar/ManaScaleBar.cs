using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaScaleBar : MonoBehaviour
{
	//魔法バーオブジェクト
	[SerializeField] GameObject magicBar;
	RectTransform RTmagicBar;
	//バーの幅
	float barWidth;
	ManaBar manaBar;
	//バーの原点。マナ0のときの場所
	Vector2 originPos;
	//現在のバーの場所。xのみ
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
		//magicBarの一番左の座標を求める。(真ん中(0) - (バーの幅 / 2), 真ん中(0))
		originPos = new Vector2(barWidth / 2.0f * -1, 0.5f);
	}
	public void ChangeScaleBarPos(float _maxValue, int _needMana)
	{
		//現在バー座標xを計算。 0の時pos + (幅 / 最大 * 必要マナ)
		currentPosX = originPos.x + (barWidth / _maxValue * _needMana);
		myRT.anchoredPosition = new Vector2(currentPosX, originPos.y);
	}
}
