using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> tasks = new List<Task>();
    [SerializeField] private List<DialogueTreeObject> treesRelatedToTasks = new List<DialogueTreeObject>();
    [SerializeField] private GameObject taskListUI;
    [SerializeField] Inventory inventory;
    [SerializeField] MiracleManager miracleManager;

    private int currentTask = 0;
    private List<Func<bool>> taskChecks = new List<Func<bool>>();
    private void Start()
    {
        foreach(DialogueTreeObject tree in treesRelatedToTasks)
        {
            tree.SetCorrectPathChosen(false);
        }
        inventory.collectedWheatEvent += ArbitraryTaskComplete;
        miracleManager.miracleEvent += ArbitraryTaskComplete;

        for(int i = 0; i < taskListUI.transform.childCount; i++)
        {
            taskListUI.transform.GetChild(i).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = tasks[i].title;
            taskListUI.transform.GetChild(i).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = tasks[i].description;
        }
    }
    private void PopulateTaskListRequirements()
    {
        taskChecks.Add(()=>inventory.CheckInventoryContainsAmountOfItem("Wheat", 6)); //Task1
                                                                                      //  taskChecks.Add(() => CheckCandlesLit(); //Task2
                                                                                      //taskChecks.Add(() => inventory.CheckInventoryContainsAmountOfItem("Money", 30)); //Task3
                                                                                      //taskChecks.Add(() => SheepNPC.happy = true; //
    }
    private void ArbitraryTaskComplete()
    {
        switch (currentTask)
        {
            case 0:
                Task1Complete();
                break;
            case 1:
                Task2Complete();
                break;
        }
        currentTask++;
    }
    private void Task1Complete()
    {
        inventory.collectedWheatEvent -= Task1Complete;
        treesRelatedToTasks[0].SetTaskComplete(true);
    }
    private void Task2Complete()
    {
        treesRelatedToTasks[1].SetTaskComplete(true);
    }
    public bool CheckLevelComplete(int _task)
    {
        return CheckRequirements(taskChecks[_task - 1]);
    }
    public bool CheckRequirements(Func<bool> taskCheck)
    {
        return taskCheck();
    }
    [System.Serializable]
    public struct Task
    {
        public string title;
        public string description;
    }
    public void OnApplicationQuit()
    {
        treesRelatedToTasks[0].ResetTaskComplete();
    }
}
