using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Passives", menuName = "Create Passive")]
public class Passives : ScriptableObject
{
	[SerializeField]
	private string passiveName = "";
	[SerializeField]
	[MultilineAttribute]
	private string information = "";
	[SerializeField]
	private Sprite iconImage;
	[SerializeField]
	private int order;
	[SerializeField]
	private bool useFlag;


	//�X�L���̖��O
	public string GetName()
	{
		return passiveName;
	}

	//�X�L�����
	public string GetInformation()
	{
		return information;
	}

	//���@�A�C�R��
	public Sprite GetIconImage()
	{
		return iconImage;
	}

	//�\������Ƃ����Ԗڂ�
	public int GetOrder()
	{
		return order;
	}

	public bool UseFlag
	{
		get
		{
			return this.useFlag;
		}
		set
		{
			this.useFlag = value;
		}
	}
}