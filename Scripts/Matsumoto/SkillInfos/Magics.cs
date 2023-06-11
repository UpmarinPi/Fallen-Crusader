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


	//スキルの名前
	public string GetName()
	{
		return magicName;
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