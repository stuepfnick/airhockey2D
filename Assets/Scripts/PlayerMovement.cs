using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Networking.PlayerConnection;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    public Transform boundaryHolder;

    private bool _hasBeenClicked, _canMove;
    private Rigidbody2D _rb;
    private Collider2D _collider;

    private float4 _playerBoundary;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<Collider2D>();

        var circleCol = (CircleCollider2D) _collider;
        _playerBoundary = new float4(
            boundaryHolder.GetChild(0).position.x + circleCol.radius,
            boundaryHolder.GetChild(1).position.x - circleCol.radius,
            boundaryHolder.GetChild(2).position.y + circleCol.radius,
            boundaryHolder.GetChild(3).position.y - circleCol.radius);
    }

    // Update is called once per frame
    void Update()
    {
        // Down Event
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = GetMousePos();
            _canMove = _collider.OverlapPoint(mousePos);
        }

        // Drag Event
        if (Input.GetMouseButton(0))
        {
            if (_canMove)
            {
                var mousePos = GetMousePos();
                _rb.MovePosition(mousePos);
            }
        }

        Vector2 GetMousePos()
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var clampedMousePos = new Vector2(Mathf.Clamp(mousePos.x, _playerBoundary[0], _playerBoundary[1]),
                Mathf.Clamp(mousePos.y, _playerBoundary[2], _playerBoundary[3]));

            return clampedMousePos;
        }
    }
}