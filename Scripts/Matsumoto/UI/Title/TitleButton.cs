using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{

	TitleStart titleStart;
	[SerializeField]
	TitleStart.titleCursor thisCursor;
	[SerializeField]
	GameObject start;
	
	void Start()
	{
		titleStart = start.transform.GetComponent<TitleStart>();
		GetComponent<Image>().color = Color.white;
	}

	
	void Update()
	{
		if (titleStart.cursor == thisCursor)
		{
			GetComponent<Image>().color = Color.white;
		}
		else
		{
			GetComponent<Image>().color = Color.gray;
		}
	}
}
