using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(ParticleSystemRenderer))]
public class IsometricParticleSystemRenderer : MonoBehaviour
{
    ParticleSystemRenderer particleSystem;
    [SerializeField] private float Yoffset;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystemRenderer>();
        particleSystem.sortingOrder = (int)((transform.position.y + Yoffset) * -10);
    }

}
