using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Gniling _gnilingPrefab;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private GameOverHolder _gameOverHolder;
    [SerializeField] private Bar01 _health;
    [SerializeField] private Bar01 _psych;
    [SerializeField] private Bar01 _sleep;
    [SerializeField] private Bar01 _joy;
    [SerializeField] private Bar01 _saturation;
    public override void InstallBindings()
    {
        var player = PlayerBinding();

        ProgressBarsSetup(player);

        SpawnerSetup();

        GameOverSetup();

        Debug.Log("Bind");
    }

    private PlayerGnilingBrian PlayerBinding()
    {
        var gniling = Container.InstantiatePrefab(_gnilingPrefab);
        var playerGnilingBrain = Container.InstantiateComponent<PlayerGnilingBrian>(gniling);
        playerGnilingBrain.name = "Player";
        playerGnilingBrain.Init();
        Container.Bind<PlayerGnilingBrian>().FromInstance(playerGnilingBrain).AsSingle();
        return playerGnilingBrain;
    }
    private void ProgressBarsSetup(PlayerGnilingBrian playerGnilingBrain)
    {
        _health.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.PHYSICAL_HELATH).Current);
        _psych.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.PSYCHICAL_HELATH).Current);
        _sleep.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.SLEEP_QUALITY).Current);
        _joy.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.JOY).Current);
        _saturation.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.SATURATION).Current);
    }
    private void SpawnerSetup()
    {
        _spawner.SpawnWillson();
    }
    private void GameOverSetup()
    {
        _gameOverHolder.Init();
    }
}
