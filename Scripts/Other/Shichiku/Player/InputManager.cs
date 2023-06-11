using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
	[SerializeField]EditOptions editOptions;
	[SerializeField, EnumIndex(typeof(KeyBindings.KeyBindNumber))]
	public InputCheck[] inputCheck = new InputCheck[KeyBindings.numOfKeyBinds];//キーバインドの数だけキー状態判定クラスを作る

	const double deadLineX = 0;
	const double deadLineY = 0;

	void Start()
	{
		//Debug.LogWarning("InputManager: 変に処理が重いときはここをご確認ください");
	}

	
	void Update()
	{
		for(int i = 0; i < KeyBindings.numOfKeyBinds; i++)
		{
			inputCheck[i].UpdateInputCheck((KeyBindings.KeyBindNumber)i);//キーバインドの入力チェック
		}
		//ArrowInputCheck();//コントローラーのスティックの入力チェック
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

		bool lastButtonFlag = false;//直前のボタンが押されていたかの履歴保存

		//keycodeまたはpadcodeがパッドのキーなのか確認。"Joystick"文字列がkeycodeに入っているならtrueを返す
		public bool IsItGamepad(KeyCode keycode)
		{
			return keycode.ToString().Contains("Joystick");
		}

		//キー入力がされたかチェックする。updateで読み込む。重くならないか要検証
		public void UpdateInputCheck(KeyBindings.KeyBindNumber i)
		{
			if(Input.GetKey(KeyCode2Class.TransformCode(keyCode)) || Input.GetKey(KeyCode2Class.TransformCode(padCode)))//keycodeまたはpadcodeが入力されたらtrueを返す
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

			if(lastButtonFlag != buttonFlag)//ボタンが押された瞬間、離された瞬間を検知
			{
				lastButtonFlag = buttonFlag;//更新
				if(lastButtonFlag)//押された瞬間ならbuttondownをtrue,離れた瞬間ならbuttonupをtrue
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
	//キーの設定を変更・取得するための関数
	//
	//---------------------------------

	//1つだけキーを渡す
	public KeyCode2 GetKeyCode(KeyBindings.KeyBindNumber _keyCode)
	{
		return inputCheck[(int) _keyCode].keyCode;
	}
	//pad版
	public KeyCode2 GetPadCode(KeyBindings.KeyBindNumber _keyCode)
	{
		return inputCheck[(int) _keyCode].padCode;
	}

	//数字参照のオーバーロード
	public KeyCode2 GetKeyCode(int _keyNum)
	{
		return inputCheck[_keyNum].keyCode;
	}
	//pad版
	public KeyCode2 GetPadCode(int _keyNum)
	{
		return inputCheck[_keyNum].padCode;
	}

	//1つだけデフォルトキーを渡す
	public KeyCode2 GetOneDefaultKey(KeyBindings.KeyBindNumber keyBind)
	{
		return inputCheck[(int) keyBind].defaultKey;
	}
	//pad版
	public KeyCode2 GetOneDefaultPad(KeyBindings.KeyBindNumber keyBind)
	{
		return inputCheck[(int) keyBind].defaultPad;
	}

	//すべてのデフォルトキーを渡す
	public KeyCode2[] GetAllDefaultKey()
	{
		KeyCode2[] _defaultKeyBinds = new KeyCode2[KeyBindings.numOfKeyBinds];
		for(int i = 0; i < _defaultKeyBinds.Length; i++)
		{
			_defaultKeyBinds[i] = inputCheck[i].defaultKey;
		}
		return _defaultKeyBinds;
	}
	//pad版
	public KeyCode2[] GetAllDefaultPad()
	{
		KeyCode2[] _defaultKeyBinds = new KeyCode2[KeyBindings.numOfKeyBinds];
		for(int i = 0; i < _defaultKeyBinds.Length; i++)
		{
			_defaultKeyBinds[i] = inputCheck[i].defaultPad;
		}
		return _defaultKeyBinds;
	}

	//全キーバインドを変更
	public void SetKeyBindings(KeyCode2[] _keyBinds, KeyCode2[] _padCodes)
	{
		if(_keyBinds.Length < inputCheck.Length || _padCodes.Length < inputCheck.Length)
		{
			Debug.LogError("KeyConfig: 読み込むKeyCodeの量が少なすぎます");
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
		//にて操作するやつあり

		return;
	}
}