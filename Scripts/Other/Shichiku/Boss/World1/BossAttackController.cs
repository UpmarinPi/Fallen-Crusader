using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    int childNumber;
    List<GameObject> childObjects = new List<GameObject>();
    Vector3 myPos;

    bool stopFlag = false;
    CameraShakeManager camManager;
    CameraShake CamShake;

    bool root2Flag = false;
    bool offrootFlag = false;

    // 根が出ない場所のランダムの範囲
    const int minRange = 5;
    const int maxRange = 8;

    
    void Start()
    {
        camManager = new CameraShakeManager();
        CamShake = camManager.GetShakeScript();
        childNumber = this.transform.childCount;

        for (int i = 0; i < childNumber; i++)
        {
            childObjects.Add(transform.GetChild(i).gameObject);
        }
    }

    
    void Update()
    {

    }
    void MovePosX(float minPosX, float maxPosX, float plus)
    {
        myPos = transform.localPosition;
        myPos.x = Mathf.Lerp(minPosX, maxPosX, plus);
        transform.localPosition = myPos;
    }
    void MovePosY(float minPosY, float maxPosY, float plus)
    {
        myPos = transform.localPosition;
        myPos.y = Mathf.Lerp(minPosY, maxPosY, plus);
        transform.localPosition = myPos;
    }

    public IEnumerator Root()
    {
        // rand[0] 根が出ない数　rand[1] 弱点の出る場所 rand[2]～rand[childNumber] 根が出ない場所
        int[] rand = new int[childNumber];

        // 乱数の初期化
        Random.InitState(System.DateTime.Now.Millisecond);

        // 初期化
        foreach (GameObject child in childObjects)
        {
            child.SetActive(true);

            RootInit(child);
        }

        // 乱数
        rand[0] = Random.Range(minRange, maxRange);
        rand[1] = Random.Range(0, childNumber);
        if (rand[1] == childNumber - 1)
        {
            rand[2] = 0;
        }
        else
        {
            rand[2] = rand[1] + 1;
        }
        if (rand[1] == 0)
        {
            rand[2] = childNumber - 1;
        }
        else
        {
            rand[3] = rand[1] - 1;
        }
        for (int i = 4; i < rand[0] + 2; i++)
        {
            rand[i] = Random.Range(0, childNumber);

            for (int j = 1; j < i; j++)
            {
                if (rand[i] == rand[j])
                {
                    i--;
                    break;
                }
            }
        }

        // 弱点用の仮コード
        childObjects[rand[1]].GetComponent<SpriteRenderer>().color = Color.red;
        childObjects[rand[1]].transform.GetChild(0).GetComponent<EnemyDamageBossRoot>().IsWeakness = true;

        // 出さないやつ
        for (int i = 0; i < rand[0]; i++)
        {
            childObjects[rand[i + 2]].SetActive(false);
        }

        // ちょっと出る
        for (float plus = 0.0f; plus <= 1; plus += 0.01f)
        {
            MovePosY(0.5f, 1f, plus);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.34f);

        // アタックアニメーション
        transform.parent.GetComponent<BossController>().AttackAnim();

        yield return new WaitForSeconds(0.66f);

        // 飛び出す直前設定
        foreach (GameObject child in childObjects)
        {
            RootBeforeAttack(child);
        }
        CamShake.ShakeCamera(0.2f, 0.2f);
        // ShakeCamera(float ShakeTime, float shakeStrength)
        // 揺れる時間、揺れる強さ

        // 飛び出す
        for (float plus = 0.0f; plus <= 1; plus += 0.1f)
        {
            MovePosY(1f, 1.5f, plus);
            yield return new WaitForSeconds(0.001f);
        }

        // 飛び出した直後の設定
        foreach (GameObject child in childObjects)
        {
            RootAfterAttack(child);
        }

        // シールドが削れてコアが出る時
        for (float time = 0; time <= 3; time += 0.01f)
        {
            if (stopFlag)
            {
                stopFlag = false;
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        // 根っこが戻るコルーチン
        yield return StartCoroutine(OffRoot());
    }

    public IEnumerator Root2()
    {
        Debug.Log("root2");
        yield return new WaitForSeconds(2);
        while (true)
        {
            if(root2Flag)
                yield break;
            root2Flag = true;
            // 初期化
            foreach (GameObject child in childObjects)
            {
                child.SetActive(true);

                RootInit(child);

                child.SetActive(false);
            }

            int[] rand = new int[2];
            int backNumber = 0;

            // 乱数初期化
            Random.InitState(System.DateTime.Now.Millisecond);

            do
            {
                rand[0] = Random.Range(0, childNumber);
            } while (rand[0] == backNumber);

            // 乱数初期化
            Random.InitState(System.DateTime.Now.Millisecond);

            backNumber = rand[0];
            rand[1] = Random.Range(0, 10);
            if (rand[1] >= 0 && rand[1] <= 4)
            {
                childObjects[rand[0]].GetComponent<SpriteRenderer>().color = Color.red;
                childObjects[rand[0]].transform.GetChild(0).GetComponent<EnemyDamageBossRoot>().IsWeakness = true;
            }

            childObjects[rand[0]].SetActive(true);

            // ちょっと出る
            for (float plus = 0.0f; plus <= 1; plus += 0.01f)
            {
                MovePosY(0.5f, 1f, plus);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(1);

            // 飛び出す直前の設定
            foreach (GameObject child in childObjects)
            {
                RootBeforeAttack(child);
            }

            CamShake.ShakeCamera(0.2f, 0.1f);
            // ShakeCamera(float ShakeTime, float shakeStrength)
            // 揺れる時間、揺れる強さ

            // 飛び出す
            for (float plus = 0.0f; plus <= 1; plus += 0.1f)
            {
                MovePosY(1f, 1.5f, plus);
                yield return new WaitForSeconds(0.001f);
            }

            // 飛び出す直後の設定
            foreach (GameObject child in childObjects)
            {
                RootAfterAttack(child);
            }

            // シールドが削れてコアが出る時
            for (float time = 0; time <= 1; time += 0.01f)
            {
                if (stopFlag)
                {
                    root2Flag = false;
                    stopFlag = false;
                    yield break;
                }
                yield return new WaitForSeconds(0.01f);
            }

            yield return StartCoroutine(OffRoot());

            root2Flag = false;
        }
    }

    // 根っこの初期化関数
    void RootInit(GameObject child)
	{
        child.GetComponent<SpriteRenderer>().color = Color.white; //仮
        child.transform.GetChild(0).GetComponent<EnemyDamageBossRoot>().IsWeakness = false;

        child.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        child.transform.GetChild(1).GetComponent<Collider2D>().isTrigger = true;
        child.transform.GetChild(1).tag = "Untagged";
        child.transform.GetChild(1).gameObject.layer = 12;
        child.transform.GetChild(2).GetComponent<Collider2D>().enabled = false;
    }
    
    // 根っこの攻撃直前の関数
    void RootBeforeAttack(GameObject child)
	{
        child.transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
        child.transform.GetChild(1).tag = "BossRoot";
        child.transform.GetChild(2).GetComponent<Collider2D>().enabled = true;
	}

    // 根っこの攻撃直後の関数
    void RootAfterAttack(GameObject child)
	{
        child.transform.GetChild(1).tag = "Untagged";
        child.transform.GetChild(1).GetComponent<Collider2D>().isTrigger = false;
        child.transform.GetChild(1).gameObject.layer = 8;
    }

	public bool StopRootCoroutine
    {
		get
		{
            return stopFlag;
		}
        set
        {
            stopFlag = value;
        }
    }
    public IEnumerator OffRoot()
    {
        Debug.Log("offroot2");
        if(offrootFlag)
            yield break;
        offrootFlag = true;

        // 引っ込む直前の設定
        foreach (GameObject child in childObjects)
        {
            child.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
            child.transform.GetChild(2).GetComponent<Collider2D>().enabled = false;
        }
        // 引っ込む
        for (float plus = 1.0f; plus >= 0; plus -= 0.05f)
        {
            MovePosY(0.5f, 1.5f, plus);
            yield return new WaitForSeconds(0.002f);
        }
        // 引っ込んだ直後の設定
        foreach (GameObject child in childObjects)
        {
            child.transform.GetChild(1).tag = "BossRoot";
            child.transform.GetChild(1).gameObject.layer = 12;
            child.SetActive(false);
        }

        offrootFlag = false;
    }

    public IEnumerator Core()
    {
        yield return new WaitForSeconds(1);

        for (float plus = 0.0f; plus <= 1; plus += 0.05f)
        {
            MovePosY(-2f, -1f, plus);
            yield return new WaitForSeconds(0.001f);
        }

        GetComponent<Collider2D>().enabled = true;

        yield return StartCoroutine(transform.parent.GetComponent<BossController>().HealShield());

        if (stopFlag)
        {
            stopFlag = false;
            yield break;
        }

        StartCoroutine(OffCore());
    }

    public IEnumerator OffCore()
    {
        GetComponent<Collider2D>().enabled = false;

        for (float plus = 1.0f; plus >= 0; plus -= 0.05f)
        {
            MovePosY(-2f, -1f, plus);
            yield return new WaitForSeconds(0.002f);
        }
    }

    // ムチ攻撃
    public IEnumerator Wave()
    {
        // 初期化
        gameObject.SetActive(true);
        transform.localPosition = new Vector3(0.8f, -2, 0);

        // ムチ攻撃準備
        for (float plus = 0.0f; plus <= 1; plus += 0.01f)
        {
            MovePosY(-2f, -0.8f, plus);
            yield return new WaitForSeconds(0.01f);
        }

        transform.parent.GetComponent<BossController>().AttackAnim();

        yield return new WaitForSeconds(0.66f);


        // ムチ攻撃開始
        CamShake.ShakeCamera(0.5f, 0.2f);
        // ShakeCamera(float ShakeTime, float shakeStrength)
        // 揺れる時間、揺れる強さ
        transform.parent.GetComponent<BossController>().WaveSE();

        for (float plus = 1.0f; plus >= 0; plus -= 0.01f)
        {
            MovePosX(-4.3f, 0.8f, plus);
            yield return new WaitForSeconds(0.005f);
        }

        transform.parent.GetComponent<BossController>().StopSE();

        // ムチ攻撃収める
        for (float plus = 1.0f; plus >= 0; plus -= 0.05f)
        {
            MovePosY(-2f, -0.8f, plus);
            yield return new WaitForSeconds(0.002f);
        }
        gameObject.SetActive(false);
        yield return null;
    }

    public IEnumerator Drop(GameObject branch)
    {
        transform.parent.GetComponent<BossController>().AttackAnim();

        yield return new WaitForSeconds(0.66f);

        transform.parent.GetComponent<BossController>().DropSE();

        CamShake.ShakeCamera(0.4f, 0.05f);
        // ShakeCamera(float ShakeTime, float shakeStrength)
        // 揺れる時間、揺れる強さ

        Random.InitState(System.DateTime.Now.Millisecond);

        int rand = (int)Random.Range(10, 16);

        for (int i = 0; i <= rand; i++)
        {
            var prefab = Instantiate(branch, transform.position, Quaternion.identity);
            prefab.transform.parent = transform;

            yield return new WaitForSeconds(0.1f);
        }

        yield return new WaitForSeconds(3);
    }

    public IEnumerator Summon(GameObject effectPrefab, GameObject enemy)
    {
        float effectTime = 1;
        float delay = 5;

        GameObject spawnEffect = Instantiate(effectPrefab, transform.position, Quaternion.identity, transform) as GameObject;
        yield return new WaitForSeconds(effectTime);
        Destroy(spawnEffect);
        GameObject prefabEnemy = Instantiate(enemy, transform.position, Quaternion.identity, transform) as GameObject;
        yield return new WaitForSeconds(delay);
    }
}
