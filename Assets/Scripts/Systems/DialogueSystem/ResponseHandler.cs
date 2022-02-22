using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform repsonseBox;
    [SerializeField] private RectTransform repsonseButtonTemplate;
    [SerializeField] private RectTransform repsonseContainer;
    private bool responseChosen = false;
    public bool GetResponseChosen() => responseChosen;
    public void SetResponseChosen(bool _responseChosen) => responseChosen = _responseChosen;

    public void ShowReponses(DialogueObject _dialogueObject)
    {
        float responseBoxHeight = 0;
        int index = 0;
        List<GameObject> responseButtons = new List<GameObject>();
        foreach (string response in _dialogueObject.GetResponses)
        {
            GameObject responseButton = Instantiate(repsonseButtonTemplate.gameObject, repsonseContainer);
            responseButtons.Add(responseButton);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response;
            if (index == 0) { responseButton.GetComponent<Button>().onClick.AddListener(() => ChooseResponseTrue(_dialogueObject, responseButtons)); }
            else { responseButton.GetComponent<Button>().onClick.AddListener(() => ChooseResponseFalse(_dialogueObject, responseButtons)); }
            index++;
            responseBoxHeight += repsonseButtonTemplate.sizeDelta.y;
            repsonseBox.sizeDelta = new Vector2(repsonseBox.sizeDelta.x, responseBoxHeight);
            repsonseBox.gameObject.SetActive(true);
        }
    }
    public void ChooseResponseFalse(DialogueObject _dialogueObject, List<GameObject> _responseButtons)
    {
        _dialogueObject.GetAssociatedNPC().MakeDecision(false);
        responseChosen = true;
        CleanUp(_responseButtons);
    }
    public void ChooseResponseTrue(DialogueObject _dialogueObject, List<GameObject> _responseButtons)
    {
        _dialogueObject.GetAssociatedNPC().MakeDecision(true);
        responseChosen = true;
        CleanUp(_responseButtons);
    }
    private void CleanUp(List<GameObject> _responseButtons)
    {
        foreach (GameObject response in _responseButtons)
        {
            Destroy(response);
        }
        repsonseBox.gameObject.SetActive(false);
    }
   
}
