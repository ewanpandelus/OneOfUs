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
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Vector3 dir = new Vector3(0, Yoffset, 0);
        Gizmos.DrawRay(transform.position, dir);
    }


}
