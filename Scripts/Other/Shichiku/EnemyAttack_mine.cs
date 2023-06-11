using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack_mine: MonoBehaviour
{
    Transform myParent;
    Transform attackCol;
    Animator anim;

    bool timeFlag = false;

    const float TIME = 2.0f;

    // éÊìæÇ∆èâä˙âª
    void Start()
    {
        myParent = transform.parent;
        attackCol = myParent.transform.Find("AttackCollision");
        anim = myParent.GetComponent<Animator>();
        timeFlag = true;
    }

    
    void Update()
    {

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player") && timeFlag)
        {
            attackCol.tag = "Enemies";
            timeFlag = false;
            StartCoroutine(WaitAnim());
            Invoke("OnTime", TIME);
        }
    }

    IEnumerator WaitAnim()
    {
        anim.SetTrigger("AttackState");
        yield return null;
        yield return new WaitForAnimation(anim, 0);
        attackCol.tag = "Player";
    }

    void OnTime()
    {
        timeFlag = true;
    }
}
