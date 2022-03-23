using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private ResponseHandler responseHandler;
    [SerializeField] private List<DialogueTreeObject> dialogueTrees = new List<DialogueTreeObject>();
    DialogueTreeObject dialogueTree;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject indoctrinationPrefab;
    [SerializeField] private string name;
    [SerializeField] TMP_Text nameText;
    private int currentConversation = 0;
    private float influenceLevel = 50;
    private PlayerStats player;
    private bool currentlyTalking = false;
    public float GetInfluenceLevel() => influenceLevel;
    public void Awake()
    {
        PopulateDialogueNodes();
        player = GameObject.FindObjectOfType<PlayerStats>();
    }
    private void PopulateDialogueNodes()
    {
        List<DialogueNode> allNodes = new List<DialogueNode>();
        if (dialogueTrees[currentConversation])
        {
            dialogueTree = dialogueTrees[currentConversation];
            dialogueTree.Reset();
            dialogueTree.Initialise();
            allNodes = dialogueTree.GetAllNodes();
        }
        foreach (var _node in allNodes)
        {
            _node.GetDialogueObject().SetAssociatedNPC(this);
        }
    }
    public void RunDialogue()
    {
        StartCoroutine(RunThroughDialogueTree());  //Runs through dialogue tree for specific NPC   
        dialogueTree.SetOriginalInfluenceChance(player.GetInfluence());
        player.UpdateInfluenceChanceBar(player.GetInfluence());
        currentlyTalking = true;
        nameText.text = name;

    }
    public IEnumerator RunThroughDialogueTree()
    {
        if (dialogueTree.GetCurrentNode() == null|| dialogueTree.GetCurrentNode().AllChildrenNull()) 
        {
            dialogueTree.Reset(); //Possibly add catch phrase here
      
            yield return null; 
        }
        do
        {
            dialogueTree.UpdateTotalInfluenceChance(dialogueTree.GetCurrentNode().GetChanceEffect());
            player.UpdateInfluenceChanceBar(dialogueTree.GetCurrentInfluenceChance());
            responseHandler.SetResponseChosen(false);
            dialogueUI.ShowDialogue(dialogueTree.GetCurrentNode().GetDialogueObject());
            yield return new WaitUntil(() => responseHandler.GetResponseChosen() == true);
        }
        while (dialogueTree.GetCurrentNode() != null&&currentlyTalking);
    }
    public void EvaluatePersausionChance()
    {
        int rand = Random.Range(0, 101);
        if (rand <= dialogueTree.GetCurrentInfluenceChance())
        {
            influenceLevel = Mathf.Clamp(influenceLevel + dialogueTree.GetNPCInfluenceChange(), 0, 100);
            Instantiate(indoctrinationPrefab, gameObject.transform.position+new Vector3(0,-0.25f,0), gameObject.transform.rotation);
        }
        else
        {
            influenceLevel = Mathf.Clamp(influenceLevel - dialogueTree.GetNPCInfluenceChange(), 0,100);
        }
        currentlyTalking = false;
        nameText.text = "";
        player.CalculateInfluence();
        dialogueTree.IncrementConversationAffectedNPC();
    }
    public void MakeDecision(int _direction)
    {   
        dialogueTree.Traverse(_direction);
        dialogueTree.AssessRightPath();
    }
    public void SetDialogueTree(DialogueTreeObject _dialogueTree) => dialogueTree = _dialogueTree;
    public void IncrementConversation()     
    {
        if (currentConversation == dialogueTrees.Count-1) { return; }
        currentConversation++;
        PopulateDialogueNodes();
    }
    public string GetName() => name;
    
 
}
