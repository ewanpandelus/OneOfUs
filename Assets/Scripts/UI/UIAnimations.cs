using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class UIAnimations : MonoBehaviour
{
    [SerializeField] private  RectTransform miracleToolBar;
    [SerializeField] UIManager UIManager;
    private Vector3 toolBarStartPos, toolBarEndPos;
    private bool toolBarShowing = false;

    private void Start()
    {
        toolBarStartPos = miracleToolBar.transform.localPosition;
        toolBarEndPos = miracleToolBar.transform.localPosition += new Vector3(0, 200, 0);
    }




    void Update()
    {
        toolBarShowing = true;
        if (CheckToolBarShow(Input.mousePosition) && miracleToolBar.transform.position != toolBarEndPos&&!UIManager.GetStaticUIShowing())
        {
            TweenPosition(toolBarEndPos, 0.75f, miracleToolBar);
        }
        else if (miracleToolBar.transform.position!= toolBarStartPos)
        {
            toolBarShowing = false;
            TweenPosition(toolBarStartPos, 0.75f, miracleToolBar);
        }
    }
           

    private bool CheckToolBarShow(Vector3 _mousePos)
    {
        return(_mousePos.y < (Screen.height / 9) && _mousePos.x > (Screen.width / 2) - Screen.width/4 && _mousePos.x < (Screen.width / 2) +(Screen.width/4));
       
    }
    private void TweenPosition(Vector3 _endPos, float _duration, RectTransform _transform)
    {
        _transform.DOLocalMove(_endPos, _duration);
    }
    public bool GetToolBarShowing()
    {
        return toolBarShowing;
    }
}
