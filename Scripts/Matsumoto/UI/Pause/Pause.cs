using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
	public enum PauseChoice
	{
		Continue,
		Restart,
		//Option,
		Quit
	};
	public bool pauseFlag = false;//ポーズをとっているか
	public bool moveFlag;//動作が可能か。予期しないところで動作するのを防ぐ
	bool pushingFlag;//押しっぱなし回避
					 //public bool enteringFlag; //true時はオプション中など別画面。その時の入力を防ぐ
	public PauseChoice select;//現在どこを表示しているか
	int countSelect = System.Enum.GetValues(typeof(PauseChoice)).Length;//ポーズ画面の個数

	[SerializeField]Image flush;
	[SerializeField] InputManager inputManager;
	[SerializeField] SelectMagic selectMagic;
	[SerializeField] SkillInfoInPause skillInfoInPause;
	[SerializeField] PlayerController playerController;

	[SerializeField] GameObject optionGO;
	DisplayOption displayOption;

	[SerializeField] GameObject selectMagicUIGO;//ポーズ中は隠すのに用いる

	[SerializeField] SEController SE;

	
	void Start()
	{
		select = 0;//ポーズ画面の一番上を選択
		pauseFlag = false;
		//enteringFlag = false;
		transform.GetChild(0).gameObject.SetActive(false);
		displayOption = optionGO.GetComponent<DisplayOption>();
	}

	
	void Update()
	{
		if(pauseFlag)//ポーズ中の動作
		{
			if(moveFlag)//ポーズ中、動かすのが可能なら
			{
				if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.left].buttonFlag)//左ボタン
				{
					if(!pushingFlag)
					{
						SE.SystemSE(3);
						pushingFlag = true;
						if(select <= 0)
						{
							select = (PauseChoice) countSelect - 1;
						}
						else
						{
							select--;
						}
					}
				}
				else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.right].buttonFlag)//右ボタン
				{
					if(!pushingFlag)
					{
						SE.SystemSE(3);
						pushingFlag = true;
						if(select >= (PauseChoice) countSelect - 1)
						{

							select = 0;
						}
						else
						{
							select++;
						}
					}
				}
				else if(Input.GetKey(KeyCode.Escape))//ESCボタン
				{
					if(!pushingFlag)
					{
						SE.SystemSE(4);
						SetActivePauseMenu(false);
						Time.timeScale = 1;
						transform.GetChild(0).gameObject.SetActive(false);
						Debug.Log("now start");
						pauseFlag = false;
					}
					pushingFlag = true;
				}
				else if(inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.search].buttonFlag)//探索ボタン
				{
					if(!pushingFlag)
					{
						moveFlag = false;
						SE.SystemSE(4);
						switch(select)
						{
							case PauseChoice.Continue:
								Continue();
								break;
							case PauseChoice.Restart:
								Restart();
								break;
							case PauseChoice.Quit:
								Quit();
								break;
							//case PauseChoice.Option:
							//	displayOption.selectingOptionFlag = true;
								//break;
						}
					}
					pushingFlag = true;
				}
				else
				{
					pushingFlag = false;
				}
			}
		}
		else//ポーズ中ではないとき(!pauseFlag)
		{
			if(Input.GetKey(KeyCode.Escape))//ESCボタン
			{
				if(!pushingFlag && !selectMagic.selectingMagicFlag && !playerController.eventFlag)
				{
					SE.SystemSE(4);
					select = 0;
					transform.GetChild(0).gameObject.SetActive(true);
					SetActivePauseMenu(true);
					Time.timeScale = 0;
					Debug.Log("now stop");
					pauseFlag = true;
					moveFlag = true;
				}
				pushingFlag = true;
			}
			else
			{
				pushingFlag = false;
			}
		}
	}

	void Continue()
	{
		Time.timeScale = 1;
		transform.GetChild(0).gameObject.SetActive(false);
		//enteringFlag = false;
		Debug.Log("now start");
		pauseFlag = false;
	}
	void Restart()
	{
		Time.timeScale = 1;
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	private void Quit()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;//ゲームプレイ終了
#else
    Application.Quit();
#endif
	}

	void SetActivePauseMenu(bool _active)//ポーズ画面を開始・終了する際の処理や演出管理
	{
		if(_active)
		{
			//開始
			skillInfoInPause.Active();
		}
		else
		{
		//終了
		}
		selectMagicUIGO.SetActive(!_active);
	}
}
