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

    private AudioSource FindAudioSource(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        return s.GetSource();
    }

    public IEnumerator StartFade(string name, float duration, float targetVolume)
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
            audioSource.Pause();
        }
      

        yield break;
    }
    public void StartMiniGame()
    {
        StartCoroutine(StartFade("Calmest", 1f, 0f));
    }
    public void StartMiniGameSong()
    {
        Play("Minigame");
        StartCoroutine(StartFade("Minigame", 1f, 1f));
    }
    public void MainThemeSounds()
    {
        StartCoroutine(StartFade("Minigame", 1f, 0f));
        Play("Calmest");
        StartCoroutine(StartFade("Calmest", 5f, 0.25f));
    }
}
