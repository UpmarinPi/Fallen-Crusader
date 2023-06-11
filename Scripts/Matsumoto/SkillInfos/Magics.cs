using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Magics", menuName = "Create Magic")]
public class Magics : ScriptableObject
{
	[SerializeField]
	private string magicName = "";
	[SerializeField]
	[MultilineAttribute]
	private string information = "";
	[SerializeField]
	private Sprite iconImage;
	[SerializeField]
	private int order;
	[SerializeField]
	private int needMana;
	[SerializeField]
	private bool useFlag;


	//�X�L���̖��O
	public string GetName()
	{
		return magicName;
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

	public int GetNeedMana()
	{
		return needMana;
	}

	public bool UseFlag
	{
		get { return this.useFlag; }
		set { this.useFlag = value; }
	}
}