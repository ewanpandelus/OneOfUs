using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    [SerializeField] GameObject noteObj;
    [SerializeField] GameObject leftButton, rightButton, upButton, downButton;
    [SerializeField] Color leftNote, rightNote, upNote, downNote;
    private GameObject stage;
    private float startY = 0;
    private List<NoteProperties> noteProperties;
    
   
       
    
    public enum NoteType
    {
        Left,
        Right,
        Down,
        Up
    }
    private void Start()
    {
        InitialiseNoteProperties();
        StartCoroutine(PlayRhythm());
        stage = GameObject.FindGameObjectWithTag("Stage");



    }
    public IEnumerator PlayRhythm()
    {
        List<(NoteType, float)> notes = new List<(NoteType, float)>();
        notes = LevelCreator(10, 0.4f, 0.6f);
        foreach((NoteType, float) note in notes)
        {
            yield return new WaitForSeconds(note.Item2);
            NoteProperties noteInfo = noteProperties.Find(x => x.noteType == note.Item1);
            Note _note = Instantiate(noteObj).GetComponent<Note>();
            _note.transform.eulerAngles = new Vector3(0, 0, noteInfo.zRot);
            _note.transform.position = new Vector3(noteInfo.xPos, startY, 0);
            _note.transform.SetParent(stage.transform, false);
            _note.GetComponent<Image>().color = noteInfo.color;
            _note.transform.SetAsLastSibling();

        }
    }
    
  
    private List<(NoteType, float)> LevelCreator(int noteCount, float minWait, float maxWait)
    {
        List<(NoteType, float)> notes = new List<(NoteType, float)>();
        float waitTime = 0;
        for(int i = 0; i < noteCount; i++)
        {
            waitTime = UnityEngine.Random.Range(minWait, maxWait);
            notes.Add((GenerateRandomNote(), waitTime));
        }
        return notes;
    }
    private NoteType GenerateRandomNote()
    {
        int noteNum = UnityEngine.Random.Range(0, 4);
        if (noteNum == 0) return NoteType.Left;
        if (noteNum == 1) return NoteType.Right;
        if (noteNum == 2) return NoteType.Down;
        return NoteType.Up;
    }
    private void InitialiseNoteProperties()
    {
        noteProperties = new List<NoteProperties>() 
         {  new NoteProperties(NoteType.Down, downNote, KeyCode.DownArrow,downButton.transform.position.x, 0 ),
            new NoteProperties(NoteType.Left, leftNote, KeyCode.LeftArrow,leftButton.transform.position.x, 270 ),
            new NoteProperties(NoteType.Right, rightNote, KeyCode.RightArrow ,rightButton.transform.position.x, 90 ),
            new NoteProperties(NoteType.Up, upNote, KeyCode.UpArrow,upButton.transform.position.x, 180 )
         };
    
        startY = (2000 / 2);//Just over half screen size
    }

    public readonly struct NoteProperties
    {
        public readonly NoteType noteType;
        public readonly Color color;
        public readonly KeyCode key;
        public readonly float xPos;
        public readonly float zRot;

        public NoteProperties(NoteType _noteType , Color _color, KeyCode _key, float _xPos, float _zRot)
        {
            noteType = _noteType;
            color = _color;//new Color(1-_color.r, 1-_color.g, 1- _color.b,1);
            key = _key;
            xPos = _xPos - Screen.width / 2;
            zRot = _zRot;
        }
    }
}
