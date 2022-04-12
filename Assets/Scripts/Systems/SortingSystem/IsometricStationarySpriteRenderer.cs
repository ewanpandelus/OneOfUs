using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class IsometricSpriteRenderer : MonoBehaviour   
{
    private SpriteRenderer rend;
    [SerializeField] private float Yoffset;
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        rend.sortingOrder = (int)((transform.position.y+Yoffset) * -10);
    }
}
