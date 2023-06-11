using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static KeyBindings;

//�L�[��ݒ肷��X�N���v�g�BOneOfKeyBind�N���X��������
public class OptionSetKeyBind : MonoBehaviour
{
	string keyName;//�L�[�̖��O�B"�W�����v","�E"�Ȃ�
	[SerializeField] Text infoText;//�u�Z�Z����͂��Ă��������v
	KeyBindNumber keyBindNumber;//���̃L�[�Ɋւ��ċ��߂Ă��邩
	KeyCode2 oldKey;//�ύX�O�̃L�[�B
	public KeyCode2 SetOldKey
	{
		set
		{
			oldKey = value;
		}
	}
	public KeyCode2 returnKey;//�Ԃ��Ă����L�[�B���key�ɂ��ꂪ�K�p�����

	public bool searchingKey;//�L�[���͑҂���ԁB�L�[����͂������̂�returnKey�ɕԂ����
	public bool endSearchFlag;//returnKey���X�V���ꂽ���Ƃ�`����

	private void Start()
	{
		endSearchFlag = false;
	}
	private void Update()
	{
		if(searchingKey && Input.anyKeyDown)//�L�[���͑҂���Ԃŉ����L�[�����͂��ꂽ�Ƃ�
		{
			foreach(KeyCode code in System.Enum.GetValues(typeof(KeyCode)))//�S�L�[�𒲂ׂ�
			{
				if(Input.GetKeyDown(code))//�����T�����ꂽ�L�[�ƈ�v������
				{
					if(!code.ToString().Contains("Joystick"))//Joystick�ȊO�Ȃ�
					{
						if(code == KeyCode.Escape)//Esc�L�[�̎��͌��X�̃L�[��
						{
							returnKey = oldKey;
						}
						else//����ȊO�Ȃ牟���ꂽ�{�^����
						{
							returnKey = KeyCode2Class.TransformCode(code);//���͂��ꂽ�L�[��KeyCode2�ɕϊ����ĕԂ�
						}
						searchingKey = false;
						endSearchFlag = true;
						break;
					}
				}
			}
		}

	}
}
