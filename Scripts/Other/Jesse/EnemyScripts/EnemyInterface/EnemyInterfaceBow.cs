using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInterfaceBow : MonoBehaviour, IDamageEnemy
{
    int dmg = 15;
    float stn = 0.2f;
    Vector2 knock = new Vector2(2f, 2f);


    // bow enemy damage information function
    // ‹|‚Ì“G‚ÌUŒ‚î•ñŠÖ”
    public void GetDamage(out int damage, out float stun, out Vector2 knockback, out int scaleX)
    {
        if(transform.localScale.x > 0)
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
