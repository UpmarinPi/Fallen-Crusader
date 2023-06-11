using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverButtonUI : MonoBehaviour
{
	[SerializeField]
	GameObject GOGameOverUI;

	GameOverUI gameOverUI;

	Image image;

	[SerializeField]
	GameOverUI.GameOverChoice choiceButton;
	public Image flush;
	bool fadeOutedFlag;//ここのfadeOutedFlagはやり直し選択した場合のみtrueにすること
	[SerializeField]
	private bool doFlag;
	public bool SetPower//selectUI
	{
		set { doFlag = value; }
	}


	void Start()
	{
		doFlag = false;
		image = this.GetComponent<Image>();
		GetComponent<Image>().color = Color.white;
		gameOverUI = GOGameOverUI.GetComponent<GameOverUI>();
		fadeOutedFlag = false;
	}

	
	void Update()
	{
		if (doFlag)
		{
			switch (gameOverUI.select)
			{
				case GameOverUI.GameOverChoice.BackToCheckPoint:
					BTCP();
					doFlag = false;
					break;

				case GameOverUI.GameOverChoice.Quit:
					Quit();
					doFlag = false;
					break;
			}
		}
		if (fadeOutedFlag)//ここのfadeOutedFlagはやり直し選択した場合のみtrueにすること
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);//現在のシーンを読み込みなおす
		}
	}

	private void FixedUpdate()
	{
		if(gameOverUI.select == choiceButton)
		{
			GetComponent<Image>().color = Color.white;
		}
		else
		{
			GetComponent<Image>().color = Color.gray;
		}
	}

	void BTCP()//back to check point, 最後に来たチェックポイントへ移動
	{

		StartCoroutine(FadeOut());

	}

	void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();
#endif
	}

	IEnumerator FadeOut()
	{
		Color flushColor = flush.color;
		flushColor = Color.black;
		for (float i = 0; i <= 1; i += 0.01f)
		{
			flushColor.a = Mathf.Lerp(0f, 1f, i);
			flush.color = flushColor;
			yield return new WaitForSeconds(0.02f);
		}
		fadeOutedFlag = true;
	}

}
