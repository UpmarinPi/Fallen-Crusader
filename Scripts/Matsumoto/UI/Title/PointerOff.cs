using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerOff : MonoBehaviour
{
    
    void Start()
    {

#if UNITY_EDITOR
        // カーソル表示
        Cursor.visible = true;
#else
        // カーソル非表示
        Cursor.visible = false;
#endif
    }

    
    void Update()
    {
        
    }
}
