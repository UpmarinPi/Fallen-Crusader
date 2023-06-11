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
        // �v���C���[�����G����֐�
        if (detected)
		{
            // if detect player again reset count
            // �܂��v���C���[���G������J�E���g�����Z�b�g����
            if (detectedAgain)
			{
                count = 0;
                detectedAgain = false;
                InRange = true;
			}
            
            // if no longer in range of enemy start counting
            // ���G�͈͊O�ɂȂ����琔���n�߂�
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
        // �G���v���C���[�������Ă��玞�Ԃ���������ǂ��̂���߂�
        if (count == timeOut)
		{
            count = 0;
            detected = false;
        }
    }

    // function to see if player is still detected
    // �v���C���[���܂����G����Ă��邩�m�F����֐�
    public bool GetPlayerDetect()
	{
        return detected;
	}

    // function to reset detection count and keep detecting player
    // �v���C���[���G�J�E���g�����Z�b�g���č��G��������֐�
    public void keepDetect()
	{
        detected = true;
        count = 0;
	}
    
    // function to see if player is in visual range
    // �v���C���[�������ł���͈͂ł��邩�m�F
    public bool CanSeePlayer()
	{
        return inSight;
	}

    // detect if player is in visual range (detect if there are obstacles)
    // �v���C���[�������ł�������G���� (��Q��������΍��G�ł��Ȃ�)
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
    // ���G�͈͂���o����t���O��false
	private void OnTriggerExit2D(Collider2D collision)
	{
        if (collision.gameObject.tag == "Player")
        {
            InRange = false;
        }
    }
}
