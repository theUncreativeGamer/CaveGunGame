using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    public Animator facingDirection;
    public Animator generalAnimations;
    public bool IsFacingLeft = false;
    public bool IsGrounded = true;
    public float WalkingSpeed;
    public float WalkingAnimationSpeedMult = 1f;


    [SerializeField]
    private bool isJumping = false;
    [SerializeField]
    private bool isFiring = false;
    [SerializeField]
    private bool isAltFiring = false;
    [SerializeField]
    private bool isDead = false;

    public void Jump()
    {
        isJumping = true;
    }

    public void Fire()
    {
        isFiring = true;
    }

    public void AltFire()
    { 
        isAltFiring = true;
    }

    public void Die()
    {
        isDead = true;
    }

    private void LateUpdate()
    {
        generalAnimations.SetBool("IsDead", isDead);
        facingDirection.SetBool("IsFacingLeft", IsFacingLeft);
        generalAnimations.SetBool("IsGrounded", IsGrounded);
        generalAnimations.SetFloat("WalkingSpeed", WalkingSpeed * WalkingAnimationSpeedMult);
        if (isJumping)
        {
            generalAnimations.SetBool("Jump", isJumping);
            isJumping = false;
        }
        if(isFiring)
        {
            generalAnimations.SetBool("Fire", isFiring);
            isFiring = false;
        }
        if(isAltFiring)
        {
            generalAnimations.SetBool("AltFire", isAltFiring);
            isAltFiring = false;
        }

    }
}
