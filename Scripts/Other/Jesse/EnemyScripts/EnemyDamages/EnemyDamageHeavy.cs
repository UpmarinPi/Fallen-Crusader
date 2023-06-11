using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageHeavy : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rbody2d;
    bool hurtFlag = false;
    bool swordFlag = false;
    bool magicFlag = false;
    int damage = 0;
    int scaleX = 0;
    float buff = 1f;
    float stun = 0;
    Vector2 knockback;


    GameObject Player;
    Collider2D hitbox;
    Coroutine coroutine;
    MagicController magicController;
    PlayerController playerController;
    ControllerHeavy enemyController;
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
        enemyController = transform.parent.gameObject.GetComponent<ControllerHeavy>();
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
        // make this only sword
        if((sword = collision.gameObject.GetComponent<IDamageSword>()) != null)
        {
            if(swordFlag)
                return;
            int mpSteal = 0;
            bool critFlag = false;

            sword.GetDamage(out damage, out mpSteal, out stun, out knockback, out scaleX, out critFlag);
            DamageEffect(transform.position);
            magicController.SetManaSteal(mpSteal); // TPâÒïú
            enemyController.EnemyHP -= Mathf.CeilToInt(damage * buff);
            coroutine = null;

            if(critFlag)
                coroutine = StartCoroutine(CritFlash()); // ê‘Ç≠åıÇÈ            
            else
                coroutine = StartCoroutine(DamageFlash());
            Invoke("InvokeSword", 0.2f);

        }

        if((magic = collision.gameObject.GetComponent<IDamageMagic>()) != null)
        {
            if(magicFlag)
                return;
            magic.GetDamage(out damage, out stun, out knockback, out scaleX);
            DamageEffect(transform.position);
            enemyController.EnemyHP -= Mathf.CeilToInt(damage * buff);
            coroutine = null;
            coroutine = StartCoroutine(DamageFlash());
            Invoke("InvokeMagic", 0.4f);

        }

               
    }
    // çUåÇéÛÇØÇΩÇ∆Ç´ê‘Ç≠Ç»ÇÈ
    IEnumerator DamageFlash()
    {
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        enemyController.Damage();
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(230f / 255f, 230f / 255f, 230f / 255f);
        yield return new WaitForSeconds(0.05f);
    }

    IEnumerator CritFlash()
    {
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.color = Color.yellow;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(230f / 255f, 230f / 255f, 230f / 255f);
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
    public float StunTime
    {
        get; set;
    }

    // ìGÇ™íµÇÀï‘ÇÈä÷êî
    //private void KnockbackBig(float knockbackPowerX, float knockbackPowerY, Vector3 damagedDirection)
    //{
    //    coroutine = null;
    //    float scale;
    //    Vector3 direction = (damagedDirection - this.transform.position).normalized;
    //    // to the enemy the player is in this direction
    //    // go in the opposite direction
    //    if (direction.x > 0)
    //    {
    //        rbody2d.AddForce(new Vector2(-knockbackPowerX, knockbackPowerY), ForceMode2D.Impulse);
    //        //Debug.Log("left");
    //        scale = 1;
    //    }
    //    else
    //    {
    //        rbody2d.AddForce(new Vector2(knockbackPowerX, knockbackPowerY), ForceMode2D.Impulse);
    //        //Debug.Log("right");
    //        scale = -1;

    //    }
    //    this.transform.parent.transform.localScale = new Vector3(scale, 1, 1);
    //}
}
