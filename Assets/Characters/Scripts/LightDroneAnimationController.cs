using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightDroneAnimationController : MonoBehaviour
{
    public Animator spriteAnimator;
    public Animator facingDirectionAnimator;

    public bool IsFacingLeft
    {
        get
        {
            return facingDirectionAnimator.GetBool("IsFacingLeft");
        }
        set
        {
            facingDirectionAnimator.SetBool("IsFacingLeft", value);
        }
    }

    public bool IsDead
    {
        get
        {
            return spriteAnimator.GetBool("IsDead");
        }
        set
        {
            spriteAnimator.SetBool("IsDead", value);
        }
    }

    public void Attack()
    {
        spriteAnimator.SetBool("Attack", true);
    }
}
