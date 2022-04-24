using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject sheepEffectButton;
    [SerializeField] Image startScreen;
    public static UIManager instance;
   
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        bottomEndScreen.transform.position += new Vector3(0, -600, 0);
        topEndScreen.transform.position += new Vector3(0, 600, 0);
        StartCoroutine(LerpAlpha());

    }
   public bool GetStaticUIShowing()=>(dialogueUI.GetDialogueBoxShowing() || mapUI.GetMapOpen() || miracleManager.GetRhythmGameActive());
   public bool CheckMiracleToolBarAllWayOut()=>  UIAnimations.CheckMiracleBarAllWayOut();
   
   public bool DialogueBoxShowing() => dialogueUI.GetDialogueBoxShowing();

    public bool MapShowing() => mapUI.GetMapOpen();
    public void ShowMiracleBar(bool _show)
   {
        UIAnimations.ShowHiddenUIElement(_show, UIAnimations.GetToolBar(), UIAnimations.GetToolBarStartPos(), UIAnimations.GetToolBarEndPos(), 0.75f);
        eventSystem.SetSelectedGameObject(sheepEffectButton);
   }

    public void EndScreen()
    {
 
        bottomEndScreen.DOMoveY(540, 2f).SetEase(Ease.OutQuad);
        topEndScreen.DOMoveY(540, 2f).SetEase(Ease.OutQuad).OnComplete(()=>endText.DOColor(finishColor,1).OnComplete(()=>endButtonText.DOColor(finishColor,1)));
        SoundManager.instance.SetPitch(0.7f);
    }
    public IEnumerator LerpAlpha()
    {

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime;
            startScreen.color = new Color(0, 0, 0, 1-t);
            yield return null;
        }
       
    }
}   
