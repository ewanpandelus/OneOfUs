using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private DialogueUI dialogueUI;
    [SerializeField] private MapUI mapUI;
    [SerializeField] private MiracleManager miracleManager;
    [SerializeField] private UIAnimations UIAnimations;
    [SerializeField] public TMP_Text nameText;
    [SerializeField] private Image endScreen, endLogo;
    [SerializeField] private TMP_Text endText, endButtonText;
    [SerializeField] private Color finishColor;
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject sheepEffectButton;
    [SerializeField] private GameObject endButton;
    [SerializeField] private GameObject volumeSlider;
    private Slider volSlider;
    
    public static UIManager instance;
   
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        volSlider = volumeSlider.GetComponent<Slider>();
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

    public IEnumerator EndScreenFade()
    {
        UIAnimations.EndEffects();
        yield return new WaitForSeconds(8f);
        
        endScreen.DOColor(new Color(0, 0, 0, 1), 3f).OnComplete(() => endLogo.DOColor(new Color(1, 0, 0, 1), 1).OnComplete(() => endText.DOColor(finishColor, 1).OnComplete(() => endButtonText.DOColor(finishColor, 1))));
        endButton.SetActive(true);

    }
    public void ToggleVolumeSlider()
    {
        volumeSlider.SetActive(!volumeSlider.activeInHierarchy);
    }
    public void ChangeVolume()
    {
        SoundManager.instance.ChangeVolume(1-volSlider.value);
    }
}   
