using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystemRenderer))]
public class IsometricParticleSystemRenderer : MonoBehaviour
{
    ParticleSystemRenderer pSystem;
    [SerializeField] private float Yoffset;

    private void Start()
    {
        pSystem = GetComponent<ParticleSystemRenderer>();
        pSystem.sortingOrder = (int)((transform.position.y + Yoffset) * -10);
    }

}
