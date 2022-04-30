using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator
{
    private Animator animator;
    Vector3 movement = new Vector3(0, 0, 0);
    [SerializeField] float timeUntilIdleAnim = 0.3f;
    private float elapsedTime = 0;
    private bool shouldAutoAnimate = true;
    private int movementStatus = 0;
    public PlayerAnimator(Animator _animator)
    {
        animator = _animator;
    }
    public void UpdateMovement(Vector3 _movement)
    {
        movement = _movement;
    }
    public void UpdateAnimation()
    {
        if (!shouldAutoAnimate)
        {
            return;
        }
        AssessMovement();
        if (movement == Vector3.zero)
        {
            elapsedTime += Time.deltaTime;
        }
        else
        {
            elapsedTime = 0;
            return;
        }
        if (elapsedTime > timeUntilIdleAnim)
        {
            movementStatus += 5;
            if (movementStatus > 8)
            {
                movementStatus -= 5;
            }
            animator.SetInteger("MovementStatus", movementStatus);
            elapsedTime = 0;
        }
    }
    private void AssessMovement()
    {
        if (movement.y < 0)
        {
            animator.SetInteger("MovementStatus", 0); //Front
            movementStatus = 0;
            return;
        }
        if (movement.x < 0)
        {
            animator.SetInteger("MovementStatus", 1); //Left
            movementStatus = 1;
            return;
        }
        if (movement.y > 0)
        {
            animator.SetInteger("MovementStatus", 2); //Back
            movementStatus = 2;
            return;
        }
        if (movement.x > 0)
        {
            movementStatus = 3;
            animator.SetInteger("MovementStatus", 3); //Right
        }
    }
    public void SetShouldAutoAnimate(bool _shouldAutoAnimate)
    {
        shouldAutoAnimate = _shouldAutoAnimate;
    }
    public void ManuallySetAnimator(int _animationCode)
    {
        animator.SetInteger("MovementStatus", _animationCode);
    }
    public void SetMovementStatus(int _movementStatus) => movementStatus = _movementStatus;
}
