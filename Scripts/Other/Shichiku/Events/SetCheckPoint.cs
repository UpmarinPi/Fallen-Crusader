using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCheckPoint : MonoBehaviour
{
    public GameObject checkPointPrefab;
    [Header("チェックポイントのポジション")]
    public List<Vector3> checkPointsPos = new List<Vector3>();

    int i = 0;

    // チェックポイントの設置
    void Start()
    {
        foreach(Vector3 pos in checkPointsPos)
        {
            var checkPoint = Instantiate(checkPointPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            checkPoint.name = "CheckPoint" + i;
            checkPoint.transform.parent = transform;
            checkPoint.transform.position = pos;
            i++;
        }
    }

    
    void Update()
    {
        
    }
}
