using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWarpZone
{
	public Vector3 GetWarpPos();
}

public class WarpController : MonoBehaviour, IWarpZone
{
	[SerializeField] Vector3 warpPos;
	[SerializeField] Vector3 canvasPos;

	private void Start()
	{
		transform.GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>().localPosition = canvasPos;
		transform.GetChild(0).gameObject.SetActive(false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.name == "Player")
		{
			transform.GetChild(0).gameObject.SetActive(true);
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.name == "Player")
		{
			transform.GetChild(0).gameObject.SetActive(false);
		}
	}

	public Vector3 GetWarpPos()
	{
		return warpPos;
	}
}
