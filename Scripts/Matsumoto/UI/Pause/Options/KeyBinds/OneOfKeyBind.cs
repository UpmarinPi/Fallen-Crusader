using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KeyBindings;

//キーバインドのうちの1つ
//これがプレハブ化していく
//
public class OneOfKeyBind : MonoBehaviour
{
	InputManager inputManager;
	EditOptions editOptions;
	OptionSetKeyBind optionSetKeyBind;

	Text keyName;
	Text keybind;
	public int myNum;//自身が何番目のキーか

	public bool selectFlag;//このキーが選択されているかどうかのbool
	bool changingFlag;//キーの変更中か否かのbool

	/// <summary>
	/// OneOfKeyBindのインスタンスのためのオーバーロード。初期化諸々追加
	/// </summary>
	/// <param name="_oneOfKeyBindPrefab"></param>
	/// <param name="_parent">プレハブの親</param>
	/// <param name="_myNum">MyNum[何番目に作成されたか。これを基にkeyBindNumberとkeyCodeが当てはまる]</param>
	/// <param name="_inputManager">InputManager</param>
	/// <param name="_editOptions">EditOptions</param>
	/// <param name="_optionSetKeyBind">OptionSetKeyBind</param>
	/// <returns></returns>
	public OneOfKeyBind Instantiate(OneOfKeyBind _oneOfKeyBindPrefab, Transform _parent, int _myNum, InputManager _inputManager, EditOptions _editOptions, OptionSetKeyBind _optionSetKeyBind)
	{
		OneOfKeyBind obj = Instantiate(_oneOfKeyBindPrefab, _parent) as OneOfKeyBind;//インスタンス化
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

	//keyname, keybindのプロパティ
	public KeyBindNumber KeyName
	{
		get
		{
			foreach(KeyBindNumber code in System.Enum.GetValues(typeof(KeyBindNumber)))//keyBindNumberをstring変換して全て見て、もしなければ0を返す
			{
				if(code.ToString() == keyName.text)
				{
					return code;
				}
			}
			Debug.LogWarning("OneOfKeyBind: " + keyName.text + "をKeyBindNumber内で検索しましたが、出てきませんでした");
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
			foreach(KeyCode2 code in System.Enum.GetValues(typeof(KeyCode2)))//keycode2をstring変換して全て見て、もしなければ0を返す
			{
				if(code.ToString() == keybind.text)
				{
					return code;
				}
			}
			Debug.LogWarning("OneOfKeyBind: " + keyName.text + "をkeycode2内で検索しましたが、出てきませんでした");
			return 0;
		}
		set
		{
			keyName.text = value.ToString();
		}
	}

	KeyCode2 ChangeKey(KeyBindNumber _keybindNumber)
	{
		optionSetKeyBind.SetOldKey = editOptions.optionData.GetKeyCodes(_keybindNumber);//元々のキーを設定
		optionSetKeyBind.searchingKey = true;//キー入力待ち
		StartCoroutine(waitingReturnKey());//キーが入力されるまで待つ
		optionSetKeyBind.endSearchFlag = false;//ちゃんと届いたので次のためfalseに戻す
		return optionSetKeyBind.returnKey;

		IEnumerator waitingReturnKey()
		{
			while(!optionSetKeyBind.endSearchFlag)//optionSetKeyBindとの同期処理。optionSetKeyBind側から反応が来るまで待つ
			{
				yield return null;
			}
		}
	}
}
