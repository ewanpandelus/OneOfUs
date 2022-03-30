using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text influenceText;


    public void UpdateInfluence(float _influence)
    {
       influenceText.text = ("Influence: " + _influence.ToString());
    }
    /*public void UpdateInfluenceChanceText(float _influenceChance)
    {
       currentInfluenceChanceText.text = ("Current influence Chance: " + _influenceChance.ToString());
    }*/
}
