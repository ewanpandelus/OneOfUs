using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    public float smoothSpeed = 3f;
    public Vector3 offset;

    private void LateUpdate()
    {
        if (target)
        {
            Vector3 desiredPost = target.position + offset;
            Vector3 smoothPost = Vector3.Lerp(transform.position, desiredPost, smoothSpeed);
            transform.position = smoothPost;
        }
    }
  
}
