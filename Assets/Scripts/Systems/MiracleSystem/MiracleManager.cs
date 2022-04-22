using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiracleManager : MonoBehaviour
{
    [SerializeField] GameObject fireEffectPrefab;
    [SerializeField] private PlayerController player;
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject chiefLightEffect;
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] GameObject retryText;
    public delegate void MiracleEventDelegate();
    public event MiracleEventDelegate miracleEvent;
    private MiracleEffects miracleEffects;
    bool achievedMiracle = false;
    private bool gameOver = false;
    private NPC highChief;
    private List<MiracleButton> miracleButtons = new List<MiracleButton>();
    private Button currentButton;
    public enum MiracleType
    {
        Fire,
        UnburnToast,
        TurnIntoSheep
    }

    void Start()
    {
        miracleEffects = GetComponent<MiracleEffects>();
        miracleEffects.SetupMiracleEffects(player.GetPlayerAnimator(), fireEffectPrefab, player, chiefLightEffect);
        highChief = GameObject.FindGameObjectsWithTag("NPC").SingleOrDefault(npc => npc.name == "High Chief").GetComponent<NPC>();
        var miracleBarObj = GameObject.FindGameObjectWithTag("MiracleToolBar");
        for (int i = 0; i < miracleBarObj.transform.childCount; i++)
        {
            miracleButtons.Add(miracleBarObj.transform.GetChild(i).GetComponent<MiracleButton>());
        }
        miracleButtons[0].SetActive(true);
    }

    public IEnumerator StartMiracle()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);
        player.SetCanMove(false);
        miracleEffects.BeamLightEffect();
        yield return new WaitForSeconds(3);
        highChief.RunDialogue();
        dialogueUI.SetFontStyle(TMPro.FontStyles.Italic);
        yield return new WaitUntil(() => !highChief.GetCurrentlyTalking());
        UIManager.ShowMiracleBar(true);
    }
    public void InvokeEffect(Button button)
    {
        currentButton = button;
        if (button.gameObject.GetComponent<MiracleButton>().GetActive())
        {
            StartCoroutine(button.name);
            eventSystem.SetSelectedGameObject(null);
        }
    }
    public IEnumerator SheepEffect()
    {
        PreMiracleSetup();
        yield return new WaitUntil(() => gameOver == true);
        PostMiracleCheck(MiracleType.TurnIntoSheep);
    }
    public IEnumerator FireEffect()
    {
        PreMiracleSetup();
        yield return new WaitUntil(() => gameOver == true);
        PostMiracleCheck(MiracleType.Fire);
    }
    private void PreMiracleSetup()
    {
        mainCanvas.GetComponent<Canvas>().enabled = false;
        miracleEffects.SetBeamOfLightIntensity(0f);
        rhythmManager.transform.parent.gameObject.SetActive(true);
        GameManager.instance.SetGameState(GameManager.GameState.Rhythm);
        StartCoroutine(rhythmManager.PlayRhythmSet());
    }
    private void PostMiracleCheck(MiracleType miracleType)
    {
        if (achievedMiracle)
        {
            MiracleAchieved(miracleType);
            dialogueUI.SetFontStyle(TMPro.FontStyles.Normal);
            retryText.SetActive(false);
            eventSystem.SetSelectedGameObject(null);
        }
        else
        {
            miracleEffects.SetBeamOfLightIntensity(miracleEffects.GetSavedBeamOfLightIntensity());
            retryText.SetActive(true);
            eventSystem.SetSelectedGameObject(currentButton.gameObject);
        }
        ResetAfterMiracle();
    }
    private void MiracleAchieved(MiracleType miracleType)
    {
        miracleEffects.DestroyBeam();
        miracleEffects.SetGlobalLightIntensity(1f);
        if(miracleType == MiracleType.Fire)
        {
            StartCoroutine(miracleEffects.FireEffect());
        }
        if(miracleType == MiracleType.TurnIntoSheep)
        {
            miracleEffects.SheepEffect();
        }
        UIManager.ShowMiracleBar(false);
        StartCoroutine(miracleEffects.FlashOfLight());
        miracleEvent?.Invoke();
        rhythmManager.IncreaseLevel();
        miracleButtons[1].SetActive(true);
    }
    private void ResetAfterMiracle()
    {
        gameOver = false;
        achievedMiracle = false;
        rhythmManager.transform.parent.gameObject.SetActive(false);
        GameManager.instance.SetGameState(GameManager.GameState.Standard);
        mainCanvas.GetComponent<Canvas>().enabled = true;
    }

    public void SetAchievedMiracle(bool _achievedMiracale)
    {
        gameOver = true;
        achievedMiracle = _achievedMiracale;
    }

    public bool GetRhythmGameActive()
    {
        return rhythmManager.transform.parent.gameObject.activeInHierarchy;
    }
}
