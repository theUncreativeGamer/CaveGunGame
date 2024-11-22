using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Utilities;

// No complicated path finding algorithm! 
// Just press jump button when see wall!
[RequireComponent(typeof(Rigidbody2D))]
public class GroundEnemyAI : MonoBehaviour
{
    [Header("Pathfinding")]
    [Tooltip("If this reference is not set, it will be set to the first game object with tag \"Player\" it finds.")]
    public Transform target;
    public LayerMask targetLayers;
    public LayerMask wallLayers;
    public Vector2 eyePositionOffset;
    public float wallCheckingDistance = 10f;

    [Header("Physics")]
    public float accelaration = 10f;
    public Collider2D groundSensor;
    public float jumpHeight = 1f;
    public float jumpInterval = 0.1f;

    [Header("Custom Behaviours")]
    public CharacterAnimationController animationController;
    public bool jumpEnabled = true;

    private Rigidbody2D rb;
    private float jumpTimer = 0f;
    private bool isFacingLeft = true;
    private bool isGrounded;
    
    public void SetTargetToPlayer()
    {
        target = GameObject.Find("Player").transform;
    }

    private bool IsSeeingWall(bool isFacingLeft)
    {
        Vector2 seeDirection = isFacingLeft ? Vector2.left : Vector2.right;
        var items = Physics2D.RaycastAll((Vector2)transform.position + eyePositionOffset, seeDirection, targetLayers | wallLayers);
        if (items.Length == 0) return false;
        var orderedList = Raycast2DUtil.SortHits(items);

        // If the enemy is also seeing the player, it shouldn't jump over them.
        if (orderedList[0].collider.CompareTag("Player")) return false;
        return true;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        if (target == null)
        {
            target = GameObject.Find("Player").transform;
        }
    }

    private void FixedUpdate()
    {
        // Direction
        isFacingLeft = target.position.x < rb.position.x;

        if (animationController != null)
            animationController.IsFacingLeft = isFacingLeft;

        // Ground check
        isGrounded = groundSensor.IsTouchingLayers(wallLayers);
        if (animationController != null) 
            animationController.IsGrounded = isGrounded;

        // Running
        Vector2 runDirection = isFacingLeft ? Vector2.left : Vector2.right;
        rb.AddForce(accelaration * rb.mass * runDirection, ForceMode2D.Force);

        if (animationController != null) 
            animationController.WalkingSpeed = rb.velocity.x * (isFacingLeft ? -1 : 1);

        // Jumping
        jumpTimer -= Time.fixedDeltaTime;
        if (isGrounded && jumpTimer <= 0 && IsSeeingWall(isFacingLeft)) 
        {
            jumpTimer = jumpInterval;
            rb.AddForce(2 * jumpHeight * rb.mass * rb.gravityScale * Vector2.up, ForceMode2D.Impulse);

            if (animationController != null)
                animationController.Jump();
        }
    }
}
