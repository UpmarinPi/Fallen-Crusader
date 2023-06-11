using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TUTORIAL
{
    public string tutorialName;
    [Header("�`���[�g���A���|�C���g�̃|�W�V����")]
    public Vector3 tutorialPointsPos;
    public enum TutorialType
	{
        talk = 0,
        read,
        watch,
        special
	}
    [Header("�`���[�g���A���̌`��(0:��b, 1:�Ŕ�, 2:��, 3:����)")]
    public TutorialType types;
    [Header("�`���[�g���A���ԍ�")]
    public int tutorialNumber;
}

public class SetTutorialPoint : MonoBehaviour
{
    [SerializeField]EditData editData;
    [SerializeField]float tutorialEnd = 119;

    [Header("��b�^�v���n�u")]
    public GameObject tutorialPointPrefab;
    [Header("�Ŕ^�v���n�u")]
    public GameObject tutorialPointPrefab2;
    [Header("�󔠌^�v���n�u")]
    public GameObject tutorialPointPrefab3;
    [Header("����^�v���n�u")]
    public GameObject specialPointPrefab;
    
    [Header("�`���[�g���A���ݒ�")]
    public List<TUTORIAL> tutorialPoints = new List<TUTORIAL>();

    int i = 0;
    int videoNumber = 0;
    bool notFlag = false;

    // �`�F�b�N�|�C���g�̐ݒu
    void Start()
    {
        // ���[�h
        if (editData.data.savePosition.x > tutorialEnd)
        {
            notFlag = true;
        }
        else
        {
            notFlag = false;
        }
        foreach (TUTORIAL points in tutorialPoints)
        {
            switch((int)points.types)
            {
                case 0:
                    if(!notFlag)
                    {
                        var tutorialPoint1 = Instantiate(tutorialPointPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                        tutorialPoint1.name = "TutorialPoint" + i;
                        tutorialPoint1.transform.parent = transform;
                        tutorialPoint1.transform.position = points.tutorialPointsPos;
                        tutorialPoint1.GetComponent<EventNumber>().GetSetEventNumber = points.tutorialNumber;
                    }
                    break;
                case 1:
                    var tutorialPoint2 = Instantiate(tutorialPointPrefab2, new Vector3(0, 0, 0), Quaternion.identity);
                    tutorialPoint2.name = "TutorialPointIta" + i;
                    tutorialPoint2.transform.parent = transform;
                    tutorialPoint2.transform.position = points.tutorialPointsPos;
                    tutorialPoint2.GetComponent<EventNumber>().GetSetEventNumber = points.tutorialNumber;
                    break;
                case 2:
                    var tutorialPoint3 = Instantiate(tutorialPointPrefab3, new Vector3(0, 0, 0), Quaternion.identity);
                    tutorialPoint3.name = "TutorialPointTra" + i;
                    tutorialPoint3.transform.parent = transform;
                    tutorialPoint3.transform.position = points.tutorialPointsPos;
                    tutorialPoint3.GetComponent<EventNumber>().GetSetEventNumber = points.tutorialNumber;
                    tutorialPoint3.GetComponent<EventNumber>().GetSetVideoNumber = videoNumber;
                    videoNumber++;
					break;
                case 3:
                    var specialPoint = Instantiate(specialPointPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                    specialPoint.name = "SpecialPoint" + i;
                    specialPoint.transform.parent = transform;
                    specialPoint.transform.position = points.tutorialPointsPos;
                    specialPoint.GetComponent<EventNumber>().GetSetEventNumber = points.tutorialNumber;
                    specialPoint.GetComponent<EventNumber>().GetSetVideoNumber = videoNumber;
                    videoNumber++;
                    break;
				default:
                    Debug.LogError("Type�̐����͂O����R�őI�����Ă�������");
					break;
            }
            i++;
        }
    }

    
    void Update()
    {
        
    }
}
