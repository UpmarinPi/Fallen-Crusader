using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct DamageData
{
    public int dmg;
    public float stn;
    public Vector2 knock;
}

public interface IDamageBall
{
    public int GetDamage();
}
public class GimmickBallAttackController : MonoBehaviour, IDamageEnemy, IDamageBall
{
    [SerializeField] DamageData damageData = new DamageData { dmg = 15, stn = 0.25f, knock = new Vector2(2.5f, 2.5f) };

    // IDamageEnemy‚ÌGetDamage()
    public void GetDamage(out int damage, out float stun, out Vector2 knockback, out int scaleX)
    {
        if (transform.parent.localScale.x > 0)
        {
            scaleX = -1;
        }
        else
        {
            scaleX = 1;
        }
        damage = damageData.dmg;
        stun = damageData.stn;
        knockback = damageData.knock;
    }

    // IDamageBall‚ÌGetDamage()
    public int GetDamage()
    {
        DestroyBall();
        return damageData.dmg;
    }

    void DestroyBall()
    {
        StartCoroutine(transform.parent.GetComponent<GimmickBallController>().Explosion());
    }
}
