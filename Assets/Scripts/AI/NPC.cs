using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private TextTreeObject textTreeObject;
    [SerializeField] private DialogueUI dialogueUI;

    public void Awake()
    {
        List<Node> allNodes = new List<Node>();
        allNodes = textTreeObject.GetAllNodes();
        foreach(Node _node in allNodes)
        {
            _node.GetDialogueObject().SetAssociatedNPC(this);
        }
        textTreeObject.Initialise();
    }
    public void RunDialogue()
    {
        if (textTreeObject.GetCurrentNode()== null) return;
        dialogueUI.ShowDialogue(textTreeObject.GetCurrentNode().GetDialogueObject());
       
    }
    //public IEnumerator RunThroughDialogueTree()
    //{

    //}
    public void MakeDecision(bool _left)
    {
        textTreeObject.Traverse(_left);
    }
}
