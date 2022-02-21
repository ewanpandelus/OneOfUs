using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffects : MonoBehaviour
{
    private DialogueUI dialogueUI;
    [SerializeField] float timeBetweenCharacters = 0;
    private void Start()
    {
        dialogueUI = GetComponent<DialogueUI>();
        Run("This is a test! Wonderful!\nLets add more lines!\n....");
    }
    public void Run(string _textToType)
    {
        StartCoroutine(TypingText(_textToType));
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
        dialogueUI.ShowDialogueBox(false);
    }
}
