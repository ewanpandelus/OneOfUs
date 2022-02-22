using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private TextTreeObject textTreeObject;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private DialogueObject dialogueObject;
 
    public void RunDialogue()
    {
        dialogueUI.ShowDialogue(dialogueObject);
    }
    public void MakeDecision(bool _left)
    {
        textTreeObject.Traverse(_left);
    }
}
