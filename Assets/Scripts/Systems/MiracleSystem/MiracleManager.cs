using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class MiracleManager : MonoBehaviour
{
    [SerializeField] GameObject fireEffectPrefab;
    [SerializeField] private PlayerController player;
    [SerializeField] private RhythmManager rhythmManager;
    [SerializeField] private GameObject mainCanvas;
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
        miracleEffects.SetupMiracleEffects(player.GetPlayerAnimator(), fireEffectPrefab, player);

    }
    public void InvokeEffect(Button button)
    {
        StartCoroutine(button.name);
    }
  
    public IEnumerator FireEffect()
    {
        mainCanvas.GetComponent<Canvas>().enabled = false;
        rhythmManager.transform.parent.gameObject.SetActive(true);
        StartCoroutine(rhythmManager.PlayRhythm(0.8f, 1.4f));
        yield return new WaitUntil(() => gameOver == true);
        if (achievedMiracle)
        {
            StartCoroutine(miracleEffects.FireEffect());
        }
        gameOver = false;
        achievedMiracle = false;
        rhythmManager.transform.parent.gameObject.SetActive(false);
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
