using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public Sound[] sounds;
    private float volume;
    public delegate void AudioDelegate(AudioSource audioSource);
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
 
        foreach (Sound s in sounds)
        {
            s.SetSource(gameObject.AddComponent<AudioSource>());
            s.GetSource().clip = s.GetClip();
            s.GetSource().volume = s.GetVolume();
            s.GetSource().pitch = s.GetPitch();
            s.GetSource().loop = s.GetLoop();
        }
    }


    private void Start()
    {
        MainThemeSounds();
    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s != null)
        {
            s.GetSource().Play();
        }
    }
    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s != null)
        { 
            s.GetSource().PlayOneShot(s.GetSource().clip);
        }
    }
    public void SetPitch(float _pitch) 
    {
 
        foreach(Sound s in sounds)
        {
            s.GetSource().pitch = _pitch;
        }
    }
    private AudioSource FindAudioSource(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        return s.GetSource();
    }

    public IEnumerator StartFade(string name, float duration, float targetVolume, AudioDelegate audioDelegate)
    {
        AudioSource audioSource = FindAudioSource(name);
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        if (targetVolume == 0)
        {
            audioDelegate(audioSource);
        }
      
        yield break;
    }
    public void StartMiniGame()
    {
        StartCoroutine(StartFade("Calmest", 1f, 0f, PauseSound));
    }
    public void StartMiniGameSong()
    {
        Play("Minigame");
        StartCoroutine(StartFade("Minigame", 1f, 1f, NoEffect));
    }
    public void MainThemeSounds()
    {
        StartCoroutine(StartFade("Minigame", 1f, 0f, StopSound));
        Play("Calmest");
        StartCoroutine(StartFade("Calmest", 5f, 0.25f, NoEffect));
    }

    void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }
    void PauseSound(AudioSource audioSource)
    {
        audioSource.Pause();
    }
    void NoEffect(AudioSource audioSource)
    {
        return;
    }
}
