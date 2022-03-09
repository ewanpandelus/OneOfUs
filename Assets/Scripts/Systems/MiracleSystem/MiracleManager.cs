using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiracleManager : MonoBehaviour
{
    [SerializeField] GameObject fireEffectPrefab;
    [SerializeField] private PlayerController player;
    private MiracleEffects miracleEffects;
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
        //StartCoroutine(miracleEffects.FireEffect());
    }
}
