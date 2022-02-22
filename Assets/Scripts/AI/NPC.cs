using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private TextTreeObject textTreeObject;
    [SerializeField] private DialogueUI dialogueUI;
 
    public void RunDialogue()
    {
        dialogueUI.ShowDialogue(textTreeObject.GetCurrentNode().GetDialogueObject());
    }
    public void MakeDecision(bool _left)
    {
        textTreeObject.Traverse(_left);
    }
}
