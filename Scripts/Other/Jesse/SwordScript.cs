using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// players sword attack interface
// �v���C���[�̌��̍U���C���^�[�t�F�[�X
interface IDamageSword
{
    void GetDamage(out int damage, out int mpSteal, out float stun, out Vector2 knockback, out int scaleX, out bool critFlag);

}

// players magic attack interface
// �v���C���[�̖��@�̍U���C���^�[�t�F�[�X
interface IDamageMagic
{
    void GetDamage(out int damage, out float stun, out Vector2 knockback, out int scaleX);

}

// sword's attack info
// ���̍U����ނƂ��̏��
public class SwordScript : MonoBehaviour, IDamageSword
{
    // damages
    // �_���[�W��
    int swordDmg1 = 2;
    int swordDmg2 = 4;
    int swordDmg3 = 6;
    int critDmg1 = 10;

    // MP abosorption rate
    // MP�z����
    int swordMP1 = 1;
    int swordMP2 = 2;
    int swordMP3 = 3;
    int critMP1 = 5;

    // stun time
    // �X�^������
    float swordStun1 = 0.1f;
    float swordStun2 = 0.2f;
    float swordStun3 = 0.3f;
    float critStun1 = 0.3f;

    // knockback
    // �m�b�N�o�b�N
    Vector2 swordKnoc1 = new Vector2(1, 1.5f);
    Vector2 swordKnoc2 = new Vector2(1.5f, 2);
    Vector2 swordKnoc3 = new Vector2(2.5f, 2.5f);
    Vector2 critKnoc1 = new Vector2(3, 3);

    // the function the enemy script calls to get the attack information
    // �G�̃X�N���v�g���ĂԌ��̍U�����
	public void GetDamage(out int damage, out int mpSteal, out float stun, out Vector2 knockback, out int scaleX, out bool critFlag)
	{
        // direciton 
        // ����
        if (transform.parent.localScale.x > 0)
		{
            scaleX = 1;
		}
        else
		{
            scaleX = -1;
		}

        // checks current tag to determine type of damage
        // �^�O�����čU������^����
        if (gameObject.CompareTag("PlayerCrit1"))
        {
            damage = critDmg1;
            mpSteal = critMP1;
            stun = critStun1;
            knockback = critKnoc1;
            critFlag = true;
        }
        else if (gameObject.CompareTag("PlayerAttack3"))
		{
            damage = swordDmg3;
            mpSteal = swordMP3;
            stun = swordStun3;
            knockback = swordKnoc3;
            critFlag = false;
        }
        else if (gameObject.CompareTag("PlayerAttack2"))
        {
            damage = swordDmg2;
            mpSteal = swordMP2;
            stun = swordStun2;
            knockback = swordKnoc2;
            critFlag = false;
        }
        else
		{
            damage = swordDmg1;
            mpSteal = swordMP1;
            stun = swordStun1;
            knockback = swordKnoc1;
            critFlag = false;
        }
    }
    
}
