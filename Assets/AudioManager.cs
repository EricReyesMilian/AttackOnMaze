using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager speaker;
    public AudioSource Sfx;
    private void Awake()
    {
        if (speaker)
        {
            Destroy(this.gameObject);
        }
        else
        {
            speaker = this;
        }

    }
    public void Play(AudioClip clip)
    {
        Sfx.PlayOneShot(clip);
    }
}
