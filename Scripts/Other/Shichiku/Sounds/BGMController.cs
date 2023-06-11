using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMController : MonoBehaviour
{
    static public bool FIGHTBGMFLAG = false;
    bool onceFlag = false;
    AudioSource audioSource;
    [Header("ノーマル、戦闘、ボス1、ボス2")]
    [SerializeField]
    List<AudioClip> audioClips = new List<AudioClip>();

    const float maxVolume = 0.25f;

    enum BGMType
    {
        Nomal = 0,
        Fight = 1,
        Boss1 = 2,
        Boss2 = 3,
        Fire = 4
    };
    
    void Awake()
    {
        FIGHTBGMFLAG = false;
        audioSource = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if(FIGHTBGMFLAG && !onceFlag)
        {
            FightBGM();
            PlayBGM();
            StartCoroutine(FadeInPlay());
            onceFlag = true;
        }
        else if(!FIGHTBGMFLAG && onceFlag)
        {
            NomalBGM();
            PlayBGM();
            StartCoroutine(FadeInPlay());
            onceFlag = false;
        }
    }

    public void Boss1BGM()
    {
        audioSource.clip = audioClips[(int)BGMType.Boss1];
    }
    public void Boss2BGM()
    {
        audioSource.clip = audioClips[(int)BGMType.Boss2];
    }
    public void NomalBGM()
    {
        audioSource.clip = audioClips[(int)BGMType.Nomal];
    }
    public void FightBGM()
    {
        audioSource.clip = audioClips[(int)BGMType.Fight];
    }
    public void FireBGM()
    {
        audioSource.clip = audioClips[(int)BGMType.Fire];
    }
    public void StopBGM()
    {
        audioSource.Stop();
    }
    public void PlayBGM()
    {
        audioSource.Play();
    }
    public IEnumerator FadeInPlay()
    {
        float plus = 0;
        while (plus <= 1)
        {
            audioSource.volume = Mathf.Lerp(0, maxVolume, plus);
            plus += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public IEnumerator FadeInPlay(float minVolume, float maxVolume = 0.25f)
    {
        Debug.Log("fadein");
        minVolume = maxVolume * minVolume;
        float plus = 0;
        while (plus <= 1)
        {
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, plus);
            plus += 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public IEnumerator FadeOutPlay()
    {
        float plus = 1;
        while (plus >= 0)
        {
            audioSource.volume = Mathf.Lerp(0, maxVolume, plus);
            plus -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }
    public IEnumerator FadeOutPlay(float minVolume, float maxVolume = 0.25f)
    {
        Debug.Log("fadeout");
        minVolume = maxVolume * minVolume;
        Debug.Log(minVolume);
        float plus = 1;
        while (plus >= 0)
        {
            audioSource.volume = Mathf.Lerp(minVolume, maxVolume, plus);
            plus -= 0.01f;
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void SetVolume(float volume)
	{
        audioSource.volume = maxVolume * volume;
	}
}
