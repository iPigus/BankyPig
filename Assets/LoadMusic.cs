using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class LoadMusic : MonoBehaviour
{
    public AudioMixer Sounds;
    public AudioMixer Musics;

    private void Awake()
    {
        Sounds.SetFloat("Sound", Mathf.Log10(PlayerPrefs.GetFloat("Sound") * 20f));
        Musics.SetFloat("Music", Mathf.Log10(PlayerPrefs.GetFloat("Music") * 20f));
    }
}
