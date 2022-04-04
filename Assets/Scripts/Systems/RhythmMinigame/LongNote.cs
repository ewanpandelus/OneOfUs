using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LongNote : BaseNote
{
    private float _elapsedTime = 0f;
    private ParticleSystem particleSystem;
    bool assignedParticleSystem = false;
    private Vector3 reduceY  = new Vector3(0, -8.5f, 0);


 
 
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
           

        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Button" && !alreadyExited)
        {
            if (_elapsedTime > (initialYScale / (fallSpeed * 2.7f))) //need to make formula based on speed + ySize
            {
                noteManager.RemoveNote(this);
                noteManager.PlayParticleEffect(false, colour, key1);
                noteManager.UpdateFeedbackText(true, colour);
                canBePressed = false;
                alreadyExited = true;
                SoundManager.instance.PlayOneShot(soundName);
            }
            else
            {
                RemoveNote();
                noteManager.UpdateFeedbackText(false, colour);
            }
           
        }
    }

}
