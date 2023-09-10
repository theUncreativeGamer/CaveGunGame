using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    public CharacterAnimationController animationController;
    public FootstepsEffects footstepsEffects;
    public Transform weaponTransform;
    public LightDarkRifleAttack attack;
    public Collider2D groundSensor;
    public ContactFilter2D groundFilter;
    public CinemachineVirtualCamera playerCamera;
    public VoicelineController voiceline;

    [Header("Values")]
    public float movementSpeed;
    public float movementAcceleration;
    public float movementTurnDirectionAcceleration;
    public float jumpHeight;
    public float coyoteTimeSeconds;
    public float aimCameraOffsetDistance;



    private new Rigidbody2D rigidbody;
    private PlayerInput playerInput;
    private float airborneDurationSeconds = 0;
    private float movementInput = 0;
    private Vector2 aimingDirection = Vector2.zero;
    private CinemachineFramingTransposer playerCameraTransposer;
    private Vector3 playerCameraDefaultOffset;
    private float jumpCooldownSeconds = 0;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        playerCameraTransposer = playerCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        playerCameraDefaultOffset = playerCameraTransposer.m_TrackedObjectOffset;
    }

    private float JumpSpeed()
    {
        float gravity = Physics2D.gravity.magnitude * rigidbody.gravityScale;
        return Mathf.Sqrt(2 * jumpHeight * gravity);
    }

    public void OnMove(InputValue value)
    {
        movementInput = value.Get<Vector2>().x;
    }

    public void OnJump()
    {
        if(jumpCooldownSeconds<=0&& airborneDurationSeconds<coyoteTimeSeconds)
        {
            jumpCooldownSeconds = coyoteTimeSeconds;
            rigidbody.AddForce(new Vector2(0, JumpSpeed() * rigidbody.mass), ForceMode2D.Impulse);
            //Debug.Log(rigidbody.velocity.y);
            animationController.Jump();
            if (footstepsEffects != null) footstepsEffects.Jump();
            if (voiceline != null) voiceline.PlayJump();
        }
    }

    public void OnAim(InputValue value)
    {
        Vector2 input = value.Get<Vector2>();
        

        if (playerInput.currentControlScheme=="Gamepad")
        {
            // To prevent calculation error, aiming angles too close to straight up or down will be ignored.
            if (Mathf.Abs(input.normalized.x) < 0.01f)
            {
                return;
            }
            // Directly get aiming direction when using joystick to aim.
            aimingDirection = input;
        }
        else
        {
            // Convert input mouse position into world position when using mouse to aim.
            Vector2 mousePosition = Camera.main.ScreenToViewportPoint(input);
            mousePosition -= new Vector2(0.5f, 0.5f);
            // The mousePosition here has value between {-0.5f, -0.5f} and {0.5f, 0.5f} that 
            // represent mouse position proportion to the screen.

            // Scale the x value so that {0.5, 0} and {0, 0.5} will actually have the same length.
            mousePosition.x *= Camera.main.aspect;
            Vector2 result = Vector2.ClampMagnitude(mousePosition * 3, 1);

            // To prevent calculation error, aiming angles too close to straight up or down will be ignored.
            if (Mathf.Abs(result.normalized.x) < 0.01f)
            {
                return;
            }
            aimingDirection = result;
        }
    }

    public void OnAttack()
    {
        if(attack.Fire())
        {
            animationController.Fire();
            if(voiceline!=null) voiceline.PlayAttack();
        }
        
    }

    public void OnAltAttack()
    {
        if (attack.AltFire())
        {
            animationController.AltFire();
            if (voiceline != null) voiceline.PlayAttack();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        jumpCooldownSeconds -= Time.fixedDeltaTime;
        // Ground detecting
        Collider2D[] grounds = new Collider2D[1];
        int overlapCount = Physics2D.OverlapCollider(groundSensor, groundFilter, grounds);
        if(overlapCount > 0)
        {
            if(airborneDurationSeconds > 0.01f)
            {
                if (footstepsEffects != null) footstepsEffects.Land();
            }
            airborneDurationSeconds = 0;
        }
        else
        {
            airborneDurationSeconds += Time.fixedDeltaTime;
        }
        animationController.IsGrounded = airborneDurationSeconds < 0.01f;

        // Aiming
        if (aimingDirection.magnitude >= 0.05f) 
        {
            weaponTransform.LookAt(weaponTransform.position + new Vector3(aimingDirection.x, aimingDirection.y, 0));
            if (aimingDirection.x > 0)
            {
                animationController.IsFacingLeft = false;
            }
            else if (aimingDirection.x < 0)
            {
                animationController.IsFacingLeft = true;
            }
            playerCameraTransposer.m_TrackedObjectOffset = playerCameraDefaultOffset + new Vector3(aimingDirection.x, aimingDirection.y) * aimCameraOffsetDistance;
        }
        else
        {
            playerCameraTransposer.m_TrackedObjectOffset = playerCameraDefaultOffset;
        }


        // Movement
        if (movementInput > 0.01f)
        {
            // Go Right
            if (rigidbody.velocity.x < 0)
            {
                // Turning direction
                rigidbody.AddForce(new Vector2(movementInput * movementTurnDirectionAcceleration * rigidbody.mass, 0), ForceMode2D.Force);
                if(rigidbody.velocity.x > 0)
                {
                    rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                }
                if (footstepsEffects != null && airborneDurationSeconds <= 0.01f) footstepsEffects.Walk(2f);
            }
            else if (rigidbody.velocity.x < movementSpeed * movementInput)
            {
                // Acceleration
                rigidbody.AddForce(new Vector2(movementInput * movementAcceleration * rigidbody.mass, 0), ForceMode2D.Force);

                // Can't go too fast
                if (rigidbody.velocity.x > movementSpeed * movementInput)
                {
                    rigidbody.velocity = new Vector2(movementSpeed * movementInput, rigidbody.velocity.y);
                }
                if (footstepsEffects != null && airborneDurationSeconds <= 0.01f) footstepsEffects.Walk();
            }
            animationController.WalkingSpeed = movementInput;
        }
        else if (movementInput < -0.01f) 
        {
            // Go Left
            if (rigidbody.velocity.x > 0)
            {
                // Turning direction
                rigidbody.AddForce(new Vector2(movementInput * movementTurnDirectionAcceleration * rigidbody.mass, 0), ForceMode2D.Force);
                if (rigidbody.velocity.x < 0)
                {
                    rigidbody.velocity = new Vector2(0, rigidbody.velocity.y);
                }
                if (footstepsEffects != null && airborneDurationSeconds <= 0.01f) footstepsEffects.Walk(2f);
            }
            else if (rigidbody.velocity.x > movementSpeed * movementInput)
            {
                // Acceleration
                rigidbody.AddForce(new Vector2(movementInput * movementAcceleration * rigidbody.mass, 0), ForceMode2D.Force);

                // Can't go too fast
                if (rigidbody.velocity.x < movementSpeed * movementInput)
                {
                    rigidbody.velocity = new Vector2(movementSpeed * movementInput, rigidbody.velocity.y);
                }
                if (footstepsEffects != null && airborneDurationSeconds <= 0.01f) footstepsEffects.Walk();
            }
            animationController.WalkingSpeed = -movementInput;
        }
        else
        {
            // Not walking
            if(airborneDurationSeconds > 0.01f)
            {
                // Airborne
                if (rigidbody.velocity.x < -0.01f)
                {
                    rigidbody.AddForce(new Vector2(0.1f * movementTurnDirectionAcceleration * rigidbody.mass, 0), ForceMode2D.Force);
                }
                else if (rigidbody.velocity.x > 0.01f)
                {
                    rigidbody.AddForce(new Vector2(-0.1f * movementTurnDirectionAcceleration * rigidbody.mass, 0), ForceMode2D.Force);
                }
                else
                {
                    rigidbody.velocity = new Vector2(0f, rigidbody.velocity.y);
                }
            }
            else
            {
                if (rigidbody.velocity.x < -0.1f)
                {
                    rigidbody.AddForce(new Vector2(0.3f * movementTurnDirectionAcceleration * rigidbody.mass, 0), ForceMode2D.Force);
                }
                else if (rigidbody.velocity.x > 0.1f)
                {
                    rigidbody.AddForce(new Vector2(-0.3f * movementTurnDirectionAcceleration * rigidbody.mass, 0), ForceMode2D.Force);
                }
                else
                {
                    rigidbody.velocity = new Vector2(0f, rigidbody.velocity.y);
                }
            }
            animationController.WalkingSpeed = 0;
        }

    }
}
