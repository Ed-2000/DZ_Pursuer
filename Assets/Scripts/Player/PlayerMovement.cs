using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _strafeSpeed = 2f;
    [SerializeField] private float _jumpSpeed = 2f;
    [SerializeField] private float _gravityFactor = 2f;

    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    private Vector3 _verticalVelocity;
    private CharacterController _characterController;
    private Transform _transform;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _transform = transform;
    }

    private void Update()
    {
        if (_characterController != null)
        {
            Vector2 playerInput = new Vector2(Input.GetAxis(Horizontal), Input.GetAxis(Vertical));
            Vector3 direction = Vector3.ProjectOnPlane(playerInput.y * _transform.forward + playerInput.x * _transform.right, Vector3.up).normalized;

            direction.z *= _speed;
            direction.x *= _strafeSpeed;

            if (_characterController.isGrounded == true)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                    _verticalVelocity = Vector3.up * _jumpSpeed;
                else
                    _verticalVelocity = Vector3.down;

                _characterController.Move((direction + _verticalVelocity) * Time.deltaTime);
            }
            else
            {
                Vector3 horizontalVelocity = _characterController.velocity;
                horizontalVelocity.y = 0;
                _verticalVelocity += Physics.gravity * _gravityFactor * Time.deltaTime;
                _characterController.Move((horizontalVelocity + _verticalVelocity) * Time.deltaTime);
            }
        }
    }
}