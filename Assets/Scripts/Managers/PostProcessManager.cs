using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
public class PostProcessManager : MonoBehaviour
{
    [SerializeField] private GameObject globalPostProcess;
    [SerializeField] private GameObject darkeningPostProcess;
    private float darkeningWeight = 0;
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
        globalBloom.intensity.value = 1.5f;
 
    }
    public void StandardPostProcess()
    {
        globalBloom.intensity.value = 1;
        darkeningVolume.weight = darkeningWeight;
    }
    public IEnumerator IncreaseDarkeningWeight()
    {
        darkeningWeight += 0.25f;
        float x = 0;
        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            x = Mathf.Lerp(darkeningWeight-0.25f, darkeningWeight, t);
            darkeningVolume.weight = x;
            yield return null;
        }
    }
}
