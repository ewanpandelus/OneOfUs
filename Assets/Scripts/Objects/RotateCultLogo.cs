using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCultLogo : MonoBehaviour
{
    [SerializeField] Material _mat;
    [SerializeField] float rotationDuration = 0.2f;
    private float intervalMultiplier = 1f;
    [SerializeField] private bool updatesShader;
    private float elapsedTime = 0;


    void Start()
    {
        _mat.SetFloat("_Transparency", 0);
        _mat.SetFloat("_ManualTime", 0);
        StartCoroutine(RotateZ(rotationDuration, 1));
    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        _mat.SetFloat("_ManualTime", elapsedTime);
    }
    IEnumerator RotateZ(float duration, int direction)
    {
        float startRotation = transform.eulerAngles.z;
        float endRotation = startRotation + (360.0f*direction);
        float t = 0.0f;
        bool finished = false;
        while (!finished)
        {
            if (t < duration)
            {
                t += Time.deltaTime;
                float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % 360.0f;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, zRotation);
                SetShaderTransparency(t / duration, direction);
           
            
                yield return null;
            }
            else
            {
                yield return new WaitForSeconds(1f);
                if (direction == 1) StartCoroutine(RotateZ(duration, -1));
                finished = true;
            }
        }
    }
    private void SetShaderTransparency(float percentage, int direction)
    {
        if (!updatesShader) 
        {
            return;
        }
        if (direction == 1) {
            _mat.SetFloat("_Transparency", percentage);        
        }
        else _mat.SetFloat("_Transparency", (percentage *= -1) + 1);
    }
    private void OnDestroy()
    {
        _mat.SetFloat("_Transparency", 0);
        _mat.SetFloat("_ManualTime", 0);
    }
}
