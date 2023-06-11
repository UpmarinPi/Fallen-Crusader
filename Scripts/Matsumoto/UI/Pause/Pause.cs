using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
	public enum PauseChoice
	{
		Continue,
		Restart,
		//Option,
		Quit
	};
	public bool pauseFlag = false;//�|�[�Y���Ƃ��Ă��邩
	public bool moveFlag;//���삪�\���B�\�����Ȃ��Ƃ���œ��삷��̂�h��
	bool pushingFlag;//�������ςȂ����
					 //public bool enteringFlag; //true���̓I�v�V�������ȂǕʉ�ʁB���̎��̓��͂�h��
	public PauseChoice select;//���݂ǂ���\�����Ă��邩
	int countSelect = System.Enum.GetValues(typeof(PauseChoice)).Length;//�|�[�Y��ʂ̌�

	[SerializeField]Image flush;
	[SerializeField] InputManager inputManager;
	[SerializeField] SelectMagic selectMagic;
	[SerializeField] SkillInfoInPause skillInfoInPause;
	[SerializeField] PlayerController playerController;

	[SerializeField] GameObject optionGO;
	DisplayOption displayOption;

	[SerializeField] GameObject selectMagicUIGO;//�|�[�Y���͉B���̂ɗp����

	[SerializeField] SEController SE;

	
	void Start()
	{
		select = 0;//�|�[�Y��ʂ̈�ԏ��I��
		pauseFlag = false;
		//enteringFlag = false;
		transform.GetChild(0).gameObject.SetActive(false);
		displayOption = optionGO.GetComponent<DisplayOption>();
	}

	
	void Update()
	{
		if(pauseFlag)//�|�[�Y���̓���
		{
			if(moveFlag)//�|�[�Y���A�������̂��\�Ȃ�
			{
				if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.left].buttonFlag)//���{�^��
				{
					if(!pushingFlag)
					{
						SE.SystemSE(3);
						pushingFlag = true;
						if(select <= 0)
						{
							select = (PauseChoice) countSelect - 1;
						}
						else
						{
							select--;
						}
					}
				}
				else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.right].buttonFlag)//�E�{�^��
				{
					if(!pushingFlag)
					{
						SE.SystemSE(3);
						pushingFlag = true;
						if(select >= (PauseChoice) countSelect - 1)
						{

							select = 0;
						}
						else
						{
							select++;
						}
					}
				}
				else if(Input.GetKey(KeyCode.Escape))//ESC�{�^��
				{
					if(!pushingFlag)
					{
						SE.SystemSE(4);
						SetActivePauseMenu(false);
						Time.timeScale = 1;
						transform.GetChild(0).gameObject.SetActive(false);
						Debug.Log("now start");
						pauseFlag = false;
					}
					pushingFlag = true;
				}
				else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.search].buttonFlag)//�T���{�^��
				{
					if(!pushingFlag)
					{
						moveFlag = false;
						SE.SystemSE(4);
						switch(select)
						{
							case PauseChoice.Continue:
								Continue();
								break;
							case PauseChoice.Restart:
								Restart();
								break;
							case PauseChoice.Quit:
								Quit();
								break;
							//case PauseChoice.Option:
							//	displayOption.selectingOptionFlag = true;
								//break;
						}
					}
					pushingFlag = true;
				}
				else
				{
					pushingFlag = false;
				}
			}
		}
		else//�|�[�Y���ł͂Ȃ��Ƃ�(!pauseFlag)
		{
			if(Input.GetKey(KeyCode.Escape))//ESC�{�^��
			{
				if(!pushingFlag && !selectMagic.selectingMagicFlag && !playerController.eventFlag)
				{
					SE.SystemSE(4);
					select = 0;
					transform.GetChild(0).gameObject.SetActive(true);
					SetActivePauseMenu(true);
					Time.timeScale = 0;
					Debug.Log("now stop");
					pauseFlag = true;
					moveFlag = true;
				}
				pushingFlag = true;
			}
			else
			{
				pushingFlag = false;
			}
		}
	}

	void Continue()
	{
		Time.timeScale = 1;
		transform.GetChild(0).gameObject.SetActive(false);
		//enteringFlag = false;
		Debug.Log("now start");
		pauseFlag = false;
	}
	void Restart()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	private void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;//�Q�[���v���C�I��
#else
    Application.Quit();
#endif
	}

	void SetActivePauseMenu(bool _active)//�|�[�Y��ʂ��J�n�E�I������ۂ̏����≉�o�Ǘ�
	{
		if(_active)
		{
			//�J�n
			skillInfoInPause.Active();
		}
		else
		{
		//�I��
		}
		selectMagicUIGO.SetActive(!_active);
	}
}
