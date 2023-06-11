using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeInterface : MonoBehaviour, IDamageEnemy
{
    // the direction the spike is facing
    // とげが向いている方向
    public bool spikeUp = false;
    public bool spikeDown = false;
    public bool spikeLeft = false;
    public bool spikeRight = false;

    // damage info function
    // ダメージ情報関数
    public void GetDamage(out int damage, out float stun, out Vector2 knockback, out int scaleX)
    {
        // if scaleX is 10, it will reverse the direction of the player
        // scaleX が１０の場合プレイヤーの方向を逆にする
        if (spikeUp)
        {
            damage = 20;
            stun = 0.5f;
            knockback = new Vector2(3f, 3f);
            scaleX = 10;
        }
        else if (spikeDown)
        {
            damage = 20;
            stun = 0.5f;
            knockback = new Vector2(3f, -1f);
            scaleX = 10;
        }
        else if (spikeLeft)
        {
            damage = 20;
            stun = 0.5f;
            knockback = new Vector2(3f, 3f);
            scaleX = -1;
        }
        else if (spikeRight)
        {
            damage = 20;
            stun = 0.5f;
            knockback = new Vector2(3f, 3f);
            scaleX = 1;
        }
        else
        {
            damage = 20;
            stun = 0.5f;
            knockback = new Vector2(3f, 3f);
            scaleX = 10;
        }

    }
}
