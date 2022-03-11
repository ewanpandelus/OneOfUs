using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNote : MonoBehaviour
{
    protected bool canBePressed;
    protected KeyCode key1, key2;
    protected NoteManager noteManager;
    protected bool alreadyExited = false;
    protected Color colour;
    private void Update()
    {
        HandleHits();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Button")
        {
            canBePressed = true;
        }
     
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canBePressed = false;
        if (!alreadyExited)
        {
            noteManager.DequeueNote();
            Destroy(gameObject);
        }
        alreadyExited = true;
    }

    public void SetKeys(KeyCode _key1, KeyCode _key2)
    {
        key1 = _key1;
        key2 = _key2;
    }
    protected virtual void HandleHits()
    {
        if (!canBePressed)
        {
            return;
        }
        if (Input.GetKeyDown(key1) || Input.GetKeyDown(key2))
        {
            noteManager.RemoveNote(this, colour ,key1, true);
        }
    }

    public void SetNoteManager(NoteManager _noteManager) => noteManager = _noteManager;
    public KeyCode GetKey() => key1;
    public bool GetCanBePressed() => canBePressed;
    public void SetAlreadyExited(bool _alreadyExited) => alreadyExited = _alreadyExited;
    public void SetColour(Color _colour) => colour = _colour; 


}
