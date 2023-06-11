using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForDebug : MonoBehaviour
{
#if UNITY_EDITOR//unity editor����
	[SerializeField]GameObject debugGB;
	[SerializeField] string debugString = "External";
	int countString;
	void Start()
	{
		countString = 0;
	}
	void Update()
	{
		if(Input.anyKeyDown)//�����̃L�[�������ꂽ�Ƃ�
		{
			if(countString < debugString.Length)//�������L�[�̑��������͂��Ăق����f�o�b�O�p��������Z���Ƃ�
			{
				if(Input.GetKeyDown(debugString[countString].ToString().ToLower()))//�f�o�b�O�p����������Ԓʂ艟���Ă���Ȃ�
				{
					countString++;//1�����₵�Ă���
				}
				else
				{
					countString = 0;//�������{�^�������Z�b�g
				}
			}
		}
		if(countString >= debugString.Length)//���������͂�����
		{
			debugGB.SetActive(true);//�f�o�b�O�p�I�u�W�F�N�g��true�ɂȂ�
		}

	}
#endif
}
