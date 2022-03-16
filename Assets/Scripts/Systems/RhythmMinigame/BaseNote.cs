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
    protected Transform _transform;
    [SerializeField] protected float fallSpeed;
    protected bool shouldShrink;
    private void Awake()
    {
        _transform = transform;
    }
    private void Update()
    {
        HandleHits();
        _transform.position -= (Time.deltaTime * fallSpeed* Vector3.up);
       
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
        if (collision.tag == "Button"&&!alreadyExited)
        {
            RemoveNote();
            noteManager.UpdateFeedbackText(false, colour);
        }
    }


    protected virtual void RemoveNote()
    {
        canBePressed = false;
        if (!alreadyExited)
        {
            alreadyExited = true;
            noteManager.DequeueNote();
            Destroy(gameObject);
        }
      
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
        if (Input.GetKeyDown(key1) || Input.GetKeyDown(key2) ||Input.GetKeyUp(key1)||Input.GetKeyUp(key2))
        {
            noteManager.RemoveNote(this, colour ,key1, true);
            noteManager.UpdateFeedbackText(true, colour);
        }
    }

    public void SetNoteManager(NoteManager _noteManager) => noteManager = _noteManager;
    public KeyCode GetKey() => key1;
    public bool GetCanBePressed() => canBePressed;
    public void SetAlreadyExited(bool _alreadyExited) => alreadyExited = _alreadyExited;
    public void SetColour(Color _colour) => colour = _colour; 


}
