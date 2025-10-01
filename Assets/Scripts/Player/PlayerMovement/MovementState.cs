using System.Linq;
using UnityEngine;

public class MovementState : PlayerMovementState
{
    GameObject Entity;
    private Rigidbody _rigidbody;
    private ParticleSystem _particleSystem;
    private Transform _entityTransform;  // Cache transform reference
    private bool _isGrounded;
    private bool _canJump;

    private float _CurrentVelocity;
    private float _DefaultMoveSpeed;
    private float _MoveSpeed;
    private float _currMoveSpeed;
    private float _JumpForce;
    private Joystick _Input = Constant.JOY_LEFT;

    private float _dashSpeed;
    private float _dashCooldown;
    private Vector3 _dashVelocity;

    private float _groundCheckTimer;
    private const float GROUND_CHECK_INTERVAL = 0.08f;
    private const float GROUND_CHECK_DISTANCE = 0.7f;

    private const float DASH_DECAY_RATE = 8f;
    private const float DASH_THRESHOLD = 0.05f;
    private const float ROTATION_THRESHOLD = 1f;
    private const float AIRBORNE_MULTIPLIER = 0.7f;
    private const float SPEED_LERP_RATE = 5f;
    private const float SNEAK_MULTIPLIER = 0.3f;

    private Vector3? FollowVector = null;

    public AudioSource jumpSound;
    public AudioSource dashSound;

    /*
     * ========================================================================
     * Initialization
     * ========================================================================
     */
    public override void EnterState(PlayerMovementManager manager, MovementConfig config)
    {
        Entity = config.Entity;
        _entityTransform = Entity.transform;
        _MoveSpeed = config.MoveSpeed;
        _DefaultMoveSpeed = _MoveSpeed;
        _currMoveSpeed = _MoveSpeed;
        _JumpForce = config.JumpForce;
        _canJump = config.CanJump;
        _dashSpeed = config.DashSpeed;

        _rigidbody = Entity.GetComponent<Rigidbody>();
        _particleSystem = Entity.GetComponent<ParticleSystem>();

        GroundCollider = _entityTransform.Cast<Transform>()
            .FirstOrDefault(child => child.CompareTag("GroundCollider"))?.gameObject;

        _groundCheckTimer = 0f;

        jumpSound = config.JumpSound;
        dashSound = config.DashSound;
    }

    /* 
     * ========================================================================
     * Movement Logic
     * ========================================================================
     */
    public override void UpdateState(PlayerMovementManager manager, bool isActive)
    {
        Vector3 moveDirection;
        float horizontalInput = Input.GetAxis(_Input.Horizontal);
        float verticalInput = Input.GetAxis(_Input.Vertical);

        // Lock movement to FollowVector if mouse is active and FollowVector is set
        if (manager.IsMouseActive && FollowVector.HasValue)
        {
            Vector3 followVec = FollowVector.Value.normalized;
            float magnitude;

            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput)) magnitude = horizontalInput;
            else magnitude = verticalInput * Mathf.Sign(followVec.y);

