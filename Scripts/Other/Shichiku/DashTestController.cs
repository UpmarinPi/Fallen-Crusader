using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTestController : MonoBehaviour
{
	public string idleAnim = "";
	public string walkAnim = "";
	public string runAnim = "";
	public string dashAnim = "";
	string nowAnim = "";
	Animator anim;
	Rigidbody2D rbody2d;
	Vector2 velocity2d;
	Vector2 scale2d;
	SpriteRenderer playerRenderer;
	bool attackFlag = false;
	bool dashFlag = false;
	bool jumpFlag = false;

	Coroutine coroutine, coroutineWaitTime;

	private void Start()
	{
		// Update関数のフレームレートを約60に設定
		Application.targetFrameRate = 60;

		// 取得
		anim = GetComponent<Animator>();
		rbody2d = GetComponent<Rigidbody2D>();
		velocity2d = rbody2d.velocity;
		playerRenderer = GetComponent<SpriteRenderer>();
		scale2d = transform.localScale;
	}
	private void Update()
	{	
		if (Input.GetKey(KeyCode.RightArrow))
		{
			scale2d = transform.localScale;
			scale2d.x = -1;
			transform.localScale = scale2d;
		}

		if(Input.GetKey(KeyCode.LeftArrow))
		{
			scale2d = transform.localScale;
			scale2d.x = 1;
			transform.localScale = scale2d;
		}
		if(Input.GetKeyDown(KeyCode.D) && !attackFlag && !dashFlag)
		{
			dashFlag = true;
			velocity2d = rbody2d.velocity;
			if (transform.localScale.x > 0)
			{
				velocity2d.x = 20;
			}
			else if(transform.localScale.x < 0)
			{
				velocity2d.x = -20;
			}		
			rbody2d.velocity = velocity2d;
			nowAnim = dashAnim;
			Invoke("DashOff", 0.1f);
		}

		// 入力しなければアイドル状態
		if(!Input.anyKey && !dashFlag)
		{
			velocity2d = rbody2d.velocity;
			velocity2d.x = 0;
			rbody2d.velocity = velocity2d;
			if (!attackFlag && !jumpFlag)
				nowAnim = idleAnim;
		}
	}

	private void FixedUpdate()
	{
		// アニメーションの切り替え
		if(!attackFlag && !jumpFlag && !dashFlag)
			anim.Play(nowAnim);
	}

	void DashOff()
	{
		velocity2d = rbody2d.velocity;
		velocity2d.x = 0;
		rbody2d.velocity = velocity2d;
		dashFlag = false;
	}
}
