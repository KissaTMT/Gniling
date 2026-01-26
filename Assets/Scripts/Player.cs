using UnityEngine;
using Zenject;

public class Player : MonoBehaviour
{
    public Gniling Gniling => _gniling;

    private Vector2 _currentPoint;

    private InputHandler _input;

    private Gniling _gniling;

    [Inject]
    public void Construct(InputHandler input)
    {
        _input = input;
        _input.OnGetPosition += SetCurrentPoint;
    }
    public void Init()
    {
        _gniling = GetComponent<Gniling>();
        _gniling.Init();
    }
    private void OnDisable()
    {
        _input.OnGetPosition -= SetCurrentPoint;
        _input.Dispose();
    }
    private void SetCurrentPoint(Vector2 point)
    {
        _currentPoint = point;
    }

    private void Update()
    {
        _gniling.SetMovementDirection(CalculateDirection());
        _gniling.Tick();
    }
    private Vector2 CalculateDirection()
    {
        var delta = _currentPoint - (Vector2)_gniling.Transform.position;
        return delta.sqrMagnitude > 1 ? delta.normalized : delta;
    }
}
