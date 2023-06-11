using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IDamageEnemy
{
    void GetDamage(out int damage, out float stun, out Vector2 knockback, out int scaleX);

}


public class EnemyInterfaceMelee : MonoBehaviour, IDamageEnemy
{

    int dmg = 15;
    float stn = 0.25f;
    Vector2 knock = new Vector2(2.5f, 2.5f);

    // sword enemy damage information function
    // Œ•‚Ì“G‚ÌUŒ‚î•ñŠÖ”
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
        damage = dmg;
        stun = stn;
        knockback = knock;
    }

}
