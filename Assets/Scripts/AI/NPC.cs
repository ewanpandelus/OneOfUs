using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private ResponseHandler responseHandler;
    [SerializeField] private DialogueTreeObject dialogueTreeObject;
    [SerializeField] private DialogueUI dialogueUI;

    public void Awake()
    {
        List<Node> allNodes = new List<Node>();
        if (dialogueTreeObject)
        {
            allNodes = dialogueTreeObject.GetAllNodes();
        }
 
        foreach(Node _node in allNodes)
        {
            _node.GetDialogueObject().SetAssociatedNPC(this);
        }
        dialogueTreeObject.Reset();
    }
    public void RunDialogue()
    {
        StartCoroutine(RunThroughDialogueTree());  //Runs through dialogue tree for specific NPC     
    }
    public IEnumerator RunThroughDialogueTree()
    {
        if (dialogueTreeObject.GetCurrentNode() == null) 
        { 
            dialogueTreeObject.Reset(); 
            yield return null; 
        }
        do
        {
            responseHandler.SetResponseChosen(false);
            dialogueUI.ShowDialogue(dialogueTreeObject.GetCurrentNode().GetDialogueObject());
            yield return new WaitUntil(() => responseHandler.GetResponseChosen() == true);
        }
        while (dialogueTreeObject.GetCurrentNode() != null);
        
    }
    public void MakeDecision(bool _left)
    {
        dialogueTreeObject.Traverse(_left);
    }
}
