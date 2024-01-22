using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _distance = 10f;
    [SerializeField] float _height = 1.5f;
    [SerializeField] float _minPitch = 0f;
    [SerializeField] float _maxPitch = 60f;
    private Vector3 _lookAtPosition;
    private float _yaw;
    private float _pitch;

    private void LateUpdate()
    {
        // look at position
        _lookAtPosition = new Vector3(_target.position.x, _target.position.y + _height, _target.position.z);

        // input for camera rotation
        _yaw += Input.GetAxis("Mouse X");
        _pitch -= Input.GetAxis("Mouse Y");

        // limit y camera rotation
        _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

        // camera rotation
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);

        // camera postion
        transform.position = _lookAtPosition + rotation * (Vector3.forward * -_distance);

        // look at
        transform.LookAt(_lookAtPosition);
    }
}
