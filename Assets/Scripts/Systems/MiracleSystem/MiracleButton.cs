using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class MiracleButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField] private UIManager UIManager;
    [SerializeField] TMP_Text tipText;
    [SerializeField] string inactiveTip;
    [SerializeField] string tip;
    [SerializeField] EventSystem eventSystem;
    private bool active;
    private bool mouseOver = false;
    void Start()
    {
        tipText.text = "";
    }
    void Update()
    {
        if (eventSystem.currentSelectedGameObject == gameObject)
        {
            if (active) tipText.text = tip;
            else tipText.text = inactiveTip;
        }
        else
        {
            if(!mouseOver) tipText.text = "";
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        return;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseOver = true;
        if (UIManager.CheckMiracleToolBarAllWayOut())
        {
            if (active) tipText.text = tip;
            else tipText.text = inactiveTip;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        tipText.text = "";
        mouseOver = false;
    }
    public void SetActive(bool _active) => active = _active;

    public bool GetActive() => active;
}
