using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform repsonseButtonTemplate;
    [SerializeField] private RectTransform repsonseContainer;
    [SerializeField] private RectTransform dialogueBox;
    private bool availableResponses = false;
    private DialogueButtonNav dialogueButtonNav;
    private Vector3 initialPosResponseBox = new Vector3();

    private void Start()
    {
        dialogueButtonNav = GetComponent<DialogueButtonNav>();
        initialPosResponseBox = responseBox.transform.position;
    }

    private bool responseChosen = false;
    public bool GetResponseChosen() => responseChosen;
    public bool GetAvailableResponsesChosen() => availableResponses;
    public void SetResponseChosen(bool _responseChosen) => responseChosen = _responseChosen;

 
    public void SetupResponses(DialogueNode _dialogueNode) // Dynamically creates the player dialogue choice UI
    {
        float responseBoxHeight = 0;
        int index = 0;
        AssignAvailableResponses(_dialogueNode);
        List<GameObject> responseButtons = new List<GameObject>();
        foreach (string response in _dialogueNode.GetResponses())
        {
            GameObject responseButton = Instantiate(repsonseButtonTemplate.gameObject, repsonseContainer);
            responseButtons.Add(responseButton);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response;
            if (index == 0) { responseButton.GetComponent<Button>().onClick.AddListener(() => ChooseResponseLeft(_dialogueNode, responseButtons)); }
            if (index == 1) { responseButton.GetComponent<Button>().onClick.AddListener(() => ChooseResponseMiddle(_dialogueNode, responseButtons)); }
            if(index ==2) {responseButton.GetComponent<Button>().onClick.AddListener(() => ChooseResponseRight(_dialogueNode, responseButtons)); }
            index++;
            responseBoxHeight += repsonseButtonTemplate.sizeDelta.y;
            responseBox.sizeDelta = new Vector2(responseBox.sizeDelta.x, responseBoxHeight);
            responseBox.gameObject.SetActive(true);
            responseBox.transform.position = initialPosResponseBox - ((Vector3.up * repsonseButtonTemplate.sizeDelta.y*_dialogueNode.GetResponses().Length)
                - new Vector3(0, repsonseButtonTemplate.sizeDelta.y, 0));

        }
        dialogueButtonNav.AddNavigationToButtons(responseButtons);
    }
    private void AssignAvailableResponses(DialogueNode _dialogueNode)
    {
        if (_dialogueNode.GetResponses().Length == 0)
        {
            availableResponses = false;
            return;
        }
        availableResponses = true;
    }
    public void AdjustPositionBasedOnResponses(int responses)
    {
        responseBox.transform.position = initialPosResponseBox - ((Vector3.up * repsonseButtonTemplate.sizeDelta.y) - new Vector3(0, repsonseButtonTemplate.sizeDelta.y, 0));
        if(responses == 1)
        {
            switch (responses)
            {
                case 1:
                    dialogueBox.transform.position -= 120*Vector3.up;
                    break;
                case 2:
                    dialogueBox.transform.position -= 60 * Vector3.up;
                    break;
                 default:
                    break;
            }
        }
    }
    public void ChooseResponseLeft(DialogueNode _dialogueObject, List<GameObject> _responseButtons)
    {
        _dialogueObject.GetAssociatedNPC().MakeDecision(0);
        CleanUp(_responseButtons);
    }
    public void ChooseResponseMiddle(DialogueNode _dialogueObject, List<GameObject> _responseButtons)
    {
        _dialogueObject.GetAssociatedNPC().MakeDecision(1);
        CleanUp(_responseButtons);
    }
    //Possibly refactor these methods
    public void ChooseResponseRight(DialogueNode _dialogueObject, List<GameObject> _responseButtons)
    {
        _dialogueObject.GetAssociatedNPC().MakeDecision(2);
        CleanUp(_responseButtons);
    }
    private void CleanUp(List<GameObject> _responseButtons)
    {
        responseChosen = true;
        foreach (GameObject response in _responseButtons)
        {
            Destroy(response);
        }
        responseBox.gameObject.SetActive(false);
        dialogueButtonNav.NullButtons();
    }
   
}
