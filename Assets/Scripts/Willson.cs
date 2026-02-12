using System.Collections;
using UnityEngine;

public class Willson : MonoBehaviour
{
    private const string GROUND = "Ground";
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
        if (collision.GetComponentInParent<Water>())
        {
            if(_return == null)_return = StartCoroutine(ReturnRoutine(-2 * _currentDirection));
        }
        if (collision.tag == GROUND)
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
        var slowdown = 1/_speed;
        var initSpeed = _speed/2;
        for(var i = 0f; i < 1; i += 0.01f * slowdown * Time.deltaTime)
        {
            _currentDirection += direction * i;
            _speed = initSpeed;
            yield return null;
        }
        _currentDirection.Normalize();
        _speed = initSpeed*2;
        while (true)
        {
            _speed = initSpeed*2;
            yield return null;
        }
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
