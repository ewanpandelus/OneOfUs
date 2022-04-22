using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIAnimations : MonoBehaviour
{
    [SerializeField] private  RectTransform miracleToolBar;
    [SerializeField] private RectTransform taskList;
    private float elapsedTime = 0;
    private Vector3 toolBarStartPos, toolBarEndPos, taskListStartPos, taskListEndPos;
    private bool taskBarShowing = false;

    private void Start()
    {

        toolBarStartPos = miracleToolBar.transform.localPosition;
        toolBarEndPos = miracleToolBar.transform.localPosition += new Vector3(0, 200, 0);

        taskListStartPos = taskList.transform.localPosition;
        taskListEndPos = taskList.transform.localPosition-= new Vector3(600,0,0);
        ShowHiddenUIElement(false, miracleToolBar, toolBarStartPos, toolBarEndPos,0.01f);
        ShowHiddenUIElement(false, taskList, taskListStartPos, taskListEndPos,0.01f);

    }

    private void Update()
    {
        elapsedTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.J)&&elapsedTime>0.75f&&!UIManager.instance.GetStaticUIShowing())
        {
            ShowHiddenUIElement(!taskBarShowing, taskList, taskListStartPos, taskListEndPos,0.75f);
            taskBarShowing = !taskBarShowing;
            elapsedTime = 0;
            return;
        }
    }



    public void ShowHiddenUIElement(bool _show, RectTransform UIElement, Vector3 startPos, Vector3 endPos, float lerpTime)
    {
        if (_show)
        {
            UIElement.gameObject.SetActive(_show);
            TweenPosition(endPos, lerpTime, UIElement);
        }
        else
        {
            TweenPosition(startPos, lerpTime, UIElement, _show);
        }

    }  


    private void TweenPosition(Vector3 _endPos, float _duration, RectTransform _transform, bool _show)
    {
        _transform.DOLocalMove(_endPos, _duration).OnComplete(()=>_transform.gameObject.SetActive(_show));
    }
    private void TweenPosition(Vector3 _endPos, float _duration, RectTransform _transform)
    {
        _transform.DOLocalMove(_endPos, _duration);
    }

    public bool CheckMiracleBarAllWayOut()
    {
        return miracleToolBar.transform.localPosition.y >toolBarEndPos.y-100 &&
            miracleToolBar.transform.localPosition.y < toolBarEndPos.y + 100;
    }
    public Vector3 GetToolBarStartPos() => toolBarStartPos;
    public Vector3 GetToolBarEndPos() => toolBarEndPos;
    public RectTransform GetToolBar() => miracleToolBar;
    public Vector3 GetTaskListStartPos() => taskListStartPos;
    public Vector3 GetTaskListEndPos() => taskListEndPos;

    public RectTransform GetTaskList() => taskList;
}
