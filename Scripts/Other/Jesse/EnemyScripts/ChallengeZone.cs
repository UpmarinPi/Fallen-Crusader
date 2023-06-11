using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChallengeZone : MonoBehaviour
{
    // a class that hold enemy wave information
    // 敵のウェーブについての情報を持つクラス
    [System.Serializable]
    public class GameobjectListClass
    {
        public string name;
        public List<GameObject> EnemyGroup;
        public float spawnDelay;
    }

    // these values are to be used in the inspector to change the incoming waves of enemies, and the delay between spawns
    // これらの値はインスペクターで変えて、出現する敵の数と種類と、出現時間を変える
    [SerializeField] List<GameobjectListClass> enemyWaves;
    [SerializeField] GameObject[] WallPositions;
    [SerializeField] float[] wallLengths;
    [SerializeField] GameObject effectPrefab;
    [SerializeField] GameObject wallPrefab;
    [SerializeField] float effectTime = 1.5f;
    [SerializeField] float spawnDelay;
    [SerializeField] float spawnOffsetY = -1.5f;
    GameObject enemyParent;
    int enemyCount = 0;
    float GroupDelay = 2;
    bool wallFlag = false;
    bool battleStartFlag = false;
    bool battleEndFlag = false;
    bool activationFlag = false;
    bool delayFlag = false;
    bool startFlag = false;
    bool wallUpFlag = false;
    bool wallDownFlag = false;
    bool finalFlag = false;
    Vector3 pos;
    BoxCollider2D spawnCollider;

    void Start()
    {
        spawnCollider = GetComponent<BoxCollider2D>();
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
        // if player is detected start battle
        // プレイヤーを索敵したら戦闘開始
        if (activationFlag)
        {
            if (battleStartFlag && !startFlag)
            {
                startFlag = true;
                enemyParent = new GameObject("enemyHolder");
                enemyParent.transform.position = transform.position;
            }
            if (wallFlag && battleStartFlag && !wallUpFlag)
            {
                wallUpFlag = true;
                for (int i = 0; i < wallLengths.Length; i++)
                    StartCoroutine(RaiseWall(i));
            }

            // if all enemies are defeated lower the walls and end battle
            // 敵が全て倒されたら壁おろして戦闘終了
            if (enemyParent != null)
            {
                if (enemyParent.transform.childCount == 0 && battleEndFlag && !finalFlag)
                {
                    finalFlag = true;
                }
                if (finalFlag && !wallDownFlag)
                {                    
                    wallDownFlag = true;
                    activationFlag = false;
                    BGMController.FIGHTBGMFLAG = false;
                    Destroy(enemyParent);
                    for (int i = 0; i < wallLengths.Length; i++)
                        StartCoroutine(LowerWall(i));                    
                }
            }
            // delay new waves if there are more than 3 enemies out
            // 敵が３体以上いたら次のウェーブを遅らせる
            if (enemyCount >= 3 && !delayFlag)
            {
                enemyCount = 0;
                delayFlag = true;
                Invoke("InvokeDelay", GroupDelay);
            }

            // if the battle is over or time is stopped, dont spawn new enemies
            // 戦闘が終わってるか、時が止まっていたら敵を出現しない
            if (battleEndFlag || MagicController.freezeFlag)
            {
                return;
            }

            if (enemyParent != null)
            {
                if (battleStartFlag)
                {
                    StartCoroutine(BattleLoop());
                    battleStartFlag = false;
                }
            }
        }
    }



    // battle loop
    // 戦闘ループ
    IEnumerator BattleLoop()
    {
        int waveNumber;
        int counter = 0;
        for (waveNumber = 0; waveNumber < enemyWaves.Count; waveNumber++)
        {
            while(counter < enemyWaves[waveNumber].EnemyGroup.Count)
            {
                while(MagicController.freezeFlag)
                {
                    yield return new WaitForFixedUpdate();
                }
                StartCoroutine(SpawnEnemy(enemyWaves[waveNumber].EnemyGroup[counter], enemyWaves[waveNumber].spawnDelay));
                delayFlag = true;
                counter++;
                while (delayFlag)
                {
                    yield return new WaitForFixedUpdate();
                }
            }
            counter = 0;

            while(0 < enemyParent.transform.childCount)
            {
                yield return new WaitForFixedUpdate();
            }

        }
        battleEndFlag = true;
    }


    // enemy spawn function
    // 敵出現関数
    IEnumerator SpawnEnemy(GameObject enemy, float delay)
    {
        delayFlag = true;
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
            yield return new WaitForSeconds(delay + GroupDelay);
            delayFlag = false;
        }
        else
        {
            yield return new WaitForSeconds(delay);
            delayFlag = false;
        }
    }

    // raise wall
    // 壁を下す
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
        Debug.Log(wallLengths.Length);
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
                pos.x += wallLengths[counter] / 75f;
                wall.transform.position = pos;
                pos = sprite.size;
                pos.y += (wallLengths[counter] * 2) / 75f;
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
                pos.y += wallLengths[counter] / 75f;
                wall.transform.position = pos;
                pos = sprite.size;
                pos.y += (wallLengths[counter] * 2) / 75f;
                sprite.size = pos;
                collider.size = pos;
                yield return new WaitForSeconds(0.01f);
            }
        }

    }

    // lower wall
    // 壁を下す
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
                pos.x -= wallLengths[counter] / 50f;
                wall.transform.position = pos;
                pos = sprite.size;
                pos.y -= (wallLengths[counter] * 2) / 50f;
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
                if (counter == (wallLengths.Length - 1))
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
                pos.y -= wallLengths[counter] / 50f;
                wall.transform.position = pos;
                pos = sprite.size;
                pos.y -= (wallLengths[counter] * 2) / 50f;
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
                if (counter == (wallLengths.Length - 1))
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }

    void InvokeDelay()
    {
        delayFlag = false;
    }

    // if player is detected start battle and play battle BGM
    // プレイヤーが索敵されたら戦闘開始して、戦闘BGMを流す
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !activationFlag)
        {
            battleStartFlag = true;
            activationFlag = true;
            BGMController.FIGHTBGMFLAG = true;
        }

    }

    // function to detect if battle is over
    // 戦闘が終わったか確認する関数
    public bool EndBattle()
    {
        return battleEndFlag;
    }
}
