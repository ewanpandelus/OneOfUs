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
    private bool active;
    void Start()
    {
        tipText.text = "";
    }
 
    public void OnPointerClick(PointerEventData eventData)
    {
        return;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UIManager.CheckMiracleToolBarAllWayOut())
        {
            if (active) tipText.text = tip;
            else tipText.text = inactiveTip;
        }
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        tipText.text = "";
    }
    public void SetActive(bool _active) => active = _active;

    public bool GetActive() => active;
}
