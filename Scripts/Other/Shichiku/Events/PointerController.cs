using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    
    void Start()
    {
        
    }

    
    void Update()
	{

	}

	public IEnumerator FadeColorAlpha()
	{
        float decrement = 0.05f;
		Color spriteColor = GetComponent<SpriteRenderer>().color;
		while(spriteColor.a > 0)
		{
			decrement = 0.005f + (decrement * 0.8f);
			spriteColor.a -= decrement;
			GetComponent<SpriteRenderer>().color = spriteColor;
			yield return new WaitForSeconds(decrement);
		}
	}
}
