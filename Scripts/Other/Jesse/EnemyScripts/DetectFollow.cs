using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectFollow : MonoBehaviour
{
    public int count = 0;
    int timeOut = 100;
    bool detected = false;
    bool detectedAgain = false;
    bool InRange = false;
    bool lineTest = false;
    bool inSight = false;
    [SerializeField] LayerMask groundMask;


    
    void FixedUpdate()
    {
        // player detection proces
        // プレイヤーを索敵する関数
        if (detected)
		{
            // if detect player again reset count
            // またプレイヤー索敵したらカウントをリセットする
            if (detectedAgain)
			{
                count = 0;
                detectedAgain = false;
                InRange = true;
			}
            
            // if no longer in range of enemy start counting
            // 索敵範囲外になったら数え始める
            if (!InRange)
			{
                count++;
			}
            else if (InRange)
			{
                count = 0;
			}
		}

        // if enemy lost sight of player for too long, stop following
        // 敵がプレイヤー見失ってから時間がたったら追うのをやめる
        if (count == timeOut)
		{
            count = 0;
            detected = false;
        }
    }

    // function to see if player is still detected
    // プレイヤーがまだ索敵されているか確認する関数
    public bool GetPlayerDetect()
	{
        return detected;
	}

    // function to reset detection count and keep detecting player
    // プレイヤー索敵カウントをリセットして索敵し続ける関数
    public void keepDetect()
	{
        detected = true;
        count = 0;
	}
    
    // function to see if player is in visual range
    // プレイヤーが直視できる範囲であるか確認
    public bool CanSeePlayer()
	{
        return inSight;
	}

    // detect if player is in visual range (detect if there are obstacles)
    // プレイヤーが直視できたら索敵する (障害物があれば索敵できない)
    private void OnTriggerStay2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Player")
		{
            lineTest = Physics2D.Linecast(transform.position, collision.transform.position, groundMask);
            if (!lineTest)
			{
                Debug.DrawLine(transform.position, collision.transform.position, Color.red);
                detected = true;
                detectedAgain = true;
                InRange = true;
                inSight = true;
			}
            else
			{
                inSight = false;
			}
		}
	}

    // if player leaves detection range make flag false
    // 索敵範囲から出たらフラグをfalse
	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Player")
        {
            InRange = false;
        }
    }
}
