using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> tasks = new List<Task>();
    [SerializeField] private List<DialogueTreeObject> treesTriggeredBySubTasks = new List<DialogueTreeObject>();
    [SerializeField] private List<DialogueTreeObject> recruitingTrees = new List<DialogueTreeObject>();

    [SerializeField] private GameObject taskListUI;
    [SerializeField] Inventory inventory;
    [SerializeField] MiracleManager miracleManager;

    private int currentTask, currentSubTask = 0;

    private void Start()
    {
        foreach(DialogueTreeObject tree in recruitingTrees)
        {
            tree.finishedTaskEvent += FullTaskComplete;
        }

        foreach (DialogueTreeObject tree in treesTriggeredBySubTasks)
        {
            tree.SetCorrectPathChosen(false);
        }
        inventory.collectedWheatEvent += SubTaskComplete;
        miracleManager.miracleEvent += SubTaskComplete;

        for(int i = 0; i < taskListUI.transform.childCount; i++)
        {
            taskListUI.transform.GetChild(i).transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>().text = tasks[i].title;
            taskListUI.transform.GetChild(i).transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>().text = tasks[i].description;
        }
    }
    private void FullTaskComplete()
    {
        recruitingTrees[currentTask].finishedTaskEvent -= FullTaskComplete;
        TaskComplete(currentTask);
        currentTask++;
    }
    private void SubTaskComplete()
    {
        treesTriggeredBySubTasks[currentSubTask].SetTaskComplete(true);
        switch (currentSubTask)
        {   

            case 0:
                inventory.collectedWheatEvent -= SubTaskComplete;
                break;
            case 1:
                miracleManager.miracleEvent -= SubTaskComplete;
                break;
        }
        currentSubTask++;
    }
    private void TaskComplete(int taskComplete)
    {
      
        taskListUI.transform.GetChild(taskComplete).transform.GetChild(2).gameObject.SetActive(true);
        StartCoroutine(PostProcessManager.instance.IncreaseDarkeningWeight());
    }

  
    [System.Serializable]
    public struct Task
    {
        public string title;
        public string description;
        public bool complete;

        public void SetComplete(bool _complete)
        {
            complete = _complete;
        }
    }
    public void OnApplicationQuit()
    {
        foreach(DialogueTreeObject tree in treesTriggeredBySubTasks)
        {
            tree.ResetTaskComplete();
        }
    }
}
