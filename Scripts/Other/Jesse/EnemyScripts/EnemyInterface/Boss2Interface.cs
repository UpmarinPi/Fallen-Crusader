using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2Interface : MonoBehaviour, IDamageEnemy
{
    [SerializeField] float fade = 0f;
    SpriteRenderer spriteRenderer;

    PlayerFinder PlayerFinder;
    GameObject player;

	public enum attackType
	{
        CrossAimAttack = 0, 
        CrowAttack, 
        CrossStabAttack,
        Crownado,
        SpikeStart,
        SpikeEnd,
	}

    public attackType thisType;

    int dmg0 = 30;
    int dmg1 = 50;
    int dmg2 = 40;
    int dmg3 = 30;
    int dmg4 = 15;
    int dmg5 = 40;
    float stn0 = 0.2f;
    float stn1 = 0.3f;
    float stn2 = 0.25f;
    float stn3 = 0.25f;
    float stn4 = 0.3f;
    float stn5 = 0.35f;
    Vector2 knock0 = new Vector2(2.5f, 2f);
    Vector2 knock1 = new Vector2(3.5f, 2f);
    Vector2 knock2 = new Vector2(3f, 2.5f);
    Vector2 knock3 = new Vector2(3.5f, 3f);
    Vector2 knock4 = new Vector2(4f, 2.5f);
    Vector2 knock5 = new Vector2(3f, 3f);

	private void Start()
	{
        PlayerFinder = new PlayerFinder();
        player = PlayerFinder.GetPlayer();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetFloat("_Fade", fade);
    }

    // stage 2 boss damage information function
    // ステージ２のボス攻撃情報関数
    public void GetDamage(out int damage, out float stun, out Vector2 knockback, out int scaleX)
    {
        if(player.transform.position.x > transform.position.x)		
            scaleX = 1;		
        else		
            scaleX = -1;
		
        switch(thisType)
		{
            case attackType.CrossAimAttack:
                damage = dmg0;
                stun = stn0;
                knockback = knock0;
                break;
            case attackType.CrossStabAttack:
                damage = dmg1;
                stun = stn1;
                knockback = knock1;
                break;
            case attackType.CrowAttack:
                damage = dmg2;
                stun = stn2;
                knockback = knock2;
                break;
            case attackType.Crownado:
                damage = dmg3;
                stun = stn3;
                knockback = knock3;
                break;
            case attackType.SpikeStart:
                damage = dmg4;
                stun = stn4;
                knockback = knock4;
                break;
            case attackType.SpikeEnd:
                damage = dmg5;
                stun = stn5;
                knockback = knock5;
                break;
            default:
                damage = 10;
                stun = 2;
                knockback = new Vector2(5, 5);                
                break;

        }
    }
    private void FixedUpdate()
    {
        spriteRenderer.material.SetFloat("_Fade", fade);
    }
}
