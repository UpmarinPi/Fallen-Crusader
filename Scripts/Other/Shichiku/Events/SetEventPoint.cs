using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GAME_EVENT
{
    [Header("ゲームイベントポイントのポジション")]
    public Vector3 eventPointsPos;
    public enum EventType
	{
        Boss = 0,
        Kinomi,
        Goal
	}
    [Header("ゲームイベントの形式(0:ボスイベント, 1:呪いの実イベント, 2:目標達成イベント)")]
    public EventType eventType;
    [Header("ゲームイベントの番号")]
    public int eventNumber;
    //[System.NonSerialized]
    [Header("第２ポジション(プレイヤーを変更したい位置など)")]
    public Vector3 secondPos;
    [Header("第３ポジション(カメラの移動範囲)など")]
    public Vector3 thirdPos;
}

public class SetEventPoint : MonoBehaviour
{
    [Header("ボスイベントプレハブ")]
    public GameObject eventPointPrefab;
    [Header("木の実イベントプレハブ")]
    public GameObject eventPointPrefab2;
    [Header("ゴール型プレハブ")]
    public GameObject eventPointPrefab3;

    [Header("ゲームイベント設定")]
    public List<GAME_EVENT> eventPoints = new List<GAME_EVENT>();

    int i = 0;

    // チェックポイントの設置
    void Start()
    {   
        foreach(GAME_EVENT points in eventPoints)
        {
            switch((int)points.eventType)
			{
                case 0:
                    var eventPoint1 = Instantiate(eventPointPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    eventPoint1.name = "BossPoint" + i;
                    eventPoint1.transform.parent = transform;
                    eventPoint1.transform.position = points.eventPointsPos;
                    eventPoint1.GetComponent<EventNumber>().GetSetEventNumber = points.eventNumber;
                    eventPoint1.GetComponent<EventNumber>().GetSetSecondPos = points.secondPos;
                    eventPoint1.GetComponent<EventNumber>().GetSetThirdPos = points.thirdPos;
                    break;
                case 1:
                    var eventPoint2 = Instantiate(eventPointPrefab2, new Vector3(0, 0, 0), Quaternion.identity);
                    eventPoint2.name = "KinomiPoint" + i;
                    eventPoint2.transform.parent = transform;
                    eventPoint2.transform.position = points.eventPointsPos;
                    eventPoint2.GetComponent<EventNumber>().GetSetEventNumber = points.eventNumber;
                    eventPoint2.GetComponent<EventNumber>().GetSetSecondPos = points.secondPos;
                    eventPoint2.GetComponent<EventNumber>().GetSetThirdPos = points.thirdPos;
                    break;
                case 2:
                    var eventPoint3 = Instantiate(eventPointPrefab3, new Vector3(0, 0, 0), Quaternion.identity);
                    eventPoint3.name = "GoalPoint" + i;
                    eventPoint3.transform.parent = transform;
                    eventPoint3.transform.position = points.eventPointsPos;
                    eventPoint3.GetComponent<EventNumber>().GetSetEventNumber = points.eventNumber;
                    eventPoint3.GetComponent<EventNumber>().GetSetSecondPos = points.secondPos;
                    eventPoint3.GetComponent<EventNumber>().GetSetThirdPos = points.thirdPos;
                    break;
                default:
                    break;
			}
            i++;
        }
    }
}
