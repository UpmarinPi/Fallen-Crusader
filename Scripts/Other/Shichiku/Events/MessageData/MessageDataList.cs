using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessageDataBase", menuName = "Create MessageDataList")]
public class MessageDataList : ScriptableObject
{
    [Header("���b�Z�[�W�\��(�ŏ���2�̓r�M�j���O�ƃG���f�B���O)")]
    public MESSAGE[] messageConstitution;
}
