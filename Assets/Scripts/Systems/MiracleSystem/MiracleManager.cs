using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MiracleManager : MonoBehaviour
{
    [SerializeField] GameObject fireEffectPrefab;
    [SerializeField] private PlayerController player;
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject chiefLightEffect;
    private MiracleEffects miracleEffects;
    bool achievedMiracle = false;
    private bool gameOver = false;
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
    }

    public void StartMiracle()
    {
        miracleEffects.BeamLightEffect();
    }
    public void InvokeEffect(Button button)
    {
        StartCoroutine(button.name);
        eventSystem.SetSelectedGameObject(null);
    }
  
    public IEnumerator FireEffect()
    {
        mainCanvas.GetComponent<Canvas>().enabled = false;
        rhythmManager.transform.parent.gameObject.SetActive(true);
        GameManager.instance.SetGameState(GameManager.GameState.Rhythm);
        StartCoroutine(rhythmManager.PlayRhythmSet());
        yield return new WaitUntil(() => gameOver == true);
        if (achievedMiracle)
        {
            StartCoroutine(miracleEffects.FireEffect());
        }
        ResetAfterMiracle();

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
