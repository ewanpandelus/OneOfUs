using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongNote : BaseNote
{
    private float _elapsedTime = 0f;
    ParticleSystem particleSystem;
    bool assignedParticleSystem = false;
    protected override void HandleHits()
    {
        if (!canBePressed)
        {
            return;
        }
        if (Input.GetKey(key1) || Input.GetKey(key2))
        {
            _elapsedTime += Time.deltaTime;
            if(!assignedParticleSystem)
            {
                particleSystem = noteManager.LongBurst(colour, key1);
                assignedParticleSystem = true;
            }
            if (_elapsedTime > 0.5f)
            {
                noteManager.RemoveNote(this, colour, key1, false);
                canBePressed = false;
            }
            return;
        }
      
  
    }
}
