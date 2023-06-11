using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealScript : MonoBehaviour
{
    
    // chestFlag changes whether the object should be treated as a trap or a script to activate enemies in an area
    // chestFlag �̓I�u�W�F�N�g��㩂̈��������邩�A�G���A���̓G��\������X�N���v�g
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
    // �G����̏o�������āA�퓬�I�u�W�F�N�g�𔭓�����
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
    // �v���C���[��������X�N���v�g�𔭓�����
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
