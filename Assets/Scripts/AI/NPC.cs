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
    private TMP_Text nameText;
    [SerializeField] bool showIndoctrinateEffect = true;
    private float influenceLevel = 50;
    private PlayerStats player;
    private bool currentlyTalking = false;
    public float GetInfluenceLevel() => influenceLevel;
    public void Start()
    {
        player = GameObject.FindObjectOfType<PlayerStats>();
        currentDialogueTree = initialDialogueTree;
        PopulateDialogueNodes();
        nameText = UIManager.instance.nameText;
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
            responseHandler.SetResponseChosen(false);
            dialogueUI.ShowDialogue(currentDialogueTree.GetCurrentNode().GetDialogueObject());
            yield return new WaitUntil(() => responseHandler.GetResponseChosen() == true);
        }
        while (currentDialogueTree.GetCurrentNode() != null&&currentlyTalking);
    }
  
    public void CheckCorrectRouteTaken()
    {
        if (currentDialogueTree.GetCorrectPathChosen())
        {
            influenceLevel = Mathf.Clamp(influenceLevel + currentDialogueTree.GetNPCInfluenceChange(), 0, 100);
            ShowIndoctrinateEffect();
           
        }
        else
        {
            influenceLevel = Mathf.Clamp(influenceLevel - currentDialogueTree.GetNPCInfluenceChange(), 0, 100);
        }
        PostConversation();
    }
    private void ShowIndoctrinateEffect()
    {
        var effect = Instantiate(indoctrinationPrefab, gameObject.transform.position
            + new Vector3(0, -0.25f, 0), gameObject.transform.rotation);

        if (!showIndoctrinateEffect) { 
            effect.transform.GetChild(0).gameObject.SetActive(false);
            effect.transform.GetChild(1).gameObject.SetActive(false);
            effect.transform.GetChild(2).gameObject.SetActive(false);
        }

    }
    private void PostConversation()
    {
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
