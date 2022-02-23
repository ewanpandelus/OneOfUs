using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "Dialogue/DialogueTreeBaseObject")]

public class DialogueTreeBaseObject : ScriptableObject
{
    public int currentID = 1;
    public void Reset()
    {
        currentID = 1;
    }
    public virtual void Traverse(int _direction)  //Moves through the tree pertaining to player's decisions
    {
        if (_direction == 0) currentID *= 2;
        else
        {
            currentID *= 2;
            currentID += 1;
        }

    }
    public List<Node> dialogueTree = new List<Node>();
    public Node GetCurrentNode() => dialogueTree.Find(x => x.GetID() == currentID);

    
    public List<Node> GetAllNodes() => dialogueTree;
}

