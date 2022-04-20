using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RhythmManager : MonoBehaviour
{
    [SerializeField] GameObject shortNote, longNote, slowNote;
    [SerializeField] List<GameObject> buttons;// leftButton, rightButton, upButton, downButton;
    [SerializeField] List<Material> matList = new List<Material>();
    [SerializeField] List<Color> colours = new List<Color>();
    [SerializeField] int noteCount;
    [SerializeField] private GameObject stage;
    [SerializeField] private MiracleManager miracleManager;
    List<RhythmLevel> levelList;
    private float startY = 0;
    private List<NoteProperties> noteProperties;
    private NoteManager noteManager;
    private RhythmLevelManager rhythmLevelManager;
    private bool waiting = false;
    private float elapsedTime = 0;
    List<(NoteType, float)> notes = new List<(NoteType, float)>();
    (NoteType, float) currentNote;
    private int counter = 0;
    private int level = 1;
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
        noteManager.PopulatePositions(matList);
        InitialiseNoteProperties();
        rhythmLevelManager = GetComponent<RhythmLevelManager>();
        shortNote.GetComponent<BaseNote>().SetSwapChance(0);
        longNote.GetComponent<BaseNote>().SetSwapChance(0);
        shortNote.GetComponent<BaseNote>().SetFallSpeed(8);
        longNote.GetComponent<BaseNote>().SetFallSpeed(8);
      
    }
    private void FixedUpdate()
    {
        if (waiting)
        {
            elapsedTime += Time.fixedDeltaTime;
            if(elapsedTime>= currentNote.Item2 - levelList[counter - 1].timeToWait)
            {
                waiting = false;
                elapsedTime = 0;
            }
        }
    }
    public IEnumerator PlayRhythmSet()
    {
        notes = SetLevelCreator();
        levelList = rhythmLevelManager.GetCurrentLevel(level);
        noteManager.SetTotalNoteCount(levelList.Count);
        counter = 0;
        waiting = true;
        foreach ((NoteType, float) note in notes)
        {
            currentNote = note;
            if (counter > 0)
            {
                yield return new WaitUntil(() => waiting == false);
                waiting = true;
            }
            NoteProperties noteInfo = noteProperties.Find(x => x.noteType == note.Item1);
            SetupNote(noteInfo,levelList[counter].noteName, levelList[counter].isLong, levelList[counter].slowEffect);
            counter++;
        }
        yield return new WaitUntil(() => noteManager.CheckNoNotesLeft());
        yield return new WaitForSeconds(2f);
        miracleManager.SetAchievedMiracle(EvaluateResult());
        noteManager.ResetGame();
    }
    private bool EvaluateResult()
    {
        return (((float)noteManager.GetTotalHitCount() / (float)noteCount) >= 0.5f);
    }
    private void SetupNote(NoteProperties noteInfo, string soundName, bool isLong, bool slowEffect)
    {
        BaseNote _note = Instantiate(SpawnVariedSizedNotes(isLong, slowEffect)).GetComponent<BaseNote>();
        _note.transform.position = new Vector3(noteInfo.xPos, startY, 0);
        _note.transform.SetParent(stage.transform, false);
        _note.transform.SetAsFirstSibling();
        _note.gameObject.GetComponent<Image>().material = noteInfo.noteMaterial;
        _note.SetColour(noteInfo.color);
        _note.SetNoteManager(noteManager);
        _note.SetKeys(noteInfo.keys.Item1, noteInfo.keys.Item2);
        _note.SetPosIndex(noteInfo.posIndex);
        _note.SetSoundName(soundName);
        noteManager.EnqueueNote(_note);
        if (slowEffect)
        {
            SetSlowShader(_note, noteInfo);
        }
    }
    private void SetSlowShader(BaseNote _note, NoteProperties _noteInfo)
    {
        _note.gameObject.GetComponent<Image>().material = matList[4];
        _note.gameObject.GetComponent<Image>().material.SetColor("_ColourA",
            Color.black);
        _note.gameObject.GetComponent<Image>().material.SetColor("_ColourB", _noteInfo.color);
        
    }
    GameObject SpawnVariedSizedNotes(bool _isLong, bool _isSlow)
    {
        if (_isLong)
        {
            return longNote;
        }
        if (_isSlow)
        {
            return slowNote;
        }
        return shortNote;

    }

    private List<(NoteType, float)> SetLevelCreator()
    {
        List<(NoteType, float)> notes = new List<(NoteType, float)>();
        List<RhythmLevel> levelList = rhythmLevelManager.GetCurrentLevel(level);
        for (int i = 0; i < levelList.Count; i++) { 
            notes.Add((GenerateRandomNote(), levelList[i].timeToWait));
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
    public void IncreaseLevel()
    {
        level++;
        if (level == 2)
        {
            shortNote.GetComponent<BaseNote>().SetSwapChance(15);
            longNote.GetComponent<BaseNote>().SetSwapChance(15);
            shortNote.GetComponent<BaseNote>().SetFallSpeed(11);
            longNote.GetComponent<BaseNote>().SetFallSpeed(11);
            slowNote.GetComponent<BaseNote>().SetFallSpeed(11);
        }
    }
    private void InitialiseNoteProperties()
    {
        noteProperties = new List<NoteProperties>()
         {  new NoteProperties(NoteType.Left, matList[0], colours[0], (KeyCode.LeftArrow, KeyCode.A)
            ,buttons[0].GetComponent<RectTransform>().anchoredPosition.x, 0),

             new NoteProperties(NoteType.Up, matList[1], colours[1], (KeyCode.UpArrow, KeyCode.W)
            ,buttons[1].GetComponent<RectTransform>().anchoredPosition.x, 1),

            new NoteProperties(NoteType.Down, matList[2], colours[2], (KeyCode.DownArrow, KeyCode.S),
           buttons[2].GetComponent<RectTransform>().anchoredPosition.x, 2),

            new NoteProperties(NoteType.Right, matList[3], colours[3], (KeyCode.RightArrow, KeyCode.D)
           ,buttons[3].GetComponent<RectTransform>().anchoredPosition.x, 3)
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
    
 

        public NoteProperties(NoteType _noteType, Material _noteMaterial, Color _color ,
            (KeyCode, KeyCode) _keys, float _xPos, int _posIndex)
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
