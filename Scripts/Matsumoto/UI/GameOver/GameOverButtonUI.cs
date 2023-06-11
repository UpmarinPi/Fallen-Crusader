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
	bool fadeOutedFlag;//������fadeOutedFlag�͂�蒼���I�������ꍇ�̂�true�ɂ��邱��
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
		if (fadeOutedFlag)//������fadeOutedFlag�͂�蒼���I�������ꍇ�̂�true�ɂ��邱��
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);//���݂̃V�[����ǂݍ��݂Ȃ���
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

	void BTCP()//back to check point, �Ō�ɗ����`�F�b�N�|�C���g�ֈړ�
	{

		StartCoroutine(FadeOut());

	}

	void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
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
