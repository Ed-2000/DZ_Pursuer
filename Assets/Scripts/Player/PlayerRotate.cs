using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _horizontalTurnSensitivity = 2.5f;
    [SerializeField] private float _verticalTurnSensitivity = 5.0f;
    [SerializeField] private float _verticalMinAngle = -89.0f;
    [SerializeField] private float _verticalMaxAngle = 89.0f;

    private const string MouseX = "Mouse X"; 
    private const string MouseY = "Mouse Y"; 

    private Transform _transform;
    private float _cameraAngle;

    private void Awake()
    {
        _transform = transform;
        _cameraAngle = _cameraTransform.localEulerAngles.x;
    }

    private void Update()
    {
        _cameraAngle -= Input.GetAxis(MouseY) * _verticalTurnSensitivity;
        _cameraAngle = Mathf.Clamp(_cameraAngle, _verticalMinAngle, _verticalMaxAngle);
        _cameraTransform.localEulerAngles = Vector3.right * _cameraAngle;

        _transform.Rotate(Vector3.up * Input.GetAxis(MouseX) * _horizontalTurnSensitivity);
    }
}