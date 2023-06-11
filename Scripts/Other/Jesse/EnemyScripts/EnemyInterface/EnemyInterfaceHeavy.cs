using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInterfaceHeavy : MonoBehaviour, IDamageEnemy
{
    int dmg = 50;
    float stn = 0.4f;
    Vector2 knock = new Vector2(4f, 3f);

    // hobgoblin enemy damage information function
    // �z�u�S�u�����̓G�̍U�����֐�
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
