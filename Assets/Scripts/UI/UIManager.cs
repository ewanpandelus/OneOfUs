using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] MapUI mapUI;
    [SerializeField] MiracleManager miracleManager;
    [SerializeField] UIAnimations UIAnimations;
   public bool GetStaticUIShowing()
   {
        return (dialogueUI.GetDialogueBoxShowing() || mapUI.GetMapOpen() || miracleManager.GetRhythmGameActive());
   }
   public bool GetDynamicUIShowing()
   {
        return UIAnimations.GetToolBarShowing();
   }
}   
