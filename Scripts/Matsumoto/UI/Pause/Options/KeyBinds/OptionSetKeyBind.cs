using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KeyBindings;

//キーを設定するスクリプト。OneOfKeyBindクラスが動かす
public class OptionSetKeyBind : MonoBehaviour
{
	string keyName;//キーの名前。"ジャンプ","右"など
	[SerializeField] Text infoText;//「〇〇を入力してください」
	KeyBindNumber keyBindNumber;//何のキーに関して求めているか
	KeyCode2 oldKey;//変更前のキー。
	public KeyCode2 SetOldKey
	{
		set
		{
			oldKey = value;
		}
	}
	public KeyCode2 returnKey;//返ってきたキー。上のkeyにこれが適用される

	public bool searchingKey;//キー入力待ち状態。キーを入力したものがreturnKeyに返される
	public bool endSearchFlag;//returnKeyが更新されたことを伝える

	private void Start()
	{
		endSearchFlag = false;
	}
	private void Update()
	{
		if(searchingKey && Input.anyKeyDown)//キー入力待ち状態で何かキーが入力されたとき
		{
			foreach(KeyCode code in System.Enum.GetValues(typeof(KeyCode)))//全キーを調べる
			{
				if(Input.GetKeyDown(code))//もし探索されたキーと一致したら
				{
					if(!code.ToString().Contains("Joystick"))//Joystick以外なら
					{
						if(code == KeyCode.Escape)//Escキーの時は元々のキーを
						{
							returnKey = oldKey;
						}
						else//それ以外なら押されたボタンを
						{
							returnKey = KeyCode2Class.TransformCode(code);//入力されたキーをKeyCode2に変換して返す
						}
						searchingKey = false;
						endSearchFlag = true;
						break;
					}
				}
			}
		}

	}
}
