using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageMelee : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] bool BossFlag = false;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rbody2d;
    bool hurtFlag = false;
    bool swordFlag = false;
    bool magicFlag = false;
    int damage = 0;
    int scaleX = 0;
    float buff = 1f;
    float stun = 0;
    Vector2 knockback = new Vector2(0, 0);

    Collider2D hitbox;
    GameObject Player;
    Coroutine coroutine;
    MagicController magicController;
    PlayerController playerController;
    ControllerMelee enemyController;
    PlayerFinder playerFinder;

    IDamageSword sword;
    IDamageMagic magic;


    void Start()
    {
        playerFinder = new PlayerFinder();
        Player = playerFinder.GetPlayer();
        playerController = Player.GetComponent<PlayerController>();
        magicController = Player.GetComponent<MagicController>();
        spriteRenderer = transform.parent.gameObject.GetComponent<SpriteRenderer>();
        rbody2d = transform.parent.gameObject.GetComponent<Rigidbody2D>();
        enemyController = transform.parent.gameObject.GetComponent<ControllerMelee>();
        GetComponent<BoxCollider2D>().enabled = true;
        hitbox = GetComponent<Collider2D>();

    }

	private void FixedUpdate()
	{
		if (MagicController.freezeFlag)
		{
            buff = 1.5f;
		}
        else
		{
            buff = 1f;
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        //Debug.Log(hitbox.enabled);
        // make this only sword
        if((sword = collision.gameObject.GetComponent<IDamageSword>()) != null)
        {
            if(swordFlag)
                return;
            int mpSteal = 0;
            bool critFlag = false;

            sword.GetDamage(out damage, out mpSteal, out stun, out knockback, out scaleX, out critFlag);
            DamageEffect(transform.position);
            if (BossFlag)
			{
                mpSteal *= 2;
			}
            magicController.SetManaSteal(mpSteal); // TPâÒïú
            enemyController.EnemyHP -= Mathf.CeilToInt(damage * buff);
            StunTime = stun;
            hurtFlag = true;
            coroutine = null;

            if(critFlag)
                coroutine = StartCoroutine(CritFlash()); // ê‘Ç≠åıÇÈ            
            else
                coroutine = StartCoroutine(DamageFlash());

            rbody2d.velocity = new Vector2(0, 0);
            KnockbackBig(knockback.x, knockback.y, scaleX);
            swordFlag = true;
            Invoke("InvokeSword", 0.2f);

        }
        
        if((magic = collision.gameObject.GetComponent<IDamageMagic>()) != null)
        {
            if(magicFlag)
                return;
            magic.GetDamage(out damage, out stun, out knockback, out scaleX);
            DamageEffect(transform.position);
            enemyController.EnemyHP -= Mathf.CeilToInt(damage * buff);
            StunTime = stun;
            hurtFlag = true;
            coroutine = null;
            coroutine = StartCoroutine(DamageFlash());
            rbody2d.velocity = new Vector2(0, 0);
            if (collision.gameObject.CompareTag("MagicExplosion"))
			{
                if (collision.gameObject.transform.position.x < transform.position.x)
				{
                    // right
                    scaleX = 1;
				}
                else
				{
                    // left
                    scaleX = -1;
				}
			}
            KnockbackBig(knockback.x, knockback.y, scaleX);
            magicFlag = true;
            Invoke("InvokeMagic", 0.4f);
            
        }

    }
    // çUåÇéÛÇØÇΩÇ∆Ç´ê‘Ç≠Ç»ÇÈ
    IEnumerator DamageFlash() 
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.05f);
    }

    IEnumerator CritFlash()
    {
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
        yield return new WaitForSeconds(0.05f);
    }

    void InvokeSword()
    {
        swordFlag = false;
    }

    void InvokeMagic()
    {
        magicFlag = false;
    }

    void DamageEffect(Vector3 pos)
	{
        GameObject effect = Instantiate(hitEffect) as GameObject;
        effect.transform.position = pos;
	}


    // çUåÇéÛÇØÇΩÇ©ÇéÊìæ
    public bool GetEnemyHit()
    {
        return hurtFlag;
    }
    // bool ÉäÉZÉbÉgÇ∑ÇÈ
    public void ResetEnemyHit()
	{
        hurtFlag = false;
	}
    // çUåÇÇÃéÌóﬁÇéÊìæ

    public float StunTime
	{
        get; set;
	}

    // ìGÇ™íµÇÀï‘ÇÈä÷êî
    private void KnockbackBig(float knockbackPowerX, float knockbackPowerY, int scaleX)
	{
        coroutine = null;
        if (scaleX > 0)
        {
            rbody2d.AddForce(new Vector2(knockbackPowerX, knockbackPowerY), ForceMode2D.Impulse);
            //Debug.Log("right");
        }
        else
        {
            rbody2d.AddForce(new Vector2(-knockbackPowerX, knockbackPowerY), ForceMode2D.Impulse);
            //Debug.Log("left");

        }
        this.transform.parent.transform.localScale = new Vector3(-scaleX, 1, 1);
    }


}
