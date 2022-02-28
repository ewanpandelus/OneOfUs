using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class IsometricSpriteRenderer : MonoBehaviour   
{
    private SpriteRenderer renderer;
    [SerializeField] private float Yoffset;
    private void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        renderer.sortingOrder = (int)((transform.position.y+Yoffset) * -10);
    }
}
