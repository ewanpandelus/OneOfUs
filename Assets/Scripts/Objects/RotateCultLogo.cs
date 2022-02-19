using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCultLogo : MonoBehaviour
{
    [SerializeField] Material _mat;
    [SerializeField] float rotationDuration = 0.2f;
    private float intervalMultiplier = 1f;



    void Start()
    {
        StartCoroutine(RotateZ(rotationDuration, 1));
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
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.x, zRotation);
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
        if (direction == 1) _mat.SetFloat("_Transparency", percentage);
        else _mat.SetFloat("_Transparency", (percentage*=-1)+1);
    }

    void Update()
    {
        
    }
}
