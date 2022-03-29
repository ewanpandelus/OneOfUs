using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/ThreeOptions/DialogueTreeObject")]
[System.Serializable]
public class DialogueTreeObject : ScriptableObject
{
    private int currentID = 1;
    private float currentInfluenceChance;
    private bool correctPathChosen = false;
    private List<NPC> AffectedNPCs = new List<NPC>();
    [SerializeField] private float NPCInfluenceChange;
    [SerializeField] private List<AffectedNPCS> triggerableNPCS;
    [SerializeField] private bool taskComplete;
    public void IncrementConversationAffectedNPC()
    {
        if (AffectedNPCs.Count > 0 && correctPathChosen&& taskComplete)
        {
            int i = 0;
            foreach (NPC npc in AffectedNPCs)
            {
                if (triggerableNPCS[i].triggeredTree != null)
                {
                    npc.SetDialogueTree(triggerableNPCS[i].triggeredTree);
                }
                i++;
            }
        }
        AffectedNPCs.Clear();
    }
    public void Initialise()
    {
        correctPathChosen = false;
    }
    public void Reset()
    {
        currentID = 1;
        AffectedNPCs.Clear();
        if (triggerableNPCS.Count!=0)
        {
            foreach (AffectedNPCS npcToTrigger in triggerableNPCS)
            {
                AffectedNPCs.Add(GameObject.FindGameObjectsWithTag("NPC").SingleOrDefault(npc => npc.name == npcToTrigger.name).GetComponent<NPC>());
            }
        }
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
    public void SetTaskComplete(bool _taskComplete) => taskComplete = _taskComplete;

}
[System.Serializable]
public struct AffectedNPCS
{
    public string name;
    public DialogueTreeObject triggeredTree;
}

