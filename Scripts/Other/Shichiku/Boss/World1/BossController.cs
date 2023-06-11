using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class BossController : MonoBehaviour, IBoss
{
    [SerializeField]
    private GameObject branchPrefab;
    [SerializeField]
    private GameObject effectPrefab;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private GameObject endingPrefab;

    [SerializeField] BGMController BGM;
    [SerializeField] SEController SE;

    EventController eventController;

    Animator anim;

    const int maxBossShield = 100;
    const int maxBossHP = 100;

    int bossShield = maxBossShield;
    int bossHP = maxBossHP;

    bool breakFlag = false;
    bool deathFlag = false;
    bool redFlag = false;

    bool onceFlag = false;

    Coroutine coroutine, root2Coroutine = null;

    enum ATTACK_TYPE
    {
        ROOT = 0,
        WAVE = 1,
        DROP = 2,
        SUMMON = 3
    }

    ATTACK_TYPE attackType = new ATTACK_TYPE();

    List<BossAttackController> attackScript = new List<BossAttackController>();

    private void Start()
    {
        anim = GetComponent<Animator>();
        for (int i = 0; i < this.transform.childCount; i++)
        {
            attackScript.Add(transform.GetChild(i).GetComponent<BossAttackController>());
        }
    }

    
    private void Update()
    {
        if(deathFlag && !onceFlag)
        {
            onceFlag = true;
            StartCoroutine(Death());
        }
    }

    public IEnumerator StartBoss(EventController eventController)
    {
        this.eventController = eventController;
        transform.GetChild(5).GetComponent<BoxCollider2D>().enabled = true;

        // 話すアニメーション
        yield return StartCoroutine(TalkBoss());

        Random.InitState(System.DateTime.Now.Millisecond);

        while (!deathFlag)
        {
            if (!redFlag)
            {
                for (int i = 0; i < 2; i++)
                {
                    attackType = (ATTACK_TYPE)Random.Range(1, 4/*4*/);

                    switch (attackType)
                    {
                        case ATTACK_TYPE.WAVE:
                            yield return StartCoroutine(attackScript[(int)attackType].Wave());
                            break;
                        case ATTACK_TYPE.DROP:
                            yield return StartCoroutine(attackScript[(int)attackType].Drop(branchPrefab));
                            break;
                        case ATTACK_TYPE.SUMMON:
                            yield return StartCoroutine(attackScript[(int)attackType].Summon(effectPrefab, enemy));
                            break;
                    }
                }

                attackType = ATTACK_TYPE.ROOT;
                yield return StartCoroutine(attackScript[(int)attackType].Root());
            }
            else
            {
                attackType = (ATTACK_TYPE)Random.Range(1, 4/*4*/);

                switch (attackType)
                {
                    case ATTACK_TYPE.WAVE:
                        yield return StartCoroutine(attackScript[(int)attackType].Wave());
                        break;
                    case ATTACK_TYPE.DROP:
                        yield return StartCoroutine(attackScript[(int)attackType].Drop(branchPrefab));
                        break;
                    case ATTACK_TYPE.SUMMON:
                        yield return StartCoroutine(attackScript[(int)attackType].Summon(effectPrefab, enemy));
                        break;
                }
            }

            if (!redFlag && breakFlag)
            {
                while (breakFlag)
                {
                    yield return null;
                    if (redFlag)
                    {
                        BGM.Boss2BGM();
                        BGM.PlayBGM();
                        break;
                    }
                }
            }
            else if (redFlag && breakFlag)
            {
                while (breakFlag)
                {
                    yield return null;
                }
                if (!deathFlag)
                {
                    if(!attackScript[0].StopRootCoroutine)
					{
                        attackScript[0].StopRootCoroutine = true;
					}
                    while(root2Coroutine == null)
					{
                        yield return null;
					}
                    while(!attackScript[0].StopRootCoroutine)
					{
                        yield return null;
					}
                    attackScript[0].StopRootCoroutine = false;
                    root2Coroutine = StartCoroutine(attackScript[0].Root2());
                }
            }
        }
    }

    IEnumerator Death()
	{
        BGM.StopBGM();

        Time.timeScale = 0.1f;

        yield return new WaitForSeconds(0.1f);

        Time.timeScale = 1;

        StartCoroutine(eventController.DefeatBoss());
        PlayerController.Stopping();

        yield return new WaitForSeconds(1);

        SE.BossSE(3);

        for(float i = 1; i >= 0; i -= 0.01f)
        {
            transform.position = Vector2.Lerp(new Vector2(transform.position.x, -30), transform.position, i);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(3);

        Instantiate(endingPrefab);

        Destroy(gameObject);
    }


    IEnumerator TalkBoss()
    {
        yield return new WaitForSeconds(1);
    }

    public void DamageShield(int damage)
    {
        bossShield -= damage;

        if (bossShield <= 0 && !breakFlag)
        {
            breakFlag = true;
            SE.BossSE(2);
            attackScript[0].StopRootCoroutine = true;
            StartCoroutine(attackScript[0].OffRoot());
            StartCoroutine(attackScript[4].Core());
        }
    }
    public IEnumerator HealShield()
    {
        for (float plus = 0; plus <= 1; plus += 0.01f)
        {
            bossShield = (int)Mathf.Lerp(0, maxBossHP, plus);
            yield return new WaitForSeconds(0.05f);
        }
        bossShield = maxBossShield;

        yield return new WaitForSeconds(1);

        breakFlag = false;
    }

    public void AttackAnim()
    {
        anim.SetTrigger("AttackState");
    }

    public void DamageHP(int damage)
    {
        if (!breakFlag)
            return;

        bossHP -= damage;
        anim.SetTrigger("DamageState");
        if (bossHP <= 0 && !deathFlag)
        {
            deathFlag = true;
        }
        else if (bossHP <= 30 && !redFlag)
        {
            Debug.Log("redFlag");
            attackScript[4].StopRootCoroutine = true;
            redFlag = true;
            if (root2Coroutine == null)
            {
                root2Coroutine = StartCoroutine(attackScript[0].Root2());
            }
            StartCoroutine(attackScript[4].OffCore());
        }
    }

    public int BossHP
    {
        get { return bossHP; }
    }
    public int BossShield
    {
        get { return bossShield; }
    }

    public void DamageSE()
    {
        SE.BossSE(4);
    }

    public void DropSE()
    {
        SE.BossSE(1);
    }

    public void WaveSE()
    {
        SE.BossSE(0);
    }
    public void StopSE()
	{
        SE.StopSE();
	}
}