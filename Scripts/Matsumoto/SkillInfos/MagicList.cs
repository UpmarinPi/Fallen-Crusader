using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagicDataBase", menuName = "Create Magic lists")]
public class MagicList : ScriptableObject
{
	[SerializeField]
	private List<Magics> magicLists = new List<Magics>();

	public List<Magics> GetMagicLists()
	{
		return magicLists;
	}
	public Magics GetMagicListOne(int _magicNumber)
	{
		return magicLists[_magicNumber];
	}
	public int GetMagicCount()
	{
		return magicLists.Count;
	}
}
