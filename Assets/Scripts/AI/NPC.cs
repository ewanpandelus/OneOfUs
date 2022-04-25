using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class NPC : MonoBehaviour
{
    [SerializeField] private ResponseHandler responseHandler;
    [SerializeField] DialogueTreeObject initialDialogueTree;
    DialogueTreeObject currentDialogueTree;
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private GameObject indoctrinationPrefab;
    [SerializeField] private string name;
    [SerializeField] private GameObject qMark, qMarkMap;
    private Animator animator;
    private TMP_Text nameText;
    [SerializeField] bool showIndoctrinateEffect = true;
    private float influenceLevel = 50;
    private bool currentlyTalking = false;
    public float GetInfluenceLevel() => influenceLevel;
    public void Start()
    {
        animator = GetComponent<Animator>();
        currentDialogueTree = initialDialogueTree;
        currentDialogueTree.SetNPCAttachedTo(this);
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
        currentlyTalking = true;
        nameText.text = name;
      
    }
    public IEnumerator RunThroughDialogueTree()
    {
        if (currentDialogueTree.GetCurrentNode() == null|| currentDialogueTree.GetCurrentNode().AllChildrenNull()) 
        {
            currentDialogueTree.Reset(); 
      
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
            qMark.SetActive(false);
            qMarkMap.SetActive(false);
            if (currentDialogueTree.GetShowsEffect()) { ShowIndoctrinateEffect(); }
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
        _dialogueTree.SetNPCAttachedTo(this);
        PopulateDialogueNodes();
    }
   public IEnumerator SetQuestionMarksActive()
    {
        yield return new WaitForSeconds(1f);
        qMark.SetActive(true);
        qMarkMap.SetActive(true);
    }
    public bool GetCurrentlyTalking() => currentlyTalking;
    public IEnumerator SetIndoctrinated()
    {
        yield return new WaitForSeconds(1f);
        SpriteRenderer rend = GetComponent<SpriteRenderer>();
        rend.DOColor(new Color(1, 1, 1, 0), 0.25f);
        yield return new WaitForSeconds(0.25f);
        animator.SetTrigger("JoinedCult");
        rend.DOColor(Color.white, 0.4f);
    
    }
}
