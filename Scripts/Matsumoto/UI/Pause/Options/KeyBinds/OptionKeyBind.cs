using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KeyBindings;

//
public class OptionKeyBind : MonoBehaviour
{
	[SerializeField] GameObject oneOfKeyBindPrefab;
	[SerializeField] EditOptions editOptions;
	[SerializeField] OptionSetKeyBind optionSetKeyBind;
	[SerializeField] InputManager inputManager;
	[SerializeField] Transform parentForPrefab;//���̏ꏊ�Ƀv���n�u���ݒ肳���

	OneOfKeyBind oneOfKeyBind;
	[SerializeField]List<OneOfKeyBind> keyBindPrefab = new List<OneOfKeyBind>();

	private void Start()
	{
		oneOfKeyBind = oneOfKeyBindPrefab.GetComponent<OneOfKeyBind>();
		OneOfKeyBind instancekeyBind;
		for(int i = 0; i < numOfKeyBinds; i++)
		{
			instancekeyBind = oneOfKeyBind.Instantiate(oneOfKeyBind, parentForPrefab, i, inputManager, editOptions, optionSetKeyBind);
			keyBindPrefab.Add(instancekeyBind);
		}
	}
}