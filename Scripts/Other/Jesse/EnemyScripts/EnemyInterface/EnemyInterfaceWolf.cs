using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInterfaceWolf : MonoBehaviour, IDamageEnemy
{
    int dmgSlash = 30;
    int dmgRun = 10;
    int dmgJumpSlash = 40;
    float stnSlash = 0.3f;
    float stnRun = 0.15f;
    float stnJumpSlash = 0.4f;
    Vector2 knockSlash = new Vector2(3f, 3f);
    Vector2 knockRun = new Vector2(2f, 1f);
    Vector2 knockJumpSlash = new Vector2(4f, 3f);
    int damageType = 1;


    // wolf enemy damage information function
    // ˜T‚ÌUŒ‚î•ñŠÖ”
    public void GetDamage(out int damage, out float stun, out Vector2 knockback, out int scaleX)
    {
        if(transform.parent.localScale.x > 0)
        {
            scaleX = 1;
        }
        else
        {
            scaleX = -1;
        }

        if (damageType == 1)
		{
            damage = dmgSlash;
            stun = stnSlash;
            knockback = knockSlash;
        }
        else if (damageType == 2)
		{
            damage = dmgRun;
            stun = stnRun;
            knockback = knockRun;
        }
        else
		{
            damage = dmgJumpSlash;
            stun = stnJumpSlash;
            knockback = knockJumpSlash;
        }
    }

    public void setAttack(int atackNumber)
	{
        damageType = atackNumber;
	}
    



}
