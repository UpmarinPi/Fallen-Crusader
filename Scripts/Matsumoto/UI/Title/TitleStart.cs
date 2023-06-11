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

	bool startFlag;//�I�񂾂��̃t���O
	bool quitFlag;
	bool newGameFlag;
	bool alreadyStartFlag;//�R���[�`������񂵂����s�����Ȃ����߂̃t���O
	bool pushingFlag;//�ꉟ���ꓮ��
	bool fadeOutedFlag;//�t�F�[�h�A�E�g����������
	[Header("�t���b�V��")]
	public Image flush;
	public string startScene;//���̃V�[��
	public titleCursor cursor;//���݂ǂ���I�����Ă��邩
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
        if (!finishOpening || selectFlag�@|| fadeToDemo.timeFlag)
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
				Debug.Log(cursor + "�Ɉړ��I");
			}
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			if (cursor < titleCursor.Exit && !pushingFlag)
			{
				SE.SystemSE(0);
				cursor++;
				pushingFlag = true;
				Debug.Log(cursor + "�Ɉړ��I");
			}
		}
		if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.DownArrow))//��󂪉�����ĂȂ��Ȃ�
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
				case titleCursor.Continue://�Q�[���J�n
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
			UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
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
