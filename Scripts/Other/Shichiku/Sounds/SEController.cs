using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SECLIPS
{
    public List<AudioClip> clips = new List<AudioClip>();
    public List<float> volume = new List<float>();
}

public enum ESystemSE
{
    Talk = 0,
    GameOver,
    Chest,
    MagicSelect,
    Decide,
    Save,
    Guid,
    GameClear,
    Fire,
}

public enum EPlayerAttackSE
{
    Attack = 0,
}

public enum EPlayerSE
{
    Avoidance = 0,
    Damage,
    Walk,
    Jump,
    Grouding
}

public enum EMagicSE
{
    Drain,
    Slash,
    Explotion,
    Explotion_contact,
    Explotion_inovation,
    TimeStop
}

public enum EEnemySE
{

}

public enum EBossSE
{
    Root = 0,
    Wave_or_Drop,
    ShieldBreak,
    Death,
    DamageHit
}

public class SEController : MonoBehaviour
{
    AudioSource audioSource;

    // 0話すSE、1GameOverSE
    [Header("0:話すSE、1:GameOverSE")]
    [SerializeField]SECLIPS system;
    [SerializeField]SECLIPS playerAttack;
    [SerializeField]SECLIPS player;
    [SerializeField]SECLIPS magic;
    [SerializeField]SECLIPS enemy;
    [SerializeField]SECLIPS boss;

    float maxVolume;

    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }

    public IEnumerator FadeInPlay()
    {
        float plus = 0;
        while(plus <= 1)
        {
            audioSource.volume = Mathf.Lerp(0, maxVolume, plus);
            plus += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public IEnumerator FadeInPlay(float minVolume)
    {
        Debug.Log("fadein");
        minVolume = maxVolume * minVolume;
        float plus = 0;
        while(plus <= 1)
        {
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, plus);
            plus += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public IEnumerator FadeOutPlay()
    {
        float plus = 1;
        while(plus >= 0)
        {
            audioSource.volume = Mathf.Lerp(0, maxVolume, plus);
            plus -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public IEnumerator FadeOutPlay(float minVolume)
    {
        Debug.Log("fadeout");
        minVolume = maxVolume * minVolume;
        Debug.Log(minVolume);
        float plus = 1;
        while(plus >= 0)
        {
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, plus);
            plus -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    void SetVolume(float v)
    {
        audioSource.volume = v;
        maxVolume = v;
    }

    public void SystemSE(int i)
    {
        if(system.volume[i] == 0)
        {
            system.volume[i] = 0.5f;
        }
        SetVolume(system.volume[i]);
        audioSource.PlayOneShot(system.clips[i]);
    }

    public void PlayerSE(int i)
    {
        if (system.volume[i] == 0)
        {
            system.volume[i] = 0.5f;
        }
        //StopSE();
        SetVolume(player.volume[i]);
        audioSource.PlayOneShot(player.clips[i]);
    }

    public void PlayerAttackSE(int i)
    {
        if (system.volume[i] == 0)
        {
            system.volume[i] = 0.5f;
        }
        //StopSE();
        SetVolume(playerAttack.volume[i]);
        audioSource.PlayOneShot(playerAttack.clips[i]);
    } 

    public void MagicSE(int i)
    {
        if (system.volume[i] == 0)
        {
            system.volume[i] = 0.5f;
        }
        //StopSE();
        SetVolume(magic.volume[i]);
        audioSource.PlayOneShot(magic.clips[i]);
    }

    public void BossSE(int i)
    {
        if (system.volume[i] == 0)
        {
            system.volume[i] = 0.5f;
        }
        SetVolume(boss.volume[i]);
        audioSource.PlayOneShot(boss.clips[i]);
    }

    public void StopSE()
    {
        audioSource.Stop();
    }
}
