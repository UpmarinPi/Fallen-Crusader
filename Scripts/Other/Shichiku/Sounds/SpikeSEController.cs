using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpikeStateSE
{
    Normal = 0,
}

public class SpikeSEController : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField, EnumIndex(typeof(SpikeStateSE))] List<AudioClip> clips = new List<AudioClip>();
    
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        
    }

    public void SpikeSE(SpikeStateSE spikeStateSE)
    {
        audioSource.PlayOneShot(clips[(int)spikeStateSE]);
    }
}
