using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlushController : MonoBehaviour
{
	Image flush;
	Color flushColor;
	private void Awake()
	{
		flush = GetComponent<Image>();
		flushColor = flush.color;
	}
	public IEnumerator FadeOut(float fadeTime = 0.01f)
	{
		for(float i = 0; i <= 1; i += 0.01f)
		{
			flushColor.a = Mathf.Lerp(0f, 1f, i);
			flush.color = flushColor;
			yield return new WaitForSeconds(fadeTime);
		}
	}
	public IEnumerator FadeIn(float fadeTime = 0.01f)
	{
		for(float i = 1; i >= 0; i -= 0.01f)
		{
			flushColor.a = Mathf.Lerp(0f, 1f, i);
			flush.color = flushColor;
			yield return new WaitForSeconds(fadeTime);
		}
	}

	public void ChangeColor(Color imageColor)
	{
		flushColor = new Color(imageColor.r, imageColor.g, imageColor.b);
	}

	public void FlushColor(Color imageColor)
    {
		flushColor = new Color(imageColor.r, imageColor.g, imageColor.b);
		flush.color = imageColor;
    }

}
