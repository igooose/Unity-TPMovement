## 🏃‍♂️ TPMovement
**TPMovement** is a thrid person movement system for unity project, using unity's `character controller` as its base component.
Still using unity's old input, and no animation implemented since only focus on movement.

<div align="center">
  
| Features | Control | Preview |
| --- | --- | --- |
| Basic Move | `WASD` | <img src="https://github.com/vianagus/Unity-TPMovement/assets/58974402/c97030ca-8fbc-494b-b8f1-e3ed01c568c1" alt="TPMovementPreviewBasicMove" width="480"/> |
| Run | `Left Shift` | <img src="https://github.com/vianagus/Unity-TPMovement/assets/58974402/0c1800aa-77e0-43e0-a386-9ab0ace61a9d" alt="TPMovementPreviewRun" width="480"/> |
| Jump | `Space` | <img src="https://github.com/vianagus/Unity-TPMovement/assets/58974402/a988cc53-0926-4a48-8b50-57f1f99b0cce" alt="TPMovementPreviewJump" width="480"/> |

_More feartures and changes will be added in the future._

</div>

## 🔗 Download
unity package download: [click here](https://www.dropbox.com/scl/fi/5scubjyq3jf9xr6wc6mby/TPMovement.unitypackage?rlkey=o3yrxtugbkiikp1xeldflqea9&dl=1)

## ⚙ Setup
1. Download and import the unity package into unity project.
2. Create a `ground layer` if not already created.
3. Make sure to change the ground objects' layer into `ground layer`.
4. Put `TPMovement.cs` into player object.
5. Set `Ground Layer` attribute with the `ground layer`.

Adjust the following attributes to affect the movement.
| Attribute | Type  | Description |
| --- | --- | --- |
| `Move Speed` | float | speed of the basic move |
| `Run Speed` | float | speed of the run move |
| `Jump Height` | float | height of the jump |
| `Smooth Facing` | float | smooth character facing rotation |
| `Gravity Scale` | float | affect the jump and fall speed |
| `Ground Layer` | LayerMask | layer for ground check purpose |
