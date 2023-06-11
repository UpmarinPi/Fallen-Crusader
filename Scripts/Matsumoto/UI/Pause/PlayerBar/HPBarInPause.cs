using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBarInPause : MonoBehaviour
{
	Slider slider; // put in inspector
	[SerializeField] Text displayText; // need to use regular text

	[SerializeField] GameObject Player;
	PlayerController playerController;

	int displayValue;
	int displayReValue;//’l‚ª•Ï‰»‚µ‚Ä‚¢‚È‚¢‚©Šm”F‚·‚é

	private float currentValue = 1.0f;
	public readonly float maxValue = 500f;
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
		playerController = Player.GetComponent<PlayerController>();
		CurrentValue = currentValue;
		displayReValue = int.MaxValue;
	}

	
	void Update()
	{
		displayValue = Mathf.RoundToInt(playerController.PlayerHP);
		if (displayValue != displayReValue)
		{
			displayReValue = displayValue;
			CurrentValue = displayValue / maxValue;
		}
	}
}
