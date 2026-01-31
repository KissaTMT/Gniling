using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnilingStatsHandler : IDisposable
{
    private StatsRepository _statsRepository;
    private Gniling _gniling;
    private Dictionary<Stat, List<Coroutine>> _routines;
    public GnilingStatsHandler(Gniling gniling)
    {
        _gniling = gniling;
        _statsRepository = gniling.StatsRepository;

        _routines = new Dictionary<Stat, List<Coroutine>>();
        _routines[_statsRepository.GetStat(Stats.SATURATION)] = new List<Coroutine>(1);
        _routines[_statsRepository.GetStat(Stats.SLEEP_QUALITY)] = new List<Coroutine>(2);
        _routines[_statsRepository.GetStat(Stats.JOY)] = new List<Coroutine>(1);

        SetupStatsInfluence();
    }
    public void GetHungry(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.SATURATION).Reduce(value * Time.deltaTime);
    }
    public void GetJoy(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.JOY).Add(value * Time.deltaTime);
    }
    public void GetTired(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.SLEEP_QUALITY).Reduce(value * Time.deltaTime);
    }
    public void GetSad(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.JOY).Reduce(value * Time.deltaTime);
    }
    public void GetRest(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.SLEEP_QUALITY).Add(value * Time.deltaTime);
    }
    public void GetDeath(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.PHYSICAL_HEALTH).Reduce(value * Time.deltaTime);
    }
    public void GetMad(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Reduce(value * Time.deltaTime);
    }
    private void SetupStatsInfluence()
    {
        _statsRepository.GetStat(Stats.SATURATION).Current.OnChanged += SaturationInfluence;

        _statsRepository.GetStat(Stats.JOY).Current.OnChanged += JoynInfluence;

        _statsRepository.GetStat(Stats.SLEEP_QUALITY).Current.OnChanged += SleepInfluence;
    }
    private void SaturationInfluence(float oldValue, float newValue)
    {
        var diff = newValue - oldValue;

        if (_statsRepository.IsOutOfRange(Stats.SATURATION) == 1)
        {
            var stat = _statsRepository.GetStat(Stats.SATURATION);
            if (_routines[stat].Count > 0) return;
            _routines[stat].Add(_gniling.StartCoroutine(GetChangeRoutine(_statsRepository.GetStat(Stats.PHYSICAL_HEALTH))));
        }
        else
        {
            var stat = _statsRepository.GetStat(Stats.SATURATION);
            if (_routines[stat].Count > 0)
            {
                _gniling.StopCoroutine(_routines[stat][0]);
                _routines[stat].Clear();
            }
        }

        _statsRepository.GetStat(Stats.PHYSICAL_HEALTH).Change(diff);
    }
    private void JoynInfluence(float oldValue, float newValue)
    {
        var diff = newValue - oldValue;

        if (_statsRepository.IsOutOfRange(Stats.JOY) == 1)
        {
            var stat = _statsRepository.GetStat(Stats.JOY);
            if (_routines[stat].Count > 0) return;
            _routines[stat].Add(_gniling.StartCoroutine(GetChangeRoutine(_statsRepository.GetStat(Stats.PSYCHICAL_HEALTH))));
        }
        else
        {
            var stat = _statsRepository.GetStat(Stats.JOY);
            if (_routines[stat].Count > 0)
            {
                _gniling.StopCoroutine(_routines[stat][0]);
                _routines[stat].Clear();
            }
        }
        _statsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Change(diff);
    }
    private void SleepInfluence(float oldValue, float newValue)
    {
        var diff = newValue - oldValue;

        if (_statsRepository.IsOutOfRange(Stats.SLEEP_QUALITY) == 1)
        {
            var stat = _statsRepository.GetStat(Stats.SLEEP_QUALITY);
            if (_routines[stat].Count > 0) return;
            _routines[stat].Add(_gniling.StartCoroutine(GetChangeRoutine(_statsRepository.GetStat(Stats.PSYCHICAL_HEALTH))));
            _routines[stat].Add(_gniling.StartCoroutine(GetChangeRoutine(_statsRepository.GetStat(Stats.PHYSICAL_HEALTH))));
        }
        else
        {
            var stat = _statsRepository.GetStat(Stats.SLEEP_QUALITY);
            if (_routines[stat].Count > 0)
            {
                _gniling.StopCoroutine(_routines[stat][0]);
                _gniling.StopCoroutine(_routines[stat][1]);
                _routines[stat].Clear();
            }
        }
        _statsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Change(diff / 2);
        _statsRepository.GetStat(Stats.PHYSICAL_HEALTH).Change(diff / 2);
    }
    private IEnumerator GetChangeRoutine(Stat stat, float value = -0.01f)
    {
        while (true)
        {
            stat.Change(value * Time.deltaTime);
            yield return null;
        }
    }
    public void Dispose()
    {
        _statsRepository.GetStat(Stats.SATURATION).Current.OnChanged -= SaturationInfluence;

        _statsRepository.GetStat(Stats.JOY).Current.OnChanged -= JoynInfluence;

        _statsRepository.GetStat(Stats.SLEEP_QUALITY).Current.OnChanged -= SleepInfluence;
    }
}
