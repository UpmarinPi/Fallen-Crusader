using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    [SerializeField]
    Image flush;
    bool pushFlag = false;

    [SerializeField] float fadeOutTime = 0.02f;
    [SerializeField] float endOutTime = 0.5f;

    [Header("エンディングロールの最大値")]
    [SerializeField] float maxRollY = 300f;

    
    void Start()
    {
        StartCoroutine(TextUP());
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !pushFlag)
        {
            StartCoroutine(BackToStart());
            pushFlag = true;
        }
    }

    IEnumerator TextUP()
    {
        while(GetComponent<RectTransform>().localPosition.y <= maxRollY)
        {
            transform.Translate(0, 0.01f, 0);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator BackToStart()
    {
        flush.GetComponent<Image>().enabled = true;
        Color flushColor = flush.color;
        flushColor = Color.black;
        for (float i = 0; i <= 1; i += 0.01f)
        {
            flushColor.a = Mathf.Lerp(0f, 1f, i);
            flush.color = flushColor;
            yield return new WaitForSeconds(fadeOutTime);
        }
        yield return new WaitForSeconds(endOutTime);
        SceneManager.LoadScene(0);
    }
}
