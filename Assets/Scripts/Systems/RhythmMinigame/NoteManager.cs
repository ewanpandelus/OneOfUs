using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [SerializeField] float fallSpeed;
    [SerializeField] GameObject buttonBurst;
    [SerializeField] Color leftColour, rightColour, upColour, downColour;
    [SerializeField] TMP_Text percentageText;
    Queue<Note> noteQueue = new Queue<Note>();
    private int totalHitCount = 0;
    private int totalNoteCount = 0;
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
                RemoveNote(closestNote, downColour);
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            if (releventKey == KeyCode.UpArrow)
            {
                RemoveNote(closestNote, upColour);
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
        {
            if (releventKey == KeyCode.LeftArrow)
            {
                RemoveNote(closestNote, leftColour);
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
        {
            if (releventKey == KeyCode.RightArrow)
            {
                RemoveNote(closestNote ,rightColour);
            }
        }
    }
    private void RemoveNote(Note _closestNote, Color _colour)
    {
        var tmp = Instantiate(buttonBurst, transform);
        tmp.transform.position = _closestNote.transform.position;
        tmp.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor",_colour);
        Destroy(tmp, 1f);
        _closestNote.SetAlreadyExited(true);    
        Destroy(noteQueue.Dequeue().gameObject);
        UpdatePercentageText(true);
    
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
        UpdatePercentageText(false);
    }
 
    public void UpdatePercentageText(bool _hit)
    {
        if (_hit) 
        {
            totalHitCount++;
        }
        percentageText.text = totalHitCount.ToString() + "/" + totalNoteCount;

    }
    public void SetTotalNoteCount(int _noteCount)
    {
        totalNoteCount = _noteCount;
        UpdatePercentageText(false);
    }
   
}
