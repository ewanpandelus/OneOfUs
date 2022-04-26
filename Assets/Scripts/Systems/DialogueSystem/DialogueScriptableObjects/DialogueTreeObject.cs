using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Dialogue/ThreeOptions/DialogueTreeObject")]
[System.Serializable]
public class DialogueTreeObject : ScriptableObject
{
    private int currentID = 1;
    private float currentInfluenceChance;
    private bool correctPathChosen = false;
    private NPC npcAttachedTo;
    private List<NPC> AffectedNPCs = new List<NPC>();
    [SerializeField] private float NPCInfluenceChange;
    [SerializeField] private List<AffectedNPCS> triggerableNPCS;
    [SerializeField] private bool taskComplete = false;
    [SerializeField] private bool invokesMiracle = false;
    [SerializeField] private bool finishesTask = false;
    [SerializeField] private bool storyProgressesOnSameCharacter;
    [SerializeField] private bool overrideProgression;
    [SerializeField] private bool showsEffect = true;
    private bool alreadyOverridden;
    private bool miracleInvoked = false;
    public delegate void FinishedTaskDelegate();
    public event FinishedTaskDelegate finishedTaskEvent;

    public void IncrementConversationAffectedNPC()
    {
        SetupAffectedNPCs();
        if (AffectedNPCs.Count > 0 && correctPathChosen && taskComplete)
        {
            int i = 0;
            foreach (NPC npc in AffectedNPCs)
            {
                if (triggerableNPCS[i].triggeredTree != null)
                {
                    if (overrideProgression && i == 0) { i++; continue;}
                    npc.SetDialogueTree(triggerableNPCS[i].triggeredTree);
                    if(npc!= npcAttachedTo||storyProgressesOnSameCharacter)
                    {
                        npc.StartCoroutine(npc.SetQuestionMarksActive());
                    }
                    triggerableNPCS[i].triggeredTree.Initialise();
              
                }
                i++;
            }
        }
        PostConversationEvents();
    }
    private void PostConversationEvents()
    {
        InvokeMiracle();
        FinishesTask();
        if (overrideProgression&&!alreadyOverridden) OverrrideConversationProgression();
        AffectedNPCs.Clear();
    }
    private void OverrrideConversationProgression()
    {
        if (AffectedNPCs.Count > 0 && triggerableNPCS[0].triggeredTree != null)
        {
            AffectedNPCs[0].SetDialogueTree(triggerableNPCS[0].triggeredTree);
            AffectedNPCs[0].StartCoroutine(AffectedNPCs[0].SetQuestionMarksActive());
            triggerableNPCS[0].triggeredTree.Initialise();
            alreadyOverridden = true;
        }
    }
    private void InvokeMiracle()
    {
        if (invokesMiracle && !miracleInvoked)
        {
            miracleInvoked = true;
            GameObject.FindGameObjectWithTag("MiracleManager").GetComponent<MiracleManager>().StartCoroutine("StartMiracle");
        }
    }
    private void FinishesTask()
    {
        if (finishesTask)
        {
            finishedTaskEvent?.Invoke();
        }
    }
    private void SetupAffectedNPCs()
    {
        AffectedNPCs.Clear();
        if (triggerableNPCS.Count != 0)
        {
            foreach (AffectedNPCS npcToTrigger in triggerableNPCS)
            {
                AffectedNPCs.Add(GameObject.FindGameObjectsWithTag("NPC").SingleOrDefault(npc => npc.name == npcToTrigger.name).GetComponent<NPC>());
            }
        }
    }
    public void Initialise()
    {
        correctPathChosen = false;
        miracleInvoked = false;
        alreadyOverridden = false;
        AssessRightPath();
    }
    public void Reset()
    {
        currentID = 1;
    }
    public virtual void Traverse(int _direction)  //Moves through the tree based on player's decisions
    {
        if (_direction == 0 && !GetCurrentNode().LeftChildNull()) { currentID = GetCurrentNode().GetLeftChild().GetID(); return; }
        if (_direction == 1 && !GetCurrentNode().MiddleChildNull()) { currentID = GetCurrentNode().GetMiddleChild().GetID(); return; }
        if (!GetCurrentNode().RightChildNull()) { currentID = GetCurrentNode().GetRightChild().GetID(); }
        else
        {
            currentID = int.MaxValue;
        }
    }
    public void AssessRightPath()
    {
        if (GetCurrentNode() != null)
        {
            correctPathChosen = GetCurrentNode().GetCorrectChoice();
        }
  
    }
    public List<DialogueNode> dialogueTree = new List<DialogueNode>();
    public DialogueNode GetCurrentNode() => dialogueTree.Find(x => x.GetID() == currentID);
    public List<DialogueNode> GetAllNodes() => dialogueTree;
    public float GetNPCInfluenceChange() => NPCInfluenceChange;
    public float GetCurrentInfluenceChance() => currentInfluenceChance;
    public void UpdateTotalInfluenceChance(float _chanceEffect)=> currentInfluenceChance = Mathf.Clamp(currentInfluenceChance + _chanceEffect, 0, 100);
    public void SetOriginalInfluenceChance(float _influence) => currentInfluenceChance = _influence;
    public void SetCorrectPathChosen(bool _correctPathChosen) => correctPathChosen = _correctPathChosen;
    public bool GetCorrectPathChosen() => correctPathChosen;
    public void ResetTaskComplete() => taskComplete = false;
    public void SetNPCJoined() => npcAttachedTo.StartCoroutine(npcAttachedTo.SetIndoctrinated());
    public void SetNPCAttachedTo(NPC _npc)=> npcAttachedTo = _npc;
    public bool GetShowsEffect() => showsEffect;
    public bool GetFinishesTask() => finishesTask;
    public void SetTaskComplete(bool _taskComplete) 
    {
        taskComplete = _taskComplete;
        if (correctPathChosen)
        {
            IncrementConversationAffectedNPC();
        }
    }
    
    
 

}
[System.Serializable]
public struct AffectedNPCS
{
    public string name;
    public DialogueTreeObject triggeredTree;
}
    



