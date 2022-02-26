using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private ResponseHandler responseHandler;
    [SerializeField] private DialogueTreeObject dialogueTreeObject;
    [SerializeField] private DialogueUI dialogueUI;
    private PlayerStats player;

    public void Awake()
    {
        PopulateDialogueNodes();
        player = GameObject.Find("Player").GetComponent<PlayerStats>();
    }
    private void PopulateDialogueNodes()
    {
        List<DialogueNode> allNodes = new List<DialogueNode>();
        if (dialogueTreeObject)
        {
            allNodes = dialogueTreeObject.GetAllNodes();
        }

        foreach (var _node in allNodes)
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
            player.SetInfluence(dialogueTreeObject.GetCurrentNode().GetHappinessEffect());
            responseHandler.SetResponseChosen(false);
            dialogueUI.ShowDialogue(dialogueTreeObject.GetCurrentNode().GetDialogueObject());
            yield return new WaitUntil(() => responseHandler.GetResponseChosen() == true);
        }
        while (dialogueTreeObject.GetCurrentNode() != null);
        
    }
    public void MakeDecision(int _direction)
    {
        dialogueTreeObject.Traverse(_direction);
    }
}
