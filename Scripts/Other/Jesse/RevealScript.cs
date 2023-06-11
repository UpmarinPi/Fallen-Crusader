using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealScript : MonoBehaviour
{
    
    // chestFlag changes whether the object should be treated as a trap or a script to activate enemies in an area
    // chestFlag はオブジェクトが罠の扱いをするか、エリア内の敵を表示するスクリプト
    [SerializeField] bool chestFlag = false;
    [SerializeField] GameObject effectPrefab;
    [SerializeField] GameObject enemy;
    GameObject holder;
    bool detectFlag = false;
    Animator anim;

    private void Start()
    {
        holder = new GameObject("enemyHolder");
        holder.transform.position = transform.position;
        transform.GetChild(0).gameObject.SetActive(false);
        if (chestFlag)
        {
            anim = transform.GetComponent<Animator>();
            anim.enabled = false;
        }
    }


    // spawn one enemy, and then activate the battle object
    // 敵を一体出現させて、戦闘オブジェクトを発動する
    IEnumerator delayActive()
    {
        Vector3 pos = transform.position;
        GameObject spawnEffect = Instantiate(effectPrefab) as GameObject;
        pos.y -= 0.3f;
        spawnEffect.transform.position = pos;
        yield return new WaitForSeconds(0.8f);
        Destroy(spawnEffect);
        GameObject prefabEnemy = Instantiate(enemy) as GameObject;
        prefabEnemy.transform.position = pos;
        prefabEnemy.transform.parent = holder.transform;
        yield return new WaitForSeconds(0.8f);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    // if player is detected activate script
    // プレイヤーをしたらスクリプトを発動する
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !detectFlag)
        {
            detectFlag = true;
            if(!chestFlag)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }
            else if(chestFlag)
            {
                anim.enabled = true;
                StartCoroutine(delayActive());
            }
        }
    }

}
