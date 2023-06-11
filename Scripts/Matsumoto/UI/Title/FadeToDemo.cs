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
	GameObject back;//�w�i�������Ė������t�F�[�h�A�E�g������

	[Header("����J�n�܂ŉ��b�҂��B1sec. = 60")]
	[SerializeField]
	int startDemoTime;

	//�^�C�}�[
	int timer;

	//bool���낢��
	[HideInInspector]
	public bool timeFlag;              //�^�C�}�[�����B�^�C�}�[���҂����Ԓ�������~�߂邽��
	bool fadeOutedFlag;         //�t�F�[�h�A�E�g�������̊m�F�B������J�n����
	bool stopFadeFlag;          //�t�F�[�h��~�����B�t�F�[�h�A�E�g���Ƀ{�^�����������Ƃ�
	bool startFadeFlag;         //�t�F�[�h�J�n�����BtimeFlag���������Ƃ���񂾂�start�𔭓�
	bool startDemoFlag;
	bool reloadSceneFlag;
	void Start()
	{
		//������
		videoPlayer = this.GetComponent<VideoPlayer>();
		timer = 0;

		//����I�����m
		videoPlayer.loopPointReached += EndVideo;

		//bool����
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
		if (stopFadeFlag)//�t�F�[�h���Ƀ{�^���������ꂽ��
		{
			stopFadeFlag = false;//�f���r�f�I�𗬂������Z�b�g
			fadeOutedFlag = false;
			timeFlag = false;
			timer = 0;
		}
	}
	private void FixedUpdate()
	{
		if (!timeFlag)//���Ԃ𑝂₷����
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
		if (startFadeFlag && timeFlag)//�t�F�[�h�J�n���B�t�F�[�h�J�n���timeFlag��true�p��
		{
			startFadeFlag = false;
			StartCoroutine(FadeOut());
		}
		if (fadeOutedFlag && !stopFadeFlag)//�t�F�[�h�A�E�g�I����Ē�~�������������ĂȂ��ꍇ����𗬂�
		{
			startDemoFlag = true;
			fadeOutedFlag = false;
			particle.SetActive(false);//�p�[�e�B�N��������
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
		if (reloadSceneFlag)//�V�[���̃����[�h�v���B����I��肩"���撆�Ƀ{�^�����������Ƃ�"��������
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
