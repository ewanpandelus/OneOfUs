using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float movementSpeed;
    Vector3 movement = new Vector3 (0,0,0);
    private EvaluateEnvironment evaluateEnvironment;
    private PlayerAnimator playerAnim;

    private Transform _transform;
    private bool canMove = true;
    private bool firstInteraction = false;
    private void Awake()
    {
        evaluateEnvironment = new EvaluateEnvironment(transform);
        playerAnim = new PlayerAnimator(GetComponent<Animator>());
        _transform = transform;
    }

    void Update()
    {
        if (UIManager.instance.GetStaticUIShowing()||MiracleManager.instance.GetMiracleOccuring()) return;
        EvaluateInput();
        if (!canMove) return;
        playerAnim.UpdateMovement(movement);
        playerAnim.UpdateAnimation();
        _transform.position += movement * Time.deltaTime;
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
                closestNPC.RunDialogue();
                if (!firstInteraction) firstInteraction = true;
            }
        }
    }
    public PlayerAnimator GetPlayerAnimator()
    {
        return playerAnim;
    }
    public void SetCanMove(bool _canMove) => canMove = _canMove;
    public bool GetFirstInteraction() => firstInteraction;
    
}
