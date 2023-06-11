using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleStart : MonoBehaviour
{
	public enum titleCursor {Continue, NewGame, Exit };

	[SerializeField]
	FadeToDemo fadeToDemo;

	bool startFlag;//選んだかのフラグ
	bool quitFlag;
	bool newGameFlag;
	bool alreadyStartFlag;//コルーチンを一回しか実行させないためのフラグ
	bool pushingFlag;//一押し一動作
	bool fadeOutedFlag;//フェードアウト完了したか
	[Header("フラッシュ")]
	public Image flush;
	public string startScene;//次のシーン
	public titleCursor cursor;//現在どこを選択しているか
	bool selectFlag;

	[HideInInspector]
	public bool finishOpening;

	[SerializeField] SEController SE;

	private void Awake()
	{
		Application.targetFrameRate = 60;
	}
	void Start()
	{
		startFlag = false;
		quitFlag = false;
		alreadyStartFlag = false;
		fadeOutedFlag = false;
		finishOpening = false;
		selectFlag = false;
	}

	void Update()
	{
        if (!finishOpening || selectFlag　|| fadeToDemo.timeFlag)
        {
			return;
        }
		if (Input.GetKey(KeyCode.UpArrow))
		{
			if (cursor > (titleCursor)0 && !pushingFlag)
			{
				SE.SystemSE(0);
				cursor--;
				pushingFlag = true;
				Debug.Log(cursor + "に移動！");
			}
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			if (cursor < titleCursor.Exit && !pushingFlag)
			{
				SE.SystemSE(0);
				cursor++;
				pushingFlag = true;
				Debug.Log(cursor + "に移動！");
			}
		}
		if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))//矢印が押されてないなら
		{
			pushingFlag = false;
		}
		if (Input.GetKey(KeyCode.Return))
		{
			SE.SystemSE(1);
			selectFlag = true;
			switch (cursor)
			{
				case titleCursor.NewGame:
					newGameFlag = true;
					break;
				case titleCursor.Continue://ゲーム開始
					startFlag = true;
					break;
				case titleCursor.Exit:
					quitFlag = true;
					break;
			}
		}
	}

	private void FixedUpdate()
	{
		if (newGameFlag)
		{
			File.Delete(Application.dataPath + "/saveData.json");
			startFlag = true;
		}

		if (startFlag && !alreadyStartFlag)
		{
			alreadyStartFlag = true;
			StartCoroutine(FadeOut());
		}
		if (fadeOutedFlag)
		{
			SceneManager.LoadScene(startScene);
		}

		if (quitFlag)
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();
#endif
		}
	}
	IEnumerator FadeOut()
	{
		Color flushColor = flush.color;
		flushColor = Color.black;
		for (float i = 0; i <= 100; i += 1)
		{
			flushColor.a = Mathf.Lerp(0f, 1f, (float)i / 100);
			flush.color = flushColor;
			yield return new WaitForFixedUpdate();
		}
		fadeOutedFlag = true;
	}
}
