using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public Sound[] sounds;
    private float volume;
    public delegate void AudioDelegate(AudioSource audioSource);
    private bool volChanged = false;
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
        volume = 0.27f;
      

        if (SceneManager.GetActiveScene().buildIndex!=0)
        {
            Play("AmbientTown");
            MainThemeSounds();
        }
  

    }
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s != null)
        {
            s.SetVolume(volume * s.GetStartingVolume());
            s.GetSource().Play();
        }
    }
    public void PlayOneShot(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.GetName() == name);
        if (s != null)
        {
            s.SetVolume(volume * s.GetStartingVolume());
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
    private Sound FindSound(string name)
    {
        return Array.Find(sounds, sound => sound.GetName() == name);
    }
    public void PauseSound(string name)
    {
        Sound s = FindSound(name);
        s.GetSource().Pause();
    }
    public void UnPauseSound(string name)
    {
        Sound s = FindSound(name);
        s.GetSource().UnPause();
    }
    public void FadeOutMainTheme(float timeToFade)
    {
        StartCoroutine(StartFade("Calmest", timeToFade, 0f, PauseSound));
    }
    public void FadeInMainTheme()
    {
        Play("Calmest");
        FindSound("Calmest").SetVolume(0f);
        StartCoroutine(StartFade("Calmest", 5f, volume, NoEffect));
    }
    public IEnumerator StartFade(string name, float duration, float targetVolume, AudioDelegate audioDelegate)
    {
        volChanged = false;
        AudioSource audioSource = FindAudioSource(name);
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration&&volChanged != true)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume*volume*FindSound(name).GetStartingVolume(), currentTime / duration);
            yield return null;
        }
        if (targetVolume == 0)
        {
            audioDelegate(audioSource);
        }
        yield break;
    }
    public IEnumerator SlowDownTimeEffect()
    {
        StartCoroutine(ChangePitch(0.1f, 1, 0.6f));
        yield return new WaitForSeconds(1);
        StartCoroutine(ChangePitch(0.1f, 0.6f, 1f));
        yield return null;
    }
    public IEnumerator ChangePitch(float duration,float initialPitch, float targetPitch)
    {
        float currentTime = 0;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            Time.timeScale = Mathf.Lerp(initialPitch, targetPitch, currentTime / duration);
            foreach (Sound s in sounds)
            {
                s.SetPitch(Mathf.Lerp(initialPitch, targetPitch, currentTime / duration));
                s.GetSource().pitch = Mathf.Lerp(initialPitch, targetPitch, currentTime / duration);


            }
            yield return null;
        }
        yield break;
    }

    public void ChangeVolume(float _volume)
    {
        volume = _volume;
        foreach (Sound s in sounds)
        {
      
            s.GetSource().volume = _volume*s.GetStartingVolume();
            volChanged = true;
        }
    }
    public void StartMiniGame()
    {
        StartCoroutine(StartFade("Calmest", 1f, 0f, PauseSound));
    }
    public void StartMiniGameSong()
    {
   
        Play("Minigame");
        FindSound("Minigame").SetVolume(0f);
        StartCoroutine(StartFade("Minigame", 1f, volume+0.17f, NoEffect));
    }
    public void MainThemeSounds()
    {
         StartCoroutine(StartFade("Minigame", 0.8f , 0f, StopSound));
         Play("Calmest");
         FindSound("Calmest").SetVolume(0f);
         StartCoroutine(StartFade("Calmest", 5f, volume, NoEffect));
    }

    public void StopSound(AudioSource audioSource)
    {
        audioSource.Stop();
    }
    public void PauseSound(AudioSource audioSource)
    {
        audioSource.Pause();
    }
    public void NoEffect(AudioSource audioSource)
    {
        return;
    }
}
