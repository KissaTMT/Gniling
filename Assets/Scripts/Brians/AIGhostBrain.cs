using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;
using Zenject;

public class AIGhostBrain : MonoBehaviour
{
    public Ghost Ghost => _ghost;

    private Behaviour _behaviour;
    private Ghost _ghost;
    private Gniling _gniling;
    private Vector2 _currentPoint;

    [Inject]
    public void Construct(PlayerGnilingBrian player)
    {
        _gniling = player.Gniling;
        _gniling.StatsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Current.OnChanged += SetBehaviour;
    }

    private void SetBehaviour(float arg1, float arg2)
    {
        if (arg2 < 0.01) _behaviour = Behaviour.Agressive;
        else _behaviour = Behaviour.Passive;
    }

    public void Init()
    {
        _ghost = GetComponent<Ghost>();
        _ghost.Init();
        _behaviour = Behaviour.Passive;
        StartCoroutine(SetPointRoutine());
    }
    public void Update()
    {
        _ghost.SetMovementDirection(CalculateDirection());
        _ghost.Tick();
        if (_behaviour == Behaviour.Passive) return;
        if ((_ghost.Transform.position - _gniling.Transform.position).sqrMagnitude < 2 * 2f) _ghost.Attack(_gniling);
    }
    private void OnDisable()
    {
        _gniling.StatsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Current.OnChanged -= SetBehaviour;
    }
    private IEnumerator SetPointRoutine()
    {
        while (true)
        {
            if (_behaviour == Behaviour.Passive)
            {
                _currentPoint = GetRandomPoint();
                yield return new WaitUntil(() => ((Vector2)_ghost.Transform.position - _currentPoint).sqrMagnitude < 0.01f);
                yield return new WaitForSeconds(3);
            }
            if(_behaviour == Behaviour.Agressive)
            {
                var delta = _gniling.Transform.position - _ghost.Transform.position;
                delta.Normalize();
                _currentPoint = _gniling.Transform.position - delta * 1.5f;
            }
            yield return null;
        }
    }

    private Vector2 GetRandomPoint() => new Vector2(Random.Range(-16, 16), Random.Range(-8, 8));

    private Vector2 CalculateDirection()
    {
        var delta = _currentPoint - (Vector2)_ghost.Transform.position;
        return delta.sqrMagnitude > 1 ? delta.normalized : delta;
    }
    private enum Behaviour
    {
        Agressive,
        Passive
    }
}
