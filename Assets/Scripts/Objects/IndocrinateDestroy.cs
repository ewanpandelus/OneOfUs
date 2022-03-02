using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;


public class IndocrinateDestroy : MonoBehaviour
{
    private float elapsedTime = 0;
    [SerializeField] Light2D pointLight;
     Light2D globalLight;
    private void Start()
    {
        pointLight.intensity = 0;
        globalLight = GameObject.Find("Global Light 2D").GetComponent<Light2D>();
        globalLight.intensity = 1f;
        pointLight.intensity = 0.5f;
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < 1.4f)
        {
            if (pointLight.intensity < 4f)
            {
                pointLight.intensity += Time.deltaTime*2;
            }

            pointLight.pointLightOuterRadius = (elapsedTime / 1.4f);
            if (globalLight.intensity > 0.4f)
            {
                globalLight.intensity -= Time.deltaTime/2;
            }
       
        }
        if (elapsedTime >= 2f&&pointLight.intensity>0)
        {
            pointLight.intensity -= Time.deltaTime;
            pointLight.pointLightOuterRadius -= Time.deltaTime;
            if (globalLight.intensity < 1.4f)
                globalLight.intensity += Time.deltaTime/1.5f;
        }
        if (elapsedTime > 3)
        {
            globalLight.intensity = 1f;
            Destroy(gameObject);
        }
    }
}
