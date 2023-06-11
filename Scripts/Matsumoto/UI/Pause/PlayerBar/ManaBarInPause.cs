using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//This is my vibes
public class ManaBarInPause : MonoBehaviour
{
	Slider slider; // put in inspector
	[SerializeField] Text displayText; // need to use regular text
	[SerializeField] Image fill; // need to use regular text
	//[SerializeField] Image[] scales; // need to use regular text
	[SerializeField] SelectMagicUI selectMagicUI;
	SelectMagic selectMagic;//ñÇñ@ëIëíÜèÛë‘éÊìæ

	[SerializeField] GameObject player;
	MagicController magicController;

	Color onColor = new Color(0.322f, 0.706f, 0.984f, 1f);
	Color offColor = new Color(0.322f, 0.706f, 0.984f, 0.5f);
	private float currentValue = 0f;
	public float maxValue = 100f;

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
			displayText.text = (slider.value * maxValue).ToString("0") + "/" + maxValue;
			//displayText.text = (slider.value * 100).ToString("0.00") + "%";
		} // this is the important part
	}


	void Start()
	{
		slider = GetComponent<Slider>();
		magicController = player.GetComponent<MagicController>();
		selectMagic = player.GetComponent<SelectMagic>();
		CurrentValue = 0f;
		needMana = magicController.GetNeedMana(selectMagicUI.GetSelectMagic);
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

		if (selectMagic.selectingMagicFlag)
		{
			needMana = magicController.GetNeedMana(selectMagicUI.GetSelectMagic);
		}

		int displayvalue = Mathf.RoundToInt(magicController.mana);
		CurrentValue = displayvalue / 100f;
	}

}