            moveDirection = magnitude * new Vector3(followVec.x, 0f, followVec.z).normalized;
            _MoveSpeed = _DefaultMoveSpeed * SNEAK_MULTIPLIER;
        }
        else
        {
            moveDirection = new Vector3(horizontalInput, 0f, verticalInput);
            _MoveSpeed = _DefaultMoveSpeed;
        }

        UpdateTimers();
        UpdateGroundCheck();
        ProcessDashDecay();

        if (isActive)
        {
            ProcessJumpInput();
            ProcessDashInput(moveDirection);
        }

        ApplyMovement(moveDirection, manager.SmoothTime);
    }

    /*
     * Update timers.
     */
    private void UpdateTimers()
    {
        if (_dashCooldown > 0)
            _dashCooldown -= Time.deltaTime;

        _groundCheckTimer -= Time.deltaTime;
    }

    /*
     * Check if entity is on ground.
     */
    private void UpdateGroundCheck()
    {
        if (_groundCheckTimer <= 0f)
        {
            _isGrounded = Physics.Raycast(_entityTransform.position, Vector3.down, GROUND_CHECK_DISTANCE);
            _groundCheckTimer = GROUND_CHECK_INTERVAL;
        }
    }

    /*
     * Reduces dash/strafe velocity over time.
     */
    private void ProcessDashDecay()
    {
        if (_dashVelocity.sqrMagnitude > DASH_THRESHOLD)
        {
            _dashVelocity = Vector3.Lerp(_dashVelocity, Vector3.zero, Time.deltaTime * DASH_DECAY_RATE);
        }
        else
        {
            _dashVelocity = Vector3.zero;
        }
    }

    /*
     * Jump Input Handling
     */
    private void ProcessJumpInput()
    {
        if (!_canJump || !_isGrounded || _rigidbody.linearVelocity.y > 2f)
            return;

        if (Input.GetButtonDown("MouseJump"))
        {
            if (jumpSound != null)
            {
                jumpSound.PlayOneShot(jumpSound.clip);
            }
            _rigidbody.AddForce(Vector3.up * _JumpForce, ForceMode.Impulse);
        }
    }

    /*
     * Dash and Strafe Input Handling
     */
    private void ProcessDashInput(Vector3 moveDirection)
    {
        if (_dashCooldown > 0) return;

        if (_canJump)
        {   
            HandleDash(moveDirection);
        }
        else
        {
            HandleStrafe();
        }
    }

    /*
     * Mouse dash
     * Dashes in the input direction, or forward if no input.
     */
    private void HandleDash(Vector3 moveDirection)
    {
        if (!Input.GetButtonDown("MouseDash")) return;

        Vector3 dashDir = moveDirection.sqrMagnitude > 0 ? moveDirection.normalized : _entityTransform.forward;
        _dashVelocity = dashDir * _dashSpeed;
        _dashCooldown = 0.8f;

        if (_particleSystem != null)
        {
            _particleSystem.Play();
        }

        if (dashSound != null)
        dashSound.PlayOneShot(dashSound.clip);

        var dashAngle = Mathf.Atan2(dashDir.x, dashDir.z) * Mathf.Rad2Deg;
        _entityTransform.rotation = Quaternion.Euler(0f, dashAngle, 0f);
    }

    /*
     * Mech strafe
     * Strafes left or right based on input.
     */
    private void HandleStrafe()
    {
        if (Input.GetButtonDown("MechLeftStrafe"))
        {
            _dashVelocity = -_entityTransform.right * _dashSpeed;
            _dashCooldown = 0.5f;
            if (_particleSystem != null)
            {
                var shapeModule = _particleSystem.shape;
                shapeModule.rotation = new Vector3(0, -90, 0);
                _particleSystem.Play();
            }
        }

        if (Input.GetButtonDown("MechRightStrafe"))
        {
            _dashVelocity = _entityTransform.right * _dashSpeed;
            _dashCooldown = 0.5f;
            if (_particleSystem != null)
            {
                var shapeModule = _particleSystem.shape;
                shapeModule.rotation = new Vector3(0, 90, 0);
                _particleSystem.Play();
            }
        }
    }

    /*
     *  Movement based on input direction, curr speed, and dash speed.
     */
    private void ApplyMovement(Vector3 moveDirection, float smoothTime)
    {
        float airborneSpeed = _MoveSpeed;
        if (_canJump && !_isGrounded)
            airborneSpeed *= AIRBORNE_MULTIPLIER;
        _currMoveSpeed = Mathf.Lerp(_currMoveSpeed, airborneSpeed, Time.deltaTime * SPEED_LERP_RATE);

        Vector3 currentVelocity = _rigidbody.linearVelocity;
        Vector3 horizontalVelocity = _dashVelocity;

        if (moveDirection.sqrMagnitude > 0)
        {
            float normalMovementMultiplier = 1f;
            if (_dashVelocity.sqrMagnitude > ROTATION_THRESHOLD)
            {
                normalMovementMultiplier = _canJump ? 0.3f : 0.1f;
            }

            horizontalVelocity += moveDirection * _currMoveSpeed * normalMovementMultiplier;

            if (_dashVelocity.sqrMagnitude < ROTATION_THRESHOLD)
            {
                var targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                var angle = Mathf.SmoothDampAngle(_entityTransform.eulerAngles.y, targetAngle, ref _CurrentVelocity, smoothTime);
                _entityTransform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            }
        }

        _rigidbody.linearVelocity = new Vector3(horizontalVelocity.x, currentVelocity.y, horizontalVelocity.z);
    }

    /*
     * ========================================================================
     * Joystick
     * ========================================================================
     */
    public override void UpdateJoyStick(Joystick Input) => _Input = Input;

    /* 
     * ========================================================================
     * Locked Movement
     * ========================================================================
     */
    public override void setFollowVector(Vector3? vec) => FollowVector = vec;
}