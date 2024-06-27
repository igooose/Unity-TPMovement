## 🏃‍♂️ TPMovement
**TPMovement** is a thrid person movement system for unity project.
There are two version of TPMovement, using `CharacterController` and using `Rigidbody`.
|Features|Description|
|---|---|
|Basic Move|Idle, Walk, Run, Sprint.|
|Jump|Able to multiple jump, not only double jump.|
|Dash|Able to perform dash both on ground and on air.|

## ⚙ Installation
### TPMovement in general (with `CharacterController` and `Rigidbody`):
1. Download or copy/paste `TPMovement.cs`.
2. Attach the script into player object.
3. Don't forget to setup `ground layer` on Unity and `TPMovement inspector`.

Make sure follow setting below if using **Rigidbody**.

<img src="./_Readme/installation_rigidbody_setting.png" alt="TPMovement Jump Preview"/>

## 📔 How to Use
Just get the TPMovement component and use the public methods. But first of all, make sure to call `TPMovement.OnUpdate()` in Unity's `Update()`, or Unity's `FixedUodate()` if using rigidbody.

### Example
```c#
public class PlayerController : MonoBehaviour
{
    // component
    private TPMovement _tpMovement;

    void Start()
    {
        _tpMovement = GetComponent<TPMovement>();
    }

    void Uodate()
    {
        // read input
        Vector2 inputAxis = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        bool inputJump = Input.GetButtonDown("Jump");
        bool inputDash = Input.GetButtonDown("Dash");

        // must call this OnUpdate() in unity's Update()
        // call in FixedUpdate() if using Rigidody
        _tpMovement.OnUpdate(inputAxis);

        // input and action
        if(inputAxis.magnitude > 0)
            _tpMovement.Run();
        else
            _tpMovement.Idle();

        if(inputJump)
            _tpMovement.Jump();

        if(inputDash)
            _tpMovement.Dash();
    }
}
```

## 📡 Public Method
| Method | Type  | Description |
| --- | --- | --- |
| `Idle()` | void | Will not moving, require to stop basic movement. |
| `Walk()` | void | Move with walk speed. |
| `Run()` | void | Move with run speed. |
| `Sprint()` | void | Move with sprint speed. |
| `Jump()` | void | Trigger jump. |
| `Dash()` | void | Trigger dash. |

## 🧩 Attribute on Inspector
| Attribute | Type  | Description |
| --- | --- | --- |
| **Move** |  |  |
| `Walk Speed` | float | Walk speed. |
| `Run Speed` | float | Run speed. |
| `Sprint Speed` | float | Sprint speed. |
| **Facing Speed** |  |  |
| `Normal Facing` | float | Facing rotation speed. |
| `Sprint Speed` | float | Facing rotation speed when on sprint. |
| **Jump** |  |  |
| `Jump Height` | float | Jump height. |
| `Max Jump` | int | Maximum number of jump can perform. |
| **Dash** |  |  |
| `Dash Speed` | float | Dash speed. |
| `Dash Duration` | float | Dash duration. |
| `Dash Cooldown` | float | Dash cooldown. |
| **Gravity** |  |  |
| `Gravity` | float | Gravity power. |
| `Gravity Scale` | float | Gravity power's multiplier. |
| **Ground Check** |  |  |
| `Ground Check Radius` | float | Radius of the ground check. |
| `Ground Layer` | LayerMask | Layer that ground check able to detect. |
