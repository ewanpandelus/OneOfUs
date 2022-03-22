using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class NoteManager : MonoBehaviour
{
    [SerializeField] GameObject instantBurst, longBurst;
    [SerializeField] GameObject leftButton, rightButton, upButton, downButton;
    [SerializeField] GameObject leftButtonPress, rightButtonPress, upButtonPress, downButtonPress;
    [SerializeField] TMP_Text percentageText, feedbackText;
    Queue<BaseNote> noteQueue = new Queue<BaseNote>();
    private List<NotePositions> notePositions;
    private int totalHitCount = 0;
    private int totalNoteCount = 0;
    bool canTween = true;
    int accumulator = 0;

    private void Start()
    {
        PopulatePositions();
    }

    void Update()
    {
        if (noteQueue.Count == 0)
        {
            return;
        }
        SetButtonPressUI();
    }
    private void SetButtonPressUI()
    {
        leftButtonPress.SetActive(CheckKeysPressed(KeyCode.LeftArrow, KeyCode.A));
        rightButtonPress.SetActive(CheckKeysPressed(KeyCode.RightArrow, KeyCode.D));
        upButtonPress.SetActive(CheckKeysPressed(KeyCode.UpArrow, KeyCode.W));
        downButtonPress.SetActive(CheckKeysPressed(KeyCode.DownArrow, KeyCode.S));
    }
    public void RemoveNote(BaseNote _closestNote, Color _colour, KeyCode _associatedKey, bool _pop)
    {
        if (_pop)
        {
            SetupParticles(instantBurst, _colour, _associatedKey);
        }
        _closestNote.SetAlreadyExited(true);    
        Destroy(noteQueue.Dequeue().gameObject);
        UpdatePercentageText(true);
    }
    public ParticleSystem LongBurst(Color _colour, KeyCode _associatedKey)
    {
        var tmp = SetupParticles(longBurst, _colour, _associatedKey);
        return tmp.GetComponent<ParticleSystem>();
    }
    GameObject SetupParticles(GameObject particlePrefab, Color _colour, KeyCode _associatedKey)
    {
        var tmp = Instantiate(particlePrefab, transform);
        tmp.transform.position = DecideParticlePosition(_associatedKey);
        tmp.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", _colour);
        Destroy(tmp, 1f);
        return tmp;
    }
    private Vector3 DecideParticlePosition(KeyCode _key)
    {
        if (_key == KeyCode.LeftArrow) return leftButton.transform.position;
        if (_key == KeyCode.RightArrow) return rightButton.transform.position;
        if (_key == KeyCode.UpArrow) return upButton.transform.position;
        return downButton.transform.position;
    }

    IEnumerator ScaleText(TMP_Text _text, Color _colour)
    {
        yield return new WaitUntil(()=>canTween);
        canTween = false;
        var textObjTransform = _text.gameObject.transform;
        textObjTransform.localScale = Vector3.zero;
        _text.DOColor(_colour, 0.25f);
        textObjTransform.DOScale(1f, 0.25f).SetEase(Ease.InOutElastic).OnComplete(() =>
        _text.DOColor(new Color(0, 0, 0, 0), 0.2f)).OnComplete(()=>canTween = true);
    }
    public void EnqueueNote(BaseNote _note)
    {
        noteQueue.Enqueue(_note);
        if (_note.EvaluateShouldSwap())
        {
           
        }
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
    public void UpdateFeedbackText(bool _hit, Color _colour)
    {
        
        StartCoroutine(ScaleText(feedbackText, _colour));
        accumulator = (_hit) ? accumulator + 1 : 0;
        if (_hit)
        {
            feedbackText.text = "+" + accumulator.ToString();
            IncreaseFontSizeIfRally(accumulator);
            return;
        }
        feedbackText.text = "Ouch!";
        feedbackText.fontSize = 60;

     
    }
    private void IncreaseFontSizeIfRally(int _acc)
    {
        feedbackText.fontSize = 60 + (_acc*1.5f);
    }
    private IEnumerator SwapNotePosition(BaseNote _note, int _notePos)
    {
        var waitTime = UnityEngine.Random.Range(0.2f, 1f);
        yield return null;
    }
    private void PopulatePositions()
    {
        notePositions = new List<NotePositions>()
         {  new NotePositions(0,leftButton.transform.position.x,new int[] {1}),
            new NotePositions(1,upButton.transform.position.x, new int[] {0,2}),
            new NotePositions(2,downButton.transform.position.x,new int[] {1,3}),
            new NotePositions(3,rightButton.transform.position.x,new int[] {2}),
         };
    }
    public void SetTotalNoteCount(int _noteCount)
    {
        totalNoteCount = _noteCount;
        UpdatePercentageText(false);
    }
    public bool CheckKeysPressed(KeyCode key1, KeyCode key2)
    {
        return (Input.GetKeyDown(key1) || Input.GetKeyDown(key2) || Input.GetKey(key1) || Input.GetKey(key2));
    }
        
    public bool CheckNoNotesLeft()
    {
        return noteQueue.Count == 0;
    }
    public int GetTotalHitCount()
    {
        return totalHitCount;
    }
    
    public readonly struct NotePositions
    {
        readonly int posIndex;
        readonly float xPos;
        readonly int[] potentialSwaps;
        public NotePositions(int _posIndex, float _xpos, int[] _potentialSwaps)
        {
            posIndex = _posIndex;
            xPos = _xpos;
            potentialSwaps = _potentialSwaps;
        }
    }
}
