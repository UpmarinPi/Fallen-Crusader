using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    IDamageMagic magic;

    private void Start()
	{
        // if arrow doesnt hit anything after 6 seconds destroy itself
        // やが6秒何も当たらなかったら Destroy
        StartCoroutine(DestroyAfter());
    }
	private void OnTriggerEnter2D(Collider2D collision)
    {
        // if arrow collides with player, ground, or a magic attack destroy itself
        // 矢がプレイヤー、地面、や魔法と衝突したら Destroy
        if((magic = collision.gameObject.GetComponent<IDamageMagic>()) != null)
		{
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Ground"))
		{
            Destroy(this.gameObject);
        }           

    }

    // after a set time destroy object
    // 一定時間後 destroy
    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(6f);
        Destroy(this.gameObject);
    }

}
