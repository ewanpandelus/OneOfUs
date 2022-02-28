using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private ResponseHandler responseHandler;
    [SerializeField] private DialogueTreeObject dialogueTreeObject;
    [SerializeField] private DialogueUI dialogueUI;
    private float influenceLevel = 50;
    private PlayerStats player;
    public float GetInfluenceLevel() => influenceLevel;
    public void Awake()
    {
        PopulateDialogueNodes();
        player = GameObject.FindObjectOfType<PlayerStats>();
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
            UpdateInfluencelevel(dialogueTreeObject.GetCurrentNode().GetHappinessEffect());
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
    private void UpdateInfluencelevel(float _influenceChange)
    {
        influenceLevel = Mathf.Clamp(influenceLevel + _influenceChange, 0, 100);
        player.CalculateInfluence();
    }
}
