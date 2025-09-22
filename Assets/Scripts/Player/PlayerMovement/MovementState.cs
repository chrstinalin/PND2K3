using UnityEngine;
using static UnityEngine.Rendering.STP;
public class MovementState : PlayerMovementState
{
    GameObject Entity;
    private Rigidbody _rigidbody;
    private bool _isGrounded;
    private bool _canJump;
    
    private float _CurrentVelocity;
    private float _MoveSpeed;
    private float _JumpForce;
    private Joystick _Input = Constant.JOY_LEFT;

    private float _dashSpeed;
    private float _dashCooldown;
    private Vector3 _dashVelocity;

    public override void EnterState(PlayerMovementManager manager, MovementConfig config)
    {
        Entity = config.Entity;
        _MoveSpeed = config.MoveSpeed;
        _JumpForce = config.JumpForce;
        _canJump = config.CanJump;
        _dashSpeed = config.DashSpeed;
        
        _rigidbody = Entity.GetComponent<Rigidbody>();
    }

    public override void UpdateState(PlayerMovementManager manager, bool isActive)
    {
        Vector3 moveDirection = new Vector3(Input.GetAxis(_Input.Horizontal), 0f, Input.GetAxis(_Input.Vertical));

        if (isActive)
        {
            // Jump (Mouse)
            if (_canJump)
            {
                _isGrounded = Physics.Raycast(Entity.transform.position, Vector3.down, 0.7f);
                if (Input.GetButtonDown("Jump") && _isGrounded && _rigidbody.linearVelocity.y <= 0f)
                {
                    _rigidbody.AddForce(Vector3.up * _JumpForce, ForceMode.Impulse);
                }
            }

            // Dash (Mouse)
            if (_dashCooldown > 0)
            {
                _dashCooldown -= Time.deltaTime;
            }

            if (_dashVelocity.sqrMagnitude > 0.1f)
            {
                // Dash duration = 8f
                _dashVelocity = Vector3.Lerp(_dashVelocity, Vector3.zero, Time.deltaTime * 8f);
                Entity.transform.position += _dashVelocity * Time.deltaTime;
                return;  // Disable other movement
            }
            else
            {
                _dashVelocity = Vector3.zero;
            }
            if (Input.GetButtonDown("Movement"))
            {
                Vector3 dashDir = moveDirection.sqrMagnitude > 0 ? moveDirection.normalized : Entity.transform.forward;
                _dashVelocity = dashDir * _dashSpeed;
                _dashCooldown = 1f;
                var dashAngle = Mathf.Atan2(dashDir.x, dashDir.z) * Mathf.Rad2Deg;
                Entity.transform.rotation = Quaternion.Euler(0f, dashAngle, 0f);
            }
        }
        
        if (moveDirection.sqrMagnitude == 0) return;
        
        // Movement
        Entity.transform.position += moveDirection * _MoveSpeed * Time.deltaTime;

        // Rotation
        var targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(Entity.transform.eulerAngles.y, targetAngle, ref _CurrentVelocity, manager.SmoothTime);
        Entity.transform.rotation = Quaternion.Euler(0.0f, targetAngle, 0.0f);
    }
    
    public override void UpdateJoyStick(Joystick Input) => _Input = Input;

}