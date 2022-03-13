using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    [SerializeField] GameObject instantBurst, longBurst;
    [SerializeField] GameObject leftButton, rightButton, upButton, downButton;
    [SerializeField] GameObject leftButtonPress, rightButtonPress, upButtonPress, downButtonPress;
    [SerializeField] TMP_Text percentageText;
    Queue<BaseNote> noteQueue = new Queue<BaseNote>();
    private int totalHitCount = 0;
    private int totalNoteCount = 0;
 
    void Update()
    {
        if (noteQueue.Count == 0)
        {
            return;
        }
        leftButtonPress.SetActive(CheckKeysPressed(KeyCode.LeftArrow, KeyCode.A));
        rightButtonPress.SetActive(CheckKeysPressed(KeyCode.RightArrow, KeyCode.D));
        upButtonPress.SetActive(CheckKeysPressed(KeyCode.UpArrow, KeyCode.W));
        downButtonPress.SetActive(CheckKeysPressed(KeyCode.DownArrow, KeyCode.S));
    }
    private void SetButtonPressUI()
    {

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
    
    public void EnqueueNote(BaseNote _note)
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
    public bool CheckKeysPressed(KeyCode key1, KeyCode key2)
    {
        return (Input.GetKeyDown(key1) || Input.GetKeyDown(key2) || Input.GetKey(key1) || Input.GetKey(key2));
    }
}
