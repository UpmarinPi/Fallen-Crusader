using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    Animator anim;
    
    void Start()
    {
        anim = GetComponent<Animator>();
        StartCoroutine(WaitEffect());
    }

    
    void Update()
    {
        
    }

    IEnumerator WaitEffect()
	{
        yield return new WaitForAnimation(anim, 0);
        Destroy(gameObject);
	}
}
