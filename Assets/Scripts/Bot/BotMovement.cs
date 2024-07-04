using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BotMovement : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _obstacleChecker;
    [SerializeField] private Transform _bottom;
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _slopeLimit = 30f;
    [SerializeField] private float _stepOffset = 0.3f;
    [SerializeField] private float _speed = 3f;
    [SerializeField] private float _squaredOffset = 4f;

    private Rigidbody _rigidbody;
    private Transform _transform;
    private Vector3 _gravityForce;
    private Vector3 _normal;
    private Vector3 _direction;
    private Vector3 _directionAlongSurface;
    private bool _isCanGo = true;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = GetComponent<Transform>();
        _normal = Vector3.up;
        _gravityForce = Vector3.down;

        Vector3 newPosition = _obstacleChecker.position;
        newPosition.y = _bottom.position.y + _stepOffset;
        _obstacleChecker.position = newPosition;
    }

    private void Update()
    {
        if (_rigidbody != null)
        {
            _direction = _target.position - _transform.position;
            LookAt(_direction);

            if (IsOnGround() == true)
                _gravityForce = Vector3.down;
            else
                _gravityForce = Physics.gravity;

            if (Vector3.SqrMagnitude(_direction) > _squaredOffset)
            {
                _directionAlongSurface = Vector3.ProjectOnPlane(_direction, _normal).normalized;
                float surfaceAngle = Vector3.Angle(_transform.forward, _directionAlongSurface);

                if (surfaceAngle > _slopeLimit)
                {
                    _isCanGo = false;

                    if (Physics.Raycast(_obstacleChecker.position, Vector3.down, out RaycastHit hitInfo, _layerMask))
                    {
                        Vector3 targetPoint = hitInfo.point;

                        if (targetPoint.y < _bottom.position.y + _stepOffset && Mathf.Approximately(targetPoint.y, 0f) == false)
                            _transform.position = targetPoint + Vector3.up;
                    }
                }
                else
                {
                    _isCanGo = true;
                }
            }
            else
            {
                _isCanGo = false;
            }


            if (_isCanGo)
                _rigidbody.velocity = _directionAlongSurface * _speed + _gravityForce;
            else
                _rigidbody.velocity = Vector3.zero + _gravityForce;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        _normal = collision.contacts[0].normal;
    }

    private bool IsOnGround()
    {
        float checkDistance = 1.15f;

        return Physics.Raycast(_transform.position, Vector3.down, checkDistance, _layerMask);
    }

    private void LookAt(Vector3 direction)
    {
        Vector3 lookDirection = Vector3.ProjectOnPlane(_directionAlongSurface, Vector3.up);
        _transform.forward = lookDirection;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + _normal * 3);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + _directionAlongSurface.normalized * 3);
    }
}