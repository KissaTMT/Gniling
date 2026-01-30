using UnityEngine;
using Zenject;

public class Ghost : MonoBehaviour
{
    public Transform Transform => _transform;
    [SerializeField] private float _movementSpeed = 6;

    private Vector3 _movementDirection;

    private Transform _transform;
    private Transform _root;
    public void Init()
    {
        _transform = GetComponent<Transform>();
        _root = _transform.GetChild(0);
    }
    public void SetMovementDirection(Vector3 direction)
    {
        _movementDirection = direction;
    }
    public void Tick()
    {
        if(_movementDirection == Vector3.zero) return;
        Move();
        Flip(_movementDirection.x);
    }
    private void Move()
    {
        _transform.position += _movementDirection * _movementSpeed * Time.deltaTime;
    }
    private void Flip(float sign)
    {
        _root.localScale = new Vector3(Mathf.Sign(sign) * Mathf.Abs(_root.localScale.x), _root.localScale.y, _root.localScale.z);
    }
    public void Attack(Gniling gniling)
    {
        gniling.StatsRepository.GetStat(Stats.PHYSICAL_HEALTH).Reduce(0.1f * Time.deltaTime);
    }
}
