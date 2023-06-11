using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventNumber : MonoBehaviour
{
    int eventNumber;
    int videoNumber;
    Vector3 secondPos;
    Vector3 thirdPos;
    public int GetSetEventNumber
    {
        get
        { 
            return eventNumber;       
        }
        set
        {
            eventNumber = value;
        }
    }
    public Vector3 GetSetSecondPos
	{
		get
		{
            return secondPos;
		}
		set
		{
            secondPos = value;
		}
	}
    public Vector3 GetSetThirdPos
	{
		get
		{
            return thirdPos;
		}
		set
		{
            thirdPos = value;
		}
	}
    public int GetSetVideoNumber
    {
        get
        {
            return videoNumber;
        }
        set
        {
            videoNumber = value;
        }
    }
}
