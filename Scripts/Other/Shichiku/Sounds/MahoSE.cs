using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStateSE
{
    Damage = 0,
    Attack,
    Death,
    Block,
}

public class MahoSE : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField, EnumIndex(typeof(EnemyStateSE))]List<AudioClip> clips = new List<AudioClip>();
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }

    public void EnemySE(EnemyStateSE enemyStateSE)
    {
        audioSource.PlayOneShot(clips[(int)enemyStateSE]);
    }

    public void DamageSE()
    {
        audioSource.PlayOneShot(clips[(int)EnemyStateSE.Damage]);
    }

    public void AttackSE()
    {
        audioSource.PlayOneShot(clips[(int)EnemyStateSE.Attack]);
    }

    public void DeathSE()
    {
        audioSource.PlayOneShot(clips[(int)EnemyStateSE.Death]);
    }
}
