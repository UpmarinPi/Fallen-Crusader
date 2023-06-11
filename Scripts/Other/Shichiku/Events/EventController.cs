using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;


// メッセージテキストのクラス
[System.Serializable]
public class MESSAGE
{
	public enum Fonts
    {
		Devil = 0,
		Human = 1,
		System = 2
    };
	public string eventName;
	[Header("テキスト(最後は必ず改行)")]
	[TextArea(3, 10)]
	public string[] messageText;
	[Header("フォント")]
	public Fonts[] messageFont;

	public enum Talker
	{
		Contractor = 0,
		Loss,
		Zepar,
		Clario,
		Agares,
		Nothing = 100,
		Mystery,
		LossIdle = 200,
		LossTalk,
		LossThink,
		LossSurprise,
		LossLaugh,
		LossEvil,
		LossAnger
	}
	[Header("話している人物")]
	public Talker[] talker_name;

	public string TalkerSwitch(Talker talker)
	{
		switch (talker)
		{
			case Talker.Contractor: return "フォーセル";
			case Talker.Loss: return "ロ  ス";
			case Talker.Zepar: return "ゼ  パ  ル";
			case Talker.Clario: return "クラリオ";
			case Talker.Nothing: return "";
			case Talker.Mystery: return "? ? ?";
			default: return "";
		}
	}

	public (Talker talker, string state) TalkerSwitchCamp(Talker talker)
    {
        switch (talker)
        {
			case Talker.Contractor: return (Talker.Contractor, "IdleState");
			case Talker.Loss: return (Talker.Loss, "");
			case Talker.LossIdle: return (Talker.Loss, "IdleState");
			case Talker.LossTalk: return (Talker.Loss, "TalkState");
			case Talker.LossThink: return (Talker.Loss, "ThinkingState");
			case Talker.LossSurprise: return (Talker.Loss, "SurpriseState");
			case Talker.LossLaugh: return (Talker.Loss, "LaughState");
			case Talker.LossEvil: return (Talker.Loss, "EvilState");
			case Talker.LossAnger: return (Talker.Loss, "AngerState");
			default: return (Talker.Mystery, "");
		}
    }

	public enum Command
    {
		[InspectorName("PlayerGO")]
		PlayerGo = 0,
		[InspectorName("PlayerAttack")]
		PlayerAttack,
		[InspectorName("ClarioGO")]
		ClarioGo = 10,
		[InspectorName("ClarioWarpOn")]
		ClarioWarpOn,
		[InspectorName("ClarioWarpOff")]
		ClarioWarpOff,
		[InspectorName("BlackOut")]
		BlackOut = 100,
		[InspectorName("CloseUP")]
		CloseUp
    }
}

[System.Serializable]
public class POINTER
{
	[Header("ポインターの座標(z = オイラー角度)")]
	public Vector3 pointerPos;
	[Header("チュートリアル番号")]
	public int pointerNumber;
}

public class EventController : MonoBehaviour
{
	[SerializeField] GameObject grobalVolume;
	[SerializeField] VolumeProfile volumeProfile;

	[SerializeField]PlayerController playerController;

	[SerializeField]
	public BGMController BGM;
	[SerializeField]
	public SEController SE;

	[Header("解読文字")]
	[SerializeField] string[] decordings = new string[System.Enum.GetValues(typeof(KeyBindings.KeyBindNumber)).Length];
	string[] keyString = new string[System.Enum.GetValues(typeof(KeyBindings.KeyBindNumber)).Length];

	[Header("トークフォント")]
	[SerializeField]Font[] talkFont;

	[Header("トレーニング用のエネミースポナー")]
	[SerializeField] SpawnEnemy spawn;

	[Header("野営オブジェクト")]
	[SerializeField] GameObject yaeiObj;

	[Header("イベント時の画面縮小")]
	[SerializeField] GameObject blackUp;
	[SerializeField] GameObject blackDown;

	[Header("イベント時のStatus 表示or非表示")]
	[SerializeField] GameObject playerStatus;
	[SerializeField] GameObject bossStatus;
	[SerializeField] GameObject magicStatus;

	[Header("話すパネルオブジェクト")]
	[SerializeField] GameObject messagePanel;

	[Header("読むパネルオブジェクト")]
	[SerializeField] GameObject readPanel;

	[Header("見るパネルオブジェクト")]
	[SerializeField] GameObject watchPanel;

	[Header("トレーニングオブジェクト")]
	[SerializeField] GameObject trainingPanel;

	//[Header("メッセージ構成(最初の2つはビギニングとエンディング)")]
	//[SerializeField] MESSAGE[] messageConstitution;

	[Header("メッセージデータリスト")]
	[SerializeField] MessageDataList messageDataList;
	//[Header("メッセージデータリスト(野営シーン)")]
	//[SerializeField] MessageDataList messageDataListCamp;

	[Header("テキスト(野営シーン用)")]
	[SerializeField] GameObject campPanel;
	[SerializeField] Text contractorText;
	[SerializeField] Text lossText;
	[SerializeField] Animator shadowAnim; 
	string beforeState = "IdleState";

	[Header("ポインター")]
	[SerializeField] POINTER[] pointer;
	[Header("指し示すオブジェクト")]
	[SerializeField] GameObject pointerObject;

	[Header("ボス")]
	[SerializeField] GameObject bossObject;
	[SerializeField] float bossArea;

	[Header("フラッシュ")]
	[SerializeField] Image flush;
	FlushController flushController;

