using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Gniling _gnilingPrefab;
    [SerializeField] private Ghost _ghostPrefab;
    [SerializeField] private Bar01 _health;
    [SerializeField] private Bar01 _psych;
    [SerializeField] private Bar01 _sleep;
    [SerializeField] private Bar01 _joy;
    [SerializeField] private Bar01 _saturation;
    public override void InstallBindings()
    {
        var gniling = Container.InstantiatePrefab(_gnilingPrefab);
        var playerGnilingBrain = Container.InstantiateComponent<PlayerGnilingBrian>(gniling);
        playerGnilingBrain.name = "Player";
        playerGnilingBrain.Init();
        Container.Bind<PlayerGnilingBrian>().FromInstance(playerGnilingBrain).AsSingle();

        var ghost = Container.InstantiatePrefab(_ghostPrefab, new Vector3(0, 6, 6), Quaternion.identity, null);
        var aiGhostBrain = Container.InstantiateComponent<AIGhostBrain>(ghost);
        aiGhostBrain.name = "Ghost";
        aiGhostBrain.Init();
        Container.Bind<AIGhostBrain>().FromInstance(aiGhostBrain).AsSingle();

        _health.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.PHYSICAL_HELATH).Current);
        _psych.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.PSYCHICAL_HELATH).Current);
        _sleep.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.SLEEP_QUALITY).Current);
        _joy.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.JOY).Current);
        _saturation.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.SATURATION).Current);


    }
}
