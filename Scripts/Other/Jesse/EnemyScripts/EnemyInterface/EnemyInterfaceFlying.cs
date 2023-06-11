using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInterfaceFlying : MonoBehaviour, IDamageEnemy
{
    int dmg = 30;
    float stn = 0.3f;
    Vector2 knock = new Vector2(3f, 2.5f);

    // flying enemy damage information function
    // ”ò‚Ô“G‚ÌUŒ‚î•ñŠÖ”
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
