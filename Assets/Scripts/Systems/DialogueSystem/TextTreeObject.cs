using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
[CreateAssetMenu(menuName = "Dialogue/TextTreeObject")]

public class TextTreeObject :ScriptableObject
{
    private int currentID = 1;
    public void Traverse(bool left)
    {
        if (left) currentID *= 2;
        else currentID *= 2 + 1;
    }
    public List<Node> dialogueTree = new List<Node>();
    public Node GetCurrentNode() => dialogueTree.Find(x => x.id == currentID);
}
[System.Serializable]
public class Node
{
    [SerializeField] public int id;
    [SerializeField] DialogueObject dialogueObject;
}