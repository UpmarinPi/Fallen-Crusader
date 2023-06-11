using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGCamera : MonoBehaviour
{
    public Vector2 GetPos { get { return transform.position; } }
    [SerializeField]
    GameObject mainCamera;
    [SerializeField, RangeAttribute(0, 1)]
    float scrollYratio;
    public float originY;
    float camerasDistanceY;
    public float latestPos;


    [SerializeField]
    private float speedX;
    public float GetSpeedX
    {
        get { return speedX; }
    }

    private void Start()
    {
        originY = transform.position.y;
        camerasDistanceY = originY - mainCamera.transform.position.y;
        latestPos = GetPos.x;
    }
    private void Update()
    {
        speedX = GetXSpeed(GetPos.x, latestPos);
        latestPos = GetPos.x;
        transform.position = new Vector3(mainCamera.transform.position.x, (camerasDistanceY - mainCamera.transform.position.y) * scrollYratio * 0.1f + originY, -10);
    }

    private void FixedUpdate()
    {

    }

    //スピードを求める。ここではyは固定としているため計算していない
    //マイナス値も出てくる
    private float GetXSpeed(float _nowX, float _latestX)
    {
        if (Time.deltaTime == float.NaN || Time.deltaTime == 0)
        {
            return 0;
        }
        else
        {
            return (_nowX - _latestX) / Time.deltaTime;
        }
    }
}