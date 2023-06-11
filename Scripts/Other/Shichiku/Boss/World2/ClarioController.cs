using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IBoss
{
    IEnumerator StartBoss(EventController eventController);
    int BossHP { get; }
    int BossShield { get; }
}

public class ClarioController : MonoBehaviour, IBoss
{
    Animator anim;
    [SerializeField] string teleportStart;
    [SerializeField] string spikeAttackAnim;
    [SerializeField] string spikeAttackAnim2;
    [SerializeField] string diveAttackAnim;
    [SerializeField] string crossStartAnim;

    int attackNum = 0;
    float scale = 0;
    float scaleX = 1.25f;
    float scaleY = 1.25f;
    float direction = 1;

    [SerializeField] GameObject BossStageCenter;
    [SerializeField] float phase2HeightOffset;
    GameObject player;
    GameObject attackHolder;
    GameObject hitbox;
    float stageCenterDistance;
    float teleportOffset = 1.5f;

    [SerializeField] GameObject crossPrefab;
    [SerializeField] GameObject crownadoPrefab;
    [SerializeField] GameObject spikePrefab;
    [SerializeField] GameObject riftPrefab;
    [SerializeField] float crossHeight;
    [SerializeField] string startState;
    [SerializeField] string endState;
    [SerializeField] string moveState;



    Vector3 playerPos;
    Vector3 bossPos;
    Vector3 stageCenter;

    Boss2Interface damageInterface;

    int bossHP = 500;
    int bossShield = 100;
    float flyingHeight = -4f;
    float crownadoOffsetY = 0.09f;
    bool deathFlag = false;
    bool attackEndFlag = false;
    bool attackingFlag = false;
    bool turnFlag = false;
    bool phase1Flag = true;
    bool phase2Flag = false;
    public bool animDescendFlag = false;
    public bool animMoveFlag = false;

    PlayerFinder playerFinder;
    CameraShakeManager cameraShakeManager;
    CameraShake cameraShake;
    Rigidbody2D rbody;

    
    void Start()
    {
        playerFinder = new PlayerFinder();
        player = playerFinder.GetPlayer();
        cameraShakeManager = new CameraShakeManager();
        cameraShake = cameraShakeManager.GetShakeScript();
        rbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        stageCenter = BossStageCenter.transform.position;
        stageCenterDistance = stageCenter.x - BossStageCenter.transform.GetChild(0).transform.position.x;
        attackHolder = transform.Find("BossAttackHolder").gameObject;
        //attackHolder = GameObject.Find("BossAttackHolder");
        hitbox = transform.Find("HitCollision").gameObject;
    }


	private void FixedUpdate()
	{
        if (transform.localScale.x > 0)        
            scale = 1f;        
        else
            scale = -1f;

        if (player.transform.position.x > transform.position.x)
            direction = 1f;
        else
            direction = -1f;

        if (direction*scaleX != transform.localScale.x && !turnFlag && !attackingFlag)
        {
            turnFlag = true;
            Invoke("InvokeTurn", 0.5f);
        }

    }

	public IEnumerator StartBoss(EventController eventController)
    {
        eventController.CameraMoveBoss();

        yield return new WaitForSeconds(2f);


        while(!deathFlag)
		{

            int rand;
            
            if(phase1Flag)			
                rand = Random.Range(0, 4);            
            else
                rand = Random.Range(0, 5); 




            if (attackNum >= Random.Range(3, 6) && phase1Flag)
			{
                rand = -1;
			}
            else if (attackNum >= Random.Range(3, 6) && phase2Flag)
			{
                rand = -1;
			}


            while (turnFlag)
            {
                yield return null;
            }


            if (bossHP < 400 && phase1Flag)
            {
                PhaseChange();
            }
            else if (rand == -1 && phase1Flag)
			{
                StartCoroutine(Teleport());
                attackNum = -1;
			}
            else if (rand == -1 && phase2Flag)
			{
                StartCoroutine(WingMove(rand));
                attackNum = -1;
            }
            else
			{
				//StartCoroutine(Ascend());

			    if (phase1Flag)
				{
				    if (rand == 0)
                        CrossAttack();
				    else if (rand == 1)
					    StartCoroutine(CrossAim1());
				    else if (rand == 2)
                        StartCoroutine(Crownado1());
				    else if (rand == 3)
                        StartCoroutine(SpikeAttack1());

				}
                else
				{
                    if (rand == 0)
                        CrossAttack();
                    else if (rand == 1)
                        StartCoroutine(CrossAim2());
                    else if (rand == 2)
                        StartCoroutine(Crownado1());
                        //StartCoroutine(CrossAim2());
                    else if (rand == 3)
                        StartCoroutine(SpikeAttack2());
                    else if (rand == 4)
                        StartCoroutine(CrossAll());
                        //StartCoroutine(CrossAim2());
                }


			}

			attackNum++;

            while (!attackEndFlag)
			{
                yield return null;
			}
            attackingFlag = false;

            yield return new WaitForSeconds(2f);
            attackEndFlag = false;

        }


    }

