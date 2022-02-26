using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private StatsUI statsUI;
    private float influence = 0;
    private void Start()
    {
        statsUI = GetComponent<StatsUI>();
    }
    public void SetInfluence(float _influence) {
        influence  =Mathf.Clamp(influence + _influence, 0,100 );
        statsUI.UpdateInfluence(influence);
    }
    private void CalculateInfluence()
    {
        //NPC avg happiness
    }
    private void UpdateInfluenceBar()
    {
        //Feed influence to UI 
    }
}
