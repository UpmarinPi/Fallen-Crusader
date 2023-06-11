using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAllMagics : MonoBehaviour
{
	[SerializeField] EditData editdata;
	
	private void OnEnable()
	{
		for(int i = 0; i < editdata.data.magicFlag.Count; i++)
		{
			editdata.data.magicFlag[i] = true;
		}
		for(int i = 0; i < editdata.data.passiveFlag.Count; i++)
		{
			editdata.data.passiveFlag[i] = true;
		}
	}
}