    void InvokeTurn()
	{
        turnFlag = false;
        if(!attackingFlag)
		{
            transform.localScale = new Vector2(direction*scaleX, scaleY);
		}
	}


    IEnumerator Teleport()
	{
        attackingFlag = true;
        anim.Play(teleportStart);
        yield return new WaitForSeconds(0.1f);
        hitbox.SetActive(false);
        yield return new WaitForAnimation(anim, 0);
        float TPPosX = 0;
        if (transform.position.x > stageCenter.x)
		{
            TPPosX = Random.Range( -stageCenterDistance + teleportOffset, 0 - teleportOffset);
		}
        else
		{
            TPPosX = Random.Range(0 + teleportOffset, stageCenterDistance - teleportOffset);
		}
        transform.position = new Vector2(stageCenter.x + TPPosX, stageCenter.y);
        anim.SetTrigger(endState);
        if (transform.position.x > player.transform.position.x)
            transform.localScale = new Vector2(-scaleX, scaleX);
        else 
            transform.localScale = new Vector2(scaleX, scaleX);
        GetComponent<SpriteRenderer>().color = Color.white;
        yield return null;
        yield return new WaitForAnimation(anim, 0);
        hitbox.SetActive(true);

        attackEndFlag = true;
	}

    
    // ëÊàÍíiäKÇÃè\éöâÀçUåÇ
    // édólåàíËÇ≈ÇÕÇ»Ç¢

