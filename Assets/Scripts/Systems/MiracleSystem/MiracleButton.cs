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
    [SerializeField] string tip;
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
        if(UIManager.CheckMiracleToolBarAllWayOut())
        tipText.text = tip;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tipText.text = "";

    }
}
