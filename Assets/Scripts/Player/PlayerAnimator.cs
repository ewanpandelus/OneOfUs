using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator animator;
    Vector3 movement = new Vector3(0, 0, 0);
    [SerializeField] float timeUntilIdleAnim = 0.3f;
    private float elapsedTime = 0;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void UpdateMovement(Vector3 _movement)
    {
        movement = _movement;
    }
    private void Update()
    {
        AssessMovement();
        if (movement == Vector3.zero && animator.GetInteger("MovementStatus") == 0)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            elapsedTime = 0;
        }
        if (elapsedTime > timeUntilIdleAnim)
        {
            animator.SetInteger("MovementStatus", 4);
        }
    }
    private void AssessMovement()
    {
        if (movement.y < 0)
        {
            animator.SetInteger("MovementStatus", 0); //Back
        }
        if (movement.x < 0)
        {
            animator.SetInteger("MovementStatus", 1); //Left
            return;
        }
        if (movement.x > 0)
        {
            animator.SetInteger("MovementStatus", 2); //Right
            return;
        }
        if (movement.y > 0)
        {
            animator.SetInteger("MovementStatus", 3); //Back
        }
    }
}
