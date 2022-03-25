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
    [SerializeField] List<GameObject> buttons;// leftButton, rightButton, upButton, downButton;
    [SerializeField] List<Material> matList = new List<Material>();
    [SerializeField] List<Color> colours = new List<Color>();
    [SerializeField] List<AudioSource> sounds;
    [SerializeField] int noteCount;
    [SerializeField] private GameObject stage;
    [SerializeField] private MiracleManager miracleManager;
    
    private float startY = 0;
    private List<NoteProperties> noteProperties;
    private NoteManager noteManager;
    private RhythmLevelManager rhythmLevelManager;
    
       
    
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
        rhythmLevelManager = GetComponent<RhythmLevelManager>();
        InitialiseNoteProperties();
   
    }
    public IEnumerator PlayRhythm(float minSeperationTime, float maxSeperationTime)
    {
        List<(NoteType, float)> notes = new List<(NoteType, float)>();
        notes = RandomLevelCreator(noteCount, minSeperationTime, maxSeperationTime);
        noteManager.SetTotalNoteCount(noteCount);
        foreach((NoteType, float) note in notes)
        {
            yield return new WaitForSeconds(note.Item2);
            NoteProperties noteInfo = noteProperties.Find(x => x.noteType == note.Item1);
            SetupNote(noteInfo,"");
        }
        yield return new WaitUntil(() => noteManager.CheckNoNotesLeft());
        yield return new WaitForSeconds(1f);
        miracleManager.SetAchievedMiracle(EvaluateResult());
    }
    public IEnumerator PlayRhythmSet()
    {
        List<(NoteType, float)> notes = new List<(NoteType, float)>();
        notes = SetLevelCreator();
        List<RhythmLevel> levelList = rhythmLevelManager.GetLevel1();
        noteManager.SetTotalNoteCount(levelList.Count);
        int counter = 0;
        foreach ((NoteType, float) note in notes)
        {
            if (counter > 0)
            {
                yield return new WaitForSeconds(note.Item2 - levelList[counter - 1].timeToWait);
            }
            NoteProperties noteInfo = noteProperties.Find(x => x.noteType == note.Item1);
            SetupNote(noteInfo,levelList[counter].noteName);
            counter++;
        }
        yield return new WaitUntil(() => noteManager.CheckNoNotesLeft());
        yield return new WaitForSeconds(1f);
        miracleManager.SetAchievedMiracle(EvaluateResult());
    }
    private bool EvaluateResult()
    {
        return (((float)noteManager.GetTotalHitCount() / (float)noteCount) >= 0.5f);
    }
    private void SetupNote(NoteProperties noteInfo, string soundName)
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
        _note.SetPosIndex(noteInfo.posIndex);
        _note.SetNoteSound(noteInfo.noteSound);
        _note.SetSoundName(soundName);
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
    private List<(NoteType, float)> RandomLevelCreator(int noteCount, float minWait, float maxWait)
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
    private List<(NoteType, float)> SetLevelCreator()
    {
        List<(NoteType, float)> notes = new List<(NoteType, float)>();
        List<RhythmLevel> levelList = rhythmLevelManager.GetLevel1();
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
    private void InitialiseNoteProperties()
    {
        noteProperties = new List<NoteProperties>()
         {  new NoteProperties(NoteType.Left, matList[0], colours[0], (KeyCode.LeftArrow, KeyCode.A)
            ,buttons[0].GetComponent<RectTransform>().anchoredPosition.x, 0, sounds[0]),

             new NoteProperties(NoteType.Up, matList[1], colours[1], (KeyCode.UpArrow, KeyCode.W)
            ,buttons[1].GetComponent<RectTransform>().anchoredPosition.x, 1, sounds[1]),

            new NoteProperties(NoteType.Down, matList[2], colours[2], (KeyCode.DownArrow, KeyCode.S),
           buttons[2].GetComponent<RectTransform>().anchoredPosition.x, 2, sounds[2]),

            new NoteProperties(NoteType.Right, matList[3], colours[3], (KeyCode.RightArrow, KeyCode.D)
           ,buttons[3].GetComponent<RectTransform>().anchoredPosition.x, 3,sounds[3])
         };

        startY = (Screen.height / 2) + 10;//Just over half screen size
    }

    public readonly struct NoteProperties
    {
        public readonly AudioSource noteSound;
        public readonly Color color;
        public readonly Material noteMaterial;
        public readonly NoteType noteType;
        public readonly (KeyCode,KeyCode) keys;
        public readonly float xPos;
        public readonly int posIndex;
    
 

        public NoteProperties(NoteType _noteType, Material _noteMaterial, Color _color ,
            (KeyCode, KeyCode) _keys, float _xPos, int _posIndex, AudioSource _noteSound)
        {
            color = _color;
            noteMaterial = _noteMaterial;
            noteSound = _noteSound;
            noteType = _noteType;
            keys = _keys;
            posIndex = _posIndex;
            xPos = _xPos;//- Screen.width / 2;

        }
    }
}
