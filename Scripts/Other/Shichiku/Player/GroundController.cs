using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    PlayerController player;
    [SerializeField] LayerMask Ground;

    bool groundFlag = false;

    bool Se = false;
    bool Sw = false;

    [SerializeField] Vector2 Se_Start_Vector;
    [SerializeField] Vector2 Se_End_Vector;
    [SerializeField] Vector2 Sw_Start_Vector;
    [SerializeField] Vector2 Sw_End_Vector;

    float Se_Start_X;
    float Se_Start_Y;
    float Se_End_X;
    float Se_End_Y;

    float Sw_Start_X;
    float Sw_Start_Y;
    float Sw_End_X;
    float Sw_End_Y;

    private void Start()
    {

        Se_Start_X = Se_Start_Vector.x;
        Se_Start_Y = Se_Start_Vector.y;
        Se_End_X   = Se_End_Vector.x;
        Se_End_Y   = Se_End_Vector.y;

        Sw_Start_X = Sw_Start_Vector.x;
        Sw_Start_Y = Sw_Start_Vector.y;
        Sw_End_X   = Sw_End_Vector.x;
        Sw_End_Y   = Sw_End_Vector.y;

        player = transform.parent.GetComponent<PlayerController>();
    }
	private void Update()
	{
        GroundLineSensor();
	}

	private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            
            //GroundLineSensor();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ground"))
        {
            
            //GroundLineSensor();
        }
    }

    void GroundLineSensor()
	{
        /*if(transform.parent.localScale.x == -1)
		{
            transform.localScale = new Vector3(-1, 1, 1);
		}
        else
		{
            transform.localScale = new Vector3(1, 1, 1);
		}*/

        //ìÏìå-----------------------------------------------------------------------------------------------
        Debug.DrawLine(transform.position + new Vector3(Se_Start_X, Se_Start_Y, 0),
                        transform.position + new Vector3(Se_End_X, Se_End_Y, 0));
        //Se SensorÇÃé¿ëï
        Se = Physics2D.Linecast(transform.position + new Vector3(Se_Start_X, Se_Start_Y, 0),
                            transform.position + new Vector3(Se_End_X, Se_End_Y, 0), Ground);
        //ìÏêº-----------------------------------------------------------------------------------------------
        Debug.DrawLine(transform.position + new Vector3(Sw_Start_X, Sw_Start_Y, 0),
                        transform.position + new Vector3(Sw_End_X, Sw_End_Y, 0));
        //Sw SensorÇÃé¿ëï
        Sw = Physics2D.Linecast(transform.position + new Vector3(Sw_Start_X, Sw_Start_Y, 0),
                            transform.position + new Vector3(Sw_End_X, Sw_End_Y, 0), Ground);

        

        if((Se || Sw) && !groundFlag && player.powerY <= 0 && !player.DashFlag)
		{
            player.GroundEnter();
            groundFlag = true;
        }
        else if((Se || Sw) && groundFlag && player.powerY <= 0 && !player.DashFlag)
		{
            player.GroundStay();
		}
        else
		{
            player.GroundExit();
            groundFlag = false;
        }
    }
}
