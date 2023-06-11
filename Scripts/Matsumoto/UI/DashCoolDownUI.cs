using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DashCoolDownUI : MonoBehaviour
{
	[SerializeField]
	PlayerAfterDash playerAfterDash;

	float coolDown;
	float alpha;
	[SerializeField]
	float alphaSpeed;
	[SerializeField]
	Pause pause;
	Material dashCoolDownShader;


	CanvasGroup thisCG;//自身のキャンバスグループ。アルファいじり用

	GameObject colorBar;//色つきバー。child(0)に指定
	GameObject backBar;//背景バー。child(1)に指定

	//各々の色
	[SerializeField] Color colorOfColorBar;
	[SerializeField] Color colorOfBackBar;

	private void Start()
	{
		//
		colorBar = transform.GetChild(4).gameObject;
		dashCoolDownShader = colorBar.GetComponent<Renderer>().material;
		dashCoolDownShader.SetColor("_Color", colorOfColorBar);

		//backBar = transform.GetChild(1).gameObject;
		//backBar.GetComponent<SpriteRenderer>().color = colorOfBackBar;

		/*thisCG = this.GetComponent<CanvasGroup>();
		alpha = 0.0f;
		thisCG.alpha = alpha;*/
	}
	void Update()
	{
		if (pause.pauseFlag)
			return;

		coolDown = playerAfterDash.TimeCoolDown;
		//Debug.Log("DashCoolDownUI: " + coolDown);
		if (coolDown >= 1.0f)
		{
			if (alpha - alphaSpeed > 0.0f)

				alpha -= alphaSpeed;
			else
				alpha = 0.0f;
		}
		else
		{
			alpha = 1.0f;
		}
	}
	private void FixedUpdate()
	{
		//thisCG.alpha = alpha;
		dashCoolDownShader.SetFloat("_Ratio", coolDown);
	}
}
