using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed;
    [SerializeField] public float walkSpeed;
    [SerializeField] public float sprintSpeed;
    [SerializeField] public float groundDrag; // Drag

    [Header("Stamina Curve")]
    [SerializeField] public float baseStamina = 100f;
    [SerializeField] public float minStamina = 20f;
    [SerializeField] public float staminaFallofRate = 0.1386f;

    [Header("Stamina Settings")]
    [SerializeField] public float currentStamina = 100f; // Dont change in inspector
    [SerializeField] public float currentMaxStamina; // Dont change in inspector
    [SerializeField] public float drainRate = 1f;
    [SerializeField] public float rechargeRate = 1f;
    [SerializeField] public float rechargeCooldown = 5f;
    [SerializeField] public float sprintCooldown = 1f;
    [SerializeField] public float fatigueTimer = 0f;
    [SerializeField] public float exponentialPenalty = 1f;

    [Header("Exhausted SFX")]
    [SerializeField] public AnimationCurve volumeCurve;
    [SerializeField] public AudioSource audioSource;
    [SerializeField] public float maximumVolume = 0.3f;
    [SerializeField, Range(0f, 1f)] public float smoothing = 0.15f;
    public float _progress;
    private float _velocity;

    private bool _isFatigued;
    private bool isSprinting;
    private bool _sprintInCooldown;
    private float _timeSinceStopSprint;


    // These values are added on top of the default sprint speed (apart from crouch).
    [Header("Happiness Speed Modifiers")]
    [SerializeField] public float unhappySpeed;
    [SerializeField] public float neutralSpeed;
    [SerializeField] public float happySpeed;
    [SerializeField] public float overjoyedSpeed;

    [Header("Jumping")]
    [SerializeField] public float jumpForce;
    [SerializeField] public float jumpCooldown;
    [SerializeField] public float airMultiplier;
    [SerializeField] private bool readyToJump = true;

    [Header("Keybinds")]
    [SerializeField] public KeyCode jumpKey = KeyCode.Space;
    [SerializeField] public KeyCode sprintKey = KeyCode.LeftShift;

    [Header("Ground Check")]
    [SerializeField] public float playerHeight;
    [SerializeField] public LayerMask whatIsGround;
    [SerializeField] public bool grounded;

    [Header("Slope Handling")]
    [SerializeField] public float maxSlopeAngle;
    [SerializeField] private RaycastHit slopeHit;
    [SerializeField] private bool exitingSlope;

    [SerializeField] public Transform orientation;

    private float _horizontalInput;
    private float _verticalInput;

    private HappinessController _happinessController;
    private float _currentSpeedModifier;

    public Vector3 moveDirection;
    private Rigidbody rb;
    public MovementState state;

    private float _time;

    public enum MovementState
    {
        walking,
        sprinting,
        air
    }

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _happinessController = this.GetComponent<HappinessController>();

        _timeSinceStopSprint = 0.0f;
        currentMaxStamina = baseStamina;
    }

    // Update is called once per frame
    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround); // Casts raycast down, and if touching object mark as ground, then set player as grounded.

        currentMaxStamina = minStamina + (baseStamina - minStamina) * Mathf.Exp(-staminaFallofRate * _currentSpeedModifier); // Max stamina calculation
        isSprinting = Input.GetKey(sprintKey) && currentStamina > 0f && !_isFatigued && !_sprintInCooldown; // Enable sprinting flag if conditions are met

        // Put sprint on cooldown if key is pressed and not fatigued (prevents sprint spam)
        if (Input.GetKeyUp(sprintKey) && currentStamina > 0f && !_isFatigued) 
        {
            _sprintInCooldown = true;
        }                    

        MyInput();
        SpeedControl();
        StateHandler();
        HappinessModifier();
        HandleStamina();
        ExhaustedSound();

        // Handle drag
        if (grounded)
        {
            rb.drag = groundDrag;
        } else
        {
            rb.drag = 0;
        }

        // Debug.Log("Current Speed:" + rb.velocity.magnitude);
    }

    private void FixedUpdate()
    {
        MovePlayer(); // Player movement is handled at a fixed 50 frames per second.
    }

    private void MyInput()
    {
        // Store raw input values for movement in both axis.
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        // If jump conditions are met and action key is pressed, jump
        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    // Handle player states
    private void StateHandler()
    {

        if (grounded && Input.GetKey(sprintKey)) // Sprinting
        {
            //state = MovementState.sprinting;
            //moveSpeed = sprintSpeed;
            HandleSprint();
        } 
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
        } 
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // calculate movement direction
        moveDirection = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        // If player is on slope.
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20f, ForceMode.Force); // Add parallel force to slope

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 160f, ForceMode.Force); // Apply downward force to keep player on slope
            }
        }
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force); // Add normal movement force to the rigidbody so player moves
        } 
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force); // Add force to rigidbody when player is in the air.
        }

        rb.useGravity = !OnSlope(); // Disable gravity if on slope to prevent sliding down
    }

    // Handles movement states, and movement speed when sprinting, as well as exponential stamina penalty that increases the
    // higher the player sprints for (also punishes players who hold shift even after running out of stamina
    private void HandleSprint() 
    {
        if (isSprinting)
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed + _currentSpeedModifier;
            exponentialPenalty += Time.deltaTime / 20f;
        }
        else
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            isSprinting = false;
        }
    }

    // Handles stamina logic
    private void HandleStamina() 
    {
        // exponential penalty logic
        if (!isSprinting && exponentialPenalty > 1f)
        {
            exponentialPenalty -= Time.deltaTime / 20f;
            if (exponentialPenalty < 1) exponentialPenalty = 1f;           
        }

        // Drain stamina if sprinting
        if (isSprinting)
        {
            currentStamina -= (Time.deltaTime * drainRate * exponentialPenalty);
        }
        else if (!_isFatigued && !_sprintInCooldown) // Else recharge it, if not in cooldown and player is not fatigued
        {
            currentStamina += Time.deltaTime * rechargeRate;
        }

        if (currentStamina <= 0f && fatigueTimer <= 3f) // Fatigue player if they run out of stamina.
        {
            fatigueTimer += Time.deltaTime;
            _isFatigued = true;
        }
        else if (fatigueTimer >= 3f) // Reset Fatigue after enough time has elapsed, and recharge stamina as normal.
        {
            currentStamina += Time.deltaTime * rechargeRate;
            _isFatigued = false;
            fatigueTimer = 0f;
        }

        if (_sprintInCooldown)  // Handle sprint cooldown timer and then resets it back to false.
        {
            _timeSinceStopSprint += Time.deltaTime;

            // If enough time has passed, reset sprint cooldown
            if (_timeSinceStopSprint >= sprintCooldown) 
            {
                _sprintInCooldown = false;
                _timeSinceStopSprint = 0f;
            }
        }

        // Prevent stamina from going above/below maximum/minimum values.
        if (currentStamina < 0f) currentStamina = 0f;
        if (currentStamina > currentMaxStamina) currentStamina = currentMaxStamina;
    }

    // Handles player speed so rigidbody velocity doesnt exceed set movement speed.
    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope())
        {
            if(rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else // limiting speed on ground or in air
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity when going above set speed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    // Handle jump behaviour
    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse); // Add upwards force to rigidbody
    }

    // Reset Jump flags
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    // Determines if player is on Slope by casting a raycast downwards to the players position and comparing the angle between itself and the normal of the object.
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;

        }

        return false;
    }

    // Project movement direction pararell to the slope normal to allow player to move along the slope normally 
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    // Handles movement speed modifier based on current Happiness Meter state.
    private void HappinessModifier() 
    {
        switch (_happinessController.state) 
        {
            case HappinessController.HappinessState.unhappy:
                _currentSpeedModifier = unhappySpeed;
                break;

            case HappinessController.HappinessState.neutral:
                _currentSpeedModifier = neutralSpeed;
                break;

            case HappinessController.HappinessState.happy:
                _currentSpeedModifier = happySpeed;
                break;

            case HappinessController.HappinessState.overjoyed:
                _currentSpeedModifier = overjoyedSpeed;
                break;
        }

        // Debug
        _time += Time.deltaTime;

        if (_time >= 2.0f) 
        {
            Debug.Log("Current Happiness: " + _happinessController.state);
            Debug.Log("[HAPPINESS MODIFIER] Current Sprint Speed: " + (sprintSpeed + _currentSpeedModifier));
            _time = 0.0f;
        }
    }

    // Handles the sound effect that plays once player is exhausted. Uses similiar approach compared to SFX in the HappinessController.cs script.
    private void ExhaustedSound() 
    {
        _progress = Mathf.Clamp01(currentStamina / currentMaxStamina);
        float _targetVolume = Mathf.Lerp(maximumVolume, 0.0f, volumeCurve.Evaluate(_progress));
        audioSource.volume = Mathf.SmoothDamp(audioSource.volume, _targetVolume, ref _velocity, smoothing);
    }
}