using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextAnim : MonoBehaviour
{
    Text textToMove;
    [SerializeField]string str;

    
    private void Start()
    {
        textToMove = GetComponent<Text>();
        textToMove.text = "";
        this.enabled = false;
    }
    void OnEnable()
    {
        if(textToMove != null)
            StartCoroutine(TextMove(str));
    }
    IEnumerator TextMove(string s)
    {
        textToMove.text = s;

        int maxCount = 3;
        for(int i = 0; i < 4; i++)
        {
            for (int j = 0; j < maxCount; j++)
            {
                yield return new WaitForSeconds(0.3f);
                textToMove.text += ".";
            }
            if (i == 3) break;
            yield return new WaitForSeconds(0.2f);

            textToMove.text = s;
        }
        
        textToMove.text = "";
        this.enabled = false;
    }

    
    void Update()
    {
        
    }
}
