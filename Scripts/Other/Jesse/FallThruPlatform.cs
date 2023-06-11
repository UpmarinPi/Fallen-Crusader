using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallThruPlatform : MonoBehaviour
{
    bool contactFlag = false;
    CompositeCollider2D compBox;
    Pause pause;
    SelectMagic selectMagic;
    public PlayerController playerController;
    [SerializeField] InputManager inputManager;

    private void Start()
    {
        compBox = GetComponent<CompositeCollider2D>();

        selectMagic = playerController.transform.GetComponent<SelectMagic>();
        pause = GameObject.FindWithTag("Pause").transform.GetComponent<Pause>();
    }

    void Update()
    {
        if (playerController.GetEventTime() || pause.pauseFlag || selectMagic.GetSelectFlag())
            return;

        // when pressing down on a platform change collider to trigger
        // 下キーを押したときコライダーをトリガーに変える
        if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.down].buttonDownFlag && contactFlag)
        {
            compBox.isTrigger = true;
            contactFlag = false;
            Invoke("unTrigger", 0.3f);
        }
    }

    // revert collision back 
    // コライダーを元に戻す
    void unTrigger()
	{
        compBox.isTrigger = false;
    }

    // if player contacts the platform, activate flag 
    // プレイヤーが貫通床と接触するとフラグをつける
    private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
            contactFlag = true;
        }
    }

	private void OnCollisionExit2D(Collision2D collision)
	{
        if (collision.gameObject.CompareTag("Player"))
        {
            contactFlag = false;
        }
    }


}
