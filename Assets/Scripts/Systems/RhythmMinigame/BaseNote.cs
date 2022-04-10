using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public abstract class BaseNote : MonoBehaviour
{
    protected bool canBePressed;
    protected KeyCode key1, key2;
    protected NoteManager noteManager;
    protected bool alreadyExited = false;
    protected Color colour;
    protected int posIndex = 5;
    protected Transform _transform;
    [SerializeField] protected float fallSpeed;
    [SerializeField] protected int swapChance = 100;
    [SerializeField] protected Material clearMat; 
    private Material changeMat;
    protected string soundName = "";
    private bool pressed = false;
    protected Collider2D boundingObj;
    protected float initialYScale;
    private bool swapped = false;
    private void Awake()
    {
        _transform = transform;
        initialYScale = _transform.localScale.y;
        _transform.position -= Vector3.up * (initialYScale / 2);
    }
    void Update()
    {
        HandleHits();
        _transform.position -= (Time.deltaTime * fallSpeed* Vector3.up);
        if (boundingObj && (boundingObj.bounds.max.y > _transform.position.y + initialYScale / 2))
        {
            GetComponent<Image>().material = clearMat;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Button")
        {
            canBePressed = true;
            boundingObj = collision;
  
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Button"&&!alreadyExited)
        {
            if (pressed)
            {
                noteManager.UpdateFeedbackText(true, colour);
                SoundManager.instance.PlayOneShot(soundName);
                noteManager.RemoveNote(this);
            }
            else
            {
                RemoveNote();
                noteManager.UpdateFeedbackText(false, colour);
            }
      
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
            pressed = true;
            noteManager.PlayParticleEffect(true, colour, key1);
            canBePressed = false;
        }
    }
    public bool EvaluateShouldSwap()
    {
        int randNum = UnityEngine.Random.Range(0, 101);
        return randNum <= swapChance;
    }
    public void SetSoundName(string _soundName)
    {
        soundName = _soundName;
    }
    public void SetNoteManager(NoteManager _noteManager) => noteManager = _noteManager;
    public void SetAlreadyExited(bool _alreadyExited) => alreadyExited = _alreadyExited;
    public void SetColour(Color _colour) => colour = _colour; 
    public void SetPosIndex(int _posIndex)=> posIndex = _posIndex;
    public int GetPosIndex() =>  posIndex;
    public void SetChangeMat(Material _changeMat) => changeMat = _changeMat;
    public Material GetChangeMat() => changeMat;

    public void SetFallSpeed(int _fallSpeed) => fallSpeed = _fallSpeed;
    public void SetSwapChance(int _swapChance) => swapChance = _swapChance;
    public void SetSwapped(bool _swapped) => swapped = _swapped;
    public bool GetSwapped() {
        return swapped;
    }

}
