using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{
	float blackColor;
	[Range(0, 1)]
	public float blackSpeed = 0.1f;

	float maxBlack = 1;
	public bool goingBlackFlag;
	[SerializeField]
	Color firstColor;

	[SerializeField]
	GameObject gameOverUI;

	[SerializeField]
	[HeaderAttribute("赤色からこの色に変わって、操作可能")]
	Color discoloration;

	
	void Start()
    {
		GetComponent<Image>().color = firstColor;
		goingBlackFlag = false;
    }

	private void FixedUpdate()
	{
		if (goingBlackFlag)
		{
			if (blackColor + blackSpeed >= maxBlack)
			{
				blackColor = maxBlack;
				gameOverUI.GetComponent<GameOverUI>().canMoveFlag = true;//動作可能
			}
			else
			{
				blackColor += blackSpeed;
			}
			//r,g,bそれぞれに頑張ってフェードみたいに入れている
			GetComponent<Image>().color = new Color(Mathf.Lerp(firstColor.r, discoloration.r, blackColor),Mathf.Lerp(firstColor.g, discoloration.g, blackColor),Mathf.Lerp(firstColor.b, discoloration.b, blackColor));
		}
	}
}
