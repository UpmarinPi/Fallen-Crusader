using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectBar : MonoBehaviour
{
	Slider slider;
	[SerializeField] HPBar hpBar;
	[SerializeField] ManaBar manaBar;

	[Header("�`�F�b�N��hpBar�A�`�F�b�N�Ȃ���manaBar�I���Ƃ݂�")]
	[SerializeField] bool hpBarFlag;

	[SerializeField] int effectStart;   //�������n�߂�܂ł̎���
	[SerializeField] float effectSpeed; //������

	int effectStartTimer;   //effectStart�v���p�^�C�}�[

	float currentValue;     //���݂�HP
	float effectValue;      //���X�Ɍ�������HP�̒l�B���̒l���o�[�ɎQ�Ƃ����
	float maxValue;

	bool changeFlag;
	public void SetChangeFlag(float _changeValue)
	{

		changeFlag = true;
		currentValue = _changeValue * maxValue;
	}


	void Start()
	{
		slider = this.GetComponent<Slider>();
		if (hpBarFlag)
		{
			currentValue = hpBar.CurrentValue;
			maxValue = hpBar.maxValue;
		}
		else
		{
			currentValue = manaBar.CurrentValue;
			maxValue = manaBar.maxValue;
		}
		effectValue = currentValue;
		effectStartTimer = 0;
	}

	private void FixedUpdate()
	{
		if (changeFlag)
		{
			//Debug.Log(effectValue);
			if (currentValue >= effectValue)//HP���񕜂������������͒ǂ��������̏���
			{
				effectValue = currentValue;//�G�t�F�N�g�����݂ɍ��킹��
				effectStartTimer = 0;
				changeFlag = false;
			}
			else if (effectStartTimer < effectStart)
			{
				effectStartTimer++;
			}
			else if (currentValue < effectValue)
			{
				effectValue -= effectSpeed;
			}
			slider.value = effectValue / maxValue;
		}
	}
}
