using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterDash : MonoBehaviour
{
	[SerializeField]
	PlayerController playerController;
	[SerializeField]
	DashCoolDownUI dashCoolDownUI;

	// 回避のクールダウン
	readonly float dashCoolDown = 1.2f;
	float countCoolDown = 1.2f;
	bool startCooldownFlag = false;

	public float TimeCoolDown//クールダウンからどれほど経っているか
	{
		get
		{
			if (countCoolDown / dashCoolDown < 1.0f)
				return countCoolDown / dashCoolDown;
			else
				return 1.1f;
		}
	}


	//クールダウンが終わるまでfalseを返す
	public bool DashCoolDown()
	{
		if (!startCooldownFlag)
		{
			countCoolDown = 0;
			startCooldownFlag = true;
		}
		if (countCoolDown >= dashCoolDown)
		{
			startCooldownFlag = false;
			return true;
		}
		countCoolDown += Time.fixedDeltaTime;
		return false;
	}
}
