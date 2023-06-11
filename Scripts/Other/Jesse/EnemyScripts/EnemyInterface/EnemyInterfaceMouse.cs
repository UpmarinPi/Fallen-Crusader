using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInterfaceMouse : MonoBehaviour, IDamageEnemy
{

    public bool tackleFlag  { get; set; }
    int damageType = 0;
    int dmg = 20;
    int dmgTackle = 10;
    float stn = 0.2f;
    float stnTackle = 0.35f;
    Vector2 knock = new Vector2(2f, 2f);
    Vector2 knockTackle = new Vector2(4.5f, 3f);

    // mouse enemy damage information function
    // ƒlƒYƒ~‚Ì“G‚ÌUŒ‚î•ñŠÖ”
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
            damage = dmg;
            stun = stn;
            knockback = knock;
		}
        else
		{
            damage = dmgTackle;
            stun = stnTackle;
            knockback = knockTackle;
        }

    }


    public void SetAttack(int attackNumber)
	{
        damageType = attackNumber;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
            tackleFlag = true;
		}
	}


}
