using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // mode CC or RB
    private enum Mode { WithCC, WithRB }
    [SerializeField] Mode _mode;

    // component
    private TPMovementCC _tpMovementCC;
    private TPMovementRB _tpMovementRB;

    // input
    Vector2 _inputAxis;
    private bool _idle;
    private bool _walk;
    private bool _run;
    private bool _sprint;
    private bool _dash;
    private bool _jump;

    void Start()
    {
        if(GetComponent<TPMovementCC>())
            _tpMovementCC = GetComponent<TPMovementCC>();
        else if(GetComponent<TPMovementRB>())
            _tpMovementRB = GetComponent<TPMovementRB>();
    }


    private void Update()
    {
        ReadInput();

        if(_mode == Mode.WithCC)
        {
            _tpMovementCC.OnUpdate(_inputAxis);
            InputActionCC();
        }
        else
        {
            _tpMovementRB.OnUpdate(_inputAxis);
            InputActionRB();
        }
    }

    private void ReadInput()
    {
        _inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        _idle = _inputAxis.magnitude == 0;
        _walk = _inputAxis.magnitude < 0.5f && _inputAxis.magnitude > 0 && !_sprint;
        _run = _inputAxis.magnitude > 0.5f && !_sprint;
        _sprint = _inputAxis.magnitude > 0 && Input.GetButton("Fire3");
        _dash = Input.GetButtonDown("Fire1");
        _jump = Input.GetButtonDown("Jump");
    }

    private void InputActionCC()
    {
        if (_idle && _tpMovementCC.IsOnGround)
            _tpMovementCC.Idle();
        else if (_walk)
            _tpMovementCC.Walk();
        else if (_run)
            _tpMovementCC.Run();
        else if (_sprint)
            _tpMovementCC.Sprint();

        if (_dash)
            _tpMovementCC.Dash();

        if (_jump)
            _tpMovementCC.Jump();
    }

    private void InputActionRB()
    {
        if (_idle && _tpMovementRB.IsOnGround)
            _tpMovementRB.Idle();
        else if (_walk)
            _tpMovementRB.Walk();
        else if (_run)
            _tpMovementRB.Run();
        else if (_sprint)
            _tpMovementRB.Sprint();

        if (_dash)
            _tpMovementRB.Dash();

        if (_jump)
            _tpMovementRB.Jump();
    }
}
