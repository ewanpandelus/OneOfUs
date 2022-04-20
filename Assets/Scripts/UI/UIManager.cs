using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
public class UIManager : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] MapUI mapUI;
    [SerializeField] MiracleManager miracleManager;
    [SerializeField] UIAnimations UIAnimations;
    [SerializeField] public TMP_Text nameText;
    [SerializeField] RectTransform bottomEndScreen, topEndScreen;
    [SerializeField] TMP_Text endText, endButtonText;
    [SerializeField] private Color finishColor;
    public static UIManager instance;
   
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        bottomEndScreen.transform.position += new Vector3(0, -600, 0);
        topEndScreen.transform.position += new Vector3(0, 600, 0);
        EndScreen();
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

    public void EndScreen()
    {
        bottomEndScreen.DOMoveY(540, 2f).SetEase(Ease.OutQuad);
        topEndScreen.DOMoveY(540, 2f).SetEase(Ease.OutQuad).OnComplete(()=>endText.DOColor(finishColor,1).OnComplete(()=>endButtonText.DOColor(finishColor,1)));
    }
}   