	[Header("最初の木の実")]
	[SerializeField] GameObject firstKinomi;

	[Header("ビデオソース")]
	[SerializeField] private VideoPlayer video;
	[Header("ビデオクリップ")]
	[SerializeField] private List<VideoClip> videoClips = new List<VideoClip>();

	[SerializeField]
	private GameObject movingCameras;
	Camera[] cameras;

	[Header("ボス用カメラ位置")]
	[SerializeField]Vector3 cameraBossPos;
	[Header("敵用カメラ位置")]
	[SerializeField]Vector3 enemyCameraPos;

	[SerializeField]float fadeTime = 0.02f;

	const float minCameraSize = 4.0f;
	const float maxCameraSize = 5.0f;
	const float deltaMagnitudeDiff = 0.01f;

	Color colorWhite = new Color(1, 1, 1, 0);
	Color colorBlack = new Color(0, 0, 0, 1);

	// TraningTextの解読前の文字をとる
	string beforeDecodeText;

	// talk変数
	GameObject panel;
	Text talkerText;
	Text message;
	int text_char_number;
	// talk読み上げ時間変数
	const float onTime = 0.1f;
	const float noTime = 0;
	float time = onTime;
	bool talkFlag = false;
	bool talkSkipFlag = false;
	// CampTalkNumber変数
	int campTalkNumber = 0;

	// カメラを動かすチュートリアル番号
	const int cameraTutorialNumber = 8;

	[SerializeField] InputManager inputManager;
	[SerializeField] WhatKeyDidIInput whatKey;

	// read変数
	GameObject window;
	Text readText;

	// watch変数
	GameObject videoWindow;
	Text watchTitle;
	Text watchText;

	// StageStart
	[SerializeField]GameObject stageStartPanel;
	[SerializeField]GameObject stageStartText;

	// StageClear
	[SerializeField]GameObject clearText;
	bool campTimeFlag = false;

