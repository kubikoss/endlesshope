using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip pickUpSound;
    public AudioClip dropSound;
    public AudioClip buySound;
    public AudioClip eatSound;

    private void Awake()
    {
        if(Instance == null)
            Instance = this;
    }

    public void PlayAudio(AudioClip clip, float volume = 0.5f)
    {
        if (clip != null)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = clip;
            source.volume = volume;
            source.Play();
            Destroy(source, clip.length);
        }
    }

    public void PickupItemAudio()
    {
        PlayAudio(pickUpSound);
    }

    public void DropItemAudio()
    {
        PlayAudio(dropSound);
    }

    public void BuyItemAudio()
    {
        PlayAudio(buySound);
    }

    public void EatItemAudio()
    {
        PlayAudio(eatSound, 0.4f);
    }
}
