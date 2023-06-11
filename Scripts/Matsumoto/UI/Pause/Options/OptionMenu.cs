using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionMenu : MonoBehaviour
{
	enum OptionMenuChoice
	{
		Sounds,
		KeyBindings,
		//sikisai
		//noobsupport
		//video
		//language
	}
	[SerializeField, EnumIndex(typeof(OptionMenuChoice))] List<GameObject> optionMenuObject = new List<GameObject>();

	[SerializeField] InputManager inputManager;

	bool enterFlag;//�I�v�V�������̉�����I�������Ƃ���bool�Btrue�̊Ԃ͑I���������̂̒��g���\������A�����瑤�͓����Ȃ�
	bool pushingFlag;
	OptionMenuChoice select;
	int countSelect;
	private void Start()
	{
		pushingFlag = true;
		//optionMenuObject���X�g��enum�̕������Ȃ��ꍇ�x�����o��
		if(System.Enum.GetValues(typeof(OptionMenuChoice)).Length != optionMenuObject.Count)
		{
			Debug.LogError("OptionMenu: �I�v�V����List�̗ʂ��s��v�ł�(�K�v��: " + System.Enum.GetValues(typeof(OptionMenuChoice)).Length + ")");
		}
	}
	private void Update()
	{


		if(!enterFlag)
		{
			if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.up].buttonFlag || inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.left].buttonFlag)
			{
				if(!pushingFlag)
				{
					pushingFlag = true;
					if(select <= 0)
					{
						select = (OptionMenuChoice) countSelect - 1;
					}
					else
					{
						select--;
					}
				}
			}
			else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.down].buttonFlag || inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.right].buttonFlag)
			{
				if(!pushingFlag)
				{
					pushingFlag = true;
					if(select >= (OptionMenuChoice) countSelect - 1)
					{

						select = 0;
					}
					else
					{
						select++;
					}
				}
			}
			else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.search].buttonFlag)
			{
				if(!pushingFlag)
				{
					enterFlag = true;
					switch(select)
					{
						case OptionMenuChoice.Sounds:
							optionMenuObject[(int) OptionMenuChoice.Sounds].SetActive(true);
							break;
						case OptionMenuChoice.KeyBindings:
							optionMenuObject[(int) OptionMenuChoice.KeyBindings].SetActive(true);
							break;
					}
				}
			}
			else
			{
				pushingFlag = false;
			}
		}
	}
}
