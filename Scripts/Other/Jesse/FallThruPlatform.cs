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
        // ���L�[���������Ƃ��R���C�_�[���g���K�[�ɕς���
        if (inputManager.inputCheck[(int) KeyBindings.KeyBindNumber.down].buttonDownFlag && contactFlag)
        {
            compBox.isTrigger = true;
            contactFlag = false;
            Invoke("unTrigger", 0.3f);
        }
    }

    // revert collision back 
    // �R���C�_�[�����ɖ߂�
    void unTrigger()
	{
        compBox.isTrigger = false;
    }

    // if player contacts the platform, activate flag 
    // �v���C���[���ђʏ��ƐڐG����ƃt���O������
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
