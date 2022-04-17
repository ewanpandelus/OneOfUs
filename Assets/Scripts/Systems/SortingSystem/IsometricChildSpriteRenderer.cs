using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsometricChildSpriteRenderer : MonoBehaviour
{
    [SerializeField] private float Yoffset;
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            var childTransform = transform.GetChild(i);
            var rend = childTransform.GetComponent<SpriteRenderer>().sortingOrder =  (int)((childTransform.position.y + Yoffset) * -10);
        }     
    }

 
}
