using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    [SerializeField] UIManager UIManager;
    Vector3 movement = new Vector3 (0,0,0);
    private EvaluateEnvironment evaluateEnvironment;
    private PlayerAnimator playerAnim;
    private void Awake()
    {
        evaluateEnvironment = new EvaluateEnvironment(transform);
        playerAnim = new PlayerAnimator(GetComponent<Animator>());
    }

    void Update()
    {
        if (UIManager.GetStaticUIShowing()) return;
        EvaluateInput();
        playerAnim.UpdateMovement(movement);
        playerAnim.UpdateAnimation();
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
                if(!UIManager.GetStaticUIShowing()) closestNPC.RunDialogue();
            }
        }
    }
    public PlayerAnimator GetPlayerAnimator()
    {
        return playerAnim;
    }
}
