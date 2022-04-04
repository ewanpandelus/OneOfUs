using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostProcessManager : MonoBehaviour
{
    [SerializeField] private GameObject globalPostProcess;
    [SerializeField] private GameObject darkeningPostProcess;
    private Volume globalVolume;
    Bloom globalBloom;
    private Volume darkeningVolume;
    public static PostProcessManager instance;
    private void Awake()
    {
        globalVolume = globalPostProcess.GetComponent<Volume>();
        darkeningVolume = darkeningPostProcess.GetComponent<Volume>();

        if (!instance)
        {
            instance = this;
        }
        Bloom tmp;

        if (globalVolume.profile.TryGet<Bloom>(out tmp))
        {
            globalBloom = tmp;
        }
    }

    public void RhythmPostProcess()
    {
        darkeningVolume.weight = 0;
       // globalBloom.intensity.value = 1.5f;
    }
    public void StandardPostProcess()
    {
        darkeningVolume.weight = 1;
        globalBloom.intensity.value = 1;
    }
}
