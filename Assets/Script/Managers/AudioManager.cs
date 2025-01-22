using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClip pickUpSound;

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

    public void PickupItemSound()
    {
        PlayAudio(pickUpSound, 1f);
    }
}
