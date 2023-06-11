using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAfterDash : MonoBehaviour
{
	[SerializeField]
	PlayerController playerController;
	[SerializeField]
	DashCoolDownUI dashCoolDownUI;

	// ����̃N�[���_�E��
	readonly float dashCoolDown = 1.2f;
	float countCoolDown = 1.2f;
	bool startCooldownFlag = false;

	public float TimeCoolDown//�N�[���_�E������ǂ�قǌo���Ă��邩
	{
		get
		{
			if (countCoolDown / dashCoolDown < 1.0f)
				return countCoolDown / dashCoolDown;
			else
				return 1.1f;
		}
	}


	//�N�[���_�E�����I���܂�false��Ԃ�
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
