using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicAndSoundManager : MonoBehaviour
{
    public AudioMixer Sounds;
    public AudioMixer Musics;

    [SerializeField] Slider Sound;
    [SerializeField] Slider Music;

    private void Awake()
    {
        Sound.value = PlayerPrefs.GetFloat("Sound");
        Music.value = PlayerPrefs.GetFloat("Music");
    }

    public void SoundsUpdated(float value)
    {
        Sounds.SetFloat("Sound", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat("Sound", value);
    }
    public void MusicsUpdated(float value)
    {
        Musics.SetFloat("Music", Mathf.Log10(value) * 20f);
        PlayerPrefs.SetFloat("Music", value);
    }
}
