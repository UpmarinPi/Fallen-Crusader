using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideInPause : MonoBehaviour
{
	public bool activateFlag;
	bool changeFlag;
	[SerializeField]
	Pause pause;
	void Update()
	{
		if(!activateFlag)//activateFlag‚ªƒIƒt‚Ì‚Æ‚«‚Í“®ì‚³‚¹‚È‚¢
		{
			return;
		}
		if(pause.pauseFlag != changeFlag)
		{
			changeFlag = pause.pauseFlag;
			if(pause.pauseFlag)
			{
				for(int i = 0; i < this.gameObject.transform.childCount; i++)
				{
					this.gameObject.transform.GetChild(i).gameObject.SetActive(false);
				}
			}
			else
			{
				for(int i = 0; i < this.gameObject.transform.childCount; i++)
				{
					this.gameObject.transform.GetChild(i).gameObject.SetActive(true);
				}
			}
		}
	}
}
