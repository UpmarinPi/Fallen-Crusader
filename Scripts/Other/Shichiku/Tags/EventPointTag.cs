using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventPointTag : MonoBehaviour
{
	public enum TagType
	{
		Talk = 0,
		Read,
		Watch,
		Special,
		Boss,
		Kinomi,
		Goal
	}
	[Header("タグタイプ")]
	[SerializeField]TagType tagType;

	public TagType GetTag{ get { return tagType; } }

	public bool ComparePointTag(TagType tagType)
	{
		return tagType == this.tagType;
	}
}
