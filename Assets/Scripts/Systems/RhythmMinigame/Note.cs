using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 200f;
    private bool canBePressed;
 
    void Update()
    {
        transform.position -= (Vector3.up * Time.deltaTime*fallSpeed);
    }
}
