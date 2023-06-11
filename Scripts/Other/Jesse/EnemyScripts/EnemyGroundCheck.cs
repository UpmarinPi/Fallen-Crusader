using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    bool enemyGroundFlag = false;

    // if touching ground flag is true
    // �n�ʂƐڐG���Ă���΃t���O��true
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyGroundFlag = true;
        }
    }

    // if no longer touching ground flag is false
    // �n�ʂƗ��ꂽ��t���O�� false
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyGroundFlag = false;
        }
    }

    // function to check if grounded
    // �n�ʂƐڐG���Ă邩�m�F���֐�
    public bool GetEnemyGroundedFlag()
    {
        return enemyGroundFlag;
    }



}
