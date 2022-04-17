using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(SpriteRenderer))]
public class IsometricStationarySpriteRenderer : MonoBehaviour   
{
    private SpriteRenderer rend;
    [SerializeField] private float Yoffset;
    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        rend.sortingOrder = (int)((transform.position.y + Yoffset) * -10);
    }
  


}
