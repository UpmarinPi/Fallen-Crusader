using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KeyBindings;

//�L�[�o�C���h�̂�����1��
//���ꂪ�v���n�u�����Ă���
//
public class OneOfKeyBind : MonoBehaviour
{
	InputManager inputManager;
	EditOptions editOptions;
	OptionSetKeyBind optionSetKeyBind;

	Text keyName;
	Text keybind;
	public int myNum;//���g�����Ԗڂ̃L�[��

	public bool selectFlag;//���̃L�[���I������Ă��邩�ǂ�����bool
	bool changingFlag;//�L�[�̕ύX�����ۂ���bool

	/// <summary>
	/// OneOfKeyBind�̃C���X�^���X�̂��߂̃I�[�o�[���[�h�B���������X�ǉ�
	/// </summary>
	/// <param name="_oneOfKeyBindPrefab"></param>
	/// <param name="_parent">�v���n�u�̐e</param>
	/// <param name="_myNum">MyNum[���Ԗڂɍ쐬���ꂽ���B��������keyBindNumber��keyCode�����Ă͂܂�]</param>
	/// <param name="_inputManager">InputManager</param>
	/// <param name="_editOptions">EditOptions</param>
	/// <param name="_optionSetKeyBind">OptionSetKeyBind</param>
	/// <returns></returns>
	public OneOfKeyBind Instantiate(OneOfKeyBind _oneOfKeyBindPrefab, Transform _parent, int _myNum, InputManager _inputManager, EditOptions _editOptions, OptionSetKeyBind _optionSetKeyBind)
	{
		OneOfKeyBind obj = Instantiate(_oneOfKeyBindPrefab, _parent) as OneOfKeyBind;//�C���X�^���X��
		RectTransform objRT = this.GetComponent<RectTransform>();
		obj.transform.localPosition = new Vector3(objRT.sizeDelta.x / 2, -objRT.sizeDelta.y * (_myNum + (float)0.5), 0);
		obj.myNum = _myNum;
		obj.inputManager = _inputManager;
		obj.editOptions = _editOptions;
		obj.optionSetKeyBind = _optionSetKeyBind;

		return obj;
	}

	private void Start()
	{
		keyName = gameObject.transform.GetChild(0).GetComponent<Text>();
		keybind = gameObject.transform.GetChild(1).GetComponent<Text>();
		name = "OneOfKeyBind(" + (KeyBindNumber) myNum + ")";
		KeyName = (KeyBindNumber) myNum;
		Keybind = inputManager.GetKeyCode((KeyBindNumber) myNum);
		changingFlag = false;
	}
	private void Update()
	{
		if(selectFlag)
		{
			keyName.color = new Color32(255, 255, 255, 255);
			keybind.color = new Color32(255, 255, 255, 255);
			if(!changingFlag && inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.search].buttonDownFlag)
			{
				changingFlag = true;
				ChangeKey(KeyName);
			}
		}
		else
		{
			keyName.color = new Color32(50, 50, 50, 255);
			keybind.color = new Color32(50, 50, 50, 255);
		}
	}

	//keyname, keybind�̃v���p�e�B
	public KeyBindNumber KeyName
	{
		get
		{
			foreach(KeyBindNumber code in System.Enum.GetValues(typeof(KeyBindNumber)))//keyBindNumber��string�ϊ����đS�Č��āA�����Ȃ����0��Ԃ�
			{
				if(code.ToString() == keyName.text)
				{
					return code;
				}
			}
			Debug.LogWarning("OneOfKeyBind: " + keyName.text + "��KeyBindNumber���Ō������܂������A�o�Ă��܂���ł���");
			return 0;
		}
		set
		{
			keyName.text = value.ToString();
		}
	}

	public KeyCode2 Keybind
	{
		get
		{
			foreach(KeyCode2 code in System.Enum.GetValues(typeof(KeyCode2)))//keycode2��string�ϊ����đS�Č��āA�����Ȃ����0��Ԃ�
			{
				if(code.ToString() == keybind.text)
				{
					return code;
				}
			}
			Debug.LogWarning("OneOfKeyBind: " + keyName.text + "��keycode2���Ō������܂������A�o�Ă��܂���ł���");
			return 0;
		}
		set
		{
			keyName.text = value.ToString();
		}
	}

	KeyCode2 ChangeKey(KeyBindNumber _keybindNumber)
	{
		optionSetKeyBind.SetOldKey = editOptions.optionData.GetKeyCodes(_keybindNumber);//���X�̃L�[��ݒ�
		optionSetKeyBind.searchingKey = true;//�L�[���͑҂�
		StartCoroutine(waitingReturnKey());//�L�[�����͂����܂ő҂�
		optionSetKeyBind.endSearchFlag = false;//�����Ɠ͂����̂Ŏ��̂���false�ɖ߂�
		return optionSetKeyBind.returnKey;

		IEnumerator waitingReturnKey()
		{
			while(!optionSetKeyBind.endSearchFlag)//optionSetKeyBind�Ƃ̓��������BoptionSetKeyBind�����甽��������܂ő҂�
			{
				yield return null;
			}
		}
	}
}
