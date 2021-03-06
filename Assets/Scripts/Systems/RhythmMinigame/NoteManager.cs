using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NoteManager : MonoBehaviour
{
    [SerializeField] GameObject instantBurst, longBurst, specialBurst;
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
    void Update()
    {
        SetButtonPressUI();
    }
    public void ResetGame()
    {
        totalHitCount = 0;
        firstPress = true;
        feedbackText.text = "";
        accumulator = 0;
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
    public ParticleSystem SpecialBurst(Color _colour, KeyCode _associatedKey)
    {
        var tmp = SetupParticles(specialBurst, _colour, _associatedKey);
        SoundManager.instance.StartCoroutine("SlowDownTimeEffect");
        return tmp.GetComponent<ParticleSystem>();
    }
    GameObject SetupParticles(GameObject particlePrefab, Color _colour, KeyCode _associatedKey)
    {
        var tmp = Instantiate(particlePrefab, transform);
        tmp.transform.position = DecideParticlePosition(_associatedKey);
        tmp.GetComponent<ParticleSystemRenderer>().material.SetColor("_TintColor", _colour);
        if (tmp.GetComponent<ParticleSystemRenderer>().trailMaterial != null)
        {
            tmp.GetComponent<ParticleSystemRenderer>().trailMaterial.color = _colour;
        }
        Destroy(tmp, 1.5f);
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
        yield return new WaitUntil(() => canTween);
        canTween = false;
        var textObjTransform = _text.gameObject.transform;
        textObjTransform.localScale = Vector3.zero;
        _text.DOColor(_colour, 0.25f);
        textObjTransform.DOScale(1f, 0.25f).SetEase(Ease.InOutElastic).OnComplete(() =>
        _text.DOColor(new Color(0, 0, 0, 0), 0.2f)).OnComplete(() => canTween = true);
    }
    public void EnqueueNote(BaseNote _note)
    {
        noteQueue.Enqueue(_note);
        if (_note.EvaluateShouldSwap())
        {
            if (!_note.GetSwapped())
            {
                StartCoroutine(SwapNoteAfterWait(_note, _note.GetPosIndex()));
                _note.SetSwapped(true);
            }
          
        }
    }

    public void DequeueNote()
    {
        PlaySongOnFirstNote();
        noteQueue.Dequeue();
        PlayMissNoteSound();
        UpdatePercentageText(false);
    }
    private void PlayMissNoteSound()
    {
        if (accumulator > 4)
        {
            SoundManager.instance.Play("LoseStreak");
        }
        else
        {
            SoundManager.instance.Play("MissNote");
        }
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
        FeedbackTextVariedOptions();
        feedbackText.fontSize = 60;
    }
    public void FeedbackTextVariedOptions()
    {
        int rand = UnityEngine.Random.Range(0, 3);
        if(rand == 0) feedbackText.text = "Ouch!";
        if (rand == 1) feedbackText.text = "Whoops!";
        if (rand == 2) feedbackText.text = "Too slow!";
    }
    private void IncreaseFontSizeIfRally(int _acc)
    {
        feedbackText.fontSize = 60 + (_acc*0.75f);
    }
    private IEnumerator SwapNoteAfterWait(BaseNote _note, int _notePos)
    {
        var waitTime = UnityEngine.Random.Range(0.2f, 0.3f);
        yield return new WaitForSeconds(waitTime);
        _note.GetComponent<RectTransform>().DOAnchorPosX(FindPotentialNotePositions(_notePos, _note), 0.4f).SetEase(Ease.InOutQuad)
            .OnComplete(() => _note.gameObject.GetComponent<Image>().material = _note.GetChangeMat());
       
    }

    private float FindPotentialNotePositions(int _pos, BaseNote _note)
    {
        int[] potentialPos = notePositions.Find(x => _pos == x.posIndex).potentialSwaps;
        if (potentialPos.Length == 1)
        {
            return SetNewPositionProperites(_note, notePositions.Find(x => x.posIndex == potentialPos[0]));
        }
        int rand = UnityEngine.Random.Range(0, 2);
        return SetNewPositionProperites(_note, notePositions.Find(x => x.posIndex == potentialPos[rand]));
    }

    private float SetNewPositionProperites(BaseNote _note, NotePosition _noteAtPosition)
    {
        _note.SetKeys(_noteAtPosition.key1, _noteAtPosition.key2);
        _note.SetChangeMat(_noteAtPosition.mat);
        _note.SetColour(_noteAtPosition.colour);
        return _noteAtPosition.xPos;
    }
    public void PopulatePositions(List<Material> _materials)
    {
       
        notePositions = new List<NotePosition>()
         {  new NotePosition(0,leftButton.GetComponent<RectTransform>().anchoredPosition.x,new int[] {1},_materials[0],
         KeyCode.LeftArrow, KeyCode.A, leftButton.GetComponent<Image>().color),

            new NotePosition(1,upButton.GetComponent<RectTransform>().anchoredPosition.x, new int[] {0,2},_materials[1]
            ,KeyCode.UpArrow, KeyCode.W,upButton.GetComponent<Image>().color),

            new NotePosition(2,downButton.GetComponent<RectTransform>().anchoredPosition.x,new int[] {1,3},_materials[2],
            KeyCode.DownArrow, KeyCode.S,downButton.GetComponent<Image>().color),

            new NotePosition(3,rightButton.GetComponent<RectTransform>().anchoredPosition.x,new int[] {2},_materials[3],
            KeyCode.RightArrow, KeyCode.D,rightButton.GetComponent<Image>().color),
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
        
    public bool CheckNoNotesLeft() => noteQueue.Count == 0;
    
    public int GetTotalHitCount()=> totalHitCount;
    
    public int GetTotalNoteCount() => totalNoteCount;
    public readonly struct NotePosition
    {
        public readonly Color colour;
        public readonly Material mat;
        public readonly KeyCode key1;
        public readonly KeyCode key2;
        public readonly int posIndex;
        public readonly float xPos;
        public readonly int[] potentialSwaps;
        public NotePosition(int _posIndex, float _xpos, int[] _potentialSwaps, Material _mat, KeyCode _key1, KeyCode _key2, Color _colour)
        {
            colour = _colour;
            mat = _mat;
            key1 = _key1;
            key2 = _key2;
            posIndex = _posIndex;
            xPos = _xpos;
            potentialSwaps = _potentialSwaps;
        }
          
    }
}
