using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private ResponseHandler responseHandler;
    [SerializeField] DialogueTreeObject initialDialogueTree;
    DialogueTreeObject currentDialogueTree;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject indoctrinationPrefab;
    [SerializeField] private string name;
    [SerializeField] TMP_Text nameText;
    private int currentConversation = 0;
    private float influenceLevel = 50;
    private PlayerStats player;
    private bool currentlyTalking = false;
    public float GetInfluenceLevel() => influenceLevel;
    public void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>();
        currentDialogueTree = initialDialogueTree;
        PopulateDialogueNodes();
    }
    private void PopulateDialogueNodes()
    {
        List<DialogueNode> allNodes = new List<DialogueNode>();
        if (currentDialogueTree)
        {
            currentDialogueTree.Reset();
            currentDialogueTree.Initialise();
            allNodes = currentDialogueTree.GetAllNodes();
        }
        foreach (var _node in allNodes)
        {
            _node.GetDialogueObject().SetAssociatedNPC(this);
        }
    }

    public void RunDialogue()
    {
        StartCoroutine(RunThroughDialogueTree());  //Runs through dialogue tree for specific NPC
        currentDialogueTree.SetOriginalInfluenceChance(player.GetInfluence());
        player.UpdateInfluenceChanceBar(player.GetInfluence());
        currentlyTalking = true;
        nameText.text = name;

    }
    public IEnumerator RunThroughDialogueTree()
    {
        if (currentDialogueTree.GetCurrentNode() == null|| currentDialogueTree.GetCurrentNode().AllChildrenNull()) 
        {
            currentDialogueTree.Reset(); //Possibly add catch phrase here
      
            yield return null; 
        }
        do
        {
            currentDialogueTree.UpdateTotalInfluenceChance(currentDialogueTree.GetCurrentNode().GetChanceEffect());
            player.UpdateInfluenceChanceBar(currentDialogueTree.GetCurrentInfluenceChance());
            responseHandler.SetResponseChosen(false);
            dialogueUI.ShowDialogue(currentDialogueTree.GetCurrentNode().GetDialogueObject());
            yield return new WaitUntil(() => responseHandler.GetResponseChosen() == true);
        }
        while (currentDialogueTree.GetCurrentNode() != null&&currentlyTalking);
    }
    public void EvaluatePersausionChance()
    {
        int rand = Random.Range(0, 101);
        if (rand <= currentDialogueTree.GetCurrentInfluenceChance())
        {
            influenceLevel = Mathf.Clamp(influenceLevel + currentDialogueTree.GetNPCInfluenceChange(), 0, 100);
            Instantiate(indoctrinationPrefab, gameObject.transform.position+new Vector3(0,-0.25f,0), gameObject.transform.rotation);
        }
        else
        {
            influenceLevel = Mathf.Clamp(influenceLevel - currentDialogueTree.GetNPCInfluenceChange(), 0,100);
        }
        currentlyTalking = false;
        nameText.text = "";
        player.CalculateInfluence();
        currentDialogueTree.IncrementConversationAffectedNPC();
    }
    public void MakeDecision(int _direction)
    {   
        currentDialogueTree.Traverse(_direction);
        currentDialogueTree.AssessRightPath();
    }
    public void SetDialogueTree(DialogueTreeObject _dialogueTree) 
    {
        currentDialogueTree = _dialogueTree;
        PopulateDialogueNodes();
    }

    public bool GetCurrentlyTalking() => currentlyTalking;
 
}
