using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignBoard : MonoBehaviour
{
    GameObject pin;
    
    void Start()
    {
        pin = transform.GetChild(0).gameObject;
        pin.transform.GetComponent<SpriteRenderer>().enabled = false;
    }

    
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            pin.transform.GetComponent<SpriteRenderer>().enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            pin.transform.GetComponent<SpriteRenderer>().enabled = false;
        }

    }
}
