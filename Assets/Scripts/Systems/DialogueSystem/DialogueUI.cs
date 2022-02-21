using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    private GameObject dialogueBox;

    private void Awake()
    {
        dialogueBox = gameObject;
    }

    public void ShowText(string text)
    {
        ShowDialogueBox(true);
        textLabel.text = text;
    }
    public void ShowDialogueBox(bool on)
    {
        dialogueBox.SetActive(on);
    }
}
   
