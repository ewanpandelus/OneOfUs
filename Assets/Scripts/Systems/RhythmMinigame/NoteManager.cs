using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    Queue<Note> noteQueue = new Queue<Note>();
    [SerializeField] float fallSpeed;
    void Update()
    {
        MoveNotes();
        if (noteQueue.Count == 0)
        {
            return;
        }
        Note closestNote = noteQueue.Peek();
        KeyCode releventKey = closestNote.GetKey();
        if (!closestNote.GetCanBePressed())
        {
            return;
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.S))
        {
            if(releventKey == KeyCode.DownArrow)
            {
                closestNote.SetAlreadyExited(true);
                Destroy(noteQueue.Dequeue().gameObject);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (releventKey == KeyCode.UpArrow)
            {
                closestNote.SetAlreadyExited(true);
                Destroy(noteQueue.Dequeue().gameObject);

            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (releventKey == KeyCode.LeftArrow)
            {
                closestNote.SetAlreadyExited(true);
                Destroy(noteQueue.Dequeue().gameObject);

            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (releventKey == KeyCode.RightArrow)
            {
                closestNote.SetAlreadyExited(true);
                Destroy(noteQueue.Dequeue().gameObject);

            }
        }

    }
    private void MoveNotes()
    {
        foreach (Note note in noteQueue)
        {
            note.gameObject.transform.position -= (Vector3.up * Time.deltaTime * fallSpeed);
        }
    }
    public void EnqueueNote(Note _note)
    {
        noteQueue.Enqueue(_note);
    }
    public void DequeueNote()
    {
        noteQueue.Dequeue();

    }
   
}
