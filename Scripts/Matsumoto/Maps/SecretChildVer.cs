using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretChildVer : MonoBehaviour
{
	public int fadeSpeed = 10;
	[SerializeField] int alpha;      //�摜�̃A���t�@�l�B�����x�𑀍�
	bool insideFlag;//�v���C���[�������Ă��邩�̔���Benter����exit���̎��������Ɏg�p
	Tilemap tilemap;
	Color32 color;

	
	void Start()
	{
		alpha = 255;                        //�����l�ݒ�
		insideFlag = false;
		tilemap = transform.GetChild(0).GetComponent<Tilemap>();
		color = tilemap.color;
	}

	private void Update()
	{
		if(insideFlag)                      //�v���C���[�Ȓ��ɓ����alpha�l���ŏ�0�܂ŁAfadespeed��������
		{
			alpha = Mathf.Clamp(alpha - fadeSpeed, 0, 255);
		}
		else                                //�v���C���[���O�ɏo���alpha�l���ő�255�܂ŁAfadespeed���グ��
		{
			alpha = Mathf.Clamp(alpha + fadeSpeed, 0, 255);
		}
	}
	private void FixedUpdate()
	{
		tilemap.color = new Color32(color.r, color.g, color.b, (byte)alpha);//���ۂ̓��ߒl�w��
	}
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))//�v���C���[������������
		{
			insideFlag = true;
		}
	}
	private void OnTriggerExit2D(Collider2D collision)
	{
		if(collision.gameObject.CompareTag("Player"))//�v���C���[���o������
		{
			insideFlag = false;
		}
	}
}
