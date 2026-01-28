using UnityEngine;

public class Willson : MonoBehaviour
{
    private Transform _transform;
    private Transform _root;
    private Rigidbody2D _rb;
    private float _addedForce;
    private Vector3 _currentDirection;
    public void Init()
    {
        _transform = GetComponent<Transform>();
        _root = _transform.GetChild(0);
        _rb = GetComponent<Rigidbody2D>();
    }
    public void AddForce(Vector3 direction)
    {
        _currentDirection = direction.normalized;
        _addedForce = direction.magnitude;
        _rb.AddForce(Vector2.zero);
        _rb.AddForce(_currentDirection * _addedForce * 200);
        
    }
    private void Update()
    {
        _rb.linearVelocity = Vector2.ClampMagnitude(_rb.linearVelocity, 5);
        if (_rb.linearVelocity == Vector2.zero) return;
        Rotate();
    }
    private void Rotate()
    {
        _root.Rotate(0, 0, Mathf.Sign(-_rb.linearVelocity.x)-32 * _rb.linearVelocity.magnitude * Time.deltaTime);
    }
}
