using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretChildVer : MonoBehaviour
{
	public int fadeSpeed = 10;
	[SerializeField] int alpha;      //画像のアルファ値。透明度を操作
	bool insideFlag;//プレイヤーが入っているかの判定。enter時とexit時の持続処理に使用
	Tilemap tilemap;
	Color32 color;

	
	void Start()
	{
		alpha = 255;                        //初期値設定
		insideFlag = false;
		tilemap = transform.GetChild(0).GetComponent<Tilemap>();
		color = tilemap.color;
	}

	private void Update()
	{
		if(insideFlag)                      //プレイヤーな中に入るとalpha値を最小0まで、fadespeedずつ下げる
		{
			alpha = Mathf.Clamp(alpha - fadeSpeed, 0, 255);
		}
		else                                //プレイヤーが外に出るとalpha値を最大255まで、fadespeedずつ上げる
		{
			alpha = Mathf.Clamp(alpha + fadeSpeed, 0, 255);
		}
	}
	private void FixedUpdate()
	{
		tilemap.color = new Color32(color.r, color.g, color.b, (byte)alpha);//実際の透過値指定
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))//プレイヤーが入った判定
		{
			insideFlag = true;
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))//プレイヤーが出た判定
		{
			insideFlag = false;
		}
	}
}
