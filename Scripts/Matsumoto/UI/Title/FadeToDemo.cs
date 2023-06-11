using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeToDemo : MonoBehaviour
{
	VideoPlayer videoPlayer;

	[SerializeField]
	Image flush;

	[SerializeField]
	GameObject particle;

	[SerializeField]
	GameObject back;//背景も消して無理やりフェードアウトさせる

	[Header("動画開始まで何秒待つか。1sec. = 60")]
	[SerializeField]
	int startDemoTime;

	//タイマー
	int timer;

	//boolいろいろ
	[HideInInspector]
	public bool timeFlag;              //タイマー処理。タイマーが待ち時間超えたら止めるため
	bool fadeOutedFlag;         //フェードアウトしたかの確認。動画を開始する
	bool stopFadeFlag;          //フェード停止処理。フェードアウト中にボタンを押したとき
	bool startFadeFlag;         //フェード開始処理。timeFlagが超えたとき一回だけstartを発動
	bool startDemoFlag;
	bool reloadSceneFlag;
	void Start()
	{
		//初期化
		videoPlayer = this.GetComponent<VideoPlayer>();
		timer = 0;

		//動画終了検知
		videoPlayer.loopPointReached += EndVideo;

		//bool処理
		startFadeFlag = false;
		fadeOutedFlag = false;
		stopFadeFlag = false;
		startDemoFlag = false;
	}

	void Update()
	{
		if (timer >= startDemoTime && !timeFlag)
		{
			timeFlag = true;
			startFadeFlag = true;

		}
		if (stopFadeFlag)//フェード中にボタンが押されたら
		{
			stopFadeFlag = false;//デモビデオを流さずリセット
			fadeOutedFlag = false;
			timeFlag = false;
			timer = 0;
		}
	}
	private void FixedUpdate()
	{
		if (!timeFlag)//時間を増やす処理
		{
			if (Input.anyKey)
			{
				timer = 0;
			}
			else
			{
				timer++;
			}
		}
		if (startFadeFlag && timeFlag)//フェード開始時。フェード開始よりtimeFlagはtrue継続
		{
			startFadeFlag = false;
			StartCoroutine(FadeOut());
		}
		if (fadeOutedFlag && !stopFadeFlag)//フェードアウト終わって停止処理がかかってない場合動画を流す
		{
			startDemoFlag = true;
			fadeOutedFlag = false;
			particle.SetActive(false);//パーティクルも消す
			videoPlayer.Play();
		}
		if (startDemoFlag)
		{
			if (Input.anyKey)
			{
				back.SetActive(false);
				startDemoFlag = false;
				StartCoroutine(ReloadFade());
			}
		}
		if (reloadSceneFlag)//シーンのリロード要請。動画終わりか"動画中にボタンを押したとき"←未実装
		{
			Debug.Log("reload!");
			SceneManager.LoadScene(0);
		}
	}

	public void EndVideo(VideoPlayer vp)
	{
		videoPlayer.Pause();
		back.SetActive(false);
		startDemoFlag = false;
		StartCoroutine(ReloadFade());
	}

	IEnumerator FadeOut()
	{
		Color flushColor = flush.color;
		flushColor = Color.black;
		for (int i = 0; i <= 100; i += 1)
		{
			flushColor.a = Mathf.Lerp(0f, 1f, (float)i / 100);
			flush.color = flushColor;
			if (Input.anyKey)
			{
				stopFadeFlag = true;
				flush.color = new Color(0.0f, 0.0f, 0.0f, 0.0f);
				yield break;
			}
			yield return new WaitForFixedUpdate();
		}
		fadeOutedFlag = true;
	}
	IEnumerator ReloadFade()
	{
		for (int i = 100; i >= 0; i -= 1)
		{
			videoPlayer.targetCameraAlpha = (float)i / 100;
			yield return new WaitForFixedUpdate();
		}
		reloadSceneFlag = true;
	}
}
