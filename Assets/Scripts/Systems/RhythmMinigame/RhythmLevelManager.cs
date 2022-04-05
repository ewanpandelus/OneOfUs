using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class RhythmLevelManager : MonoBehaviour
{
    [SerializeField] List<RhythmLevel> level1 = new List<RhythmLevel>();
    [SerializeField] List<RhythmLevel> level2 = new List<RhythmLevel>();

    private List<List<RhythmLevel>> levelArray = new List<List<RhythmLevel>>();
    public List<RhythmLevel> GetLevel1() => level1;
    public List<RhythmLevel> GetCurrentLevel(int _currentLevel) => levelArray[_currentLevel - 1];

    private void Awake()
    {
        levelArray.Add(level1);
        levelArray.Add(level2);
    }
}

[System.Serializable]
public struct RhythmLevel
{
    public string noteName;
    public float timeToWait;
    public bool isLong;
    public bool slowEffect;
}