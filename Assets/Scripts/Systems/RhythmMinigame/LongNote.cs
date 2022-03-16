using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LongNote : BaseNote
{
    private float _elapsedTime = 0f;
    private ParticleSystem particleSystem;
    bool assignedParticleSystem = false;
    private Vector3 reduceY  = new Vector3(0, -5f, 0);
    float initialYScale;

    private void Start()
    {
        initialYScale = _transform.localScale.y;
    }

    protected override void HandleHits()
    {
        if (!canBePressed)
        {
            return;
        }
        if (Input.GetKey(key1) || Input.GetKey(key2))
        {
            _elapsedTime += Time.deltaTime;
            if (_transform.localScale.y > 0.05)
            {
                _transform.localScale += (reduceY * (initialYScale / fallSpeed)) * Time.deltaTime; //need to make formula based on speed + ySize
            }
         
            if(!assignedParticleSystem)
            {
                particleSystem = noteManager.LongBurst(colour, key1);
                assignedParticleSystem = true;
            }
            if (_elapsedTime > (initialYScale /(fallSpeed*2.4f))) //need to make formula based on speed + ySize
            {
                noteManager.RemoveNote(this, colour, key1, false);
                noteManager.UpdateFeedbackText(true, colour);
                canBePressed = false;
                alreadyExited = true;
            }
            return;
        }
    }
 
}
