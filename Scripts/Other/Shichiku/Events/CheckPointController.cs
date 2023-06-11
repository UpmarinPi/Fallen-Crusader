using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class CheckPointController : MonoBehaviour
{
    Animator anim;
    List<GameObject> childObjects = new List<GameObject>();
    GameObject savingText;
    
    void Start()
    {
        anim = GetComponent<Animator>();

        for(int i = 0; i < transform.childCount; i++)
		{
            childObjects.Add(transform.GetChild(i).gameObject);
            childObjects[i].SetActive(false);
		}
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (anim != null)
        {
            anim.SetBool("Saving", true);
        }
	}

    public void Lighting()
	{
        savingText = GameObject.Find("Saving");
        savingText.GetComponent<TextAnim>().enabled = true;
        foreach(GameObject childObject in childObjects)
		{
            childObject.SetActive(true);
		}
        StartCoroutine(HandLightChange());      
	}
    IEnumerator HandLightChange()
	{
        for(float i = 1; i >= 0; i -= 0.01f)
		{
            childObjects[2].GetComponent<Light2D>().intensity = Mathf.Lerp(2, 4, i);
            yield return new WaitForSeconds(0.02f);
		}
    }
}
