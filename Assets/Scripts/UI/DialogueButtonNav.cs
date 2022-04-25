using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DialogueButtonNav : MonoBehaviour
{

    Navigation navSetup = new Navigation();
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject selectedUI;
    [SerializeField] private GameObject soundIcon, soundSlider;
    List<GameObject> buttons;

    public void Start()
    {
        selectedUI.SetActive(false);
        navSetup.mode = Navigation.Mode.Explicit;
    }
    public void AddNavigationToButtons(List<GameObject> _buttons)
    {
        buttons = _buttons;
        if (_buttons.Count == 0) return;

        for (int elem = 0; elem < _buttons.Count; elem++)
        {
            if (elem != _buttons.Count - 1)
            {
                navSetup.selectOnDown = _buttons[elem + 1].GetComponent<Button>();
            }
            if (elem != 0)
            {
                navSetup.selectOnUp = _buttons[elem - 1].GetComponent<Button>();
            }
            _buttons[elem].GetComponent<Button>().navigation = navSetup;
        }
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(_buttons[0]);
    }
    public void NullButtons()
    {
        buttons = null;
    }
    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != null &&
            eventSystem.currentSelectedGameObject !=soundIcon 
            &&eventSystem.currentSelectedGameObject!=soundSlider)
        {
            var _curSelection = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
            selectedUI.SetActive(true);
            selectedUI.GetComponent<RectTransform>().transform.position = _curSelection.transform.position - new Vector3(5,0,0);
        }
        else
        {
            if (buttons != null)
            {
                eventSystem.SetSelectedGameObject(buttons[0]);
                return;
            }

            selectedUI.SetActive(false);
        }
    }
}
