using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class SliderUI : MonoBehaviour, UnityEngine.EventSystems.IPointerExitHandler
{
    [SerializeField] private EventSystem eventSystem;
    public void OnPointerExit(PointerEventData eventData)
    {
        eventSystem.SetSelectedGameObject(null);
    }

    
}
