using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInterfaceLance : MonoBehaviour, IDamageEnemy
{
    int dmg = 25;
    float stn = 0.25f;
    Vector2 knock = new Vector2(3.5f, 2f);

    // lance enemy damage information function
    // ‚â‚è‚Ì“G‚ÌUŒ‚î•ñŠÖ”
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
