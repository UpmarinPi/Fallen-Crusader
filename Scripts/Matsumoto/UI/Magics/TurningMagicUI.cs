using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * 　追記
 * これは演出がない方が良いと判断したため未実装に終わりましたが、頑張ったので追加しています。
 * 魔法を新しく入手した際の演出です。グルグル回る魔法UIで、新しく入手したときに高速回転していつの間にか新魔法に入れ替わっている演出です。
 * 合計で回る角度、合計時間からsin^2に角度が変化します。
 * CalcTurnSpeed()の計算をするため、友達などから微積分を習い、3日かけてこのスクリプトを作ることができました。
 */
public class TurningMagicUI : MonoBehaviour
{
	RectTransform thisRT;
	[SerializeField] int turnCount;//何周するか
	[SerializeField] float totalTime;//回り終わるまでにどれぐらい時間をかけるか
	float nowTime;//現在の時間。deltatimeを加算していく
	public float turnAngle;

	Vector3 axis;//回転軸
	[SerializeField] bool turnFlag;

	private void Start()
	{
		thisRT = gameObject.GetComponent<RectTransform>();
		turnFlag = false;
		nowTime = 0;
		turnAngle = 0;
		axis = new Vector3(0f, 0f, 1f);
	}
	private void Update()
	{

		if(turnFlag)
		{
			if(nowTime <= totalTime)
			{

			}
			else
			{
				turnFlag = false;
				nowTime = 0;
				turnAngle = 0;
			}
		}
	}
	private void FixedUpdate()
	{
		if(turnFlag)
		{
			nowTime += Time.fixedDeltaTime;
			Vector3 test = thisRT.rotation.eulerAngles;
			test += new Vector3(0, 0, CalcTurnSpeed(nowTime, turnCount, totalTime)*Time.fixedDeltaTime);
			thisRT.rotation = Quaternion.Euler(test);
		}
		else
		{
			thisRT.localRotation = new Quaternion(0, 0, 0, 0);
		}
	}


	//どれだけ回転するかの計算。(現時刻,何周回すか,回り終わるまでの時間。現時間のときどれだけ回転するかを返す)
	float CalcTurnSpeed(float _nowTime, int _turnCount, float _totalTime)
	{
		if(_nowTime > _totalTime)//現在の時間が回り終わりを超えてたら当然0
		{
			return 0;
		}
		// x = _nowTime, c = _turnCount, t = totalTime
		// 4πc/t sin^2(πx/t)	//弧度法
		// 720c/t sin^2(πx/t)	//度数法

		//これは度数法を用いる
		float sin = Mathf.Sin(Mathf.PI * _nowTime/_totalTime);//二乗準備の仮変数
		float a = _turnCount * 720 * sin * sin / _totalTime;
		Debug.Log("calcturnspeed: " + a);
		return a;
	}
}