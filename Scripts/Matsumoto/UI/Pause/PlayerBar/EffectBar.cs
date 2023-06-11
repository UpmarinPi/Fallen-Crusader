using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectBar : MonoBehaviour
{
	Slider slider;
	[SerializeField] HPBar hpBar;
	[SerializeField] ManaBar manaBar;

	[Header("チェックでhpBar、チェックなしでmanaBar選択とみる")]
	[SerializeField] bool hpBarFlag;

	[SerializeField] int effectStart;   //減少し始めるまでの時間
	[SerializeField] float effectSpeed; //減少量

	int effectStartTimer;   //effectStart計測用タイマー

	float currentValue;     //現在のHP
	float effectValue;      //徐々に減少するHPの値。この値がバーに参照される
	float maxValue;

	bool changeFlag;
	public void SetChangeFlag(float _changeValue)
	{

		changeFlag = true;
		currentValue = _changeValue * maxValue;
	}


	void Start()
	{
		slider = this.GetComponent<Slider>();
		if (hpBarFlag)
		{
			currentValue = hpBar.CurrentValue;
			maxValue = hpBar.maxValue;
		}
		else
		{
			currentValue = manaBar.CurrentValue;
			maxValue = manaBar.maxValue;
		}
		effectValue = currentValue;
		effectStartTimer = 0;
	}

	private void FixedUpdate()
	{
		if (changeFlag)
		{
			//Debug.Log(effectValue);
			if (currentValue >= effectValue)//HPが回復した時もしくは追いついた時の処理
			{
				effectValue = currentValue;//エフェクトを現在に合わせる
				effectStartTimer = 0;
				changeFlag = false;
			}
			else if (effectStartTimer < effectStart)
			{
				effectStartTimer++;
			}
			else if (currentValue < effectValue)
			{
				effectValue -= effectSpeed;
			}
			slider.value = effectValue / maxValue;
		}
	}
}
