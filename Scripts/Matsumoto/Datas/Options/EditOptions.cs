using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
//オプション。画面に表示するのはこのクラスで、変更したらそれぞれのクラスに適応する
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
	string datapath;//データ保存先
	public Options optionData;
	[SerializeField]
	InputManager inputManager;

	private void Awake()
	{
		datapath = Application.dataPath + "/Options.json";//datapathに保存先を指定
		InitOptions(Load());
	}
	void Start()
	{

	}

	public KeyCode2[] GetAllOptionKeyBind()
	{
		return optionData.keyCodes;
	}



	//オプションを初期化
	private void InitOptions(Options _options)
	{
		//if (_options.keyCodes.Length != optionData.keyCodes.Length)
		//{
		//	Debug.LogError("EditOptions:配列の個数が違います\noptions.json(" + _options.keyCodes.Length + ")\noptionデータ(" + optionData.keyCodes.Length + ")");
		//}

		optionData.keyCodes = _options.keyCodes;
		optionData.padCodes = _options.padCodes;
		inputManager.SetKeyBindings(optionData.keyCodes, optionData.padCodes);
	}

	//オプションの中身を変更。変更したくない箇所はnullを入力すること
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
		if (File.Exists(datapath))//"datapath"内のファイルが存在するなら
		{
			Debug.Log("EditOptions: Options.json 読み込み");
			StreamReader reader = new StreamReader(datapath);
			string datastr = reader.ReadToEnd();
			reader.Close();
			return JsonUtility.FromJson<Options>(datastr);
		}
		else//存在しないなら
		{
			Debug.Log("EditOptions: Options.json 新規作成");
			Options optionData = new Options();
			Debug.Log("keyCodes: " + optionData.keyCodes.Length);
			optionData.keyCodes = inputManager.GetAllDefaultKey();
			optionData.padCodes = inputManager.GetAllDefaultPad();

			Save(optionData);

			return optionData;
		}

	}
}
