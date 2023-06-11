using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{

	Pause pause;
	[SerializeField]
	Pause.PauseChoice thisCursor;
	[SerializeField]
	GameObject GOpause;
	
	void Start()
	{
		pause = GOpause.transform.GetComponent<Pause>();
		GetComponent<Text>().color = Color.white;
	}

	
	void Update()
	{
		if (pause.select == thisCursor)
		{
			GetComponent<Text>().color = Color.white;
		}
		else
		{
			GetComponent<Text>().color = Color.gray;
		}
	}
}
