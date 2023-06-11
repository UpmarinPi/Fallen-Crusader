using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitHPBar : MonoBehaviour
{
    [SerializeField] Slider slider; // put in inspector

    GameObject Fruit;
    JungleFruit jungleFruit;

    private float currentValue = 100f;
    float maxValue;
    bool onceFlag = false;
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
        } 
    }


    void Start()
    {
        Fruit = transform.parent.gameObject;
        jungleFruit = Fruit.GetComponent<JungleFruit>();
        currentValue = jungleFruit.fruitGetHP();
        maxValue = jungleFruit.fruitGetHP();
        CurrentValue = currentValue;
    }

    // display fruit's HP
    // ���ݖ؂̎���HP��\��
    void Update()
    {
        float displayvalue;
        displayvalue = jungleFruit.fruitGetHP();
        CurrentValue = displayvalue / maxValue;

        if (jungleFruit.fruitGetHP() <= 0 && !onceFlag)
		{
            StartCoroutine(Death());
            onceFlag = true;
		}

    }

    // slowly fade HP bar away
    // �������HP�o�[�������Ă���
    IEnumerator Death()
    {
        ColorBlock colorBlock;
        colorBlock = slider.colors;

        while (colorBlock.disabledColor.a != 0)
		{
            colorBlock.disabledColor = new Color(0f, 0f, 0f, colorBlock.disabledColor.a - 0.05f);
            slider.colors = colorBlock;
            yield return new WaitForSeconds(0.05f);
        }
    }
}
