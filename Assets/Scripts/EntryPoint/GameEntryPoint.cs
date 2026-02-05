using UnityEngine;
using Zenject;

public class GameEntryPoint : MonoBehaviour
{    
    [SerializeField] private ProgressBar _health;
    [SerializeField] private ProgressBar _psych;
    [SerializeField] private ProgressBar _sleep;
    [SerializeField] private ProgressBar _joy;
    [SerializeField] private ProgressBar _saturation;

    [SerializeField] private Spawner _spawner;
    [SerializeField] private GameOverHolder _gameOverHolder;
    [SerializeField] private PauseHolder _pauseHolder;
    [SerializeField] private Eyelids _eyelids;

    private PlayerGnilingBrian _player;
    [Inject]
    public void Construct(PlayerGnilingBrian player)
    {
        _player = player;
    }
    private void Awake()
    {
        _player.Init();
        ProgressBarsSetup(_player);
        GameOverSetup();
        PauseSetup();
        SpawnerSetup();
        _eyelids.Init();
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
        _spawner.Init();
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
