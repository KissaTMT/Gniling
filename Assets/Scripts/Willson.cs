using System.Collections;
using UnityEngine;

public class Willson : MonoBehaviour
{
    public Transform Transform => _transform;
    private Transform _transform;
    private Transform _root;
    private Vector3 _currentDirection;
    private float _speed;
    private Coroutine _return;
    public void Init()
    {
        _transform = GetComponent<Transform>();
        _root = _transform.GetChild(0);
    }
    public void SetImpulse(Vector2 direction)
    {
        if (_return != null) return;
        _currentDirection = direction.normalized;
        _speed = direction.magnitude;
        
    }
    private void Update()
    {
        Damping();

        if (_speed == 0) return;

        Move();
        Rotate();
    }
    private void Move()
    {
        _transform.position += _currentDirection * _speed * Time.deltaTime;
    }
    private void Rotate()
    {
        _root.Rotate(0, 0, -Mathf.Sign(_currentDirection.x) * 32 * _speed * Time.deltaTime);
    }
    private void Damping(float power = 1.25f)
    {
        _speed = Mathf.Max(0, _speed - 1.25f * Time.deltaTime);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_speed == 0) return;
        var normal = collision.contacts[0].normal;
        _speed *= 0.8f;
        SetImpulse(Vector3.Reflect(_currentDirection, normal).normalized * _speed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Water water))
        {
            _return = StartCoroutine(ReturnRoutine(Vector3.down));
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Water water))
        {
            if (_return != null)
            {
                StopCoroutine(_return);
                _return = null;
            }
        }
    }
    private IEnumerator ReturnRoutine(Vector3 direction)
    {
        var initSpeed = 1/_speed;
        for(var i = 0f; i < 1; i += 0.01f * initSpeed * Time.deltaTime)
        {
            _currentDirection = (_currentDirection + direction * i).normalized;
            _speed = 1;
            yield return null;
        }
        while (true)
        {
            _speed = 6;
            yield return new WaitForSeconds(2);
        }
    }
}
