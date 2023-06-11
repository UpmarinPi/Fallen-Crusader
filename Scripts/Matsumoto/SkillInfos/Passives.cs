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


	//スキルの名前
	public string GetName()
	{
		return passiveName;
	}

	//スキル情報
	public string GetInformation()
	{
		return information;
	}

	//魔法アイコン
	public Sprite GetIconImage()
	{
		return iconImage;
	}

	//表示するとき何番目か
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