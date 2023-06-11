using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    [SerializeField] GameObject[] EnemyGroup;
    [SerializeField] GameObject[] infiniteGroup;
    [SerializeField] GameObject[] WallPositions;
    [SerializeField] float[] WallLenghts;
    [SerializeField] GameObject effectPrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] float effectTime = 1.5f;
    [SerializeField] float spawnDelay;
    [SerializeField] float spawnOffsetY = -1.5f;
    [SerializeField] bool fruitFlag = false;
    GameObject enemyParent;
    bool infinteFlag = true;
    bool wallFlag = false;
    float GroupDelay = 5;
    float InfiniteDelay = 7;
    bool battleStartFlag = false;
    bool battleEndFlag = false;
    bool activationFlag = false;
    bool delayFlag = false;
    bool startFlag = false;
    bool wallUpFlag = false;
    bool wallDownFlag = false;
    bool finalFlag = false;
    int counter1 = 0;
    int enemyCount = 0;
    Vector3 pos;
    BoxCollider2D spawnCollider;
    JungleFruit jungleFruit;

    void Start()
    {
        spawnCollider = GetComponent<BoxCollider2D>();

        // if fruit spawn enemies infinitely
        //  木の実なら無限に敵を脇だす
        if (!fruitFlag)
        {
            infinteFlag = false;
        }
        else
        {
            jungleFruit = transform.parent.GetComponent<JungleFruit>();
        }
        if (WallPositions.Length == 0)
        {
            wallFlag = false;
        }
        else
        {
            wallFlag = true;
        }
    }


    void FixedUpdate()
    {
        if (activationFlag)
        {
            // start enemy spawning script
            // 敵召喚処理開始
            if (battleStartFlag && !startFlag)
            {
                startFlag = true;
                enemyParent = new GameObject("enemyHolder");
                enemyParent.transform.position = transform.position;
            }
            // erect wall
            // 壁を立てる
            if (wallFlag && battleStartFlag && !wallUpFlag)
            {
                wallUpFlag = true;
                for (int i = 0; i < WallLenghts.Length; i++)
                    StartCoroutine(RaiseWall(i));
            }
            if (enemyParent != null)
            {
                if (enemyParent.transform.childCount == 0 && battleEndFlag && !finalFlag)
                {
                    finalFlag = true;
                }
                // end battle and lower wall
                // 戦闘を中止し壁を下す
                if (finalFlag && !wallDownFlag)
                {
                    if (fruitFlag)
                    {
                        if (enemyParent.transform.childCount == 0 && !delayFlag)
                        {
                            wallDownFlag = true;
                            activationFlag = false;
                            BGMController.FIGHTBGMFLAG = false;
                            Destroy(enemyParent);
                            for (int i = 0; i < WallLenghts.Length; i++)
                                StartCoroutine(LowerWall(i));
                        }
                    }
                    else if (!fruitFlag)
                    {
                        wallDownFlag = true;
                        activationFlag = false;
                        BGMController.FIGHTBGMFLAG = false;
                        Destroy(enemyParent);
                        for (int i = 0; i < WallLenghts.Length; i++)
                            StartCoroutine(LowerWall(i));
                    }
                }
                if (fruitFlag)
                {
                    if (jungleFruit.getDead())
                    {
                        battleEndFlag = true;
                        return;
                    }
                }
            }

            if (enemyCount >= 3 && !delayFlag)
            {
                enemyCount = 0;
                delayFlag = true;
                Invoke("InvokeDelay", GroupDelay);
            }
            if(battleEndFlag || MagicController.freezeFlag)
            {
                return;
            }
            // spawn enemy and wait to spawn again
            // 敵を召喚して、遅れてまた召喚する
            if (enemyParent != null)
            {
                if (enemyParent.transform.childCount >= 6)
                {
                    return;
                }
                if (battleStartFlag)
                {
                    if (!delayFlag && counter1 < EnemyGroup.Length)
                    {
                        StartCoroutine(SpawnEnemy(EnemyGroup[counter1], spawnDelay));
                        counter1++;
                    }
                    else if (counter1 == EnemyGroup.Length && !delayFlag)
                    {
                        if (!infinteFlag && enemyParent.transform.childCount == 0)
                        {
                            battleStartFlag = false;
                            battleEndFlag = true;
                        }
                        else if (infinteFlag)
                        {
                            battleStartFlag = false;
                        }
                    }

                }
                else if (infinteFlag && !battleStartFlag)
                {
                    if (!delayFlag)
                    {
                        int random = Random.Range(0, infiniteGroup.Length);
                        StartCoroutine(SpawnEnemy(infiniteGroup[random], InfiniteDelay));

                    }
                }
            }
        }
    }

    // enemy spawning function 
    // 敵召喚関数
    IEnumerator SpawnEnemy(GameObject enemy, float delay)
    {
        bool hobFlag = false;
        if (enemy.name == "Hobgoblin")
        {
            hobFlag = true;
            enemyCount = 0;
        }
        else
        {
            enemyCount++;
        }
        delayFlag = true;
        float offsetX = Random.Range(spawnCollider.bounds.min.x, spawnCollider.bounds.max.x);
        Vector3 area = new Vector3(offsetX, transform.position.y + spawnOffsetY, -5f);
        GameObject spawnEffect = Instantiate(effectPrefab) as GameObject;
        if (hobFlag)
        {
            spawnEffect.transform.localScale = new Vector3(3f, 3f, 1f);
            area.y += 0.6f;
        }
        spawnEffect.transform.position = area;
        yield return new WaitForSeconds(effectTime);
        Destroy(spawnEffect);
        GameObject prefabEnemy = Instantiate(enemy) as GameObject;
        prefabEnemy.transform.position = area;
        prefabEnemy.transform.parent = enemyParent.transform;

        if (hobFlag)
        {
            Invoke("InvokeDelay", GroupDelay + delay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
            delayFlag = false;
        }
    }

    // function to erect wall
    // 壁を立てる関数
    IEnumerator RaiseWall(int counter)
    {
        GameObject wall = Instantiate(wallPrefab) as GameObject;
        wall.name = "wall" + counter;
        wall.gameObject.layer = 9;
        wall.transform.parent = transform;
        wall.transform.position = WallPositions[counter].transform.position;
        wall.transform.eulerAngles = WallPositions[counter].transform.eulerAngles;
        bool HorizontalFlag = false;
        if (wall.transform.eulerAngles.z == 90 || wall.transform.eulerAngles.z == -90)
        {
            HorizontalFlag = true;
        }
        Debug.Log(WallLenghts.Length);
        Destroy(WallPositions[counter]);
        SpriteRenderer sprite = wall.GetComponent<SpriteRenderer>();
        BoxCollider2D collider = wall.GetComponent<BoxCollider2D>();
        if (HorizontalFlag)
        {
            for (int j = 0; j < 5; j++)
            {
                pos = sprite.size;
                pos.y += 0.01f;
                sprite.size = pos;
                collider.size = pos;
                yield return new WaitForSeconds(0.01f);
            }
            for (int k = 0; k < 75; k++)
            {
                pos = wall.transform.position;
                pos.x += WallLenghts[counter] / 75f;
                wall.transform.position = pos;
                pos = sprite.size;
                pos.y += (WallLenghts[counter] * 2) / 75f;
                sprite.size = pos;
                collider.size = pos;
                yield return new WaitForSeconds(0.015f);
            }
        }
        else
        {
            for (int j = 0; j < 5; j++)
            {
                pos = sprite.size;
                pos.y += 0.01f;
                sprite.size = pos;
                collider.size = pos;
                yield return new WaitForSeconds(0.01f);
            }
            for (int k = 0; k < 75; k++)
            {
                pos = wall.transform.position;
                pos.y += WallLenghts[counter] / 75f;
                wall.transform.position = pos;
                pos = sprite.size;
                pos.y += (WallLenghts[counter] * 2) / 75f;
                sprite.size = pos;
                collider.size = pos;
                yield return new WaitForSeconds(0.01f);
            }
        }

    }

    // function to lower wall
    // 壁を下す関数
    IEnumerator LowerWall(int counter)
    {
        GameObject wall = transform.Find("wall" + counter).gameObject;
        SpriteRenderer sprite = wall.GetComponent<SpriteRenderer>();
        BoxCollider2D collider = wall.GetComponent<BoxCollider2D>();
        bool HorizontalFlag = false;
        if (wall.transform.eulerAngles.z == 90)
        {
            HorizontalFlag = true;
        }


        if (HorizontalFlag)
        {
            for (int k = 0; k < 50; k++)
            {
                pos = wall.transform.position;
                pos.x -= WallLenghts[counter] / 50f;
                wall.transform.position = pos;
                pos = sprite.size;
                pos.y -= (WallLenghts[counter] * 2) / 50f;
                sprite.size = pos;
                collider.size = pos;
                yield return new WaitForSeconds(0.01f);
            }
            for (int j = 0; j < 5; j++)
            {
                pos = sprite.size;
                pos.y -= 0.01f;
                sprite.size = pos;
                collider.size = pos;
                yield return new WaitForSeconds(0.01f);
                if (counter == (WallLenghts.Length - 1))
                {
                    Destroy(this.gameObject);
                }
            }
        }
        else
        {
            for (int k = 0; k < 50; k++)
            {
                pos = wall.transform.position;
                pos.y -= WallLenghts[counter] / 50f;
                wall.transform.position = pos;
                pos = sprite.size;
                pos.y -= (WallLenghts[counter] * 2) / 50f;
                sprite.size = pos;
                collider.size = pos;
                yield return new WaitForSeconds(0.01f);
            }
            for (int j = 0; j < 5; j++)
            {
                pos = sprite.size;
                pos.y -= 0.01f;
                sprite.size = pos;
                collider.size = pos;
                yield return new WaitForSeconds(0.01f);
                if (counter == (WallLenghts.Length - 1))
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    // function to delay enemy spawn
    // 敵召喚の遅延関数
    void InvokeDelay()
    {
        delayFlag = false;
    }

    // function to check if player is in battle area
    // プレイヤーが戦闘エリアに入ったか確認する関数
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activationFlag)
        {
            battleStartFlag = true;
            activationFlag = true;
            BGMController.FIGHTBGMFLAG = true;
        }

    }

    // function to show its safe to delete object
    // 戦闘終了でオブジェクトを消していいと示す関数
    public bool CanDestroy()
    {
        return battleEndFlag;
    }

    // function to show battle has ended
    // 戦闘が終わったと示す関数
    public bool EndBattle()
    {
        return battleEndFlag;
    }


}
