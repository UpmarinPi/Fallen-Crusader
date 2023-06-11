using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KeyBindings
{
	//�L�[�o�C���h�S��
	public enum KeyBindNumber
	{
		[InspectorName("left")]
		left = 0,
		[InspectorName("right")]
		right = 1,
		[InspectorName("up")]
		up = 2,
		[InspectorName("down")]
		down = 3,
		[InspectorName("jump")]
		jump = 4,
		[InspectorName("attack")]
		attack = 5,
		[InspectorName("magic")]
		magic = 6,
		[InspectorName("selectMagic")]
		selectSkill = 7,
		[InspectorName("dash")]
		dash = 8,
		[InspectorName("search")]
		search = 9,
		[InspectorName("skip")]
		skip = 10,
		[InspectorName("next")]
		next = 11,
		[InspectorName("pause")]
		pause = 12
	}

	//�L�[�o�C���h�������邩
	public readonly static int numOfKeyBinds = System.Enum.GetValues(typeof(KeyBindNumber)).Length;
}