using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private ResponseHandler responseHandler;
    [SerializeField] private DialogueTreeObject textTreeObject;
    [SerializeField] private DialogueUI dialogueUI;

    public void Awake()
    {
        List<Node> allNodes = new List<Node>();
        allNodes = textTreeObject.GetAllNodes();
        foreach(Node _node in allNodes)
        {
            _node.GetDialogueObject().SetAssociatedNPC(this);
        }
        textTreeObject.Reset();
    }
    public void RunDialogue()
    {
        StartCoroutine(RunThroughDialogueTree());  //Runs through dialogue tree for specific NPC
      /*  if (textTreeObject.GetCurrentNode() == null)
        {

            return;
        }
        dialogueUI.ShowDialogue(textTreeObject.GetCurrentNode().GetDialogueObject());*/
       
    }
    public IEnumerator RunThroughDialogueTree()
    {
        if (textTreeObject.GetCurrentNode() == null) 
        { 
            textTreeObject.Reset(); 
            yield return null; 
        }
        do
        {
            responseHandler.SetResponseChosen(false);
            dialogueUI.ShowDialogue(textTreeObject.GetCurrentNode().GetDialogueObject());
            yield return new WaitUntil(() => responseHandler.GetResponseChosen() == true);
        }
        while (textTreeObject.GetCurrentNode() != null);
        
    }
    public void MakeDecision(bool _left)
    {
        textTreeObject.Traverse(_left);
    }
}
