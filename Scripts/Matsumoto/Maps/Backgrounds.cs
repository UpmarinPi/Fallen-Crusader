using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{
    float originSizeX;//spriteRenderer内sizeの値。scaleが1のときの大きさになっている
    float originSizeY;
    float difPosX;//背景がどれぐらいずれたか
    float widthWithScaleX;//originSizeXにscaleを乗算したもの。実際のx幅
    SpriteRenderer sr;//自身の横の長さ取得・変更用

    [SerializeField]
    BGCamera bgcamera;
    float originY;
    [SerializeField]
    int posZ;

    bool changePosFlag;

    //
    [SerializeField, RangeAttribute(0, 1)]
    float scrollSpeed;
    void Start()
    {
        sr = transform.GetComponent<SpriteRenderer>();
        originY = transform.position.y;
        originSizeX = sr.size.x;
        widthWithScaleX = originSizeX * transform.localScale.x;
        originSizeY = sr.size.y;
        sr.size = new Vector2(originSizeX * 3, originSizeY);//左中右の三種類を用意。
        transform.position = (Vector3)bgcamera.GetPos + new Vector3(0, 0, 10 + posZ);
        //bool初期化
        changePosFlag = false;
    }

    
    void Update()
    {
        //BGカメラが中央を完全に映さなくなった場合
        if (Mathf.Abs(bgcamera.GetPos.x - transform.position.x) > widthWithScaleX)
        {
            //背景がどれだけずれたか計算
            //計算上だと大きく動いても同じ場所に同じ背景が来る(floatだから誤差がどうしても出そう)
            difPosX = (bgcamera.GetPos.x - transform.position.x) % widthWithScaleX;
            changePosFlag = true;
        }
    }
    private void FixedUpdate()
    {
        if (changePosFlag)
        {
            transform.position = new Vector2(bgcamera.GetPos.x - difPosX, originY);//背景中央位置をカメラに合わせる。
            changePosFlag = false;
        }
        transform.position += new Vector3(bgcamera.GetSpeedX * scrollSpeed * 0.02f, 0, 0);

    }
}
