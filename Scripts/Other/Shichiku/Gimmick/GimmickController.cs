using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GimmickController : MonoBehaviour
{
    [SerializeField] GameObject gimmickBall;

    GameObject firstTarget;
    [SerializeField] GameObject secondTarget;

    [SerializeField] Vector3 generationPosition;

    [SerializeField] float reactionDistance = 7f;
    [SerializeField] float interval = 2f;

    [SerializeField] int hpMax = 45;

    [SerializeField] Animator anim;
    string state = "BuildState";

    int hp;

    bool rangeFlag = false;

    
    void Start()
    {
        var playerFinder = new PlayerFinder();
        firstTarget = playerFinder.GetPlayer();
        Init();
    }

    void Init()
    {
        hp = hpMax;
    }

    
    void Update()
    {
        // HPÇ™0Ç…Ç»ÇÈÇ∆è¡Ç¶ÇÈ
        if (hp <= 0)
        {
            anim.SetTrigger(state);
            return;
        }

        if ((firstTarget.transform.position - transform.position).magnitude < reactionDistance && !rangeFlag)
        {
            rangeFlag = true;
            GenerationGimmickBall();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hp <= 0)
        {
            return;
        }

        var damageBall = collision.GetComponent<IDamageBall>();
        if(damageBall != null)
        {
            DamagedByBall(damageBall.GetDamage());
            StartCoroutine(DamageFlash());
        }
    }

    void ChangeFlag()
    {
        rangeFlag = false;
    }

    void GenerationGimmickBall() 
    {
        var ball = Instantiate(gimmickBall, transform.position + generationPosition, Quaternion.identity, transform);
        ball.GetComponent<GimmickBallController>().Begin(firstTarget, secondTarget);
        Invoke("ChangeFlag", interval);
    }

    void DamagedByBall(int damage)
    {
        hp -= damage;
    }

    IEnumerator DamageFlash()
    {
        var spriteRenderer = transform.parent.GetComponent<SpriteRenderer>();
        var spriteColor = spriteRenderer.color;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = spriteColor;
        yield return new WaitForSeconds(0.05f);
    }
}