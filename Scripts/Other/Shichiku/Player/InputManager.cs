using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[SerializeField]EditOptions editOptions;
	[SerializeField, EnumIndex(typeof(KeyBindings.KeyBindNumber))]
	public InputCheck[] inputCheck = new InputCheck[KeyBindings.numOfKeyBinds];//�L�[�o�C���h�̐������L�[��Ԕ���N���X�����

	const double deadLineX = 0;
	const double deadLineY = 0;

	void Start()
	{
		//Debug.LogWarning("InputManager: �ςɏ������d���Ƃ��͂��������m�F��������");
	}

	
	void Update()
	{
		for(int i = 0; i < KeyBindings.numOfKeyBinds; i++)
		{
			inputCheck[i].UpdateInputCheck((KeyBindings.KeyBindNumber)i);//�L�[�o�C���h�̓��̓`�F�b�N
		}
		//ArrowInputCheck();//�R���g���[���[�̃X�e�B�b�N�̓��̓`�F�b�N
	}

	[System.Serializable]
	public class InputCheck
	{
		[SerializeField]
		public KeyCode2 keyCode;
		[SerializeField]
		public KeyCode2 padCode;
		[SerializeField]
		public KeyCode2 defaultKey;
		[SerializeField]
		public KeyCode2 defaultPad;

		[HideInInspector]public bool buttonFlag = false;
		[HideInInspector]public bool buttonUpFlag = false;
		[HideInInspector]public bool buttonDownFlag = false;

		bool lastButtonFlag = false;//���O�̃{�^����������Ă������̗���ۑ�

		//keycode�܂���padcode���p�b�h�̃L�[�Ȃ̂��m�F�B"Joystick"������keycode�ɓ����Ă���Ȃ�true��Ԃ�
		public bool IsItGamepad(KeyCode keycode)
		{
			return keycode.ToString().Contains("Joystick");
		}

		//�L�[���͂����ꂽ���`�F�b�N����Bupdate�œǂݍ��ށB�d���Ȃ�Ȃ����v����
		public void UpdateInputCheck(KeyBindings.KeyBindNumber i)
		{
			if(Input.GetKey(KeyCode2Class.TransformCode(keyCode)) || Input.GetKey(KeyCode2Class.TransformCode(padCode)))//keycode�܂���padcode�����͂��ꂽ��true��Ԃ�
			{
				buttonFlag = true;
			}
			else if(ArrowInputCheck(i))
			{
				buttonFlag = ArrowInputCheck(i);
			}
			else
			{
				buttonFlag = false;
			}

			if(lastButtonFlag != buttonFlag)//�{�^���������ꂽ�u�ԁA�����ꂽ�u�Ԃ����m
			{
				lastButtonFlag = buttonFlag;//�X�V
				if(lastButtonFlag)//�����ꂽ�u�ԂȂ�buttondown��true,���ꂽ�u�ԂȂ�buttonup��true
				{
					buttonDownFlag = true;
				}
				else
				{
					buttonUpFlag = true;
				}
			}
			else
			{
				buttonUpFlag = false;
				buttonDownFlag = false;
			}
		}
		public bool ArrowInputCheck(KeyBindings.KeyBindNumber i)
		{
			switch(i)
			{
				case KeyBindings.KeyBindNumber.left:
					if(Input.GetAxisRaw("Horizontal") < -deadLineX || Input.GetAxisRaw("JujiHorizontal") < -deadLineX)
						return true;
					break;
				case KeyBindings.KeyBindNumber.right:
					if(Input.GetAxisRaw("Horizontal") > deadLineX || Input.GetAxisRaw("JujiHorizontal") > deadLineX)
						return true;
					break;
				case KeyBindings.KeyBindNumber.up:
					if(Input.GetAxisRaw("Vertical") > deadLineY || Input.GetAxisRaw("Vertical2") > deadLineY || Input.GetAxisRaw("JujiVertical") > deadLineY || Input.GetAxisRaw("L2") > deadLineY)
						return true;
					break;
				case KeyBindings.KeyBindNumber.down:
					if(Input.GetAxisRaw("Vertical") < -deadLineY || Input.GetAxisRaw("Vertical2") < -deadLineY || Input.GetAxisRaw("JujiVertical") < -deadLineY || Input.GetAxisRaw("R2") > -deadLineY)
						return true;
					break;
			}
			return false;
		}
	}
	public void ArrowInputCheck()
	{
		if(Input.GetAxisRaw("Horizontal") > deadLineX || Input.GetAxisRaw("JujiHorizontal") > deadLineX)
			inputCheck[(int) KeyBindings.KeyBindNumber.right].buttonFlag = true;

		if(Input.GetAxisRaw("Horizontal") < -deadLineX || Input.GetAxisRaw("JujiHorizontal") < -deadLineX)
			inputCheck[(int) KeyBindings.KeyBindNumber.left].buttonFlag = true;

		if(Input.GetAxisRaw("Vertical") > deadLineY || Input.GetAxisRaw("Vertical2") > deadLineY || Input.GetAxisRaw("JujiVertical") > deadLineY || Input.GetAxisRaw("L2") > deadLineY)
			inputCheck[(int) KeyBindings.KeyBindNumber.up].buttonFlag = true;

		if(Input.GetAxisRaw("Vertical") < -deadLineY || Input.GetAxisRaw("Vertical2") < -deadLineY || Input.GetAxisRaw("JujiVertical") < -deadLineY || Input.GetAxisRaw("R2") > -deadLineY)
			inputCheck[(int) KeyBindings.KeyBindNumber.down].buttonFlag = true;
	}

	//---------------------------------
	//
	//�L�[�̐ݒ��ύX�E�擾���邽�߂̊֐�
	//
	//---------------------------------

	//1�����L�[��n��
	public KeyCode2 GetKeyCode(KeyBindings.KeyBindNumber _keyCode)
	{
		return inputCheck[(int) _keyCode].keyCode;
	}
	//pad��
	public KeyCode2 GetPadCode(KeyBindings.KeyBindNumber _keyCode)
	{
		return inputCheck[(int) _keyCode].padCode;
	}

	//�����Q�Ƃ̃I�[�o�[���[�h
	public KeyCode2 GetKeyCode(int _keyNum)
	{
		return inputCheck[_keyNum].keyCode;
	}
	//pad��
	public KeyCode2 GetPadCode(int _keyNum)
	{
		return inputCheck[_keyNum].padCode;
	}

	//1�����f�t�H���g�L�[��n��
	public KeyCode2 GetOneDefaultKey(KeyBindings.KeyBindNumber keyBind)
	{
		return inputCheck[(int) keyBind].defaultKey;
	}
	//pad��
	public KeyCode2 GetOneDefaultPad(KeyBindings.KeyBindNumber keyBind)
	{
		return inputCheck[(int) keyBind].defaultPad;
	}

	//���ׂẴf�t�H���g�L�[��n��
	public KeyCode2[] GetAllDefaultKey()
	{
		KeyCode2[] _defaultKeyBinds = new KeyCode2[KeyBindings.numOfKeyBinds];
		for(int i = 0; i < _defaultKeyBinds.Length; i++)
		{
			_defaultKeyBinds[i] = inputCheck[i].defaultKey;
		}
		return _defaultKeyBinds;
	}
	//pad��
	public KeyCode2[] GetAllDefaultPad()
	{
		KeyCode2[] _defaultKeyBinds = new KeyCode2[KeyBindings.numOfKeyBinds];
		for(int i = 0; i < _defaultKeyBinds.Length; i++)
		{
			_defaultKeyBinds[i] = inputCheck[i].defaultPad;
		}
		return _defaultKeyBinds;
	}

	//�S�L�[�o�C���h��ύX
	public void SetKeyBindings(KeyCode2[] _keyBinds, KeyCode2[] _padCodes)
	{
		if(_keyBinds.Length < inputCheck.Length || _padCodes.Length < inputCheck.Length)
		{
			Debug.LogError("KeyConfig: �ǂݍ���KeyCode�̗ʂ����Ȃ����܂�");
			return;
		}
		for(int i = 0; i < inputCheck.Length; i++)
		{
			inputCheck[i].keyCode = _keyBinds[i];
			inputCheck[i].padCode = _padCodes[i];
		}

		editOptions.SetOptions(_keyBinds, _padCodes);

		//playerController
		//selectMagic
		//eventController
		//fallThruPlatform_Ver1
		//�ɂđ��삷������

		return;
	}
}