    IEnumerator Ascend()
    {
        attackingFlag = true;
        for (int i = 0; i < 50; i++)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + (6f / 50f));
            yield return new WaitForFixedUpdate();
        }
        transform.localScale = new Vector2(scaleX, scaleY);
        anim.Play(diveAttackAnim);
        while (!animDescendFlag)
        {
            yield return null;
        }
        for (int i = 0; i < 15; i++)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y - (6f / 15f));
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1f);
        attackEndFlag = true;
    }

    void CrossAttack()
	{
        bool facingAwayFlag = false;

        if ((transform.position.x > stageCenter.x) && scale > 0)
            facingAwayFlag = true;
        else if ((transform.position.x < stageCenter.x) && scale < 0)
            facingAwayFlag = true;
        else
            facingAwayFlag = false;

        if (phase1Flag)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) > 3f && !facingAwayFlag)
                StartCoroutine(Cross1Forward());
            else 
                StartCoroutine(Cross1Area());

        }
        else if (phase2Flag)
        {
            if (Mathf.Abs(player.transform.position.x - transform.position.x) > 3f && !facingAwayFlag)
                StartCoroutine(Cross2Forward());
            else 
                StartCoroutine(Cross2Area());

        }


	}

    IEnumerator Cross1Area()
    {
        attackingFlag = true;
        GameObject[] Crosses = new GameObject[6];
        float prefabX = 1.5f;
        int order = 0;
        for (int i = 0; i < Crosses.Length/2; i++)
        {
            //Quaternion rotation = Quaternion.Euler(0, 0, 180);
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            Vector2 pos = new Vector2(attackHolder.transform.position.x + prefabX, attackHolder.transform.position.y + crossHeight);
            Crosses[order] = Instantiate(crossPrefab, pos, rotation, attackHolder.transform) as GameObject;
            damageInterface = Crosses[order].GetComponent<Boss2Interface>();
            damageInterface.thisType = Boss2Interface.attackType.CrossStabAttack;
            order++;
            pos = new Vector2(attackHolder.transform.position.x + -prefabX, attackHolder.transform.position.y + crossHeight);
            //rotation = Quaternion.Euler(0, 0, -180);
            rotation = Quaternion.Euler(0, 0, 0);
            Crosses[order] = Instantiate(crossPrefab, pos, rotation, attackHolder.transform) as GameObject;
            damageInterface = Crosses[order].GetComponent<Boss2Interface>();
            damageInterface.thisType = Boss2Interface.attackType.CrossStabAttack;
            order++;
            yield return new WaitForSeconds(0.3f);
            prefabX += 3;
        }
        order = 0;
        for (int i = 0; i < Crosses.Length/2; i++)
		{
            StartCoroutine(CrossStabFunc(Crosses[order]));
            order++;
            StartCoroutine(CrossStabFunc(Crosses[order]));
            order++;
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1f);
        while (Crosses[Crosses.Length-1] != null)
		{
            yield return null;
		}
        attackEndFlag = true;
    }

    IEnumerator Cross1Forward()
    {
        attackingFlag = true;
        GameObject[] Crosses = new GameObject[4];
        float prefabX = 1.5f;
        int order = 0;
        for (int i = 0; i < Crosses.Length; i++)
        {
            //Quaternion rotation = Quaternion.Euler(0, 0, 180 * scale); // #################### temporary negative
            Quaternion rotation = Quaternion.Euler(0, 0, 0 * scale); // #################### temporary negative
            Vector2 pos = new Vector2(attackHolder.transform.position.x + (prefabX * scale), attackHolder.transform.position.y + crossHeight);
            Crosses[i] = Instantiate(crossPrefab, pos, rotation, attackHolder.transform) as GameObject;
            damageInterface = Crosses[order].GetComponent<Boss2Interface>();
            damageInterface.thisType = Boss2Interface.attackType.CrossStabAttack;
            yield return new WaitForSeconds(0.3f);
            prefabX += 3;
        }
        order = 0;
        for (int i = 0; i < Crosses.Length; i++)
        {
            StartCoroutine(CrossStabFunc(Crosses[i]));
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1f);
        while (Crosses[Crosses.Length - 1] != null)
        {
            yield return null;
        }
        attackEndFlag = true;
    }

    IEnumerator CrossStabFunc(GameObject Cross)
	{
        for (int i = 0; i < 30; i++)
        {
            //Cross.transform.Rotate(0, 0, (540f * direction) / 30f);
            Cross.transform.position = new Vector2(Cross.transform.position.x, Cross.transform.position.y + 1.5f / 30f);
            yield return new WaitForFixedUpdate();
        }
        for (int i = 0; i < 5; i++)
        {
            Cross.transform.position = new Vector2(Cross.transform.position.x, Cross.transform.position.y - 0.35f / 3f);
            yield return new WaitForFixedUpdate();
        }
        //Cross.GetComponent<SpriteRenderer>().sprite = attackSprite;
        Cross.GetComponent<BoxCollider2D>().enabled = true;
        for(int i = 0; i < 6; i++)
        {
            Cross.transform.position = new Vector2(Cross.transform.position.x, Cross.transform.position.y - (5f-0.35f)/6);
            yield return new WaitForFixedUpdate();
        }
        cameraShake.ShakeCamera(0.1f, 0.1f);
        float inverse = 1f;
        for (int i = 0; i < 6; i++)
        {
            Cross.transform.position = new Vector2(Cross.transform.position.x, Cross.transform.position.y + 0.2f * inverse);
            inverse *= -1;
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(0.5f);
        Cross.GetComponent<BoxCollider2D>().enabled = false;
        //Cross.GetComponent<SpriteRenderer>().sprite = originSprite;
        Animator anim = Cross.GetComponent<Animator>();
        anim.SetTrigger(endState);
        yield return null;
        yield return new WaitForAnimation(anim, 0);
        Destroy(Cross);
	}

    IEnumerator CrossAim1()
    {
        attackingFlag = true;
        float prefabX = 0;
        if(player.transform.position.x > transform.position.x)
            prefabX = -1.5f;
        else
            prefabX = 1.5f;
		
        Quaternion rotation = Quaternion.Euler(0, 0, 180);
        Vector2 pos = new Vector2(transform.position.x + prefabX, transform.position.y + crossHeight);
        GameObject crossAimPrefab = Instantiate(crossPrefab, pos, rotation, attackHolder.transform) as GameObject;
        damageInterface = crossAimPrefab.GetComponent<Boss2Interface>();
        damageInterface.thisType = Boss2Interface.attackType.CrossAimAttack;
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < 30; i++)
        {
            crossAimPrefab.transform.Rotate(0, 0, (540f * scale) / 30f);
            crossAimPrefab.transform.position = new Vector2(crossAimPrefab.transform.position.x, crossAimPrefab.transform.position.y + 1.5f / 30f);
            yield return new WaitForFixedUpdate();
        }
        float time = 1f;

        float angle = 0;
        while(time > 0)
		{
            time -= Time.deltaTime;

            Vector2 direction = player.transform.position - crossAimPrefab.transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += 90;
            rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            crossAimPrefab.transform.rotation = Quaternion.Slerp(crossAimPrefab.transform.rotation, rotation, 4f * Time.deltaTime);
            
            yield return new WaitForFixedUpdate();

        }
        yield return new WaitForSeconds(0.2f);
        Rigidbody2D rbody2d = crossAimPrefab.GetComponent<Rigidbody2D>();

        Vector3 targ = crossAimPrefab.transform.TransformDirection(-transform.up);
        Vector2 shift = new Vector2(0, 2f);
        for (int i = 0; i < 20; i++)
		{
            crossAimPrefab.transform.Translate(shift/20f, Space.Self);
            yield return new WaitForFixedUpdate();
		}
        crossAimPrefab.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        rbody2d.velocity = new Vector2(targ.x * 30, targ.y * 30);
        //crossAimPrefab.GetComponent<SpriteRenderer>().sprite = attackSprite;


        time = 2f;
        while (time > 0 && crossAimPrefab.transform.position.y > stageCenter.y -0.25f)
		{
            time -= Time.deltaTime;
            yield return null;
		}
        rbody2d.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.5f);
        crossAimPrefab.GetComponent<BoxCollider2D>().enabled = false;
        Animator anim = crossAimPrefab.GetComponent<Animator>();
        anim.SetTrigger(endState);

        yield return null;
        yield return new WaitForAnimation(anim, 0);

        Destroy(crossAimPrefab);
        attackEndFlag = true;
    }

    IEnumerator Crownado1()
    {
        attackingFlag = true;
        Vector2 playerPos = player.transform.position;
        int direction = 0;
        Vector2 crownadoCenter;
        if (playerPos.x > stageCenter.x)
            direction = 1;
        else
            direction = -1;
        if (Mathf.Abs(stageCenter.x - playerPos.x) >  stageCenterDistance-4.5f)		
            crownadoCenter = new Vector2(stageCenter.x + (stageCenterDistance - 4.5f) * direction, stageCenter.y);		
        else		
            crownadoCenter = new Vector2(playerPos.x, stageCenter.y);

        List<float> spawnSpots = new List<float> {-3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f};
        GameObject[] crownadoArray = new GameObject[3];
        for (int i = 0; i < 3; i++)
		{
            GameObject crownado = Instantiate(crownadoPrefab, crownadoCenter, crownadoPrefab.transform.rotation , attackHolder.transform);
            crownadoArray[i] = crownado;
            int random = Random.Range(0, spawnSpots.Count);
            float offsetX = spawnSpots[random];
            spawnSpots.RemoveAt(random);
            crownado.transform.position = new Vector2(crownado.transform.position.x + offsetX, crownado.transform.position.y - crownadoOffsetY);
            Animator crownadoAnim = crownado.GetComponent<Animator>();
            yield return new WaitForAnimation(crownadoAnim, 0);
            StartCoroutine(CrownadoMove(crownado));
		}
        while (crownadoArray[crownadoArray.Length - 1] != null)
        {
            yield return null;
        }
        attackEndFlag = true;

        yield return new WaitForSeconds(1f);
    }

    IEnumerator CrownadoMove(GameObject crownado)
	{
        Animator anim = crownado.GetComponent<Animator>();
        anim.SetTrigger(startState);
        yield return new WaitForSeconds(0.2f);
        int direction = Random.Range(0, 2);
        if (direction == 0)
            direction = -1;
        else
            direction = 1;
        for (int i = 0; i < 60; i++)
		{
            if (i%15 == 0)
			{
                direction = Random.Range(0, 2);
                if (direction == 0)
                    direction = -1;
                else
                    direction = 1;
            }
            Vector2 translateDistance = new Vector2(0.015f * direction, 0);
            crownado.transform.Translate(translateDistance);
            yield return new WaitForFixedUpdate();
		}
        anim.SetBool(endState, true);
        yield return null;
        yield return new WaitForAnimation(anim, 0);
        yield return null;
        yield return new WaitForAnimation(anim, 0);
        Destroy(crownado);
	}

    IEnumerator SpikeAttack1()
	{
        attackingFlag = true;
        anim.Play(spikeAttackAnim);
        yield return new WaitForSeconds(1.1f); // temporary solution
        //anim.SetTrigger(endState);   for split animation

        Vector2 spawnPos = stageCenter;
        float offset = 0.5f * scale;
        spawnPos.x = transform.position.x + offset;
        GameObject spike = Instantiate(spikePrefab, spawnPos, spikePrefab.transform.rotation, attackHolder.transform);
        Animator animSpike = spike.GetComponentInChildren<Animator>();
        yield return null;
        yield return new WaitForAnimation(animSpike, 0);
        Destroy(spike);
        attackEndFlag = true;
	}












    // ############################################################################## ëÊìÒíiäK

    void PhaseChange()
	{
        phase1Flag = false;
        phase2Flag = true;
        attackNum = 0;
        scaleX = 1.0f;
        scaleY = 1.0f;
        teleportOffset += 0.5f;
        transform.localScale = new Vector2(scaleX, scaleY);
        attackHolder.transform.localScale = new Vector2(1.5625f, 1.5625f);
        Vector3 tempPos = BossStageCenter.transform.position;
        tempPos.y += phase2HeightOffset;
        BossStageCenter.transform.position = tempPos;
        tempPos = transform.position;
        tempPos.y += phase2HeightOffset;
        transform.position = tempPos;
        anim.SetBool("Phase2", true);
        GetComponent<SpriteRenderer>().flipX = true;
        hitbox.transform.GetChild(0).gameObject.SetActive(false);
        hitbox.transform.GetChild(1).gameObject.SetActive(true);
        StartCoroutine(Ascend());
    }

    IEnumerator DrillAttack()
	{
        yield return null;
	}
    IEnumerator Cross2Area()
    {
        attackingFlag = true;
        GameObject[] Crosses = new GameObject[8];
        int prefabX = 1;
        int order = 0;
        anim.Play(crossStartAnim);

        for (int i = 0; i < Crosses.Length / 2; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            Vector2 pos = new Vector2(attackHolder.transform.position.x + prefabX, stageCenter.y + crossHeight);
            Crosses[order] = Instantiate(crossPrefab, pos, rotation, attackHolder.transform) as GameObject;
            damageInterface = Crosses[order].GetComponent<Boss2Interface>();
            damageInterface.thisType = Boss2Interface.attackType.CrossStabAttack;
            order++;
            pos = new Vector2(attackHolder.transform.position.x + -prefabX, stageCenter.y + crossHeight);
            rotation = Quaternion.Euler(0, 0, 0);
            Crosses[order] = Instantiate(crossPrefab, pos, rotation, attackHolder.transform) as GameObject;
            damageInterface = Crosses[order].GetComponent<Boss2Interface>();
            damageInterface.thisType = Boss2Interface.attackType.CrossStabAttack;
            order++;
            yield return new WaitForSeconds(0.3f);
            prefabX += 1;
        }
        order = 0;
        anim.SetTrigger(endState);
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < Crosses.Length / 2; i++)
        {
            StartCoroutine(CrossStabFunc(Crosses[order]));
            order++;
            StartCoroutine(CrossStabFunc(Crosses[order]));
            order++;
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1f);
        while (Crosses[Crosses.Length - 1] != null)
        {
            yield return null;
        }
        attackEndFlag = true;
    }

    IEnumerator Cross2Forward()
    {
        attackingFlag = true;
        GameObject[] Crosses = new GameObject[6];
        int prefabX = 1;
        int order = 0;
        anim.Play(crossStartAnim);

        for (int i = 0; i < Crosses.Length; i++)
        {
            Quaternion rotation = Quaternion.Euler(0, 0, 0 * scale); // #################### temporary negative
            Vector2 pos = new Vector2(attackHolder.transform.position.x + (prefabX * scale), stageCenter.y + crossHeight);
            Crosses[i] = Instantiate(crossPrefab, pos, rotation, attackHolder.transform) as GameObject;
            damageInterface = Crosses[order].GetComponent<Boss2Interface>();
            damageInterface.thisType = Boss2Interface.attackType.CrossStabAttack;
            yield return new WaitForSeconds(0.3f);
            prefabX += 2;
        }
        order = 0;
        anim.SetTrigger(endState);
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < Crosses.Length; i++)
        {
            StartCoroutine(CrossStabFunc(Crosses[i]));
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1f);
        while (Crosses[Crosses.Length - 1] != null)
        {
            yield return null;
        }
        attackEndFlag = true;
    }

    IEnumerator CrossAll()
    {
        attackingFlag = true;
        GameObject[] Crosses = new GameObject[14];
        int prefabX = 2;
        int order = 0;
        anim.Play(crossStartAnim);

        for (int i = 0; i < Crosses.Length / 2; i++)
        {
            //Quaternion rotation = Quaternion.Euler(0, 0, 180);
            Quaternion rotation = Quaternion.Euler(0, 0, 0);
            Vector2 pos = new Vector2(stageCenter.x + prefabX, stageCenter.y + crossHeight);
            Crosses[order] = Instantiate(crossPrefab, pos, rotation, attackHolder.transform) as GameObject;
            damageInterface = Crosses[order].GetComponent<Boss2Interface>();
            damageInterface.thisType = Boss2Interface.attackType.CrossStabAttack;
            order++;
            pos = new Vector2(stageCenter.x + -prefabX, stageCenter.y + crossHeight);
            //rotation = Quaternion.Euler(0, 0, -180);
            rotation = Quaternion.Euler(0, 0, 0);
            Crosses[order] = Instantiate(crossPrefab, pos, rotation, attackHolder.transform) as GameObject;
            damageInterface = Crosses[order].GetComponent<Boss2Interface>();
            damageInterface.thisType = Boss2Interface.attackType.CrossStabAttack;
            order++;
            yield return new WaitForSeconds(0.3f);
            prefabX += 2;
        }
        order = 0;
        anim.SetTrigger(endState);
        yield return new WaitForSeconds(0.05f);

        for (int i = 0; i < Crosses.Length / 2; i++)
        {
            StartCoroutine(CrossStabFunc(Crosses[order]));
            order++;
            StartCoroutine(CrossStabFunc(Crosses[order]));
            order++;
            yield return new WaitForSeconds(0.3f);
        }
        yield return new WaitForSeconds(1f);
        while (Crosses[Crosses.Length - 1] != null)
        {
            yield return null;
        }
        attackEndFlag = true;
    }

    IEnumerator CrossAim2()
    {
        attackingFlag = true;
        anim.Play(crossStartAnim);

        float[] spawnPosX = { 2f, 0, 2f };
        GameObject[] crossArray = new GameObject[3];

        Quaternion rotation = Quaternion.Euler(0, 0, 180);

        if (player.transform.position.x > transform.position.x)        
            spawnPosX[2] *= -1f;        
        else        
            spawnPosX[0] *= -1f;
        
        
        for (int i = 0; i < 3; i++)
        {
            Vector2 pos = new Vector2(transform.position.x + spawnPosX[i], transform.position.y + crossHeight);
            GameObject crossAimPrefab = Instantiate(crossPrefab, pos, rotation, attackHolder.transform) as GameObject;
            damageInterface = crossAimPrefab.GetComponent<Boss2Interface>();
            damageInterface.thisType = Boss2Interface.attackType.CrossAimAttack;
            crossArray[i] = crossAimPrefab;

            StartCoroutine(CrossAim2Create(crossArray[i]));
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);

        float time = 1f;

        float angle = 0;
        while (time > 0)
        {
            time -= Time.deltaTime;

            Vector2 direction = player.transform.position - crossArray[1].transform.position;
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle += 90;
            rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            float angleDiff = 5f;

            angleDiff -= Mathf.Abs(transform.position.x - player.transform.position.x)/2;
            if (angleDiff < 0)
                angleDiff = 0;

            for(int i = 0; i < 3; i++)
            {
                Quaternion rot = rotation;
                if(i == 0 && spawnPosX[0] > 0)
                    rot = Quaternion.AngleAxis(angle - angleDiff, Vector3.forward);
                else if (i == 2 && spawnPosX[0] > 0)
                    rot = Quaternion.AngleAxis(angle + angleDiff, Vector3.forward);
                else if (i == 0 && spawnPosX[0] < 0)
                    rot = Quaternion.AngleAxis(angle + angleDiff, Vector3.forward);
                else if (i == 2 && spawnPosX[0] < 0)
                    rot = Quaternion.AngleAxis(angle - angleDiff, Vector3.forward);
                crossArray[i].transform.rotation = Quaternion.Slerp(crossArray[i].transform.rotation, rot, 4f * Time.deltaTime);
            }

            yield return new WaitForFixedUpdate();

        }
        yield return new WaitForSeconds(0.2f);

        anim.SetTrigger(endState);

        if(spawnPosX[0] > 0 && player.transform.position.x > transform.position.x) // started right, stayed right
        {
            for (int i = 2; i > -1; i--)
            {
                StartCoroutine(CrossAim2AttackFunc(crossArray[i]));
                yield return new WaitForSeconds(0.2f);
            }
        }
        else if (spawnPosX[0] > 0 && player.transform.position.x < transform.position.x) // started right, went left
        {
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(CrossAim2AttackFunc(crossArray[i]));
                yield return new WaitForSeconds(0.2f);
            }
        }
        else if (spawnPosX[0] < 0 && player.transform.position.x < transform.position.x) // started left, stayed left
        {
            for (int i = 2; i > -1; i--)
            {
                StartCoroutine(CrossAim2AttackFunc(crossArray[i]));
                yield return new WaitForSeconds(0.2f);
            }
        }
        else if (spawnPosX[0] < 0 && player.transform.position.x > transform.position.x) // started left, went right
        {
            for (int i = 0; i < 3; i++)
            {
                StartCoroutine(CrossAim2AttackFunc(crossArray[i]));
                yield return new WaitForSeconds(0.2f);
            }
        }

        
        while(crossArray[2] != null)
        {
            yield return new WaitForFixedUpdate();
        }


        attackEndFlag = true;
    }

    IEnumerator CrossAim2Create(GameObject prefab)
    {
        yield return new WaitForSeconds(0.3f);
        for (int j = 0; j < 30; j++)
        {
            prefab.transform.Rotate(0, 0, (540f * scale) / 30f);
            prefab.transform.position = new Vector2(prefab.transform.position.x, prefab.transform.position.y + 1.5f / 30f);
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator CrossAim2AttackFunc(GameObject prefab)
    {
        Rigidbody2D rbody2d = prefab.GetComponent<Rigidbody2D>();
        Vector3 targ = prefab.transform.TransformDirection(-transform.up);

        Vector2 shift = new Vector2(0, 2f);
        yield return new WaitForSeconds(0.05f);
        for (int i = 0; i < 20; i++)
        {
            prefab.transform.Translate(shift / 20f, Space.Self);
            yield return new WaitForFixedUpdate();
        }
        prefab.GetComponent<BoxCollider2D>().enabled = true;
        yield return new WaitForSeconds(0.05f);
        rbody2d.velocity = new Vector2(targ.x * 30, targ.y * 30);
        //crossAimPrefab.GetComponent<SpriteRenderer>().sprite = attackSprite;


        float time = 3f;
        while (time > 0 && prefab.transform.position.y > stageCenter.y - 0.25f)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        rbody2d.velocity = new Vector2(0, 0);
        yield return new WaitForSeconds(0.5f);
        prefab.GetComponent<BoxCollider2D>().enabled = false;
        Animator animCross = prefab.GetComponent<Animator>();
        animCross.SetTrigger(endState);

        yield return new WaitForSeconds(0.5f);
        yield return new WaitForAnimation(anim, 0);

        Destroy(prefab);
    }


    IEnumerator SpikeAttack2()
    {
        attackingFlag = true;
        anim.Play(spikeAttackAnim2); 
        yield return new WaitForSeconds(1.15f); // temporary solution
        //anim.SetTrigger(endState);   for split animation

        Vector2 spawnPos = stageCenter;
        spawnPos.x = transform.position.x + 0.75f;
        GameObject spikeRight = Instantiate(spikePrefab, spawnPos, spikePrefab.transform.rotation , attackHolder.transform);
        spawnPos.x -= 1.5f;
        GameObject spikeLeft = Instantiate(spikePrefab, spawnPos, spikePrefab.transform.rotation , attackHolder.transform);
        spikeRight.transform.localScale = new Vector2(spikeRight.transform.localScale.x * scale, spikeRight.transform.localScale.y);
        spikeLeft.transform.localScale = new Vector2(spikeLeft.transform.localScale.x * -1f * scale, spikeLeft.transform.localScale.y);
        Animator animSpike = spikeLeft.GetComponentInChildren<Animator>();
        yield return null;
        yield return new WaitForAnimation(animSpike, 0);
        Destroy(spikeLeft);
        Destroy(spikeRight);
        attackEndFlag = true;
    }

    IEnumerator Crownado2()
    {
        attackingFlag = true;
        Vector2 playerPos = player.transform.position;
        int direction = 0;
        Vector2 crownadoCenter;
        if (playerPos.x > stageCenter.x)
            direction = 1;
        else
            direction = -1;
        if (Mathf.Abs(stageCenter.x - playerPos.x) > stageCenterDistance - 4.5f)
            crownadoCenter = new Vector2(stageCenter.x + (stageCenterDistance - 4.5f) * direction, stageCenter.y);
        else
            crownadoCenter = new Vector2(playerPos.x, stageCenter.y);

        List<float> spawnSpots = new List<float> { -3.5f, -2.5f, -1.5f, -0.5f, 0.5f, 1.5f, 2.5f, 3.5f };
        GameObject[] crownadoArray = new GameObject[3];
        for (int i = 0; i < 3; i++)
        {
            GameObject crownado = Instantiate(crownadoPrefab, crownadoCenter, crownadoPrefab.transform.rotation, attackHolder.transform);
            crownadoArray[i] = crownado;
            int random = Random.Range(0, spawnSpots.Count);
            float offsetX = spawnSpots[random];
            spawnSpots.RemoveAt(random);
            crownado.transform.position = new Vector2(crownado.transform.position.x + offsetX, crownado.transform.position.y - crownadoOffsetY);
            Animator crownadoAnim = crownado.GetComponent<Animator>();
            yield return new WaitForAnimation(crownadoAnim, 0);
            StartCoroutine(CrownadoMove(crownado));
        }
        while (crownadoArray[crownadoArray.Length - 1] != null)
        {
            yield return null;
        }
        attackEndFlag = true;

        yield return new WaitForSeconds(1f);
    }


    IEnumerator RiftAttack()
	{
        yield return null;

        

        /*
        the attack will flow like this
        
        1. 
        begin clario animation

        either use wait time or wait for a bool to be turned true through animation

        spawn rift prefab -> begin rift animation
        
        move crows

        delete rift prefab
          
        */



	}


    IEnumerator WingMove(int movementType)
	{
        attackingFlag = true;
        anim.SetTrigger(moveState);

        while(!animMoveFlag)
		{
            yield return new WaitForFixedUpdate();
		}

        if(movementType == -1)
		{
            // movement
            float deceleration = 0;
            float distance = 0;
            float velocity = 0;
            float time = 1f;
            bool leftFlag = false;

            float TPPosX = 0;
            if (transform.position.x > stageCenter.x)
            {
                TPPosX = Random.Range(-stageCenterDistance + teleportOffset, 0 - teleportOffset);
            }
            else
            {
                TPPosX = Random.Range(0 + teleportOffset, stageCenterDistance - teleportOffset);
            }
            distance = Mathf.Abs(TPPosX) + Mathf.Abs(stageCenter.x - transform.position.x);


            velocity = distance + 5;


            deceleration = (2 * distance - 2 * velocity * time) / (time * time);


            if (TPPosX > 0) // go right
			{
                transform.localScale = new Vector2(scaleX, scaleY);
			}
            else // go left
			{
                transform.localScale = new Vector2(-scaleX, scaleY);
                velocity *= -1f;
                deceleration *= -1f;
                leftFlag = true;
			}



            rbody.velocity = new Vector2(velocity, 0);


            while (time > 0)
            {
                if(!leftFlag)
				{
                    if (velocity > 0)
                        velocity += deceleration * Time.deltaTime;
                    if (velocity < 0)
                        velocity = 0;
				}
                else
				{
                    if (velocity < 0)
                        velocity += deceleration * Time.deltaTime;
                    if (velocity > 0)
                        velocity = 0;
                }
                rbody.velocity = new Vector2(velocity, 0);
                time -= Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            rbody.velocity = Vector2.zero;
        }
		else
		{
            // ascension
            float deceleration = 0;
            float distance = 0;
            float velocity = 0;
            float time = 0.75f;

            distance = Mathf.Abs(flyingHeight - transform.position.y);

            velocity = distance + 10;

            deceleration = (2 * distance - 2 * velocity * time) / (time * time);


            rbody.velocity = new Vector2(velocity, 0);


            while (time > 0)
            {
                if (velocity > 0)
                    velocity += deceleration * Time.deltaTime;
                if (velocity < 0)
                    velocity = 0;
                rbody.velocity = new Vector2(velocity, 0);
                time -= Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }

            rbody.velocity = Vector2.zero;
        }

        attackEndFlag = true;
	}



    public void DamageHP(int damage)
    {
        //if (!breakFlag)
        //    return;

        bossHP -= damage;
        //anim.SetTrigger("DamageState");
        //if (bossHP <= 0 && !deathFlag)
        //{
        //    deathFlag = true;
        //}
        //else if (bossHP <= 30 && !redFlag)
        //{
        //    Debug.Log("redFlag");
        //    attackScript[4].StopRootCoroutine = true;
        //    redFlag = true;
        //    if (root2Coroutine == null)
        //    {
        //        root2Coroutine = StartCoroutine(attackScript[0].Root2());
        //    }
        //    StartCoroutine(attackScript[4].OffCore());
        //}
    }

    public int BossHP { get { return bossHP; } }
    public int BossShield { get { return bossShield; } }
}
