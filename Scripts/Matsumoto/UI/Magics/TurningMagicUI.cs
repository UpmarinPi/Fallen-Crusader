using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * �@�ǋL
 * ����͉��o���Ȃ������ǂ��Ɣ��f�������ߖ������ɏI���܂������A�撣�����̂Œǉ����Ă��܂��B
 * ���@��V�������肵���ۂ̉��o�ł��B�O���O����閂�@UI�ŁA�V�������肵���Ƃ��ɍ�����]���Ă��̊Ԃɂ��V���@�ɓ���ւ���Ă��鉉�o�ł��B
 * ���v�ŉ��p�x�A���v���Ԃ���sin^2�Ɋp�x���ω����܂��B
 * CalcTurnSpeed()�̌v�Z�����邽�߁A�F�B�Ȃǂ�����ϕ����K���A3�������Ă��̃X�N���v�g����邱�Ƃ��ł��܂����B
 */
public class TurningMagicUI : MonoBehaviour
{
	RectTransform thisRT;
	[SerializeField] int turnCount;//�������邩
	[SerializeField] float totalTime;//���I���܂łɂǂꂮ�炢���Ԃ������邩
	float nowTime;//���݂̎��ԁBdeltatime�����Z���Ă���
	public float turnAngle;

	Vector3 axis;//��]��
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


	//�ǂꂾ����]���邩�̌v�Z�B(������,�����񂷂�,���I���܂ł̎��ԁB�����Ԃ̂Ƃ��ǂꂾ����]���邩��Ԃ�)
	float CalcTurnSpeed(float _nowTime, int _turnCount, float _totalTime)
	{
		if(_nowTime > _totalTime)//���݂̎��Ԃ����I���𒴂��Ă��瓖�R0
		{
			return 0;
		}
		// x = _nowTime, c = _turnCount, t = totalTime
		// 4��c/t sin^2(��x/t)	//�ʓx�@
		// 720c/t sin^2(��x/t)	//�x���@

		//����͓x���@��p����
		float sin = Mathf.Sin(Mathf.PI * _nowTime/_totalTime);//��揀���̉��ϐ�
		float a = _turnCount * 720 * sin * sin / _totalTime;
		Debug.Log("calcturnspeed: " + a);
		return a;
	}
}