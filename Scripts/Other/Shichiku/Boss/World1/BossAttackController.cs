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

    // �����o�Ȃ��ꏊ�̃����_���͈̔�
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
        // rand[0] �����o�Ȃ����@rand[1] ��_�̏o��ꏊ rand[2]�`rand[childNumber] �����o�Ȃ��ꏊ
        int[] rand = new int[childNumber];

        // �����̏�����
        Random.InitState(System.DateTime.Now.Millisecond);

        // ������
        foreach (GameObject child in childObjects)
        {
            child.SetActive(true);

            RootInit(child);
        }

        // ����
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

        // ��_�p�̉��R�[�h
        childObjects[rand[1]].GetComponent<SpriteRenderer>().color = Color.red;
        childObjects[rand[1]].transform.GetChild(0).GetComponent<EnemyDamageBossRoot>().IsWeakness = true;

        // �o���Ȃ����
        for (int i = 0; i < rand[0]; i++)
        {
            childObjects[rand[i + 2]].SetActive(false);
        }

        // ������Əo��
        for (float plus = 0.0f; plus <= 1; plus += 0.01f)
        {
            MovePosY(0.5f, 1f, plus);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.34f);

        // �A�^�b�N�A�j���[�V����
        transform.parent.GetComponent<BossController>().AttackAnim();

        yield return new WaitForSeconds(0.66f);

        // ��яo�����O�ݒ�
        foreach (GameObject child in childObjects)
        {
            RootBeforeAttack(child);
        }
        CamShake.ShakeCamera(0.2f, 0.2f);
        // ShakeCamera(float ShakeTime, float shakeStrength)
        // �h��鎞�ԁA�h��鋭��

        // ��яo��
        for (float plus = 0.0f; plus <= 1; plus += 0.1f)
        {
            MovePosY(1f, 1.5f, plus);
            yield return new WaitForSeconds(0.001f);
        }

        // ��яo��������̐ݒ�
        foreach (GameObject child in childObjects)
        {
            RootAfterAttack(child);
        }

        // �V�[���h�����ăR�A���o�鎞
        for (float time = 0; time <= 3; time += 0.01f)
        {
            if (stopFlag)
            {
                stopFlag = false;
                yield break;
            }
            yield return new WaitForSeconds(0.01f);
        }

        // ���������߂�R���[�`��
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
            // ������
            foreach (GameObject child in childObjects)
            {
                child.SetActive(true);

                RootInit(child);

                child.SetActive(false);
            }

            int[] rand = new int[2];
            int backNumber = 0;

            // ����������
            Random.InitState(System.DateTime.Now.Millisecond);

            do
            {
                rand[0] = Random.Range(0, childNumber);
            } while (rand[0] == backNumber);

            // ����������
            Random.InitState(System.DateTime.Now.Millisecond);

            backNumber = rand[0];
            rand[1] = Random.Range(0, 10);
            if (rand[1] >= 0 && rand[1] <= 4)
            {
                childObjects[rand[0]].GetComponent<SpriteRenderer>().color = Color.red;
                childObjects[rand[0]].transform.GetChild(0).GetComponent<EnemyDamageBossRoot>().IsWeakness = true;
            }

            childObjects[rand[0]].SetActive(true);

            // ������Əo��
            for (float plus = 0.0f; plus <= 1; plus += 0.01f)
            {
                MovePosY(0.5f, 1f, plus);
                yield return new WaitForSeconds(0.01f);
            }

            yield return new WaitForSeconds(1);

            // ��яo�����O�̐ݒ�
            foreach (GameObject child in childObjects)
            {
                RootBeforeAttack(child);
            }

            CamShake.ShakeCamera(0.2f, 0.1f);
            // ShakeCamera(float ShakeTime, float shakeStrength)
            // �h��鎞�ԁA�h��鋭��

            // ��яo��
            for (float plus = 0.0f; plus <= 1; plus += 0.1f)
            {
                MovePosY(1f, 1.5f, plus);
                yield return new WaitForSeconds(0.001f);
            }

            // ��яo������̐ݒ�
            foreach (GameObject child in childObjects)
            {
                RootAfterAttack(child);
            }

            // �V�[���h�����ăR�A���o�鎞
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

    // �������̏������֐�
    void RootInit(GameObject child)
	{
        child.GetComponent<SpriteRenderer>().color = Color.white; //��
        child.transform.GetChild(0).GetComponent<EnemyDamageBossRoot>().IsWeakness = false;

        child.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
        child.transform.GetChild(1).GetComponent<Collider2D>().isTrigger = true;
        child.transform.GetChild(1).tag = "Untagged";
        child.transform.GetChild(1).gameObject.layer = 12;
        child.transform.GetChild(2).GetComponent<Collider2D>().enabled = false;
    }
    
    // �������̍U�����O�̊֐�
    void RootBeforeAttack(GameObject child)
	{
        child.transform.GetChild(0).GetComponent<Collider2D>().enabled = true;
        child.transform.GetChild(1).tag = "BossRoot";
        child.transform.GetChild(2).GetComponent<Collider2D>().enabled = true;
	}

    // �������̍U������̊֐�
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

        // �������ޒ��O�̐ݒ�
        foreach (GameObject child in childObjects)
        {
            child.transform.GetChild(0).GetComponent<Collider2D>().enabled = false;
            child.transform.GetChild(2).GetComponent<Collider2D>().enabled = false;
        }
        // ��������
        for (float plus = 1.0f; plus >= 0; plus -= 0.05f)
        {
            MovePosY(0.5f, 1.5f, plus);
            yield return new WaitForSeconds(0.002f);
        }
        // �������񂾒���̐ݒ�
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

    // ���`�U��
    public IEnumerator Wave()
    {
        // ������
        gameObject.SetActive(true);
        transform.localPosition = new Vector3(0.8f, -2, 0);

        // ���`�U������
        for (float plus = 0.0f; plus <= 1; plus += 0.01f)
        {
            MovePosY(-2f, -0.8f, plus);
            yield return new WaitForSeconds(0.01f);
        }

        transform.parent.GetComponent<BossController>().AttackAnim();

        yield return new WaitForSeconds(0.66f);


        // ���`�U���J�n
        CamShake.ShakeCamera(0.5f, 0.2f);
        // ShakeCamera(float ShakeTime, float shakeStrength)
        // �h��鎞�ԁA�h��鋭��
        transform.parent.GetComponent<BossController>().WaveSE();

        for (float plus = 1.0f; plus >= 0; plus -= 0.01f)
        {
            MovePosX(-4.3f, 0.8f, plus);
            yield return new WaitForSeconds(0.005f);
        }

        transform.parent.GetComponent<BossController>().StopSE();

        // ���`�U�����߂�
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
        // �h��鎞�ԁA�h��鋭��

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
