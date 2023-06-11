using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{
    Vector3 pos;
    [SerializeField] GameObject Enemy1;

    [SerializeField] int maxCount = 0;
    [SerializeField] int maxEnemy = 0;
    [SerializeField] bool autoSpawnFlag = false;
    [SerializeField] float effectTime;
    [SerializeField] GameObject spawnEffect;
    int enemyCount = 0;
    int count = 0;
    private int maximumEnemy = 10;
    bool endSpawnFlag = true;
    bool spawningFlag = false;

    void Start()
    {
        pos = transform.position;
    }

  
    
    void FixedUpdate()
    {
        // press to end training mode
        // キー押してトレーニングモード終了させる
        if (Input.GetKeyDown(KeyCode.R))
		{
            endSpawnFlag = false;
		}
        count++;
        // instantly spawn enemy
        // 即座に敵を出現させる
        if (Input.GetKeyDown(KeyCode.Q) && enemyCount < maximumEnemy && !endSpawnFlag)
		{
            GameObject prefab1 = Instantiate(Enemy1) as GameObject;
		    prefab1.transform.position = pos;
		    prefab1.transform.parent = this.transform;

		}

        // spawn an enemy with time
        // 時間差で敵を出現させる
        if (count >= maxCount && autoSpawnFlag && enemyCount < maxEnemy && !endSpawnFlag && !spawningFlag)
		{
            count = 0;
            StartCoroutine(Spawn());

        }

        enemyCount = transform.childCount;

	}

    // start training mode
    // トレーニングモード開始
    public void StartTraining()
	{
        endSpawnFlag = false;
        if (transform.childCount == 0)
        {

        }
        else
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

        }
        
    }

    // end training mode
    // トレーニングモード終了
    public void EndTraining()
	{
        endSpawnFlag = true;
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

    }

    // spawn 1 enemy
    // 敵一体出現する
    IEnumerator Spawn()
	{
        spawningFlag = true;
        GameObject effect = Instantiate(spawnEffect) as GameObject;
        effect.transform.position = pos;
        effect.transform.parent = transform.parent;
        yield return new WaitForSeconds(effectTime);
        Destroy(effect);
        GameObject prefab1 = Instantiate(Enemy1) as GameObject;
        prefab1.transform.position = pos;
        prefab1.transform.parent = this.transform;
        spawningFlag = false;
    }
}