	Vector2 maxCurtainPos = new Vector2(0, 700f);
	Vector2 minCurtainPos = new Vector2(0, 500f);

	
	void Start()
	{
		cameras = new Camera[movingCameras.transform.childCount];
		for (int i = 0; i < cameras.Length; i++)
		{
			cameras[i] = movingCameras.transform.GetChild(i).GetComponent<Camera>();
		}
		firstKinomi.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = false;
		pointerObject.transform.GetComponent<SpriteRenderer>().enabled = false;
		talkerText = messagePanel.transform.GetChild(0).GetComponent<Text>();
		message = messagePanel.transform.GetChild(1).GetComponent<Text>();
		readText = readPanel.transform.GetChild(0).GetComponent<Text>();
		watchTitle = watchPanel.transform.GetChild(0).GetComponent<Text>();
		watchText = watchPanel.transform.GetChild(1).GetComponent<Text>();
		panel = messagePanel;
		window = readPanel;
		videoWindow = watchPanel;
		panel.SetActive(false);
		window.SetActive(false);
		videoWindow.SetActive(false);
		flushController = flush.GetComponent<FlushController>();
		beforeDecodeText = trainingPanel.transform.GetChild(0).GetComponent<Text>().text;
		StartCoroutine(Openning());
		spawn.enabled = false;

#if UNITY_EDITOR
		CheckLine();
#endif
	}

	
	void Update()
	{
		if (/*Input.GetKey(KeyCode.Return/*returnKey*/ inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.next].buttonFlag && talkFlag)
		{
			time = noTime;
		}
		if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.skip].buttonDownFlag)
		{
			// 全部スキップ
			talkSkipFlag = true;
		}
		else if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.skip].buttonUpFlag)
		{
			talkSkipFlag = false;
		}
	}

	public IEnumerator WarpTime(Vector3 pos)
    {
		yield return StartCoroutine(flushController.FadeOut());
		playerController.Warp(pos, false);
		yield return StartCoroutine(flushController.FadeIn());
    }

	void SortingNumber(int number)
	{
		Quaternion quaternion = pointerObject.transform.rotation;
		Vector3 euler = quaternion.eulerAngles;
		foreach (POINTER point in pointer)
		{
			if (point.pointerNumber == number)
			{
				pointerObject.GetComponent<SpriteRenderer>().enabled = true;
				pointerObject.transform.position = (Vector2)point.pointerPos;
				euler.z = point.pointerPos.z;
				quaternion.eulerAngles = euler;
				pointerObject.transform.rotation = quaternion;
				break;
			}
		}
	}

	public void Starting()
	{
		playerStatus.SetActive(false);
		magicStatus.SetActive(false);
	}

	// 会話のパネル上に上げる
	IEnumerator PanelUp1(Vector2 minVector2, Vector2 maxVector2)
	{
		for (float plus = 0; plus <= 1; plus += 0.02f)
		{
			panel.GetComponent<RectTransform>().localPosition = Vector2.Lerp(minVector2, maxVector2, plus);
			yield return new WaitForSeconds(0.01f);
		}
	}

	// 会話のパネル下に下ろす
	IEnumerator PanelUp2(Vector2 minVector2, Vector2 maxVector2)
	{
		for (float plus = 1; plus >= 0; plus -= 0.02f)
		{
			panel.GetComponent<RectTransform>().localPosition = Vector2.Lerp(minVector2, maxVector2, plus);
			yield return new WaitForSeconds(0.01f);
		}
	}

	// 上のブラックボードを上に上げる
	IEnumerator BlackUp1(Vector2 minVector2, Vector2 maxVector2)
	{
		for (float plus = 1; plus >= 0; plus -= 0.02f)
		{
			blackUp.GetComponent<RectTransform>().localPosition = Vector2.Lerp(minVector2, maxVector2, plus);
			yield return new WaitForSeconds(0.01f);
		}
	}

	// 上のブラックボードを下に下ろす
	IEnumerator BlackUp2(Vector2 minVector2, Vector2 maxVector2)
	{
		for (float plus = 0; plus <= 1; plus += 0.02f)
		{
			blackUp.GetComponent<RectTransform>().localPosition = Vector2.Lerp(minVector2, maxVector2, plus);
			yield return new WaitForSeconds(0.01f);
		}
	}

	// 下のブラックボードを上に上げる
	IEnumerator BlackDown1(Vector2 minVector2, Vector2 maxVector2)
	{
		for (float plus = 0; plus <= 1; plus += 0.02f)
		{
			blackDown.GetComponent<RectTransform>().localPosition = Vector2.Lerp(minVector2, maxVector2, plus);
			yield return new WaitForSeconds(0.01f);
		}
	}

	// 下のブラックボードを下に下ろす
	IEnumerator BlackDown2(Vector2 minVector2, Vector2 maxVector2)
	{
		for (float plus = 1; plus >= 0; plus -= 0.02f)
		{
			blackDown.GetComponent<RectTransform>().localPosition = Vector2.Lerp(minVector2, maxVector2, plus);
			yield return new WaitForSeconds(0.01f);
		}
	}

	// 会話時のイベント処理
	public IEnumerator TalkTime(int number, float time, UnityAction<bool> CallBack)
	{
		playerStatus.SetActive(false);
		magicStatus.SetActive(false);

		SortingNumber(number);

		StartCoroutine(BGM.FadeOutPlay());

		Vector3[] cameraPosition = new Vector3[cameras.Length];
		int i = 0;
		foreach (Camera camera in cameras)
		{
			cameraPosition[i] = camera.transform.position;
			i++;
		}
		if (number == cameraTutorialNumber)
		{
			yield return StartCoroutine(CameraMovingEnemy(cameraPosition[0]));
		}

		//panel.SetActive(true);

		StartCoroutine(BlackUp1(minCurtainPos, maxCurtainPos));
		//StartCoroutine(PanelUp1(new Vector2(panel.transform.position.x, -770f), new Vector2(panel.transform.position.x, -350f)));
		yield return StartCoroutine(BlackDown1(-maxCurtainPos, -minCurtainPos));


		yield return StartCoroutine(Talking(number));


		StartCoroutine(BlackUp2(minCurtainPos, maxCurtainPos));
		//StartCoroutine(PanelUp2(new Vector2(panel.transform.position.x, -770f), new Vector2(panel.transform.position.x, -350f)));
		yield return StartCoroutine(BlackDown2(-maxCurtainPos, -minCurtainPos));

		if (number == cameraTutorialNumber)
		{
			yield return StartCoroutine(CameraMovingEnemyBack(cameraPosition[0]));
		}

		StartCoroutine(BGM.FadeInPlay());

		playerStatus.SetActive(true);
		magicStatus.SetActive(true);

		CallBack(true);
	}

	// 看板を読む時のイベント処理
	public IEnumerator ReadTime(int number, float time, UnityAction<bool> CallBack)
	{
		window.SetActive(true);

		StartCoroutine(EnterMotion(readPanel.transform.GetChild(1).GetComponent<Text>()));

		int tnumber = 0;

		readText.text = messageDataList.messageConstitution[number].messageText[tnumber];

		readText.text = DecodingText(readText.text);

		while(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.next].buttonFlag)
		{
			yield return null;
			if(messageDataList.messageConstitution[number].messageText.Length >= 2)
			{
				if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.left].buttonDownFlag)
				{
					tnumber++;
					if(tnumber > messageDataList.messageConstitution[number].messageText.Length - 1) tnumber = 0;

					readText.text = messageDataList.messageConstitution[number].messageText[tnumber];

					readText.text = DecodingText(readText.text, (tnumber == 1));
				}
				if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.right].buttonDownFlag)
				{
					tnumber--;
					if(tnumber < 0) tnumber = messageDataList.messageConstitution[number].messageText.Length - 1;

					readText.text = messageDataList.messageConstitution[number].messageText[tnumber];

					readText.text = DecodingText(readText.text, (tnumber == 1));
				}

			}
		}
		while (!inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.next].buttonFlag)
		{
			yield return null;
			if(messageDataList.messageConstitution[number].messageText.Length >= 2)
			{
				if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.left].buttonDownFlag)
				{
					tnumber++;
					if(tnumber > messageDataList.messageConstitution[number].messageText.Length - 1)
						tnumber = 0;

					readText.text = messageDataList.messageConstitution[number].messageText[tnumber];

					readText.text = DecodingText(readText.text, (tnumber == 1));
				}
				if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.right].buttonDownFlag)
				{
					tnumber--;
					if(tnumber < 0)
						tnumber = messageDataList.messageConstitution[number].messageText.Length - 1;

					readText.text = messageDataList.messageConstitution[number].messageText[tnumber];

					readText.text = DecodingText(readText.text, (tnumber == 1));
				}
			}
		}

		window.SetActive(false);

		CallBack(true);
	}

	// 看板の文字を解読してキーコンフィグに変換する機能
	public string DecodingText(string text, bool controllerFlag = false)
	{
		int i;
		if(controllerFlag)
		{
			for(i = 0; i < keyString.Length; i++)
			{
				keyString[i] = inputManager.GetPadCode(i).ToString();
				switch(keyString[i])
				{
					case "Return":
						keyString[i] = "Enter";
						break;
					case "LeftArrow":
						keyString[i] = "←";
						break;
					case "RightArrow":
						keyString[i] = "→";
						break;
					case "UpArrow":
						keyString[i] = "↑";
						break;
					case "DownArrow":
						keyString[i] = "↓";
						break;
				}
			}
			i = 0;
			foreach(string decording in decordings)
			{
				if(text.Contains(decording))
				{
					text = text.Replace(decording, keyString[i]);
				}
				i++;
			}
			return text;
		}

		for (i = 0; i < keyString.Length; i++)
		{
			keyString[i] = inputManager.GetKeyCode(i).ToString();
			switch(keyString[i])
			{
				case "Return":
					keyString[i] = "Enter";
					break;
				case "LeftArrow":
					keyString[i] = "←";
					break;
				case "RightArrow":
					keyString[i] = "→";
					break;
				case "UpArrow":
					keyString[i] = "↑";
					break;
				case "DownArrow":
					keyString[i] = "↓";
					break;
			}
		}
		i = 0;
		foreach (string decording in decordings)
		{
			if (text.Contains(decording))
			{
				text = text.Replace(decording, keyString[i]);
			}
			i++;
		}
		return text;
	}

	// 宝箱を開いた時のイベント処理
	public IEnumerator WatchTime(int number, int videoNumber, float time, UnityAction<bool> CallBack, UnityAction<bool> Training)
	{
		yield return new WaitForSeconds(1.5f);

		pointerObject.GetComponent<SpriteRenderer>().enabled = false;
		video.clip = videoClips[videoNumber];
		video.Play();

		videoWindow.SetActive(true);

		StartCoroutine(EnterMotion(videoWindow.transform.GetChild(3).GetComponent<Text>()));
		var trainingText = videoWindow.transform.GetChild(4).GetComponent<Text>();
		trainingText.text = DecodingText(trainingText.text);

		watchTitle.text = messageDataList.messageConstitution[number].messageText[0];
		watchText.text = messageDataList.messageConstitution[number].messageText[1];

		while (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.next].buttonFlag)
		{
			yield return null;
		}
		while (!inputManager.inputCheck[(int)KeyBindings.KeyBindNumber.next].buttonFlag)
		{
			// トレーニングモードを追加
			if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.skip].buttonDownFlag)
			{
				yield return StartCoroutine(TrainingTime(Training));
			}
			yield return null;
		}

		videoWindow.SetActive(false);

		CallBack(true);
	}

	// 特殊(看板で魔法説明)時のイベント処理
	public IEnumerator SpecialTime(int number, int videoNumber, UnityAction<bool> CallBack, UnityAction<bool> Training)
	{
		video.clip = videoClips[videoNumber];
		video.Play();

		videoWindow.SetActive(true);

		StartCoroutine(EnterMotion(videoWindow.transform.GetChild(3).GetComponent<Text>()));

		var trainingText = videoWindow.transform.GetChild(4).GetComponent<Text>();
		trainingText.text = DecodingText(trainingText.text);

		watchTitle.text = messageDataList.messageConstitution[number].messageText[0];
		watchText.text = messageDataList.messageConstitution[number].messageText[1];

		while (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.next].buttonFlag)
		{
			yield return null;
		}
		while (!inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.next].buttonFlag)
		{
			// トレーニングモードを追加
			if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.skip].buttonDownFlag)
			{
				yield return StartCoroutine(TrainingTime(Training));
			}
			yield return null;
		}

		videoWindow.SetActive(false);

		CallBack(true);
	}

	// トレーニングモード時のイベント処理
	IEnumerator TrainingTime(UnityAction<bool> Training)
	{
		bool controllerFlag = false;
		if(whatKey.joyStickFlag)
		{
			controllerFlag = true;
		}

		yield return StartCoroutine(flushController.FadeOut());

		CameraMove[] cameraScripts = new CameraMove[cameras.Length];
		int j = 0;
		foreach (Camera camera in cameras)
		{
			cameraScripts[j] = camera.transform.GetComponent<CameraMove>();
			cameraScripts[j].areaFlag = false;
			j++;
		}

		spawn.enabled = true;
		videoWindow.SetActive(false);
		trainingPanel.SetActive(true);
		var trainingText = trainingPanel.transform.GetChild(0).GetComponent<Text>();
		trainingText.text = DecodingText(beforeDecodeText, controllerFlag);
		Coroutine trainingCoroutine = StartCoroutine(EnterMotion(trainingPanel.transform.GetChild(1).GetComponent<Text>()));
		Training(true);

		yield return new WaitForSeconds(2);

		yield return StartCoroutine(flushController.FadeIn());

		spawn.StartTraining();

		while (!inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.skip].buttonDownFlag)
		{
			yield return null;
		}

		flushController.ChangeColor(Color.black);

		yield return StartCoroutine(flushController.FadeOut());

		spawn.EndTraining();
		StopCoroutine(trainingCoroutine);
		trainingPanel.SetActive(false);
		videoWindow.SetActive(true);
		Training(false);

		j = 0;
		foreach (Camera camera in cameras)
		{
			cameraScripts[j] = camera.transform.GetComponent<CameraMove>();
			cameraScripts[j].areaFlag = true;
			j++;
		}

		yield return new WaitForSeconds(2);

		yield return StartCoroutine(flushController.FadeIn());

		spawn.enabled = false;
	}


	// ボス時のイベント処理
	public IEnumerator BossTime(int number, UnityAction<bool> CallBack)
	{
		Vector2 maxCurtainPos1 = new Vector2(0, 700f);
		Vector2 minCurtainPos1 = new Vector2(0, 450f);
		Vector2 maxCurtainPos2 = new Vector2(0, 650f);
		Vector2 minCurtainPos2 = new Vector2(0, 575f);

		playerStatus.SetActive(false);
		magicStatus.SetActive(false);

		StartCoroutine(BlackUp1(minCurtainPos1, maxCurtainPos1));
		yield return StartCoroutine(BlackDown1(-maxCurtainPos2, -minCurtainPos2));

		panel.GetComponent<RectTransform>().localPosition = new Vector3(0, 300, 0);

		yield return StartCoroutine(Talking(number));

		StartCoroutine(BlackUp2(minCurtainPos1, maxCurtainPos1));
		yield return StartCoroutine(BlackDown2(-maxCurtainPos2, -minCurtainPos2));

		playerStatus.SetActive(true);
		bossStatus.SetActive(true);
		magicStatus.SetActive(true);

		BGM.Boss1BGM();
		BGM.PlayBGM();
		StartCoroutine(BGM.FadeInPlay());

		EventController eventController = GetComponent<EventController>();
		StartCoroutine(bossObject.GetComponent<IBoss>().StartBoss(eventController));

		CallBack(true);
	}

	// ステージクリア時のイベント処理
	public IEnumerator GoalTime(int number, Vector3 secondPos, Vector3 thirdPos, UnityAction<bool> CallBack)
	{
		playerStatus.SetActive(false);
		magicStatus.SetActive(false);

		StartCoroutine(BGM.FadeOutPlay());

		StartCoroutine(BlackUp1(minCurtainPos, maxCurtainPos));
		yield return StartCoroutine(BlackDown1(-maxCurtainPos, -minCurtainPos));

		yield return StartCoroutine(Talking(number));

		foreach(Camera camera in cameras)
		{
			camera.transform.GetComponent<CameraMove>().enabled = false;
		}

		var tempVolume = grobalVolume.GetComponent<Volume>().profile;

		yield return StartCoroutine(CampTime(number + 1));

		grobalVolume.GetComponent<Volume>().profile = tempVolume;

		yaeiObj.SetActive(false);
		playerController.Warp(secondPos, true);
		foreach(Camera camera in cameras)
		{
			camera.transform.GetComponent<CameraMove>().enabled = true;
		}

		StartCoroutine(BlackUp1(minCurtainPos, maxCurtainPos));
		StartCoroutine(BlackDown1(-maxCurtainPos, -minCurtainPos));

		yield return StartCoroutine(flushController.FadeIn());

		foreach(Camera camera in cameras)
		{
			camera.transform.GetComponent<CameraMove>().MoveAreaChange(thirdPos, CameraMove.SwitchArea.lowerleft);
		}

		yield return StartCoroutine(Talking(number + 2));

		StartCoroutine(BlackUp2(minCurtainPos, maxCurtainPos));
		StartCoroutine(BlackDown2(-maxCurtainPos, -minCurtainPos));

		BGM.NomalBGM();
		BGM.PlayBGM();
		StartCoroutine(BGM.FadeInPlay());

		playerStatus.SetActive(true);
		magicStatus.SetActive(true);

		CallBack(true);
	}

	IEnumerator CampTime(int number)
    {
		clearText.SetActive(true);
		SE.SystemSE((int)ESystemSE.GameClear);

		yield return new WaitForSeconds(4);

		yield return StartCoroutine(flushController.FadeOut(0.02f));

		grobalVolume.GetComponent<Volume>().profile = volumeProfile;

		clearText.SetActive(false);
		yaeiObj.SetActive(true);

		StartCoroutine(BlackUp2(minCurtainPos, maxCurtainPos));
		StartCoroutine(BlackDown2(-maxCurtainPos, -minCurtainPos));

		StartCoroutine(BGM.FadeInPlay(0f, 1f));
		BGM.FireBGM();
		BGM.PlayBGM();

		yield return StartCoroutine(flushController.FadeIn());

		yield return StartCoroutine(CampTalking(number));

		StartCoroutine(BGM.FadeOutPlay(0f, 1f));
		yield return StartCoroutine(flushController.FadeOut(0.02f));
		campTimeFlag = false;
	}

	// エンディング時のイベント処理
	public IEnumerator EndingTime()
	{
		Vector2 maxCurtainPos1 = new Vector2(0, 700f);
		Vector2 minCurtainPos1 = new Vector2(0, 450f);
		Vector2 maxCurtainPos2 = new Vector2(0, 650f);
		Vector2 minCurtainPos2 = new Vector2(0, 575f);

		playerStatus.SetActive(false);
		bossStatus.SetActive(false);
		magicStatus.SetActive(false);

		StartCoroutine(BlackUp1(minCurtainPos1, maxCurtainPos1));
		yield return StartCoroutine(BlackDown1(-maxCurtainPos2, -minCurtainPos2));

		yield return StartCoroutine(Talking(1));

		flushController.ChangeColor(Color.black);
		//yield return StartCoroutine(flushController.FadeOut());

		yield return StartCoroutine(CampTime(messageDataList.messageConstitution.Length - 1));

		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
	}

	// シーンが変わるときの処理
	IEnumerator Openning()
	{
		flushController.ChangeColor(Color.black);
		yield return StartCoroutine(flushController.FadeIn(fadeTime));
	}

	// 始まりの会話
	public IEnumerator BeginningTime(UnityAction<bool> CallBack)
	{
		SortingNumber(0);
		//panel.SetActive(true);
		StartCoroutine(BlackUp1(minCurtainPos, maxCurtainPos));
		//StartCoroutine(PanelUp1(new Vector2(panel.transform.position.x, -770f), new Vector2(panel.transform.position.x, -350f)));
		yield return StartCoroutine(BlackDown1(-maxCurtainPos, -minCurtainPos));

		yield return StartCoroutine(Talking(0));

		StartCoroutine(BlackUp2(minCurtainPos, maxCurtainPos));
		//StartCoroutine(PanelUp2(new Vector2(panel.transform.position.x, -770f), new Vector2(panel.transform.position.x, -350f)));
		yield return StartCoroutine(BlackDown2(-maxCurtainPos, -minCurtainPos));

		yield return StartCoroutine(StageStart());

		//pointerObject.GetComponent<SpriteRenderer>().enabled = false;
		playerStatus.SetActive(true);
		magicStatus.SetActive(true);

		CallBack(true);
	}

	IEnumerator StageStart()
    {
		Color stageStartPanelColor = stageStartPanel.GetComponent<Image>().color;
		for(float i = 0f; i < 1f; i += 0.02f)
        {
			stageStartPanelColor.a = Mathf.Lerp(0f, 1f, i);
			stageStartPanel.GetComponent<Image>().color = stageStartPanelColor;

			yield return new WaitForSeconds(0.01f);
        }

		RectTransform stageRectTransform = stageStartText.GetComponent<RectTransform>();
		for(float i = 0f; i < 1f; i += 0.02f)
		{
			Vector3 stageClearPos = stageRectTransform.localPosition;
			stageClearPos.x = Mathf.Lerp(-1920f, 0f, i);
			stageRectTransform.localPosition = stageClearPos;

			yield return new WaitForSeconds(0.01f);
		}

		BGM.PlayBGM();
		StartCoroutine(BGM.FadeInPlay());
		yield return new WaitForSeconds(3);

		for (float i = 0f; i < 1f; i += 0.02f)
		{
			Vector3 stageClearPos = stageRectTransform.localPosition;
			stageClearPos.x = Mathf.Lerp(0f, 1920f, i);
			stageRectTransform.localPosition = stageClearPos;

			yield return new WaitForSeconds(0.01f);
		}

		for (float i = 1f; i >= 0f; i -= 0.02f)
		{
			stageStartPanelColor.a = Mathf.Lerp(0f, 1f, i);
			stageStartPanel.GetComponent<Image>().color = stageStartPanelColor;

			yield return new WaitForSeconds(0.01f);
		}
	}

	// ボスを倒したときの処理
	public IEnumerator DefeatBoss()
	{
		flushController.FlushColor(Color.white);
		yield return new WaitForSeconds(2);
		playerStatus.SetActive(false);
		bossStatus.SetActive(false);
		magicStatus.SetActive(false);
		yield return StartCoroutine(flushController.FadeIn(fadeTime));
	}

	// カメラを敵の方へ向ける
	IEnumerator CameraMovingEnemy(Vector3 cameraPos)
	{
		pointerObject.transform.GetComponent<SpriteRenderer>().enabled = false;

		foreach (Camera camera in cameras)
		{
			camera.transform.GetComponent<CameraMove>().enabled = false;
		}

		float plus = 0;


		while (plus <= 1)
		{
			plus += 0.01f;

			foreach (Camera camera in cameras)
			{
				camera.transform.position = Vector3.Lerp(cameraPos, enemyCameraPos, plus);
			}

			yield return new WaitForSeconds(0.01f);
		}
	}

	// カメラを敵の方からプレイヤーの方に戻す
	IEnumerator CameraMovingEnemyBack(Vector3 cameraPos)
	{
		float plus = 1;

		while (plus >= 0)
		{
			plus -= 0.01f;

			foreach (Camera camera in cameras)
			{
				camera.transform.position = Vector3.Lerp(cameraPos, enemyCameraPos, plus);
			}
			yield return new WaitForSeconds(0.01f);
		}

		foreach (Camera camera in cameras)
		{
			camera.transform.GetComponent<CameraMove>().enabled = true;
		}
	}

	// カメラを強制的に動かす
	public IEnumerator CameraMoving()
	{

		float plus = 0;
		Vector3[] cameraPos = new Vector3[cameras.Length];
		float[] cameraSize = new float[cameras.Length];
		int i = 0;
		foreach (Camera camera in cameras)
		{
			camera.transform.GetComponent<CameraMove>().enabled = false;
			cameraPos[i] = camera.transform.position;
			cameraSize[i] = camera.orthographicSize;
			i++;
		}

		while (plus <= 1)
		{
			plus += 0.01f;
			int j = 0;
			foreach (Camera camera in cameras)
			{
				camera.transform.position = Vector3.Lerp(cameraPos[j], cameraBossPos, plus);
				camera.orthographicSize += deltaMagnitudeDiff;
				camera.orthographicSize = Mathf.Lerp(cameraSize[j], maxCameraSize, plus);
				j++;
			}
			yield return new WaitForSeconds(0.01f);
		}
	}

	public void CameraMoveBoss()
    {
		foreach(Camera camera in cameras)
        {
			var cameraMove = camera.transform.GetComponent<CameraMove>();
			cameraMove.MoveAreaChange(new Vector2(camera.transform.position.x - bossArea, camera.transform.position.y), new Vector2(camera.transform.position.x, camera.transform.position.y));
			cameraMove.enabled = true;
        }
    }

	// 木の実イベント時の処理
	public IEnumerator KinomiTime(int number, float time, UnityAction<bool> CallBack)
	{
		playerStatus.SetActive(false);
		magicStatus.SetActive(false);

		SortingNumber(number);

		StartCoroutine(BGM.FadeOutPlay());

		StartCoroutine(BlackUp1(minCurtainPos, maxCurtainPos));
		yield return StartCoroutine(BlackDown1(-maxCurtainPos, -minCurtainPos));

		yield return StartCoroutine(Talking(number));

		StartCoroutine(BlackUp2(minCurtainPos, maxCurtainPos));
		yield return StartCoroutine(BlackDown2(-maxCurtainPos, -minCurtainPos));

		StartCoroutine(BGM.FadeInPlay());

		playerStatus.SetActive(true);
		magicStatus.SetActive(true);

		CallBack(true);

		firstKinomi.transform.GetChild(0).GetComponent<BoxCollider2D>().enabled = true;

		while(!firstKinomi.GetComponent<JungleFruit>().getDead())
		{
			yield return null;
		}
		yield return StartCoroutine(pointerObject.GetComponent<PointerController>().FadeColorAlpha());
		pointerObject.transform.GetComponent<SpriteRenderer>().enabled = false;
	}

	// 会話ウィンドウの処理
	IEnumerator Talking(int talk_text_number)
	{
		string[] talk_text = messageDataList.messageConstitution[talk_text_number].messageText;
		MESSAGE.Talker[] talker_name = messageDataList.messageConstitution[talk_text_number].talker_name;

		panel.SetActive(true);

		var skipText = panel.transform.GetChild(3).GetComponent<Text>();
		skipText.text = DecodingText(skipText.text);

		Coroutine coroutine = StartCoroutine(TalkSound());

		for (int i = 0; i < talk_text.Length; i++)
		{
			message.font = talkFont[(int)messageDataList.messageConstitution[talk_text_number].messageFont[i]];

			if (talk_text[i].Contains("/"))
			{
				foreach(string command in System.Enum.GetNames(typeof(MESSAGE.Command)))
                {
					if(talk_text[i].Contains(command))
                    {
						SwitchCommand(talk_text[i], (MESSAGE.Command)System.Enum.Parse(typeof(MESSAGE.Command), command));
						break;
                    }
                }
				continue;
			}

			talk_text[i] = DecodingText(talk_text[i]);

            talkerText.text = messageDataList.messageConstitution[talk_text_number].TalkerSwitch(talker_name[i]);
			talkerText.font = message.font;

			message.text = "";
			text_char_number = 0;
			while (text_char_number != talk_text[i].Length)
			{
				talkFlag = true;
				while (talk_text[i][text_char_number] != '\n')
				{
					message.text += talk_text[i][text_char_number];
					yield return new WaitForSeconds(time);
					text_char_number++;
					if (talkSkipFlag)
					{
						talkFlag = false;
						break;
					}
				}
				text_char_number++;
				message.text += "\n";
				if (talkSkipFlag)
				{
					break;
				}
			}
			talkFlag = false;
			panel.transform.GetChild(2).GetComponent<Text>().enabled = true;
			Coroutine motion1 = StartCoroutine(EnterMotion(panel.transform.GetChild(2).GetComponent<Text>()));
			if (talkSkipFlag)
			{
				panel.transform.GetChild(2).GetComponent<Text>().enabled = false;
				time = onTime;
				break;
			}
			while (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.next].buttonFlag)
			{
				if (talkSkipFlag)
				{
					break;
				}
				yield return null;
			}
			if (talkSkipFlag)
			{
				panel.transform.GetChild(2).GetComponent<Text>().enabled = false;
				time = onTime;
				break;
			}
			while (!inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.next].buttonFlag)
			{
				if (talkSkipFlag)
				{
					break;
				}
				yield return null;
			}
			if (talkSkipFlag)
			{
				panel.transform.GetChild(2).GetComponent<Text>().enabled = false;
				time = onTime;
				break;
			}
			while (!inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.next].buttonUpFlag)
			{
				if (talkSkipFlag)
				{
					break;
				}
				yield return null;
			}
			if (motion1 != null)
				StopCoroutine(motion1);
			panel.transform.GetChild(2).GetComponent<Text>().enabled = false;
			time = onTime;
			if (talkSkipFlag)
			{
				break;
			}
		}
		StopCoroutine(coroutine);

		panel.SetActive(false);
	}

	void SwitchCommand(string str, MESSAGE.Command command)
    {
        switch (command)
        {
			case MESSAGE.Command.PlayerGo:
				float result = float.Parse(str.Substring(str.IndexOf("(") + 1, str.IndexOf("(") - 1 - str.IndexOf("(")));
				//float result1 = float.Parse(str.Substring(str.IndexOf("(") + 1, str.IndexOf(",") - 1 - str.IndexOf("(") ));
				//float result2 = float.Parse(str.Substring(str.IndexOf(",") + 1, str.IndexOf(")") - 1 - str.IndexOf(",") ));
				//Debug.Log(result + result2);
				break;
			case MESSAGE.Command.PlayerAttack:
				break;
			case MESSAGE.Command.ClarioGo:
				break;
			case MESSAGE.Command.ClarioWarpOn:
				break;
			case MESSAGE.Command.ClarioWarpOff:
				break;
			case MESSAGE.Command.BlackOut:
				break;
			case MESSAGE.Command.CloseUp:
				break;
        }
    }

	// 野営会話ウィンドウの処理
	IEnumerator CampTalking(int talk_text_number)
	{
		string[] talk_text = messageDataList.messageConstitution[talk_text_number].messageText;
		MESSAGE.Talker[] talker_name = messageDataList.messageConstitution[talk_text_number].talker_name;

		campPanel.SetActive(true);

		var skipText = panel.transform.GetChild(3).GetComponent<Text>();
		skipText.text = DecodingText(skipText.text);

		Coroutine coroutine = StartCoroutine(TalkSound());

		Text messageCamp;
		messageCamp = contractorText;
		messageCamp.text = "";
		messageCamp = lossText;
		messageCamp.text = "";

		for (int i = 0; i < talk_text.Length; i++)
		{
			talk_text[i] = DecodingText(talk_text[i]);

			if (talk_text[i].Contains("/"))
			{
				foreach (string command in System.Enum.GetNames(typeof(MESSAGE.Command)))
				{
					if (talk_text[i].Contains(command))
					{
						SwitchCommand(talk_text[i], (MESSAGE.Command)System.Enum.Parse(typeof(MESSAGE.Command), command));
						break;
					}
				}
				continue;
			}

			var talker = messageDataList.messageConstitution[talk_text_number].TalkerSwitchCamp(talker_name[i]);

            switch (talker.talker)
            {
				case MESSAGE.Talker.Contractor:
					messageCamp = contractorText;
					break;
				case MESSAGE.Talker.Loss:
					messageCamp = lossText;
					break;
				default:
					messageCamp = null;
					Debug.LogError("Miss TalkerSelect");
					break;
			}
			if(beforeState != talker.state)
            {
				beforeState = talker.state;
				shadowAnim.SetTrigger(talker.state);
            }

			messageCamp.text = "";
			text_char_number = 0;
			while (text_char_number != talk_text[i].Length)
			{
				talkFlag = true;
				while (talk_text[i][text_char_number] != '\n')
				{
					messageCamp.text += talk_text[i][text_char_number];
					yield return new WaitForSeconds(time);
					text_char_number++;
					if (talkSkipFlag)
					{
						talkFlag = false;
						break;
					}
				}
				text_char_number++;
				messageCamp.text += "\n";
				if (talkSkipFlag)
				{
					break;
				}
			}
			talkFlag = false;
			panel.transform.GetChild(2).GetComponent<Text>().enabled = true;
			Coroutine motion1 = StartCoroutine(EnterMotion(panel.transform.GetChild(2).GetComponent<Text>()));
			if (talkSkipFlag)
			{
				panel.transform.GetChild(2).GetComponent<Text>().enabled = false;
				time = onTime;
				break;
			}
			while (inputManager.inputCheck[(int)KeyBindings.KeyBindNumber.next].buttonFlag)
			{
				if (talkSkipFlag)
				{
					break;
				}
				yield return null;
			}
			if (talkSkipFlag)
			{
				panel.transform.GetChild(2).GetComponent<Text>().enabled = false;
				time = onTime;
				break;
			}
			while (!inputManager.inputCheck[(int)KeyBindings.KeyBindNumber.next].buttonFlag)
			{
				if (talkSkipFlag)
				{
					break;
				}
				yield return null;
			}
			if (talkSkipFlag)
			{
				panel.transform.GetChild(2).GetComponent<Text>().enabled = false;
				time = onTime;
				break;
			}
			while (!inputManager.inputCheck[(int)KeyBindings.KeyBindNumber.next].buttonUpFlag)
			{
				if (talkSkipFlag)
				{
					break;
				}
				yield return null;
			}
			if (motion1 != null)
				StopCoroutine(motion1);
			panel.transform.GetChild(2).GetComponent<Text>().enabled = false;
			time = onTime;
			if (talkSkipFlag)
			{
				break;
			}
		}
		StopCoroutine(coroutine);

		campPanel.SetActive(false);
	}


	// PressEnterの処理
	IEnumerator EnterMotion(Text enterText)
	{
		enterText.text = DecodingText(enterText.text);
		Color color = enterText.color;
		while (enterText.enabled)
		{
			color.a = Mathf.Cos(Time.time * 1.25f * Mathf.PI) / 2 + 0.5f;
			enterText.color = color;
			yield return new WaitForSeconds(0.01f);
		}
	}

	// 会話時の音処理
	IEnumerator TalkSound()
	{
		while (true)
		{
			while (talkFlag)
			{
				SE.SystemSE(0);
				for (float i = 0; i <= 1.07f; i += onTime / 2)
				{
					if (!talkFlag)
					{
						SE.StopSE();
						break;
					}
					yield return new WaitForSeconds(onTime / 2);
				}
			}
			yield return null;
		}
	}

	// メッセージリストの改行チェック
	void CheckLine()
	{
		for(int i = 0; i < messageDataList.messageConstitution.Length; i++)
		{
			for(int j = 0; j < messageDataList.messageConstitution[i].messageText.Length; j++)
			{
				if(messageDataList.messageConstitution[i].messageText[j][messageDataList.messageConstitution[i].messageText[j].Length - 1] != '\n')
				{
					Debug.LogError("改行されていない所があります。 " + "(" + i + ", " + j + ")");
				}
			}
		}
	}
}
