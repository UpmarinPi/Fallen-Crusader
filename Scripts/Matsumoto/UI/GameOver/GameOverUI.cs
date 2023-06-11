using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
	public enum GameOverChoice
	{
		BackToCheckPoint,
		Quit
	};
	GameObject child;
	public GameOverChoice select;

	[SerializeField]
	private List<GameObject> GameOverButtons = new List<GameObject>(System.Enum.GetNames(typeof(GameOverChoice)).Length);

	[SerializeField]
	GameObject redPanel;

	[SerializeField]
	PlayerController player;

	int countSelect;
	float alpha;
	[Range(0, 1)]
	public float fadeSpeed = 0.1f;
	public float maxAlpha = 1;

	

	public bool gameOverFlag;
	bool pushingFlag;
	public bool canMoveFlag;//����\�Bredpanel�̃t�F�[�h���o���I����Ă���I������܂�
	bool selectFlag;
	private void Start()
	{
		countSelect = System.Enum.GetValues(typeof(GameOverChoice)).Length;
		child = transform.GetChild(0).gameObject;
		alpha = 0.0f;
		child.GetComponent<CanvasGroup>().alpha = alpha;
		canMoveFlag = false;
		gameOverFlag = false;
		selectFlag = false;

		child.SetActive(false);
	}
	private void Update()
	{
		if (selectFlag)
		{
			return;
		}
		if (player.IsDeath /*|| Input.GetKey(KeyCode.G)*/)
		{
			gameOverFlag = true;
			Debug.Log(gameOverFlag);
		}
		if (!gameOverFlag || !canMoveFlag)
		{
			return;
		}
		if (Input.GetKey(KeyCode.UpArrow))
		{
			if (!pushingFlag)
			{
				if (select <= 0)//��ԏ�ɂ���Ȃ��ԉ��Ɉړ��A����ȊO�Ȃ��Ɉړ�
				{
					select = (GameOverChoice)countSelect - 1;
				}
				else
				{
					select--;
				}
				pushingFlag = true;
			}
		}
		else if (Input.GetKey(KeyCode.DownArrow))
		{
			if (!pushingFlag)
			{
				if (select >= (GameOverChoice)countSelect - 1)//��ԉ��ɂ���Ȃ�(��
				{
					select = 0;
				}
				else
				{
					select++;
				}
				pushingFlag = true;
			}
		}
		else if (Input.GetKey(KeyCode.Return))
		{
			if (!pushingFlag)
			{
				//���݂�select�Ɠ������O�̃I�u�W�F�N�g�𔭓�������
				GameOverButtons[(int)select].GetComponent<GameOverButtonUI>().SetPower = true;
				canMoveFlag = false;
			}
		}
		else
		{
			pushingFlag = false;
		}
	}

	private void FixedUpdate()
	{
		if (gameOverFlag)
		{
			child.SetActive(true);
			if (alpha + fadeSpeed >= maxAlpha)//alpha�l��maxAlpha������Ƃ�
			{
				alpha = maxAlpha;//�ő�alpha�ɂ���

				redPanel.GetComponent<GameOverPanel>().goingBlackFlag = true;//���F�ɂ��n�߂�
			}
			else
			{
				alpha += fadeSpeed;
			}
			child.GetComponent<CanvasGroup>().alpha = alpha;
		}
		
	}
}
