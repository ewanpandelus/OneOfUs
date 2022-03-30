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
    private MiracleEffects miracleEffects;
    bool achievedMiracle = false;
    private bool gameOver = false;
    private NPC highChief;
    private bool miracleEvent = false;
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
        highChief =  GameObject.FindGameObjectsWithTag("NPC").SingleOrDefault(npc => npc.name == "High Chief").GetComponent<NPC>();
    }

    public IEnumerator StartMiracle()
    {
        Time.timeScale = 1f;
        yield return new WaitForSeconds(1f);
        player.SetCanMove(false);
        miracleEffects.BeamLightEffect();
        yield return new WaitForSeconds(1.5f);
        highChief.RunDialogue();
        yield return new WaitUntil(() => !highChief.GetCurrentlyTalking());
        UIManager.ShowMiracleBar(true);
    }
    public void InvokeEffect(Button button)
    {
        StartCoroutine(button.name);
        eventSystem.SetSelectedGameObject(null);
    }
  
    public IEnumerator FireEffect()
    {
        mainCanvas.GetComponent<Canvas>().enabled = false;
        miracleEffects.SetBeamOfLightIntensity(0f);
        rhythmManager.transform.parent.gameObject.SetActive(true);
        GameManager.instance.SetGameState(GameManager.GameState.Rhythm);
        StartCoroutine(rhythmManager.PlayRhythmSet());
        yield return new WaitUntil(() => gameOver == true);
        if (achievedMiracle)
        {
            MiracleAchieved();
        }
        else
        {
            miracleEffects.SetBeamOfLightIntensity(miracleEffects.GetSavedBeamOfLightIntensity());
        }
        ResetAfterMiracle();
    }
    private void MiracleAchieved()
    {
        miracleEffects.DestroyBeam();
        miracleEffects.SetGlobalLightIntensity(1f);
        StartCoroutine(miracleEffects.FireEffect());
        UIManager.ShowMiracleBar(false);
        player.SetCanMove(true);
        StartCoroutine(miracleEffects.FlashOfLight());
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
