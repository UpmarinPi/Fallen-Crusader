using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForDebug : MonoBehaviour
{
#if UNITY_EDITOR//unity editor限定
	[SerializeField]GameObject debugGB;
	[SerializeField] string debugString = "External";
	int countString;
	void Start()
	{
		countString = 0;
	}
	void Update()
	{
		if(Input.anyKeyDown)//何かのキーが押されたとき
		{
			if(countString < debugString.Length)//押したキーの総数が入力してほしいデバッグ用文字列より短いとき
			{
				if(Input.GetKeyDown(debugString[countString].ToString().ToLower()))//デバッグ用文字列を順番通り押しているなら
				{
					countString++;//1ずつ増やしていく
				}
				else
				{
					countString = 0;//押したボタンをリセット
				}
			}
		}
		if(countString >= debugString.Length)//文字列を入力したら
		{
			debugGB.SetActive(true);//デバッグ用オブジェクトがtrueになる
		}

	}
#endif
}
