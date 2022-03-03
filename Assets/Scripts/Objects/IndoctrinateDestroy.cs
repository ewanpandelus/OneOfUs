using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class IndoctrinateDestroy : MonoBehaviour
{
    private float elapsedTime = 0;
    Light2D globalLight;
    private void Start()
    {
        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        globalLight.intensity = 1f;
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        ChangeLighting();
        if (elapsedTime > 6f)
        {

            Destroy(gameObject);
        }
    }
    private void ChangeLighting()
    {
        if (elapsedTime < 1.4f)
        {
            if (globalLight.intensity > 0.8f)
            {
                globalLight.intensity -= Time.deltaTime / 4;
                return;
            }
        }
        if (elapsedTime >= 2.5f)
        {
            if (globalLight.intensity < 1f)
                globalLight.intensity += Time.deltaTime / 4f;
        }
    }
}