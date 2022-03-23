using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
[CreateAssetMenu(menuName = "Dialogue/ThreeOptions/DialogueNode")]
public class DialogueNode : ScriptableObject
{
    [SerializeField] private int id;
<<<<<<< Updated upstream
    [SerializeField] private bool correctChoice = false;
=======
    [SerializeField] private bool correctDialogueOption;
>>>>>>> Stashed changes
   // [SerializeField] private DialogueObject dialogueObject;
    [SerializeField] private DialogueNode leftChild;
    [SerializeField] private DialogueNode middleChild;
    [SerializeField] private DialogueNode rightChild;
    [SerializeField] private float chanceEffect;
    private NPC associatedNPC;
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] [TextArea] private string[] response = new string[2]; //2 defines the max number of reponses - can be increased later
    private UnityEvent funcToRun;
    public string[] GetDialogue() => dialogue;
    public string[] GetResponses() => response;
    public bool ResponsesExist() => response.Length != 0;
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
    public bool MiddleChildNull() => middleChild == null;
    public bool RightChildNull() => rightChild == null;

    public bool AllChildrenNull() => LeftChildNull() && MiddleChildNull() && RightChildNull();
    public DialogueNode GetLeftChild() => leftChild;
    public DialogueNode GetMiddleChild() => middleChild;
    public DialogueNode GetRightChild() => rightChild;
    public float GetChanceEffect() => chanceEffect;
    public bool GetCorrectChoice() => correctChoice;



}