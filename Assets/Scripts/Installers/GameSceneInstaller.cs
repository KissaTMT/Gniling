using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Gniling _gnilingPrefab;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private GameOverHolder _gameOverHolder;
    [SerializeField] private PauseHolder _pauseHolder;
    [SerializeField] private ProgressBar _health;
    [SerializeField] private ProgressBar _psych;
    [SerializeField] private ProgressBar _sleep;
    [SerializeField] private ProgressBar _joy;
    [SerializeField] private ProgressBar _saturation;
    public override void InstallBindings()
    {
        var player = PlayerBinding();

        ProgressBarsSetup(player);

        SpawnerSetup();

        PauseSetup();

        GameOverSetup();
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
        _health.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.PHYSICAL_HEALTH).Current);
        _psych.Init(playerGnilingBrain.Gniling.StatsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Current);
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
    private void PauseSetup()
    {
        _pauseHolder.Init();
    }
}
