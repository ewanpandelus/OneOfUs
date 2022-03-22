using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
[System.Serializable]
public class Sound
{
    [SerializeField]
    private AudioClip clip;
    [SerializeField]
    private string name;
    [SerializeField]
    [Range(0f, 1f)]
    private float volume;
    [SerializeField]
    [Range(0.1f, 3f)]
    private float pitch;
    private AudioSource source;
    [SerializeField]
    private bool loop;

    public bool GetLoop()
    {
        return this.loop;
    }

    public AudioClip GetClip()
    {
        return this.clip;
    }

    public void SetClip(AudioClip c)
    {
        this.clip = c;
    }

    public float GetPitch()
    {
        return this.pitch;
    }

    public void SetPitch(float p)
    {
        this.pitch = p;
    }

    public float GetVolume()
    {
        return this.volume;
    }

    public void SetVolume(float v)
    {
        this.source.volume = v;
    }

    public void SetSource(AudioSource s)
    {
        source = s;
    }

    public AudioSource GetSource()
    {
        return this.source;
    }

    public string GetName()
    {
        return this.name;
    }
}