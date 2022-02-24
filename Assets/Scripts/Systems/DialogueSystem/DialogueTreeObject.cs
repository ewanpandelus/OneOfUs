using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "Dialogue/TextTreeObject")]

public class DialogueTreeObject : ScriptableObject
{

    private int currentID = 1;
    public void Reset()
    {
        currentID = 1;
    }
    public float Traverse(bool left)
    {
        if (left) currentID *= 2;
        else 
        { 
            currentID *= 2;
            currentID += 1;
        }
        return EvaluateHappinessEffect();
    }
    private float EvaluateHappinessEffect()
    {
        if (GetCurrentNode() != null)
        {
            return GetCurrentNode().GetHappinessEffect();
        }
        return 0;
    }
    [SerializeField]
    private List<Node> dialogueTree = new List<Node>();
    public Node GetCurrentNode() => dialogueTree.Find(x => x.GetID() == currentID);
    public List<Node> GetAllNodes() => dialogueTree;
}
