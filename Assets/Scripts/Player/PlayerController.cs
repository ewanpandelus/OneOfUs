using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] DialogueUI dialogueUI;
    [SerializeField] private float movementSpeed;
    Vector3 movement = new Vector3 (0,0,0);
    private EvaluateEnvironment evaluateEnvironment;
    private void Awake()
    {
        evaluateEnvironment = GetComponent<EvaluateEnvironment>();
    }

    void Update()
    {
        if (dialogueUI.GetShowingText()) return;
        EvaluateInput();
        transform.position += movement * Time.deltaTime;
    }
    private void EvaluateInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        movement.y = Input.GetAxisRaw("Vertical") * movementSpeed;
        EvaluateInteraction();
      
    }
    private void EvaluateInteraction()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.E))
        {
            NPC closestNPC = evaluateEnvironment.ClosestNPC();
            if (closestNPC != null)
            {
                if(!dialogueUI.GetShowingText()) closestNPC.RunDialogue();
            }
        }
    }

  
}
