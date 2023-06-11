using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicFinish : MonoBehaviour
{
    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(DestroyTime());
    }

    IEnumerator DestroyTime()
    {
        yield return new WaitForAnimation(anim, 0);

        Destroy(gameObject);
    }
}
