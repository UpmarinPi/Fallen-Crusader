using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerOff : MonoBehaviour
{
    
    void Start()
    {

#if UNITY_EDITOR
        // �J�[�\���\��
        Cursor.visible = true;
#else
        // �J�[�\����\��
        Cursor.visible = false;
#endif
    }

    
    void Update()
    {
        
    }
}
