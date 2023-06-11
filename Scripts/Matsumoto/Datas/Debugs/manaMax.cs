using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class manaMax : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] MagicController magicController;

	private void Update()
	{
		magicController.mana = 100;
	}
#endif
}
