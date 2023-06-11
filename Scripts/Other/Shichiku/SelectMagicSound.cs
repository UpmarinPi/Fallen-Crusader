using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMagicSound : MonoBehaviour
{
    bool prevFlag;
    [SerializeField] SelectMagic select;
    [SerializeField] BGMController BGM;
    Coroutine coroutine = null;
    
    void Start()
    {
        prevFlag = select.selectingMagicFlag;
    }

    
    void Update()
    {
        if (select.selectingMagicFlag != prevFlag)
        {
            prevFlag = select.selectingMagicFlag;
            if (select.selectingMagicFlag)
            {
                if(coroutine != null)
				{
                    StopCoroutine(coroutine);
				}
                BGM.SetVolume(0.3f);
            }
            else
            {
                coroutine = StartCoroutine(BGM.FadeInPlay(0.3f));
            }
        }
    }
}
