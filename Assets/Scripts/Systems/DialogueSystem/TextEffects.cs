using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffects : MonoBehaviour
{
    private DialogueUI dialogueUI;
    [SerializeField] float timeBetweenCharacters = 0;
    private void Awake()
    {
        dialogueUI = GetComponent<DialogueUI>();
        Run("This is a test! Wonderful!\nLets add more lines!\n....");
        dialogueUI.ShowDialogueBox(false);
    }
    public Coroutine Run(string _textToType)
    {
        return StartCoroutine(TypingText(_textToType));
    }


    private IEnumerator TypingText(string _textToType)
    {
        string currentText = "";
        for(int i = 0; i< _textToType.Length; i++)
        {
            currentText += _textToType[i];
            dialogueUI.ShowText(currentText);
            yield return new WaitForSeconds(timeBetweenCharacters);
        }
    
    }
}
