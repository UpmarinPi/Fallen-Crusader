using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SelectMagic : MonoBehaviour
{

	/*
	 * SelectMagicUI��Pause�^�O(Pause�X�N���v�g����)���K�{
	 * */

	public bool selectingMagicFlag;		//���@�I��
	Pause pauseFlag;					//�|�[�Y����擾
	[SerializeField]
	GameObject selectMagicUI;           //���@UI
	[SerializeField] InputManager inputManager;
	[SerializeField]
	ManaBar manaBar;
	public int waitTime = 50;			//���@�I����ʂ܂ł̒���������
	bool doSkillFlag;					//true�Ŕ���,false�őI�𔻒�
	bool pushingFlag;               //���݃X�L����I�𒆂�
	public bool selectingSkill;
	bool reserveStopChoiceSkill;		//�X�L���I���~�\��


	void Start()
	{
		selectingMagicFlag = false;
		doSkillFlag = false;
		selectingSkill = false;
		reserveStopChoiceSkill = false;
		pauseFlag = GameObject.FindWithTag("Pause").transform.GetComponent<Pause>();

	}

	
	void Update()
	{
		if (pauseFlag.pauseFlag || GetComponent<PlayerController>().GetEventTime())//�|�[�Y���̓L�[���͂��󂯕t���Ȃ�
		{
			return;
		}
		//�X�L���L�[�������ꂽ�ꍇ
		if (inputManager.inputCheck[(int)KeyBindings.KeyBindNumber.magic].buttonFlag)
		{
			if (!pushingFlag)
			{
				doSkillFlag = true;
				pushingFlag = true;
			}
		}
		//�X�L���I���L�[�������ꂽ�ꍇ
		else if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.selectSkill].buttonDownFlag)
		{
			if (!pushingFlag)
			{
				SelectSkill();
				selectingSkill = true;
				pushingFlag = true;
			}
		}
		else
		{
			pushingFlag = false;
		}

		//�X�L���I�𒆂ɃX�L���I���L�[�������ꂽ�璆�f
		if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.selectSkill].buttonUpFlag && selectingSkill)
		{
			reserveStopChoiceSkill = true;//�X�L�����f�\��
		}
		if(reserveStopChoiceSkill && !selectMagicUI.GetComponent<SelectMagicUI>().TurningFlag())//���I����Ă���Ƃ��ɃX�L�����f
		{
			reserveStopChoiceSkill = false;
			StopSelectSkill();
		}
	}
	private void FixedUpdate()
	{
		//�X�L����������
		if (doSkillFlag)
		{
			doSkill();
			doSkillFlag = false;
		}
	}

	//IEnumerator PushSkillCoroutine(float waittime)//1waittime��fixedtime1��(0.02�b)
	//{
	//	for (int countTime = 0; countTime < waittime; countTime++)
	//	{
	//		if (!Input.GetKey(skillKey))//���ԑ҂����ɃX�L���{�^���������ꂽ��
	//		{
	//			doSkillFlag = true;
	//			yield break;
	//		}
	//		yield return new WaitForFixedUpdate();//fixedupdate1��҂�
	//	}
	//	//��莞�ԃX�L���{�^���������ςȂ��̏ꍇ
	//	doSkillFlag = false;
	//	finishCoroutineFlag = true;
	//}

	//�X�L���I��
	void SelectSkill()
	{
		Time.timeScale = 0;
		selectingMagicFlag = true;//�O���ɖ��@�I���Ƃ������Ƃ�`���邽��
		Debug.Log("selectingSkill");
	}

	//�X�L���I���̒��f
	void StopSelectSkill()
	{
		Time.timeScale = 1;
		selectingSkill = false;
		selectingMagicFlag = false;//�O���p
		manaBar.updateScaleBarFlag = true;
		Debug.Log("stopped hold A");
	}

	//�X�L������
	void doSkill()
	{
		Debug.Log("Spelled");
		GetComponent<MagicController>().SwitchMagic();
	}

	public bool GetSelectFlag()
	{
		return selectingMagicFlag;
	}
}
