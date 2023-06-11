using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayOption : MonoBehaviour
{
	[SerializeField]Pause pause;
	public bool selectingOptionFlag;
	bool pushingFlag;

	
	void Update()
	{
		if(selectingOptionFlag)
		{
			transform.GetChild(0).gameObject.SetActive(true);
			if(Input.GetKey(KeyCode.Escape))
			{
				if(!pushingFlag)
				{
					selectingOptionFlag = false;
					pause.moveFlag = true;
					transform.GetChild(0).gameObject.SetActive(false);
				}
				pushingFlag = true;
			}
			else
			{
				pushingFlag = false;
			}
		}
	}
}
