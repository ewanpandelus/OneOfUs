using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private StatsUI statsUI;
    private float averageInfluence = 0;
    private List<NPC> NPCS = new List<NPC>();
    private void Start()
    {
        statsUI = GetComponent<StatsUI>();
        PopulateNPCList();
        //CalculateInfluence();
    }
    private void PopulateNPCList()
    {
        GameObject[] NPCGameObjects = GameObject.FindGameObjectsWithTag("NPC");
        foreach (var NPC in NPCGameObjects)
        {
            NPCS.Add(NPC.GetComponent<NPC>());
        }
    }
 
    public void CalculateInfluence()
    {
        float avgNPCInfluence = 0;
        foreach(var NPC in NPCS)
        {
            avgNPCInfluence += NPC.GetInfluenceLevel();
        }
 
        averageInfluence = (avgNPCInfluence /= NPCS.Count);
        //
      //  UpdateInfluenceBar();
    }
    public void UpdateInfluenceBar()
    {
        statsUI.UpdateInfluence(Mathf.RoundToInt(averageInfluence));
    }
    /*public void UpdateInfluenceChanceBar(float _influenceChance)
    {
        statsUI.UpdateInfluenceChanceText(Mathf.RoundToInt(_influenceChance));
    }*/
    public float GetInfluence()
    {
        return averageInfluence;
    }
}
