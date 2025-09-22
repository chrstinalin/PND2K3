using UnityEngine;
using static UnityEngine.Rendering.STP;

public class MovementState : PlayerMovementState
{
    GameObject Entity;
    private Rigidbody _rigidbody;
    private ParticleSystem _particleSystem;
    private bool _isGrounded;
    private bool _canJump;
    
    private float _CurrentVelocity;
    private float _MoveSpeed;
    private float _currMoveSpeed;
    private float _JumpForce;
    private Joystick _Input = Constant.JOY_LEFT;

    private float _dashSpeed;
    private float _dashCooldown;
    private Vector3 _dashVelocity;

    public override void EnterState(PlayerMovementManager manager, MovementConfig config)
    {
        Entity = config.Entity;
        _MoveSpeed = config.MoveSpeed;
        _currMoveSpeed = _MoveSpeed;
        _JumpForce = config.JumpForce;
        _canJump = config.CanJump;
        _dashSpeed = config.DashSpeed;
        
        _rigidbody = Entity.GetComponent<Rigidbody>();
        _particleSystem = Entity.GetComponent<ParticleSystem>();
    }

    public override void UpdateState(PlayerMovementManager manager, bool isActive)
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis(_Input.Horizontal), 0f, Input.GetAxis(_Input.Vertical));

        if (_dashCooldown > 0)
        {
            _dashCooldown -= Time.deltaTime;
        }

        if (_dashVelocity.sqrMagnitude > 0.05f)
        {
            _dashVelocity = Vector3.Lerp(_dashVelocity, Vector3.zero, Time.deltaTime * 8f);
        }
        else
        {
            _dashVelocity = Vector3.zero;
        }
        
        // Jumping and Dashing
        if (isActive)
        {
            if (_canJump)
            {
                // Jump
                _isGrounded = Physics.Raycast(Entity.transform.position, Vector3.down, 0.7f);
                if (Input.GetButtonDown("MovementR") && _isGrounded && _rigidbody.linearVelocity.y <= 0f)
                {
                    _rigidbody.AddForce(Vector3.up * _JumpForce, ForceMode.Impulse);
                }
                
                // Dash
                if (Input.GetButtonDown("MovementL") && _dashCooldown <= 0)
                {
                    Vector3 dashDir = moveDirection.sqrMagnitude > 0 ? moveDirection.normalized : Entity.transform.forward;
                    _dashVelocity = dashDir * _dashSpeed;
                    _dashCooldown = 0.8f;
                    if (_particleSystem != null)
                    {
                        _particleSystem.Play();
                    }
                    var dashAngle = Mathf.Atan2(dashDir.x, dashDir.z) * Mathf.Rad2Deg;
                    Entity.transform.rotation = Quaternion.Euler(0f, dashAngle, 0f);
                }
            }
            else if (_dashCooldown <= 0)
            {
                if (Input.GetButtonDown("MovementL"))
                {
                    _dashVelocity = -Entity.transform.right * _dashSpeed;
                    _dashCooldown = 0.5f;
                    if (_particleSystem != null)
                    {
                        var shapeModule = _particleSystem.shape;
                        shapeModule.rotation = new Vector3(0, -90, 0); // Point left
                        _particleSystem.Play();
                    }
                }

                if (Input.GetButtonDown("MovementR"))
                {
                    _dashVelocity = Entity.transform.right * _dashSpeed;
                    _dashCooldown = 0.5f;
                    if (_particleSystem != null)
                    {
                        var shapeModule = _particleSystem.shape;
                        shapeModule.rotation = new Vector3(0, 90, 0); // Point left
                        _particleSystem.Play();
                    }
                }
            }
        }
        
        Vector3 totalMovement = Vector3.zero;
        totalMovement += _dashVelocity * Time.deltaTime;
        
        if (moveDirection.sqrMagnitude > 0)
        {
            float airborneSpeed = _MoveSpeed;
            if (_canJump && !_isGrounded) airborneSpeed *= 0.7f;
            _currMoveSpeed = Mathf.Lerp(_currMoveSpeed, airborneSpeed, Time.deltaTime * 5f);
            
            float normalMovementMultiplier = 1f;
            if (_dashVelocity.sqrMagnitude > 1f)
            {
                if (_canJump)
                {
                    normalMovementMultiplier = 0.3f;
                }
                else
                {
                    normalMovementMultiplier = 0.1f;
                }
            }
            
            totalMovement += moveDirection * _currMoveSpeed * normalMovementMultiplier * Time.deltaTime;
            
            if (_dashVelocity.sqrMagnitude < 1f)  // Rotate if not dashing/strafing
            {
                var targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
                var angle = Mathf.SmoothDampAngle(Entity.transform.eulerAngles.y, targetAngle, ref _CurrentVelocity, manager.SmoothTime);
                Entity.transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
            }
        }
        
        Entity.transform.position += totalMovement;
    }
    
    public override void UpdateJoyStick(Joystick Input) => _Input = Input;
}