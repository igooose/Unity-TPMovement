using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class TPMovementRB : MonoBehaviour
{
    #region variables

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _runSpeed = 10f;
    [Range(0f, 1f)] [SerializeField] private float _smoothFacing = 0.75f;
    private bool _isRunning;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private bool _onAirMove;
    [SerializeField] private bool _onAirFacing;

    [Header("Physics")]
    [SerializeField] private float _gravityScale = 2f;
    private const float _GRAVITY = -9.8f;

    [Header("Ground Check")]
    [SerializeField] private float _groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;

    // input variables
    private float _zMoveInput;
    private float _xMoveInput;
    private KeyCode _runInput = KeyCode.LeftShift;
    private KeyCode _jumpInput = KeyCode.Space;

    // component variables
    private Transform _cameraTransform;
    private Rigidbody _rigidbody;

    #endregion

    /*----------------------------------------------------------------------------------*/

    #region unity methods

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;   // get camera transform
        _rigidbody = GetComponent<Rigidbody>();     // get character controller component
    }

    private void Update()
    {
        // handle inputs
            HandleInput();
    }

    private void FixedUpdate()
    {
        // ground check
        HandleGroundCheck(_groundCheckRadius, _groundLayer);

        // handle gravity
        HandleGravity(_GRAVITY, _gravityScale);

        // handle facing roatation
        HandleFacingRotation(_smoothFacing);

        // move the character
        HandleMovement(_moveSpeed, _runSpeed);

    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
            // draw ground check
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, _groundCheckRadius);
        #endif
    }

    #endregion

    /*----------------------------------------------------------------------------------*/

    #region handler methods

    private void HandleInput()
    {
        // WASD input
        _zMoveInput = Input.GetAxisRaw("Vertical");
        _xMoveInput = Input.GetAxisRaw("Horizontal");

        if(_isGrounded)
        {
            // run / default move
            if(Input.GetKey(_runInput))
                _isRunning = true;
            else
                _isRunning = false;

            // jump
            if(Input.GetKeyDown(_jumpInput))
                Jump(_jumpHeight, _GRAVITY, _gravityScale);
        }        
    }

    private void HandleGroundCheck(float groundCheckRadius, LayerMask groundLayer)
    {
        _isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);
    }

    private void HandleGravity(float gravityPower, float gravityScale)
    {
        _rigidbody.AddForce(Vector3.up * gravityPower * gravityScale, ForceMode.Acceleration);
    }

    private void HandleFacingRotation(float smoothRotation)
    {
        if(!_isGrounded && !_onAirFacing) return;

        if (RelativeDirection().magnitude > 0)
        {
            transform.rotation = Quaternion.Lerp
            (
                transform.rotation, 
                Quaternion.LookRotation(RelativeDirection()), 
                smoothRotation * 10f * Time.deltaTime
            );
        }
    }

    private void HandleMovement(float moveSpeed, float runSpeed)
    {
        if(!_isGrounded && !_onAirMove) return;

        if(_isRunning)
            Move(runSpeed);
        else
            Move(moveSpeed);
    }

    #endregion

    /*----------------------------------------------------------------------------------*/

    #region movement methods

    private Vector3 RelativeDirection()
    {
        Vector3 zDir = _cameraTransform.forward;
        Vector3 xDir = _cameraTransform.right;
        zDir.y = 0;
        xDir.y = 0;

        return (zDir * _zMoveInput + xDir * _xMoveInput).normalized;
    }

    private void Move(float moveSpeed)
    {
        _rigidbody.velocity = new Vector3(
            RelativeDirection().x * moveSpeed,
            _rigidbody.velocity.y,
            RelativeDirection().z * moveSpeed);
    }

    private void Jump(float jumpheight, float gravity, float gravityScale)
    {
        if(_isGrounded)
        {
            _rigidbody.velocity = new Vector3(
                _rigidbody.velocity.x,
                Mathf.Sqrt(jumpheight * -2 * (gravity * gravityScale)),
                _rigidbody.velocity.z);

            if(RelativeDirection().magnitude > 0)
                transform.rotation = Quaternion.LookRotation(RelativeDirection());
        }
    }

    #endregion
}
