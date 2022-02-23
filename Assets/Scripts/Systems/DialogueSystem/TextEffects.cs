using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEffects : MonoBehaviour
{
    private DialogueUI dialogueUI;
    [SerializeField] float timeBetweenCharacters = 0;
    private float _waitTime = 0;
    private bool prevCharWasPunctuation;
    private List<Punctation> punctations = new List<Punctation>()
    {
        new Punctation(new HashSet<char>(){'.','!','?'}, 0.6f),
        new Punctation(new HashSet<char>(){',',':',';'}, 0.3f),
    };
    private void Awake()
    {
        dialogueUI = GetComponent<DialogueUI>();
        dialogueUI.ShowDialogueBox(false);
    }
    public Coroutine Run(string _textToType)
    {
        return StartCoroutine(TypingText(_textToType));
    }


    private IEnumerator TypingText(string _textToType) //Typewriter effect
    {
        prevCharWasPunctuation = false;
        string currentText = "";
        for(int i = 0; i<_textToType.Length; i++)
        {
            currentText += _textToType[i];
            dialogueUI.ShowText(currentText);
            if (i != _textToType.Length - 1)
            {
                IsPunctuation(_textToType[i], out float _waitTime);
                yield return new WaitForSeconds(_waitTime);
            }
        }
    
    }
    private void IsPunctuation(char _character, out float _waitTime)
    {
        _waitTime = timeBetweenCharacters;
        foreach(Punctation punctuationType in punctations)
        {
            if (punctuationType.punctations.Contains(_character))
            {
                _waitTime = punctuationType.waitTime;
           
                return;
            }
        }
      
    }
}
    public readonly struct Punctation
    {
        public readonly HashSet<char> punctations;
        public readonly float waitTime;

        public Punctation(HashSet<char> _punctuations, float _waitTime)
        {
            punctations = _punctuations;
            waitTime = _waitTime;
        }
    }

