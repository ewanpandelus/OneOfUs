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
    private DialogueButtonNav dialogueButtonNav;

    private void Start()
    {
        dialogueButtonNav = GetComponent<DialogueButtonNav>();
    }

    private bool responseChosen = false;
    public bool GetResponseChosen() => responseChosen;
    public void SetResponseChosen(bool _responseChosen) => responseChosen = _responseChosen;

    public void ShowReponses(DialogueNode _dialogueObject) // Dynamically creates the player dialogue choice UI
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
            if (index == 0) { responseButton.GetComponent<Button>().onClick.AddListener(() => ChooseResponseLeft(_dialogueObject, responseButtons)); }
            if (index == 1) { responseButton.GetComponent<Button>().onClick.AddListener(() => ChooseResponseMiddle(_dialogueObject, responseButtons)); }
            if(index ==2) {responseButton.GetComponent<Button>().onClick.AddListener(() => ChooseResponseRight(_dialogueObject, responseButtons)); }
            index++;
            responseBoxHeight += repsonseButtonTemplate.sizeDelta.y;
            repsonseBox.sizeDelta = new Vector2(repsonseBox.sizeDelta.x, responseBoxHeight);
            repsonseBox.gameObject.SetActive(true);
        }
        dialogueButtonNav.AddNavigationToButtons(responseButtons);
    }
    public void ChooseResponseLeft(DialogueNode _dialogueObject, List<GameObject> _responseButtons)
    {
        _dialogueObject.GetAssociatedNPC().MakeDecision(0);
        responseChosen = true;
        CleanUp(_responseButtons);
    }
    public void ChooseResponseMiddle(DialogueNode _dialogueObject, List<GameObject> _responseButtons)
    {
        _dialogueObject.GetAssociatedNPC().MakeDecision(1);
        responseChosen = true;
        CleanUp(_responseButtons);
    }
    //Possibly refactor these methods
    public void ChooseResponseRight(DialogueNode _dialogueObject, List<GameObject> _responseButtons)
    {
        _dialogueObject.GetAssociatedNPC().MakeDecision(2);
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
