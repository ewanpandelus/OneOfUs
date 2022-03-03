using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlickeringLights : MonoBehaviour
{
    private bool isFlickering = false;
    [SerializeField] private float delay;
    private Light2D light;
    private void Start()
    {
        light = GetComponent<Light2D>();
    }
    void Update()
    {
        if (!isFlickering)
        {
            StartCoroutine(Flicker());
        }
    }
    private IEnumerator Flicker()
    {
        isFlickering = true;
        light.enabled = false;
        delay = Random.Range(0.1f, 2f);
        yield return new WaitForSeconds(delay);
        light.enabled = true;
        delay = Random.Range(0.1f, 2f);
        yield return new WaitForSeconds(delay);
        isFlickering = false;

    }
}
