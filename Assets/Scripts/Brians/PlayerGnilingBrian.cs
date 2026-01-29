using UnityEngine;
using Zenject;

public class PlayerGnilingBrian : MonoBehaviour
{
    public Gniling Gniling => _gniling;

    private Vector2 _currentPoint;

    private InputHandler _input;

    private Gniling _gniling;

    private Camera _main;

    [Inject]
    public void Construct(InputHandler input)
    {
        _input = input;
        _input.OnGetScreenPosition += SetCurrentPoint;
    }
    public void Init()
    {
        _gniling = GetComponent<Gniling>();
        _gniling.Init();
        _main = Camera.main;
    }
    private void OnDisable()
    {
        _input.OnGetScreenPosition -= SetCurrentPoint;
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
        var delta = _currentPoint - (Vector2)_gniling.Transform.position;
        return delta.sqrMagnitude > 1 ? delta.normalized : delta;
    }
}
