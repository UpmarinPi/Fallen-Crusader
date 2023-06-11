using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveIf : MonoBehaviour
{
    // these values are to be used in the inspector to adjust direction, length, and speed of extending wall
    // これらの値はインスペクターで変えて、伸びる壁の長さ、方向、と伸びる速さを変える
    [SerializeField] bool moveHorizontal = false;
    [SerializeField] bool moveVertical = false;
    [SerializeField] float distance = 0;
    [SerializeField] int intervalint = 50;
    [SerializeField] bool afterBattle = false;
    [SerializeField] bool afterfruit = false;
    [SerializeField] bool afterchallenge = false;
    [SerializeField] bool doorFlag = false;
    [SerializeField] bool visibleFlag = false;
    float interval = 0f;
    bool endFlag = false;
    [SerializeField] GameObject EventAfterObject;
    BattleZone battle;
    ChallengeZone challenge;
    JungleFruit fruit;
    SpriteRenderer sprite;
    BoxCollider2D box;
    Vector3 pos;
    void Start()
    {
        interval = intervalint;
		if (!visibleFlag)
		{
            GetComponent<SpriteRenderer>().enabled = false;
		}

        // these will activate walls based on the object (EX if a challenge object, extend walls)
        // これらはオブジェクトによって壁を伸ばす (例　チャレンジのオブジェクトなら壁を伸ばす)
        if (afterchallenge)
		{
            EventAfterObject = EventAfterObject.transform.GetChild(0).gameObject;
            challenge = EventAfterObject.GetComponent<ChallengeZone>();
            sprite = GetComponent<SpriteRenderer>();
            box = GetComponent<BoxCollider2D>();
            if (doorFlag)
            {
                if (moveVertical)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    StartCoroutine(SlideObjectVertical());
                }
                else if (moveHorizontal)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    StartCoroutine(SlideObjectHorizontal());
                }
            }
        }
        else if (afterBattle)
        {
            battle = EventAfterObject.GetComponent<BattleZone>();
            sprite = GetComponent<SpriteRenderer>();
            box = GetComponent<BoxCollider2D>();
            if (doorFlag)
            {
                if (moveVertical)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    StartCoroutine(SlideObjectVertical());
                }
                else if (moveHorizontal)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    StartCoroutine(SlideObjectHorizontal());
                }
            }
        }
        else if (afterfruit)
        {
            fruit = EventAfterObject.GetComponent<JungleFruit>();
            sprite = GetComponent<SpriteRenderer>();
            box = GetComponent<BoxCollider2D>();
            if (doorFlag)
            {
                if (moveVertical)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    StartCoroutine(SlideObjectVertical());
                }
                else if (moveHorizontal)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    StartCoroutine(SlideObjectHorizontal());
                }
            }
        }
    }

    
    void FixedUpdate()
    {
        
        if (afterBattle)
        {
            if (moveHorizontal && battle.EndBattle() && !endFlag)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                endFlag = true;
                StartCoroutine(MoveObjectHorizontal());
            }
            else if (moveVertical && battle.EndBattle() && !endFlag)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                endFlag = true;
                StartCoroutine(MoveObjectVertical());
            }
        }
        else if (afterfruit)
        {
            if (doorFlag)
            {
                if (moveHorizontal && fruit.getDead() && !endFlag)
                {
                    distance = -distance;
                    endFlag = true;
                    StartCoroutine(SlideObjectHorizontal());
                }
                else if (moveVertical && fruit.getDead() && !endFlag)
                {
                    distance = -distance;
                    endFlag = true;
                    StartCoroutine(SlideObjectVertical());
                }
            }
            else
            {
                if (moveHorizontal && fruit.getDead() && !endFlag)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    endFlag = true;
                    StartCoroutine(MoveObjectHorizontal());
                }
                else if (moveVertical && fruit.getDead() && !endFlag)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    endFlag = true;
                    StartCoroutine(MoveObjectVertical());
                }
            }

        }
        else if (afterchallenge)
        {
            if (doorFlag)
            {
                if (moveHorizontal && challenge.EndBattle() && !endFlag)
                {
                    distance = -distance;
                    endFlag = true;
                    StartCoroutine(SlideObjectHorizontal());
                }
                else if (moveVertical && challenge.EndBattle() && !endFlag)
                {
                    distance = -distance;
                    endFlag = true;
                    StartCoroutine(SlideObjectVertical());
                }
            }
            else
            {
                if (moveHorizontal && challenge.EndBattle() && !endFlag)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    endFlag = true;
                    StartCoroutine(MoveObjectHorizontal());
                }
                else if (moveVertical && challenge.EndBattle() && !endFlag)
                {
                    GetComponent<SpriteRenderer>().enabled = true;
                    endFlag = true;
                    StartCoroutine(MoveObjectVertical());
                }
            }

        }
    }

    // moves object in a direction
    // オブジェクトを指定した方向に動く
    IEnumerator MoveObjectHorizontal()
    {
        for (int i = 0; i < interval; i++)
        {
            pos = transform.position;
            pos.x += distance / interval;
            transform.position = pos;
            yield return new WaitForSeconds(0.01f);
        }
    }
    IEnumerator MoveObjectVertical()
    {
        for (int i = 0; i < interval; i++)
        {
            pos = transform.position;
            pos.y += distance / interval;
            transform.position = pos;
            yield return new WaitForSeconds(0.01f);
        }
    }

    // if object is a door, this function will properly extend the wall and move it 
    // オブジェクトが扉であればこの関数は扉を伸ばして動かす
    IEnumerator SlideObjectHorizontal()
    {
        for (int j = 0; j < 5; j++)
        {
            pos = sprite.size;
            pos.y += 0.01f;
            sprite.size = pos;
            box.size = pos;
            yield return new WaitForSeconds(0.01f);
        }
        for (int k = 0; k < interval; k++)
        {
            pos = transform.position;
            pos.x += distance / interval;
            transform.position = pos;
            pos = sprite.size;
            pos.y += (distance * 2) / interval;
            sprite.size = pos;
            box.size = pos;
            yield return new WaitForSeconds(0.015f);
        }
        if (doorFlag)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
    IEnumerator SlideObjectVertical()
    {

        for (int j = 0; j < 5; j++)
        {
            pos = sprite.size;
            pos.y += 0.01f;
            sprite.size = pos;
            box.size = pos;
            yield return new WaitForSeconds(0.01f);
        }
        for (int k = 0; k < interval; k++)
        {
            pos = transform.position;
            pos.y += distance / interval;
            transform.position = pos;
            pos = sprite.size;
            pos.y += (distance * 2) / interval;
            sprite.size = pos;
            box.size = pos;
            yield return new WaitForSeconds(0.015f);
        }
        if (doorFlag)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }

    }

}
