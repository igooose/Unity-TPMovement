using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class TPMovementCC : MonoBehaviour
{
#region variables

    [Header("Movement")]
    [SerializeField] private float _walkSpeed  = 1f;
    [SerializeField] private float _moveSpeed = 3f;
    [SerializeField] private float _sprintSpeed = 7.5f;
    [SerializeField] private bool _enableWalk = true;
    [SerializeField] private bool _enableSprint = true;
    [Range(0f, 1f)] [SerializeField] private float _smoothFacing = 0.75f;
    private Vector3 _movement;

    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 2f;
    [SerializeField] private bool _onAirMove;
    [SerializeField] private bool _onAirFacing;
    
    [Header("Physics")]
    [SerializeField] private float _gravityScale = 2f;
    private const float _GRAVITY = -9.8f;

    [Header("Ground Check")]
    [SerializeField] float _groundCheckRadius = 0.1f;
    [SerializeField] private LayerMask _groundLayer;
    private bool _isGrounded;

    // locomotion state
    private float _currentMoveSpeed;
    public LocomotionState State {get; private set;}

    // input variables
    private float _zMoveInput;
    private float _xMoveInput;
    private KeyCode _walkInput = KeyCode.LeftControl;
    private KeyCode _sprintInput = KeyCode.LeftShift;
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

    private void Update()
    {
        // handle inputs
        HandleInput();

        // ground check
        HandleGroundCheck(_groundCheckRadius, _groundLayer);

        // handle gravity
        HandleGravity(_GRAVITY, _gravityScale);

        // handle facing roatation
        HandleFacingRotation(_smoothFacing);

        // move the character
        _characterController.Move(_movement * Time.deltaTime);

        // handle locomotion state
        HandleLocomotionState();
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
            // move
            MoveInput();

            // jump
            if (Input.GetKeyDown(_jumpInput))
                Jump(_jumpHeight, _GRAVITY, _gravityScale);
        }
        else
        {
            // on air move
            if(_onAirMove)
                MoveInput();
        }

        void MoveInput()
        {
            if (RelativeDirection().magnitude > 0)
            {
                if (Input.GetKey(_walkInput) && _enableWalk)
                    Move(_walkSpeed);
                else if (Input.GetKey(_sprintInput) && _enableSprint)
                    Move(_sprintSpeed);
                else
                    Move(_moveSpeed);
            }
            else
            {
                Move(0);
            }
        }       
    }

    private void HandleGroundCheck(float groundCheckRadius, LayerMask groundLayer)
    {
        _isGrounded = Physics.CheckSphere(transform.position, groundCheckRadius, groundLayer);
    }

    private void HandleGravity(float gravityPower, float gravityScale)
    {
        if(_movement.y < 0 && _isGrounded)
            _movement.y = gravityPower * gravityScale / 2f;
        else
            _movement.y += gravityPower * gravityScale * Time.deltaTime;
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

    private void HandleLocomotionState()
    {
        if (_isGrounded)
        {
            if (_currentMoveSpeed == 0)
                State = LocomotionState.Idle;
            else if (_currentMoveSpeed == _moveSpeed)
                State = LocomotionState.Move;
            else if (_currentMoveSpeed == _walkSpeed)
                State = LocomotionState.Walk;
            else if (_currentMoveSpeed == _sprintSpeed)
                State = LocomotionState.Sprint;
        }
        else
        {
            if (_characterController.velocity.y > 0)
                State = LocomotionState.OnJump;
            else
                State = LocomotionState.OnFall;
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
        _currentMoveSpeed = moveSpeed;
    }

    private void Jump(float jumpheight, float gravity, float gravityScale)
    {
        if(_isGrounded)
        {
            _movement.y = Mathf.Sqrt(jumpheight * -2 * (gravity * gravityScale));

            if(RelativeDirection().magnitude > 0)
                transform.rotation = Quaternion.LookRotation(RelativeDirection());
        }
    }

#endregion
}
