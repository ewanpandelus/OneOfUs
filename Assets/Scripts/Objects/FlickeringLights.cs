using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class FlickeringLights : MonoBehaviour
{
    private bool isFlickering = false;
    [SerializeField] private float delay;
    private Light2D flickerLight;
    private void Start()
    {
        flickerLight = GetComponent<Light2D>();
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
        flickerLight.enabled = false;
        delay = Random.Range(0.1f, 2f);
        yield return new WaitForSeconds(delay);
        flickerLight.enabled = true;
        delay = Random.Range(0.1f, 2f);
        yield return new WaitForSeconds(delay);
        isFlickering = false;

    }
}
