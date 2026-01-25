using UnityEngine;
using Zenject;

public class GameSceneInstaller : MonoInstaller
{
    [SerializeField] private Gniling _gnilingPrefab;
    [SerializeField] private Bar01 _health;
    [SerializeField] private Bar01 _psych;
    [SerializeField] private Bar01 _sleep;
    [SerializeField] private Bar01 _joy;
    [SerializeField] private Bar01 _saturation;
    public override void InstallBindings()
    {
        var unit = Container.InstantiatePrefab(_gnilingPrefab);
        var player = Container.InstantiateComponent<Player>(unit);
        player.name = "Player";
        player.Init();
        Container.Bind<Player>().FromInstance(player).AsSingle();

        _health.Init(player.Gniling.StatsRepository.GetStat(Stats.PHYSICAL_HELATH).Current);
        _psych.Init(player.Gniling.StatsRepository.GetStat(Stats.PSYCHICAL_HELATH).Current);
        _sleep.Init(player.Gniling.StatsRepository.GetStat(Stats.SLEEP_QUALITY).Current);
        _joy.Init(player.Gniling.StatsRepository.GetStat(Stats.JOY).Current);
        _saturation.Init(player.Gniling.StatsRepository.GetStat(Stats.SATURATION).Current);
    }
}
