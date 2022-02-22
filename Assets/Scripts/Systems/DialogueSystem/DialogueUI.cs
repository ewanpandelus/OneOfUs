using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
   
    [SerializeField] private TMP_Text textLabel;
    private TextEffects textEffects;
    private GameObject dialogueBox;
    private bool showingText = false;
    public bool GetShowingText() => showingText;

    private void Awake()
    {
        dialogueBox = gameObject;
        textEffects = GetComponent<TextEffects>();
    }

    public void ShowText(string _text)
    {
        ShowDialogueBox(true);
        textLabel.text = _text;
    }
    public void ShowDialogueBox(bool _on)
    {
        dialogueBox.SetActive(_on);
        showingText = _on;
    }
    public void ShowDialogue(DialogueObject _dialogueObject)
    {
        ShowDialogueBox(true);
        StartCoroutine(RunThroughDialogue(_dialogueObject));
    }
    public IEnumerator RunThroughDialogue(DialogueObject _dialogueObject)
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < _dialogueObject.GetDialogue.Length; i++)
        {
            yield return textEffects.Run(_dialogueObject.GetDialogue[i]);
            yield return new WaitUntil(()=>Input.GetMouseButtonDown(0));
        }
        ShowText(string.Empty);
        ShowDialogueBox(false);
    }
    
}
   
