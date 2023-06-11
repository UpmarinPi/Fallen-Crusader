using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveDataBase", menuName = "Create Passive lists")]
public class PassiveList : ScriptableObject
{
	[SerializeField]
	private List<Passives> passiveLists = new List<Passives>();

	public List<Passives> GetPassiveLists()
	{
		return passiveLists;
	}
	public Passives GetPassiveListOne(int _passiveNumber)
	{
		return passiveLists[_passiveNumber];
	}
	public int GetPassiveCount()
	{
		return passiveLists.Count;
	}
}
