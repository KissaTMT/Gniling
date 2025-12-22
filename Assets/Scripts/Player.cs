using System;
using System.Diagnostics;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;
    public Vector3 CurrentPoint => _currentPoint;
    public Vector3 MovementDirection => _movementDirection;

    [SerializeField] private float _movementSpeed;
    private Transform _transform;
    private Transform _root;
    private Vector3 _currentPoint;
    private Vector3 _movementDirection;

    private InputHandler _input;
   

    private void Awake()
    {
        instance = this;
        Application.targetFrameRate = 120;

        _transform = GetComponent<Transform>();
        _root = _transform.GetChild(0);
        _input = new InputHandler();

        _currentPoint = _transform.position;
    }
    private void OnEnable()
    {
        _input.OnGetPosition += SetCurrentPoint;
    }
    private void OnDisable()
    {
        _input.OnGetPosition -= SetCurrentPoint;

        _input.Dispose();
    }
    private void SetCurrentPoint(Vector3 point)
    {
        _currentPoint = point;
    }

    private void Update()
    {
        CalculateDirection();
        Move();
        Flip(_movementDirection.x);
    }
    private void CalculateDirection()
    {
        var delta = _currentPoint - _transform.position;
        _movementDirection = delta.sqrMagnitude > 1 ? delta.normalized : delta;
    }
    private void Move()
    {
        _transform.position += _movementDirection * _movementSpeed * Time.deltaTime;
    }
    private void Flip(float sign)
    {
        _root.localScale = new Vector3(Mathf.Sign(sign) * Mathf.Abs(_root.localScale.x), _root.localScale.y, _root.localScale.z);
    }
}
