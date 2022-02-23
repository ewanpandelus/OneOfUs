using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
[CreateAssetMenu(menuName = "Dialogue/DialogueNode")]
public class Node 
{
    [SerializeField] private int id;
    [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private Node leftChild;
    [SerializeField] private Node middleChild;
    [SerializeField] private Node rightChild;
    [SerializeField] private float happinessEffect;
    public int GetID() => id;
    public DialogueObject GetDialogueObject() => dialogueObject;
    public Node GetLeftChild() => leftChild;
    public Node GetMiddleChild() => middleChild;
    public Node GetRightChild() => rightChild;
    public float GetHappinessEffect() => happinessEffect;



}