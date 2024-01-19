using UnityEngine;

public class TPCamera : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] float _distance = 5f;
    [SerializeField] float _height = 1.5f;
    [SerializeField] float _minYRotation = -30f;
    [SerializeField] float _maxYRotation = 30f;
    private Vector3 _lookAtPosition;
    private float _currentXRotation;
    private float _currentYRotation;

    private void LateUpdate()
    {
        // look at position
        _lookAtPosition = new Vector3(_target.position.x, _target.position.y + _height, _target.position.z);

        // input for camera rotation
        _currentXRotation += Input.GetAxis("Mouse X");
        _currentYRotation -= Input.GetAxis("Mouse Y");

        // limit y camera rotation
        _currentYRotation = Mathf.Clamp(_currentYRotation, _minYRotation, _maxYRotation);

        // camera rotation
        Quaternion rotation = Quaternion.Euler(_currentYRotation, _currentXRotation, 0);

        // camera postion
        transform.position = _lookAtPosition + rotation * (Vector3.forward * -_distance);

        // look at
        transform.LookAt(_lookAtPosition);
    }
}
