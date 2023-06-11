using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashInterface : MonoBehaviour, IDamageSword
{

	private void Start()
	{
		
	}

	// dash attack info
	// ƒ_ƒbƒVƒ…‚ÌUŒ‚î•ñ
	public void GetDamage(out int damage, out int mpSteal, out float stun, out Vector2 knockback, out int scaleX, out bool critFlag)
    {
        damage = 1;
        mpSteal = 0;
        stun = 0.1f;
        knockback = new Vector2(2.5f, 1.5f);
        critFlag = false;
		if(transform.parent.parent.localScale.x > 0)
		{
			scaleX = 1;
		}
		else
		{
			scaleX = -1;
		}
	}


}
