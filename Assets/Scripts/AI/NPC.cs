using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private ResponseHandler responseHandler;
    [SerializeField] private DialogueTreeObject dialogueTreeObject;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject indoctrinationPrefab;
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
        if (dialogueTreeObject.GetCurrentNode() == null||dialogueTreeObject.GetCurrentNode().AllChildrenNull()) 
        { 
            dialogueTreeObject.Reset();

            yield return null; 
        }
        do
        {
            dialogueTreeObject.UpdateTotalInfluenceChance(dialogueTreeObject.GetCurrentNode().GetChanceEffect());
            player.UpdateInfluenceChanceBar(dialogueTreeObject.GetCurrentInfluenceChance());
            responseHandler.SetResponseChosen(false);
            dialogueUI.ShowDialogue(dialogueTreeObject.GetCurrentNode().GetDialogueObject());
            yield return new WaitUntil(() => responseHandler.GetResponseChosen() == true);
        }
        while (dialogueTreeObject.GetCurrentNode() != null);
    }
    public void EvaluatePersausionChance()
    {
        int rand = Random.Range(0, 100);
        if (rand <= dialogueTreeObject.GetCurrentInfluenceChance())
        {
            influenceLevel += dialogueTreeObject.GetNPCInfluenceChange();
            Instantiate(indoctrinationPrefab); 
        }
        else
        {
            influenceLevel -= dialogueTreeObject.GetNPCInfluenceChange();
        }
        player.CalculateInfluence();
    }
    public void MakeDecision(int _direction)
    {
        dialogueTreeObject.Traverse(_direction);
    }
    public void UpdateInfluencelevel(float _influenceChange)
    {
        influenceLevel = Mathf.Clamp(influenceLevel + _influenceChange, 0, 100);
    }
}
