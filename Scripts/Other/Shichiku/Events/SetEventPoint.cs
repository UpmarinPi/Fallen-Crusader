using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GAME_EVENT
{
    [Header("�Q�[���C�x���g�|�C���g�̃|�W�V����")]
    public Vector3 eventPointsPos;
    public enum EventType
	{
        Boss = 0,
        Kinomi,
        Goal
	}
    [Header("�Q�[���C�x���g�̌`��(0:�{�X�C�x���g, 1:�􂢂̎��C�x���g, 2:�ڕW�B���C�x���g)")]
    public EventType eventType;
    [Header("�Q�[���C�x���g�̔ԍ�")]
    public int eventNumber;
    //[System.NonSerialized]
    [Header("��Q�|�W�V����(�v���C���[��ύX�������ʒu�Ȃ�)")]
    public Vector3 secondPos;
    [Header("��R�|�W�V����(�J�����̈ړ��͈�)�Ȃ�")]
    public Vector3 thirdPos;
}

public class SetEventPoint : MonoBehaviour
{
    [Header("�{�X�C�x���g�v���n�u")]
    public GameObject eventPointPrefab;
    [Header("�؂̎��C�x���g�v���n�u")]
    public GameObject eventPointPrefab2;
    [Header("�S�[���^�v���n�u")]
    public GameObject eventPointPrefab3;

    [Header("�Q�[���C�x���g�ݒ�")]
    public List<GAME_EVENT> eventPoints = new List<GAME_EVENT>();

    int i = 0;

    // �`�F�b�N�|�C���g�̐ݒu
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
