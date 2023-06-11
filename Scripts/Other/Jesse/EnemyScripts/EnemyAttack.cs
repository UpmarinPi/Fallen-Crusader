using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    bool playerInRangeFlag = false;
    bool chargeHit = false;
	[SerializeField] bool isChargeAttack = false;

	// detect if player is in attack range
	// プレイヤーが攻撃の範囲内に入ってるか見る
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
            playerInRangeFlag = true;
		}
	}

	// detect if charge hit connected
	// 突撃攻撃があったったか確認する
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && isChargeAttack)
		{
			chargeHit = true;
		}
	}

	// detect if player left attack range
	// プレイヤーが攻撃の範囲内から出たかを見る
	private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRangeFlag = false;
		}
	}


	
    public bool AttackRange
	{
        get
		{
			return playerInRangeFlag;
		}
        set
		{
			playerInRangeFlag = value;
		}

	}

	public bool getChargeHit
	{
		get
		{
			return chargeHit;
		}
		set
		{
			chargeHit = value;
		}


	}
}
