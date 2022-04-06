using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] MapUI mapUI;
    [SerializeField] MiracleManager miracleManager;
    [SerializeField] UIAnimations UIAnimations;
    [SerializeField] public TMP_Text nameText;
    public static UIManager instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }
   public bool GetStaticUIShowing()
   {
        return (dialogueUI.GetDialogueBoxShowing() || mapUI.GetMapOpen() || miracleManager.GetRhythmGameActive());
   }
    
   public bool CheckMiracleToolBarAllWayOut()
   {
        return UIAnimations.CheckMiracleBarAllWayOut();
   }
    public void ShowTaskList(bool _show)
    {

    }
   public void ShowMiracleBar(bool _show)
    {
        UIAnimations.ShowHiddenUIElement(_show, UIAnimations.GetToolBar(), UIAnimations.GetToolBarStartPos(), UIAnimations.GetToolBarEndPos());
    }
}   
