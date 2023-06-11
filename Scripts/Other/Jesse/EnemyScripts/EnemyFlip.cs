using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlip : MonoBehaviour
{
	bool wallCollideFlag = false;
	Collider2D returnCollision;

    
	// function to check if collided with wall
	// �ǂɂԂ��������m�F����֐�
	public bool GetWallCollide()
	{
		return wallCollideFlag;
	}
	public void SetWallCollide(bool Flag)
	{
		wallCollideFlag = Flag;
	}
	public Collider2D GetCollider()
	{
		return returnCollision;
	}

	// flip detection flag to true on contact with wall
	// �ǂƐڐG������t���O��true�ɐݒ�
	private void OnTriggerEnter2D(Collider2D collision)
	{
		returnCollision = collision;
		if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("PatrolPoint"))
		{
			wallCollideFlag = true;
		}

	}

	// flip detection flag to false after leaving wall
	// �ǂƗ��ꂽ��t���O��false�ɐݒ�
	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("PatrolPoint"))
		{
			wallCollideFlag = false;
		}
	}


}
