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
	public bool canMoveFlag;//動作可能。redpanelのフェード演出が終わってから選択するまで
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
				if (select <= 0)//一番上にいるなら一番下に移動、それ以外なら上に移動
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
				if (select >= (GameOverChoice)countSelect - 1)//一番下にいるなら(略
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
				//現在のselectと同じ名前のオブジェクトを発動させる
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
			if (alpha + fadeSpeed >= maxAlpha)//alpha値がmaxAlpha超えるとき
			{
				alpha = maxAlpha;//最大alphaにする

				redPanel.GetComponent<GameOverPanel>().goingBlackFlag = true;//黒色にし始める
			}
			else
			{
				alpha += fadeSpeed;
			}
			child.GetComponent<CanvasGroup>().alpha = alpha;
		}
		
	}
}
