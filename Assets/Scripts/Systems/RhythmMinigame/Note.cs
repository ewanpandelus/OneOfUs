using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 200f;
    private bool canBePressed;
    private KeyCode key;
    private NoteManager noteManager;
    private bool alreadyExited = false;
 
    void Update()
    {
        transform.position -= (Vector3.up * Time.deltaTime*fallSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        canBePressed = true;
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
    public void SetKey(KeyCode _key) => key = _key;

    public void SetNoteManager(NoteManager _noteManager) => noteManager = _noteManager;
    public KeyCode GetKey() => key;
    public bool GetCanBePressed() => canBePressed;
    public void SetAlreadyExited(bool _alreadyExited) => alreadyExited = _alreadyExited;


}
