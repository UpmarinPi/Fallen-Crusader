using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    bool playerInRangeFlag = false;
    bool chargeHit = false;
	[SerializeField] bool isChargeAttack = false;

	// detect if player is in attack range
	// �v���C���[���U���͈͓̔��ɓ����Ă邩����
	private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player"))
		{
            playerInRangeFlag = true;
		}
	}

	// detect if charge hit connected
	// �ˌ��U�����������������m�F����
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Player") && isChargeAttack)
		{
			chargeHit = true;
		}
	}

	// detect if player left attack range
	// �v���C���[���U���͈͓̔�����o����������
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
