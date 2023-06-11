using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MessageDataBase", menuName = "Create MessageDataList")]
public class MessageDataList : ScriptableObject
{
    [Header("メッセージ構成(最初の2つはビギニングとエンディング)")]
    public MESSAGE[] messageConstitution;
}
