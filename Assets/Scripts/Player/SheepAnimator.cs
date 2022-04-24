using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepAnimator : MonoBehaviour
{

    [SerializeField] private float movementSpeed;
    [SerializeField] GameObject playerPos;
    private Animator animator;
    private Vector3 movement = new Vector3(0, 0, 0);
    private Transform _transform;
    private int movementStatus;
    private bool active = false;
    void Start()
    {
        animator = GetComponent<Animator>();
        _transform = gameObject.transform;
    }
    void Update()
    {
        if (!active || UIManager.instance.GetStaticUIShowing()) return;
        EvaluateInput();
        _transform.position += movement * Time.deltaTime;
        playerPos.transform.position = _transform.position;
        AssessMovement();
  
    }
    private void EvaluateInput()
    {
        movement.x = Input.GetAxisRaw("Horizontal") * movementSpeed;
        movement.y = Input.GetAxisRaw("Vertical") * movementSpeed;

    }
    public void UpdateMovement(Vector3 _movement)
    {
        movement = _movement;
    }
    private void AssessMovement()
    {
        if (movement.x < 0)
        {
            if (movement.y <= 0)
            {
                animator.SetInteger("MovementStatus", 0); //Left
                movementStatus = 0;
             
            }
            else
            {
           
                animator.SetInteger("MovementStatus", 1); //BackLeft
                movementStatus = 1;
               

            }

            return;
        }
        if (movement.x > 0)
        {
            if (movement.y <= 0)
            {
                animator.SetInteger("MovementStatus", 2); //Right
                movementStatus = 2;

            }
            else
            {
            
                animator.SetInteger("MovementStatus", 3); //BackRight
                movementStatus = 3;

            }

            return;
        }
        if (movement.y < 0)
        {
            if(movementStatus == 3 ||movementStatus == 2)
            {
                animator.SetInteger("MovementStatus", 2); //Right
                movementStatus = 2;
            }
            else
            {
                animator.SetInteger("MovementStatus", 0); //Left
                movementStatus = 0;
            }
    
            return;
        }
    }
    public void SetActive(bool _active) => active = _active;
}


