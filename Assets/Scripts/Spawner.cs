using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Ghost _ghostPrefab;
    [SerializeField] private Willson _willsonPrefab;
    [SerializeField] private Mushroom[] _mushroomsPrefabs;

    private List<Mushroom> _mushrooms = new();

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
    public void Init()
    {
        SpawnMushrooms();
        SpawnWillson();
    }

    private Ghost SpawnGhost()
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
    private Willson SpawnWillson()
    {
        _willson = Instantiate(_willsonPrefab, Vector2.right * 2, Quaternion.identity);
        _willson.Init();
        return _willson;
    }

    private void SpawnMushrooms()
    {
        var count = Random.Range(3,10);
        for (var a = 0f; a < Mathf.PI * 2; a += (Mathf.PI * 2 / count))
        {
            var position = new Vector2(13 * Mathf.Cos(a) - 1, 6 * Mathf.Sin(a) - 1);
            var mushroom = Instantiate(_mushroomsPrefabs[Random.Range(0,_mushroomsPrefabs.Length)], position, Quaternion.identity);
            mushroom.Init();
            _mushrooms.Add(mushroom);
        }
    }
    
    private void MushromsDrop()
    {
        for (var i = 0; i < _mushrooms.Count; i++)
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
        MushromsDrop();
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
