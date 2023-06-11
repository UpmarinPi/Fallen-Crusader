using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBolt : MonoBehaviour, IDamageMagic
{

    private void Start()
    {
        StartCoroutine(DestroyAfter());
    }

    // magic slash attack info
    // –‚–@ŽaŒ‚UŒ‚î•ñ
    public void GetDamage(out int damage, out float stun, out Vector2 knockback, out int scaleX)
	{
        damage = 15;
        stun = 0.2f;
        knockback = new Vector2(2.5f, 1.5f);
        if(transform.localScale.x > 0)
        {
            scaleX = 1;
        }
        else
        {
            scaleX = -1;
        }
    }

    // after a set time destroy object
    // ˆê’èŽžŠÔŒã destroy
    IEnumerator DestroyAfter()
	{
        yield return new WaitForSeconds(6f);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
            Destroy(gameObject);
    }

}
