using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossShieldBar : MonoBehaviour
{
    [SerializeField] Slider slider; // put in inspector
    [SerializeField] Text displayText; // need to use regular text

    [SerializeField] GameObject valueManager;
    IBoss bossController;
    const float maxValue = 100f;
    private float currentValue = 100f;
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
        bossController = valueManager.GetComponent<IBoss>();
        CurrentValue = bossController.BossShield;
    }

    
    void Update()
    {
        int displayvalue;
        displayvalue = Mathf.RoundToInt(bossController.BossShield);
        CurrentValue = displayvalue / maxValue;
    }
}
