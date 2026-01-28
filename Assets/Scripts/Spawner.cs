using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Ghost _ghostPrefab;
    [SerializeField] private Willson _willsonPrefab;
    [SerializeField] private Mushroom _edibleMushroomPrefab;
    [SerializeField] private Mushroom _joyfulMushroomPrefab;
    [SerializeField] private Mushroom _sadMushroomPrefab;
    private Gniling _gniling;
    private Ghost _ghost;
    private Willson _willson;
    private DiContainer _di;


    [Inject]
    public void Construct(PlayerGnilingBrian player, DiContainer di)
    {
        _gniling = player.Gniling;
        _di = di;

        _gniling.OnRise += WaterDrop;
        _gniling.StatsRepository.GetStat(Stats.PSYCHICAL_HELATH).Current.OnChanged += SpawnGhostHanlder;
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
    public Mushroom SpawnEdibleMushroom()
    {
        return Instantiate(_edibleMushroomPrefab, new Vector2(Random.Range(-15f, 15f), 5), Quaternion.identity);
    }
    public Mushroom SpawnJoyfulMushroom()
    {
        return Instantiate(_joyfulMushroomPrefab, new Vector2(Random.Range(-15f, 15f), 5), Quaternion.identity);
    }
    public Mushroom SpawnSadMushroom()
    {
        return Instantiate(_sadMushroomPrefab, new Vector2(Random.Range(-15f, 15f), 5), Quaternion.identity);
    }
    public Willson SpawnWillson()
    {
        _willson = Instantiate(_willsonPrefab, new Vector2(Random.Range(-15f, 15f), 5), Quaternion.identity);
        _willson.Init();
        return _willson;
    }
    private void WaterDrop()
    {
        var rnd = Random.value;
        if (rnd < 0.2f) SpawnEdibleMushroom();
        else if(rnd >= 0.2f && rnd < 0.6f) SpawnJoyfulMushroom();
        else SpawnSadMushroom();
    }
    private void OnDisable()
    {
        _gniling.OnRise -= WaterDrop;
        _gniling.StatsRepository.GetStat(Stats.PSYCHICAL_HELATH).Current.OnChanged -= SpawnGhostHanlder;
    }
    private void SpawnGhostHanlder(float oldV, float newV)
    {
        if(newV < 0.2f) SpawnGhost();
    }
}
