using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class AiMovement : MonoBehaviour
{
    public float maxMovementSpeed = 20f;
    public Rigidbody2D puck;
    public Transform boundaryHolder;

    private float4 _playerBoundary;
    private Rigidbody2D _rb;
    private Vector2 _startingPos;
    private Vector2 _targetPos;

    private Vector2 _oldDirection;
    private float _offsetX;

    private bool _isTop;

    float _movementSpeed = .35f;
    float _playerRadius;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _startingPos = _rb.position;
        _playerRadius = GetComponent<CircleCollider2D>().radius;

        _isTop = transform.position.y > 0;

        if (_isTop)
        {
            _playerBoundary = new float4(
                boundaryHolder.GetChild(0).position.x + _playerRadius,
                boundaryHolder.GetChild(1).position.x - _playerRadius,
                boundaryHolder.GetChild(3).position.y + _playerRadius,
                -boundaryHolder.GetChild(2).position.y - _playerRadius
            );
        }
        else
        {
            _playerBoundary = new float4(
                boundaryHolder.GetChild(0).position.x + _playerRadius,
                boundaryHolder.GetChild(1).position.x - _playerRadius,
                boundaryHolder.GetChild(2).position.y + _playerRadius,
                -boundaryHolder.GetChild(3).position.y - _playerRadius
            ); 
        }
    }

    void FixedUpdate()
    {
        var position = _rb.position;
        var direction = (_targetPos - position).normalized;

        bool hasDirectionChanged = DidDirectionChange(direction);
        if (hasDirectionChanged)
        {
            _offsetX = _playerRadius * Random.Range(-1f, 1f);
        }
        if ((_isTop && puck.position.y <= 0) || (!_isTop && puck.position.y >= 0))
        {
            if (hasDirectionChanged)
            {
                _movementSpeed = maxMovementSpeed * Random.Range(.1f, .3f);
            }

            _targetPos = new Vector2(Mathf.Clamp(puck.position.x + _offsetX, _playerBoundary[0], _playerBoundary[1]),
                _startingPos.y);
        }
        else
        {
            if (hasDirectionChanged)
            {
                _movementSpeed = maxMovementSpeed * Random.Range(.4f, 1f);
            }

            _targetPos = new Vector2(Mathf.Clamp(puck.position.x + _offsetX, _playerBoundary[0], _playerBoundary[1]),
                Mathf.Clamp(puck.position.y, _playerBoundary[2], _playerBoundary[3]));
        }

        _rb.MovePosition(Vector2.MoveTowards(position, _targetPos, _movementSpeed * Time.fixedDeltaTime));

        _oldDirection = direction;
    }

    bool DidDirectionChange(Vector2 direction)
    {
        if (direction.sqrMagnitude > 0 && _oldDirection.sqrMagnitude == 0f) return true;
        float angle = Vector2.Angle(direction, _oldDirection);
        return angle > 20;
    }
}
