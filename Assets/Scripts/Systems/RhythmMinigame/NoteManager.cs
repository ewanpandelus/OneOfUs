using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[System.Serializable]
public class NoteManager : MonoBehaviour
{
    [SerializeField] GameObject instantBurst, longBurst;
    [SerializeField] GameObject leftButton, rightButton, upButton, downButton;
    [SerializeField] GameObject leftButtonPress, rightButtonPress, upButtonPress, downButtonPress;
    [SerializeField] TMP_Text percentageText, feedbackText;
    Queue<BaseNote> noteQueue = new Queue<BaseNote>();
    private List<NotePosition> notePositions;
    private int totalHitCount = 0;
    private int totalNoteCount = 0;
    private bool canTween = true;
    private int accumulator = 0;
    private bool firstPress = true;

    public void Initialise()
    {
        firstPress = true;
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
    public void RemoveNote(BaseNote _closestNote)
    {
      
        _closestNote.SetAlreadyExited(true);    
        Destroy(noteQueue.Dequeue().gameObject);
        UpdatePercentageText(true);
        PlaySongOnFirstNote();
    }
    public void PlayParticleEffect(bool _pop, Color _colour, KeyCode _associatedKey)
    {
        if (_pop)
        {
            SetupParticles(instantBurst, _colour, _associatedKey);
        }
    }
    private void PlaySongOnFirstNote()
    {
        if (firstPress)
        {
            SoundManager.instance.StartMiniGameSong();
            firstPress = false;
        }
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
            StartCoroutine(SwapNoteAfterWait(_note, _note.GetPosIndex()));
        }
    }
    
    public void DequeueNote()
    {
        PlaySongOnFirstNote();
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
    private IEnumerator SwapNoteAfterWait(BaseNote _note, int _notePos)
    {
        var waitTime = UnityEngine.Random.Range(0.8f, 1.8f);
        yield return new WaitForSeconds(waitTime);
        StartCoroutine(SwapNotePos(FindPotentialNotePositions(_notePos, _note), _note.transform.position.x, _note));
        yield return null;
    }
    private IEnumerator SwapNotePos(float _finishPos, float _startPos, BaseNote _note)
    {
        float x = 0f;
        var t = 0f;
        while (t < 1)
        {
            t += Time.deltaTime*3;
            x = Mathf.Lerp(_startPos, _finishPos, t);
            _note.gameObject.transform.position = new Vector3(x, _note.transform.position.y, 0);
            yield return null;
        }
        _note.gameObject.GetComponent<Image>().material = _note.GetChangeMat();

    }
    private float FindPotentialNotePositions(int _pos, BaseNote _note)
    {
        int[] potentialPos = notePositions.Find(x => _pos == x.posIndex).potentialSwaps;
        if(potentialPos.Length == 1)
        {
            NotePosition noteAtPosition = notePositions.Find(x => x.posIndex == potentialPos[0]);
            _note.SetKeys(noteAtPosition.key1, noteAtPosition.key2);
            _note.SetChangeMat(noteAtPosition.mat);
            return noteAtPosition.xPos;
        }
        else
        {
            int rand = UnityEngine.Random.Range(0, 2);
            NotePosition noteAtPosition = notePositions.Find(x => x.posIndex == potentialPos[rand]);
            _note.SetKeys(noteAtPosition.key1, noteAtPosition.key2);
            _note.SetChangeMat(noteAtPosition.mat);
            return noteAtPosition.xPos;
        }
    }
    public void PopulatePositions(List<Material> _materials)
    {
        notePositions = new List<NotePosition>()
         {  new NotePosition(0,leftButton.transform.position.x,new int[] {1},_materials[0],KeyCode.LeftArrow, KeyCode.A),
            new NotePosition(1,upButton.transform.position.x, new int[] {0,2},_materials[1],KeyCode.UpArrow, KeyCode.W),
            new NotePosition(2,downButton.transform.position.x,new int[] {1,3},_materials[2],KeyCode.DownArrow, KeyCode.S),
            new NotePosition(3,rightButton.transform.position.x,new int[] {2},_materials[3],KeyCode.RightArrow, KeyCode.D),
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
    
    public readonly struct NotePosition
    {

        public readonly Material mat;
        public readonly KeyCode key1;
        public readonly KeyCode key2;
        public readonly int posIndex;
        public readonly float xPos;
        public readonly int[] potentialSwaps;
        public NotePosition(int _posIndex, float _xpos, int[] _potentialSwaps, Material _mat, KeyCode _key1, KeyCode _key2)
        {
            mat = _mat;
            key1 = _key1;
            key2 = _key2;
            posIndex = _posIndex;
            xPos = _xpos;
            potentialSwaps = _potentialSwaps;
        }
          
    }
}
