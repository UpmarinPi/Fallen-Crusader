using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBranchController : MonoBehaviour
{
    Rigidbody2D rbody2D;
    
    void Start()
    {
        Random.InitState(System.DateTime.Now.Millisecond);

        rbody2D = GetComponent<Rigidbody2D>();
        rbody2D.AddForce(new Vector2(Random.Range(-3f, 0f), 5), ForceMode2D.Impulse);
        Invoke("InLayer", 0.2f);
        Invoke("BranchDestroy", 4);
    }

    
    void FixedUpdate()
    {
        transform.Rotate(0, 0, Random.Range(1, 2));
    }

    private void BranchDestroy()
    {
        Destroy(gameObject);
    }

    private void InLayer()
    {
        transform.GetComponent<SpriteRenderer>().sortingOrder = 4;
    }
}
