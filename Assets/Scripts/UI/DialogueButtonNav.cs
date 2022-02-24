using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DialogueButtonNav : MonoBehaviour
{

    Navigation navSetup = new Navigation();
    [SerializeField] EventSystem eventSystem;
    [SerializeField] GameObject selectedUI;

    public void Start()
    {
        selectedUI.SetActive(false);
        navSetup.mode = Navigation.Mode.Explicit;
    }
    public void AddNavigationToButtons(List<GameObject> _buttons)
    {

        for(int elem = 0; elem<_buttons.Count; elem++)
        {
            if (elem != _buttons.Count - 1)
            {
                navSetup.selectOnDown = _buttons[elem + 1].GetComponent<Button>();
            }
            if (elem!=0)
            {
                navSetup.selectOnUp =  _buttons[elem -1].GetComponent<Button>();
            }
            _buttons[elem].GetComponent<Button>().navigation = navSetup;
        }
        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(_buttons[0]);
    }
    private void Update()
    {
        if (eventSystem.currentSelectedGameObject != null)
        {
            var _curSelection = eventSystem.currentSelectedGameObject.GetComponent<RectTransform>();
            selectedUI.SetActive(true);
            selectedUI.GetComponent<RectTransform>().transform.position = _curSelection.transform.position - new Vector3(_curSelection.rect.width/1.5f, 0, 0);
        }
        else 
        {
            selectedUI.SetActive(false);
        }
    }
}