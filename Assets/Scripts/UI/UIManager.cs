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
   public bool CheckMiracleToolBarAllWayOut()
   {
        return UIAnimations.CheckMiracleBarAllWayOut();
   }
   public void ShowMiracleBar(bool _show)
    {
        UIAnimations.ShowMiracleBar(_show);
    }
}   
