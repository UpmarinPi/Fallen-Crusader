using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//This is my vibes
public class ManaBar : MonoBehaviour
{
	[SerializeField] Slider slider; // put in inspector
	[SerializeField] Text displayText; // need to use regular text
	[SerializeField] Image fill; // need to use regular text
	[SerializeField] Image[] scales; // need to use regular text
	[SerializeField] SelectMagicUI selectMagicUI;
	[SerializeField] SelectMagic selectMagic;//魔法選択中状態取得
	[SerializeField] ManaScaleBar manaScaleBar;//魔法必要マナバー移動用
	[SerializeField] EffectBar mpEffectBar;//"MP"のエフェクトバーにつけること

	[SerializeField] GameObject Player;
	MagicController magicController;

	Color onColor = new Color(0.322f, 0.706f, 0.984f, 1f);
	Color offColor = new Color(0.322f, 0.706f, 0.984f, 0.5f);
	private float currentValue = 0f;
	public float maxValue = 100f;
	public bool updateScaleBarFlag;

	int needMana;
	public float CurrentValue
	{
		get
		{
			return currentValue;
		}
		set
		{
			currentValue = value;
			slider.value = currentValue;
			mpEffectBar.SetChangeFlag(currentValue);
			//displayText.text = (slider.value * 100).ToString("0.00") + "%";
		} // this is the important part
	}


	void Start()
	{
		magicController = Player.GetComponent<MagicController>();
		CurrentValue = 0f;
		needMana = magicController.GetNeedMana(selectMagicUI.GetSelectMagic);
		updateScaleBarFlag = true;
	}


	
	void Update()
	{
		if (magicController.mana >= needMana)
		{
			fill.color = onColor;
		}
		else
		{
			fill.color = offColor;
		}

		if (selectMagic.selectingMagicFlag || updateScaleBarFlag)
		{
			needMana = magicController.GetNeedMana(selectMagicUI.GetSelectMagic);
			manaScaleBar.ChangeScaleBarPos(maxValue, needMana);
			updateScaleBarFlag = false;
		}

		int displayvalue = Mathf.RoundToInt(magicController.mana);
		CurrentValue = displayvalue / 100f;
	}

}
