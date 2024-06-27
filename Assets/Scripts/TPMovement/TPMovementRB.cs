using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TPMovementRB : MonoBehaviour
{

#region Component

    private Transform _cameraTransform;     // camera that targeting player
    private Rigidbody _rigidbody;           // rigidbody component attached to player

#endregion

#region Setting

    [Header("Move")]
    [SerializeField] float _walkSpeed = 2.5f;           // walking speed
    [SerializeField] float _runSpeed = 5f;              // running speed
    [SerializeField] float _sprintSpeed = 10f;          // sprint speed

    [Header("Facing Speed")]
    [SerializeField] float _normalFacing = 3f;          // normal facing ratotion speed
    [SerializeField] float _sprintFacing = 0.15f;       // facing rotation speed for sprint
    
    [Header("Jump")]
    [SerializeField] float _jumpHeight = 2.5f;          // jump height
    [SerializeField] int _maxJump = 2;                  // maximum number of jump

    [Header("Dash")]
    [SerializeField] float _dashSpeed = 30f;            // dash speed
    [SerializeField] float _dashDuration = 0.2f;        // dash duration
    [SerializeField] float _dashCooldown = 0.2f;        // dash cooldown

    [Header("Gravity")]
    [SerializeField] float _gravity = -9.8f;            // gravity value
    [SerializeField] float _gravityScale = 3;           // gravity scale

    [Header("Ground Check")]
    [SerializeField] float _groundCheckRadius = 0.2f;   // ground check radius
    [SerializeField] LayerMask _groundLayer;            // ground layer mask

#endregion

#region Modifier

    private Vector2 _moveAxis;          // axis from input
    private Vector3 _direction;         // relative direction
    private Vector3 _directionForward;  // forward direction
    private Vector3 _velocity;            // motion movement
    private bool _isOnGround;           // is on ground?
    private float _facingSpeed;         // facing rotation speed
    private int _jumpCount;             // number of jump performed
    private float _dashTimer;           // dash timer
    private float _dashCooldownTimer;   // dash cooldown timer

    public Vector3 Velocity {get => _velocity;}
    public bool IsOnGround {get => _isOnGround;}

#endregion

#region Unity Methods

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _rigidbody = GetComponent<Rigidbody>();
    }

#endregion

#region Private Methods

    // calculate relative direction from input axis and camera angle, pass value to _direction
    // also calculate forward direction, pass value to _forwardDirection
    private void CalculateDirection()
    {
        // relative direction
        Vector3 zDir = _cameraTransform.forward, xDir = _cameraTransform.right;
        zDir.y = 0; xDir.y = 0;
        _direction = (zDir * _moveAxis.y + xDir * _moveAxis.x).normalized;

        // forward direction
        _directionForward = transform.forward.normalized;
    }

    // ground detection, pass value to _isGrounded
    private void GroundCheck()
    {
        _isOnGround = Physics.CheckSphere(transform.position, _groundCheckRadius, _groundLayer);
    }

    // smooth rotation
    private void ApplyFacing(float speed)
    {   
        if (_direction.magnitude > 0)
        {
            transform.rotation = Quaternion.Lerp
            (
                transform.rotation, 
                Quaternion.LookRotation(_direction), 
                speed * 10f * Time.deltaTime
            );
        }
    }

    // custom gravity
    private void ApplyGravity()
    {
        float fallSpeedLimit = _gravity * _gravityScale;

        if(_velocity.y < 0 && _isOnGround)
            _velocity.y = _gravity;
        else
            _velocity.y = _velocity.y > fallSpeedLimit ? _velocity.y + (_gravity * _gravityScale * Time.deltaTime) : fallSpeedLimit;
    }

    // move using certain direction and speed
    private void SetVelocity(Vector3 direction, float speed)
    {
        _velocity.x = direction.x * speed;
        _velocity.z = direction.z * speed;
    }

    // reset jump count if already on ground
    private void TryResetMultipleJump()
    {
        if(_velocity.y < 0 && _isOnGround)
        {
            _jumpCount = 0;
        }
        // lose the first jump when just fall from higher ground
        else if(_velocity.y < 0 && !_isOnGround && _jumpCount == 0)
        {
            _jumpCount = 1;
        }
    }

    // try perform dash if trggered by Dash()
    private void TryPerformDash()
    {
        _dashTimer -= Time.deltaTime;
        _dashCooldownTimer -= Time.deltaTime;
        if (_dashTimer > 0)
        {
            _facingSpeed = _sprintFacing;
            SetVelocity(_directionForward, _dashSpeed);
        }
        // discontinue dash speed on air and move forward slightly if dash is done
        else if(_dashTimer > -0.01f && !_isOnGround)
        {
            SetVelocity(_directionForward, _walkSpeed);
        }
    }

#endregion

#region Public Methods

    /// <summary>
    /// This method must be in Update() method.
    /// </summary>
    public void OnUpdate(Vector2 moveAxis)
    {
        // pass axis input
        _moveAxis = moveAxis;

        // calculation
        GroundCheck();
        CalculateDirection();

        ApplyFacing(_facingSpeed);  // implement smooth rotation
        ApplyGravity();             // implement custom gravity
        TryResetMultipleJump();     // reset jump count if already on ground
        TryPerformDash();           // perform dash if trggered by Dash()

        // perform move based on _velocity
        _rigidbody.velocity = _velocity;
    }

    /// <summary>
    /// Idle.
    /// </summary>
    public void Idle()
    {
        if(_isOnGround)
            SetVelocity(Vector3.zero, 0f);
    }

    /// <summary>
    /// Move with walk speed.
    /// </summary>
    public void Walk()
    {
        _facingSpeed = _normalFacing;
        SetVelocity(_direction, _walkSpeed);
    }

    /// <summary>
    /// Move with run speed (default move).
    /// </summary>
    public void Run()
    {
        _facingSpeed = _normalFacing;
        SetVelocity(_direction, _runSpeed);
    }

    /// <summary>
    /// Move with sprint speed.
    /// </summary>
    public void Sprint()
    {
        _facingSpeed = _sprintFacing;
        SetVelocity(_directionForward, _sprintSpeed);
    }

    /// <summary>
    /// Trigger jump.
    /// </summary>
    public void Jump()
    {
        bool onGroundJump = _isOnGround && _jumpCount == 0;
        bool onAirJump = !_isOnGround && _jumpCount > 0 && _jumpCount < _maxJump;

        if(onGroundJump || onAirJump)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * (_gravity * _gravityScale));
            _jumpCount++;
        }
    }

    /// <summary>
    /// Trigger dash.
    /// </summary>
    public void Dash()
    {
        if(_dashCooldownTimer < 0 )
        {
            _dashTimer = _dashDuration;
            _dashCooldownTimer = _dashDuration + _dashCooldown;
        }
    }

#endregion

}
