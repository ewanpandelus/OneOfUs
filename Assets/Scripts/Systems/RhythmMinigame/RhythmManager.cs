using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MonoBehaviour
{
    [SerializeField] GameObject shortNote, longNote;
    [SerializeField] GameObject leftButton, rightButton, upButton, downButton;
    [SerializeField] Material leftMat, rightMat, upMat, downMat;
    [SerializeField] Color leftColour, rightColour, upColour, downColour;
    [SerializeField] int noteCount;
    [SerializeField] private GameObject stage;
    [SerializeField] private MiracleManager miracleManager;
    private float startY = 0;
    private List<NoteProperties> noteProperties;
    private NoteManager noteManager;
    
       
    
    public enum NoteType
    {
        Left,
        Right,
        Down,
        Up
    }
    private void Awake()
    {
        noteManager = GetComponent<NoteManager>();
        InitialiseNoteProperties();
   
    }
    public IEnumerator PlayRhythm(float minSeperationTime, float maxSeperationTime)
    {
        List<(NoteType, float)> notes = new List<(NoteType, float)>();
        notes = LevelCreator(noteCount, minSeperationTime, maxSeperationTime);
        noteManager.SetTotalNoteCount(noteCount);
        foreach((NoteType, float) note in notes)
        {
            yield return new WaitForSeconds(note.Item2);
            NoteProperties noteInfo = noteProperties.Find(x => x.noteType == note.Item1);
            SetupNote(noteInfo);
        }
        yield return new WaitUntil(() => noteManager.CheckNoNotesLeft());
        yield return new WaitForSeconds(1f);
        miracleManager.SetAchievedMiracle(EvaluateResult());
    }
    private bool EvaluateResult()
    {
        return (((float)noteManager.GetTotalHitCount() / (float)noteCount) >= 0.5f);
    }
    private void SetupNote(NoteProperties noteInfo)
    {
        BaseNote _note = Instantiate(SpawnNoteWithPercentageWeight(0,1)).GetComponent<BaseNote>();
        _note.transform.position = new Vector3(noteInfo.xPos, startY, 0);
        _note.transform.SetParent(stage.transform, false);
        _note.GetComponent<Image>().color = noteInfo.color;
        _note.transform.SetAsFirstSibling();
        _note.gameObject.GetComponent<Image>().material = noteInfo.noteMaterial;
        _note.SetKeys(noteInfo.keys.Item1, noteInfo.keys.Item2);
        _note.SetColour(noteInfo.color);
        _note.SetNoteManager(noteManager);
        noteManager.EnqueueNote(_note);
    }
    GameObject SpawnNoteWithPercentageWeight(int _note1Weight, int _note2Weight)
    {
        int randomNum = UnityEngine.Random.Range(0, 10);
        if (randomNum < 7)
        {
            return shortNote;
        }
        return longNote;
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
         {  new NoteProperties(NoteType.Left, leftMat, leftColour, (KeyCode.LeftArrow, KeyCode.A)
            ,leftButton.GetComponent<RectTransform>().anchoredPosition.x, 0),

                 new NoteProperties(NoteType.Up, upMat, upColour, (KeyCode.UpArrow, KeyCode.W)
            ,upButton.GetComponent<RectTransform>().anchoredPosition.x, 1),

            new NoteProperties(NoteType.Down, downMat, downColour, (KeyCode.DownArrow, KeyCode.S),
            downButton.GetComponent<RectTransform>().anchoredPosition.x, 2),

            new NoteProperties(NoteType.Right, rightMat, rightColour, (KeyCode.RightArrow, KeyCode.D)
            ,rightButton.GetComponent<RectTransform>().anchoredPosition.x, 3)
         };

        startY = (Screen.height / 2) + 10;//Just over half screen size
    }

    public readonly struct NoteProperties
    {
        public readonly Color color;
        public readonly Material noteMaterial;
        public readonly NoteType noteType;
        public readonly (KeyCode,KeyCode) keys;
        public readonly float xPos;
        public readonly int posIndex;
    
 

        public NoteProperties(NoteType _noteType, Material _noteMaterial, Color _color ,(KeyCode, KeyCode) _keys, float _xPos, int _posIndex)
        {
            color = _color;
            noteMaterial = _noteMaterial;
            noteType = _noteType;
            keys = _keys;
            posIndex = _posIndex;
            xPos = _xPos;//- Screen.width / 2;
        }
    }
}
