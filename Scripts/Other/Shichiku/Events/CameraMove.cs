using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
	[SerializeField]EditData editData;
	GameObject player;//プレイヤーの位置を取得
	Vector2 playerPos;
	Vector2 beforePlayerPos;
	Vector3 cameraPos;
	public float cameraSpeed = 5.0f;
	[Header("範囲制限が存在するか")]
	public bool areaFlag = false;

	[Header("範囲制限をつける場合、下のオブジェクトを作ること")]
	[SerializeField] GameObject lowerLeft_camera;
	[SerializeField] GameObject upperRight_camera;

	[SerializeField] float koteiTi;

	[SerializeField]Vector2 lowerLeft;
	[SerializeField]Vector2 upperRight;

	//カメラが範囲内ならtrue
	bool moveXFlag;
	bool moveYFlag;

	//停止するときのカメラ位置保存変数
	float saveX;
	float saveY;

	bool notFollowXFlag = false;
	bool notFollowYFlag = false;

	PlayerFinder playerFinder;

	public enum SwitchArea
	{
		lowerleft = 0,
		upperRight = 1
	}

	void Start()
	{
		if (areaFlag)
		{
			lowerLeft = lowerLeft_camera.transform.position;
			lowerLeft = editData.data.saveLowerLeftCamera;
			if(lowerLeft.x > transform.position.x)
				transform.position = lowerLeft;
			upperRight = upperRight_camera.transform.position;
		}
		moveXFlag = true;
		moveYFlag = true;
		playerFinder = new PlayerFinder();
		player = playerFinder.GetPlayer();

		playerPos = player.transform.position;
		cameraPos = playerPos;
		beforePlayerPos = playerPos;
		transform.position = cameraPos;
		//Invoke("Init", 0.01f);
	}

 //   void Init()
 //   {

	//}

    private void Update()
	{
		cameraPos = transform.position;
		playerPos = player.transform.position;
		if (areaFlag)
		{
			//X座標が範囲を超えた場合Xの値を保存して停止
			if (cameraPos.x < lowerLeft.x || cameraPos.x > upperRight.x)
			{
				if (moveXFlag)
				{
					saveX = cameraPos.x;
				}
				moveXFlag = false;
			}
			if(playerPos.x > lowerLeft.x && playerPos.x < upperRight.x)
			{
				moveXFlag = true;
			}
			//Y座標が範囲を超えた場合Yの値を保存して停止
			if (cameraPos.y < lowerLeft.y || cameraPos.y > upperRight.y)
			{
				if (moveYFlag)
				{
					saveY = cameraPos.y;
				}
				moveYFlag = false;
			}

			if(playerPos.y > lowerLeft.y && playerPos.y < upperRight.y)
			{
				moveYFlag = true;
			}
		}
	}

	void LateUpdate()
	{
		//cameraPos = Vector3.Lerp(cameraPos, playerPos, cameraSpeed * Time.deltaTime);

		//カメラX座標処理
		if (!notFollowXFlag && moveXFlag && Mathf.Abs(cameraPos.x - playerPos.x) >= koteiTi /*&& Mathf.Abs(playerPos.x - beforePlayerPos.x) > 0*/)
		{ 
			cameraPos.x = Mathf.Lerp(cameraPos.x, playerPos.x, cameraSpeed * Time.deltaTime);
		}
		else if (moveXFlag || notFollowXFlag)
		{

		}
		else
		{
			cameraPos.x = saveX;
		}

		//カメラY座標処理
		if (!notFollowYFlag && moveYFlag && Mathf.Abs(cameraPos.y - playerPos.y) >= koteiTi /*&& Mathf.Abs(playerPos.y - beforePlayerPos.y) > 0*/)
		{
			cameraPos.y = Mathf.Lerp(cameraPos.y, playerPos.y, cameraSpeed * Time.deltaTime);
		}
		else if (moveYFlag || notFollowYFlag)
		{

		}
		else
		{
			cameraPos.y = saveY;
		}

		//カメラZ座標処理
		cameraPos.z = -10f;

		//カメラ位置を設定
		transform.position = cameraPos;
		beforePlayerPos = playerPos;
	}

	public void MoveAreaChange(Vector2 lowerLeft, Vector2 upperRight)
    {
		if (lowerLeft.x == upperRight.x)
		{
			notFollowXFlag = true;
		}
        else
        {
			notFollowXFlag = false;
        }
		if (lowerLeft.y == upperRight.y)
        {
			notFollowYFlag = true;
		}
        else
        {
			notFollowYFlag = false;
        }
		this.lowerLeft = lowerLeft;
		this.upperRight = upperRight;

		cameraPos = transform.position;
		saveX = cameraPos.x;
		saveY = cameraPos.y;
		transform.position = cameraPos;
		beforePlayerPos = player.transform.position;
		playerPos =	beforePlayerPos;
    }

	public void MoveAreaChange(Vector2 areaPos, SwitchArea area)
	{
		switch(area)
		{
			case SwitchArea.lowerleft:
				lowerLeft = areaPos;
				editData.data.saveLowerLeftCamera = areaPos;
				//editData.Save(editData.data);
				break;
			case SwitchArea.upperRight:
				upperRight = areaPos;
				break;
		}
		if(lowerLeft.y == upperRight.y)
		{
			notFollowYFlag = true;
		}
		else
		{
			notFollowYFlag = false;
		}

		cameraPos = transform.position;
		saveX = cameraPos.x;
		saveY = cameraPos.y;
		transform.position = cameraPos;
		beforePlayerPos = player.transform.position;
		playerPos = beforePlayerPos;
	}
}
