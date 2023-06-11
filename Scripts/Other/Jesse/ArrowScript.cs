using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    IDamageMagic magic;

    private void Start()
	{
        // if arrow doesnt hit anything after 6 seconds destroy itself
        // �₪6�b����������Ȃ������� Destroy
        StartCoroutine(DestroyAfter());
    }
	private void OnTriggerEnter2D(Collider2D collision)
    {
        // if arrow collides with player, ground, or a magic attack destroy itself
        // ��v���C���[�A�n�ʁA�▂�@�ƏՓ˂����� Destroy
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
    // ��莞�Ԍ� destroy
    IEnumerator DestroyAfter()
    {
        yield return new WaitForSeconds(6f);
        Destroy(this.gameObject);
    }

}
