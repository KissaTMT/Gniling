using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Ghost _ghostPrefab;
    [SerializeField] private Willson _willsonPrefab;
    [SerializeField] private Mushroom[] _mushrooms;
    private Gniling _gniling;
    private Ghost _ghost;
    private Willson _willson;
    private DiContainer _di;


    [Inject]
    public void Construct(PlayerGnilingBrian player, DiContainer di)
    {
        _gniling = player.Gniling;
        _di = di;

        _gniling.OnRise += OnRiseHandler;
        _gniling.StatsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Current.OnChanged += SpawnGhostHanlder;
    }

    public Ghost SpawnGhost()
    {
        if (_ghost != null) return _ghost;
        var ghost = _di.InstantiatePrefab(_ghostPrefab, new Vector2(Random.Range(-15f, 15f), 12), Quaternion.identity, null);
        var aiGhostBrain = _di.InstantiateComponent<AIGhostBrain>(ghost);
        aiGhostBrain.name = "Ghost";
        aiGhostBrain.Init();
        _di.Bind<AIGhostBrain>().FromInstance(aiGhostBrain).AsSingle();
        _ghost = ghost.GetComponent<Ghost>();
        return _ghost;
    }
    public Willson SpawnWillson()
    {
        _willson = Instantiate(_willsonPrefab, Vector2.right * 2, Quaternion.identity);
        _willson.Init();
        return _willson;
    }
    private void MusshromsDrop()
    {
        for (var i = 0; i < _mushrooms.Length; i++)
        {
            var item = _mushrooms[i];
            if (Random.value > item.GetDropProbability()) item.Drop();
        }
    }
    private void OnRiseHandler()
    {
        WaterDrop();
        _willson.Transform.position = Vector2.right * 2;
    }
    private void WaterDrop()
    {
        MusshromsDrop();
    }
    private void OnDisable()
    {
        _gniling.OnRise -= OnRiseHandler;
        _gniling.StatsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Current.OnChanged -= SpawnGhostHanlder;
    }
    private void SpawnGhostHanlder(float oldV, float newV)
    {
        if(newV < 0.2f) SpawnGhost();
    }
}
