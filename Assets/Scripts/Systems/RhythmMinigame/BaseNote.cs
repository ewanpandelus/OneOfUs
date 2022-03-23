using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public abstract class BaseNote : MonoBehaviour
{
    protected bool canBePressed;
    protected KeyCode key1, key2;
    protected AudioSource noteSound;
    protected NoteManager noteManager;
    protected bool alreadyExited = false;
    protected Color colour;
    protected int posIndex = 5;
    protected Transform _transform;
    [SerializeField] protected float fallSpeed;
    [SerializeField] protected int swapChance = 2;
    private Material changeMat;
    private void Awake()
    {
        _transform = transform;
    }
    private void Update()
    {
        HandleHits();
        _transform.position -= (Time.deltaTime * fallSpeed* Vector3.up);
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Button")
        {
            canBePressed = true;
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Button"&&!alreadyExited)
        {
            RemoveNote();
            noteManager.UpdateFeedbackText(false, colour);
        }
    }


    protected virtual void RemoveNote()
    {
        canBePressed = false;
        if (!alreadyExited)
        {
            alreadyExited = true;
            noteManager.DequeueNote();
            Destroy(gameObject);
        }
      
    }
    public void SetKeys(KeyCode _key1, KeyCode _key2)
    {
        key1 = _key1;
        key2 = _key2;
    }
    protected virtual void HandleHits()
    {
        if (!canBePressed)
        {
            return;
        }
        if (Input.GetKeyDown(key1) || Input.GetKeyDown(key2) ||Input.GetKeyUp(key1)||Input.GetKeyUp(key2))
        {
            noteManager.RemoveNote(this, colour ,key1, true);
            noteManager.UpdateFeedbackText(true, colour);
            noteSound.PlayOneShot(noteSound.clip);
    
        }
    }
    public bool EvaluateShouldSwap()
    {
        int randNum = UnityEngine.Random.Range(0, 101);
        return randNum <= swapChance;
    }

    public void SetNoteManager(NoteManager _noteManager) => noteManager = _noteManager;
    public void SetAlreadyExited(bool _alreadyExited) => alreadyExited = _alreadyExited;
    public void SetColour(Color _colour) => colour = _colour; 
    public void SetPosIndex(int _posIndex)=> posIndex = _posIndex;
    public int GetPosIndex() =>  posIndex;
    public void SetChangeMat(Material _changeMat) => changeMat = _changeMat;
    public Material GetChangeMat() => changeMat;
    public void SetNoteSound(AudioSource _noteSound) => noteSound = _noteSound;
    
}
