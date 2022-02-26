using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(menuName = "Dialogue/ThreeOptions/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    [SerializeField] private int id;
   // [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueNode leftChild;
    [SerializeField] private DialogueNode middleChild;
    [SerializeField] private DialogueNode rightChild;
    [SerializeField] private float happinessEffect;
    private NPC associatedNPC;
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] [TextArea] private string[] response = new string[2]; //2 defines the max number of reponses - can be increased later
    private UnityEvent funcToRun;
    public string[] GetDialogue => dialogue;
    public string[] GetResponses => response;
    public void SetAssociatedNPC(NPC _NPC)
    {
        associatedNPC = _NPC;
    }
    public void SetFuncToRun(UnityEvent _func)
    {
        funcToRun = _func;
    }
    public NPC GetAssociatedNPC() => associatedNPC;
    public int GetID() => id;
    public DialogueNode GetDialogueObject() => this;
    public bool LeftChildNull() => leftChild == null;
    public bool MiddleChildNull() => leftChild == null;
    public bool RightChildNull() => leftChild == null;


    public DialogueNode GetLeftChild() => leftChild;
    public DialogueNode GetMiddleChild() => middleChild;
    public DialogueNode GetRightChild() => rightChild;
    public float GetHappinessEffect() => happinessEffect;



}