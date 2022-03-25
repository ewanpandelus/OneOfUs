using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RhythmLevelManager : MonoBehaviour
{
    [SerializeField] List<RhythmLevel> level1 = new List<RhythmLevel>();
    public List<RhythmLevel> GetLevel1() => level1;
}

[System.Serializable]
public struct RhythmLevel
{
    public string noteName;
    public float timeToWait;
    public bool isLong;
}