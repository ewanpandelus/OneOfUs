using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    private NPC associatedNPC;
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] [TextArea] private string[] response = new string[2]; //2 defines the max number of reponses - can be increased later
    public string[] GetDialogue => dialogue;
    public string[] GetResponses => response;
    public void SetAssociatedNPC(NPC _NPC)
    {
        associatedNPC = _NPC;
    }
    public NPC GetAssociatedNPC() => associatedNPC;
} 
