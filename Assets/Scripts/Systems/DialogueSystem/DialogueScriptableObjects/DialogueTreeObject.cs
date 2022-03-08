using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "Dialogue/ThreeOptions/DialogueTreeObject")]

public class DialogueTreeObject : ScriptableObject
{
    private int currentID = 1;
    private float currentInfluenceChance;
    [SerializeField] private float NPCInfluenceChange;

    public void Reset()
    {
        currentID = 1;
    }
    public virtual void Traverse(int _direction)  //Moves through the tree pertaining to player's decisions
    {
        if (_direction == 0 && !GetCurrentNode().LeftChildNull()) { currentID = GetCurrentNode().GetLeftChild().GetID(); return; }
        if (_direction == 1 && !GetCurrentNode().MiddleChildNull()) { currentID = GetCurrentNode().GetMiddleChild().GetID(); return; }
        if (!GetCurrentNode().RightChildNull()) { currentID = GetCurrentNode().GetRightChild().GetID(); }
        else
        {
            currentID = int.MaxValue;
        }
    }
    public List<DialogueNode> dialogueTree = new List<DialogueNode>();
    public DialogueNode GetCurrentNode() => dialogueTree.Find(x => x.GetID() == currentID);


    public List<DialogueNode> GetAllNodes() => dialogueTree;
    public float GetNPCInfluenceChange() => NPCInfluenceChange;
    public float GetCurrentInfluenceChance() => currentInfluenceChance;
    public void UpdateTotalInfluenceChance(float _chanceEffect)
    {
        currentInfluenceChance = Mathf.Clamp(currentInfluenceChance + _chanceEffect, 0, 100);
    }
    public void SetOriginalInfluenceChance(float _influence)
    {
        currentInfluenceChance = _influence;
    }

}

