using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TPMovement : MonoBehaviour
{
    #region variables

    [Header("Movement")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _runSpeed = 10f;
    [SerializeField] private float _jumpHeight = 2f;
    [Range(0f, 1f)] [SerializeField] private float _smoothFacing = 0.75f;
    private Vector3 _movement;

    [Header("Physics")]
    [SerializeField] private float _gravityScale = 2f;
    private const float _GRAVITY = -9.8f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayer;
    private float _groundCheckRadius;
    private bool _isGrounded;

    // input variables
    private float _zMoveInput;
    private float _xMoveInput;
    private KeyCode _runInput = KeyCode.LeftShift;
    private KeyCode _jumpInput = KeyCode.Space;

    // component variables
    private Transform _cameraTransform;
    private CharacterController _characterController;

    #endregion

    /*----------------------------------------------------------------------------------*/

    #region unity methods

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;                       // get camera transform
        _characterController = GetComponent<CharacterController>();     // get character controller component
    }

    private void Start()
    {
        _groundCheckRadius = _characterController.radius;   // set ground check radius the same as character controller's radius
    }

    private void Update()
    {
        // handle inputs
        HandleInput();

        // ground check
        HandleGroundCheck();

        // handle gravity
        HandleGravity(_GRAVITY * _gravityScale);

        // handle facing roatation
        if(_isGrounded)
            HandleFacingRotation(_smoothFacing);

        // move the character
        _characterController.Move(_movement * Time.deltaTime);
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
                Move(_runSpeed);
            else
                Move(_moveSpeed);

            // jump
            if(Input.GetKeyDown(_jumpInput))
                Jump(_jumpHeight, _GRAVITY * _gravityScale);
        }        
    }

    private void HandleGroundCheck()
    {
        _isGrounded = Physics.CheckSphere(transform.position + (Vector3.up * _groundCheckRadius/2), _groundCheckRadius, _groundLayer);
    }

    private void HandleGravity(float gravityPower)
    {
        if(_movement.y < 0 && _isGrounded)
            _movement.y = gravityPower / 2f;
        else
            _movement.y += gravityPower * Time.deltaTime;
    }

    private void HandleFacingRotation(float smoothRotation)
    {
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
        _movement.x = RelativeDirection().x * moveSpeed;
        _movement.z = RelativeDirection().z * moveSpeed;
    }

    private void Jump(float jumpheight, float gravity)
    {
        _movement.y = Mathf.Sqrt(jumpheight * -2 * gravity);
        if(RelativeDirection().magnitude > 0)
            transform.rotation = Quaternion.LookRotation(RelativeDirection());
    }

    #endregion
}
