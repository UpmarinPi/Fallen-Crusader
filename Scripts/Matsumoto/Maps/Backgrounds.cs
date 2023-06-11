using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{
    float originSizeX;//spriteRenderer��size�̒l�Bscale��1�̂Ƃ��̑傫���ɂȂ��Ă���
    float originSizeY;
    float difPosX;//�w�i���ǂꂮ�炢���ꂽ��
    float widthWithScaleX;//originSizeX��scale����Z�������́B���ۂ�x��
    SpriteRenderer sr;//���g�̉��̒����擾�E�ύX�p

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
        sr.size = new Vector2(originSizeX * 3, originSizeY);//�����E�̎O��ނ�p�ӁB
        transform.position = (Vector3)bgcamera.GetPos + new Vector3(0, 0, 10 + posZ);
        //bool������
        changePosFlag = false;
    }

    
    void Update()
    {
        //BG�J���������������S�ɉf���Ȃ��Ȃ����ꍇ
        if (Mathf.Abs(bgcamera.GetPos.x - transform.position.x) > widthWithScaleX)
        {
            //�w�i���ǂꂾ�����ꂽ���v�Z
            //�v�Z�ゾ�Ƒ傫�������Ă������ꏊ�ɓ����w�i������(float������덷���ǂ����Ă��o����)
            difPosX = (bgcamera.GetPos.x - transform.position.x) % widthWithScaleX;
            changePosFlag = true;
        }
    }
    private void FixedUpdate()
    {
        if (changePosFlag)
        {
            transform.position = new Vector2(bgcamera.GetPos.x - difPosX, originY);//�w�i�����ʒu���J�����ɍ��킹��B
            changePosFlag = false;
        }
        transform.position += new Vector3(bgcamera.GetSpeedX * scrollSpeed * 0.02f, 0, 0);

    }
}
