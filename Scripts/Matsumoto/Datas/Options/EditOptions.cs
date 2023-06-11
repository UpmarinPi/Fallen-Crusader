using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
//�I�v�V�����B��ʂɕ\������̂͂��̃N���X�ŁA�ύX�����炻�ꂼ��̃N���X�ɓK������
public class Options
{
	[SerializeField, EnumIndex(typeof(KeyBindings.KeyBindNumber))]
	public KeyCode2[] keyCodes = new KeyCode2[KeyBindings.numOfKeyBinds];
	[SerializeField, EnumIndex(typeof(KeyBindings.KeyBindNumber))]
	public KeyCode2[] padCodes = new KeyCode2[KeyBindings.numOfKeyBinds];
	public KeyCode2 GetKeyCodes(KeyBindings.KeyBindNumber _keyBindNumber)
	{
		return keyCodes[(int) _keyBindNumber];
	}
}
public class EditOptions : MonoBehaviour
{
	string datapath;//�f�[�^�ۑ���
	public Options optionData;
	[SerializeField]
	InputManager inputManager;

	private void Awake()
	{
		datapath = Application.dataPath + "/Options.json";//datapath�ɕۑ�����w��
		InitOptions(Load());
	}
	void Start()
	{

	}

	public KeyCode2[] GetAllOptionKeyBind()
	{
		return optionData.keyCodes;
	}



	//�I�v�V������������
	private void InitOptions(Options _options)
	{
		//if (_options.keyCodes.Length != optionData.keyCodes.Length)
		//{
		//	Debug.LogError("EditOptions:�z��̌����Ⴂ�܂�\noptions.json(" + _options.keyCodes.Length + ")\noption�f�[�^(" + optionData.keyCodes.Length + ")");
		//}

		optionData.keyCodes = _options.keyCodes;
		optionData.padCodes = _options.padCodes;
		inputManager.SetKeyBindings(optionData.keyCodes, optionData.padCodes);
	}

	//�I�v�V�����̒��g��ύX�B�ύX�������Ȃ��ӏ���null����͂��邱��
	public void SetOptions(KeyCode2[] _keycodes, KeyCode2[] _padCodes)
	{
		if (_keycodes != null)
		{
			optionData.keyCodes = _keycodes;
			optionData.padCodes = _padCodes;
		}
	}
	public void Save(Options _options)
	{
		string jsonstr = JsonUtility.ToJson(_options);
		StreamWriter writer = new StreamWriter(datapath, false);
		writer.WriteLine(jsonstr);
		writer.Flush();
		writer.Close();
	}
	private Options Load()
	{
		if (File.Exists(datapath))//"datapath"���̃t�@�C�������݂���Ȃ�
		{
			Debug.Log("EditOptions: Options.json �ǂݍ���");
			StreamReader reader = new StreamReader(datapath);
			string datastr = reader.ReadToEnd();
			reader.Close();
			return JsonUtility.FromJson<Options>(datastr);
		}
		else//���݂��Ȃ��Ȃ�
		{
			Debug.Log("EditOptions: Options.json �V�K�쐬");
			Options optionData = new Options();
			Debug.Log("keyCodes: " + optionData.keyCodes.Length);
			optionData.keyCodes = inputManager.GetAllDefaultKey();
			optionData.padCodes = inputManager.GetAllDefaultPad();

			Save(optionData);

			return optionData;
		}

	}
}
