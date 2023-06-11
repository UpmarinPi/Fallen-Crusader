using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSound : MonoBehaviour
{
    bool prevFlag;
    [SerializeField] Pause pause;
    [SerializeField] BGMController BGM;

    Coroutine coroutine = null;
    
    void Start()
    {
        prevFlag = pause.pauseFlag;
    }

    
    void Update()
    {
        if (pause.pauseFlag != prevFlag)
		{
            prevFlag = pause.pauseFlag;
            if(pause.pauseFlag)
			{
                BGM.SetVolume(0.3f);
            }
            else
			{
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                }
                coroutine = StartCoroutine(BGM.FadeInPlay(0.3f));
            }
		}
    }
}
