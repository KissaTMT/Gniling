using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class PlayerGnilingBrian : MonoBehaviour
{
    public Transform Transform => Gniling.Transform;
    public Gniling Gniling => _gniling;

    private Vector2 _currentPoint;

    private InputHandler _input;

    private Gniling _gniling;

    private Camera _main;


    [Inject]
    public void Construct(InputHandler input)
    {
        _input = input;
        
    }
    public void Init()
    {
        _input.Reset();

        _gniling = GetComponent<Gniling>();
        _gniling.Init();

        PointReset(_gniling.Transform.position);

        _input.OnGetScreenPosition += SetCurrentPoint;
        _gniling.OnPointReset += PointReset;

        _main = Camera.main;
    }

    private void PointReset(Vector2 point)
    {
        _currentPoint = point;
    }

    private void OnDisable()
    {
        _input.OnGetScreenPosition -= SetCurrentPoint;
        _gniling.OnPointReset -= PointReset;
    }
    private void SetCurrentPoint(Vector2 point)
    {
        _currentPoint = _main.ScreenToWorldPoint(point);
    }

    private void Update()
    {
        _gniling.SetMovementDirection(CalculateDirection());
        _gniling.Tick();
    }
    private Vector2 CalculateDirection()
    {
        var delta = _currentPoint - (Vector2)Transform.position;
        return delta.sqrMagnitude > 1 ? delta.normalized : delta;
    }
}
