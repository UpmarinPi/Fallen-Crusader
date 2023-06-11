using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    bool enemyGroundFlag = false;

    // if touching ground flag is true
    // 地面と接触していればフラグをtrue
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyGroundFlag = true;
        }
    }

    // if no longer touching ground flag is false
    // 地面と離れたらフラグを false
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enemyGroundFlag = false;
        }
    }

    // function to check if grounded
    // 地面と接触してるか確認穂る関数
    public bool GetEnemyGroundedFlag()
    {
        return enemyGroundFlag;
    }



}
