using System;
using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Gniling : MonoBehaviour
{
    public StatsRepository StatsRepository => _statsRepository;
    public Transform Transform => _transform;

    public event Action OnSleep;
    public event Action OnRise;
    public event Action OnDeath;

    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _impulseForce = 5; 

    private Transform _transform;
    private Transform _root;
    private Animator _animator;

    private Vector3 _movementDirection;

    private StatsRepository _statsRepository;

    private Coroutine _death;
    private Coroutine _mad;
    public void Init()
    {
        _transform = GetComponent<Transform>();
        _root = _transform.GetChild(0);
        _animator = GetComponent<Animator>();

        _statsRepository = new StatsRepository();

        _statsRepository.Regesrty(Stats.PHYSICAL_HEALTH, new Stat01(0.5f));
        _statsRepository.Regesrty(Stats.PSYCHICAL_HEALTH, new Stat01(0.5f));
        _statsRepository.Regesrty(Stats.SATURATION, new Stat(0.5f));
        _statsRepository.Regesrty(Stats.SLEEP_QUALITY, new Stat(0.5f));
        _statsRepository.Regesrty(Stats.JOY, new Stat(0.5f));

        _statsRepository.GetStat(Stats.PHYSICAL_HEALTH).Current.OnChanged += HealthChangeHandler;

        SetupStatsInfluence();
    }

    
    public void Tick()
    {
        GetSad(0.004f);

        _animator.SetBool("IS_MOVE", _movementDirection.sqrMagnitude > 0.1f);

        if (_movementDirection == Vector3.zero) return;

        Move();
        Flip(_movementDirection.x);

        GetHungry(0.005f);
        GetTired(0.005f);
    }
    public void SetMovementDirection(Vector3 direction)
    {
        _movementDirection = direction;
    }
    private void SetupStatsInfluence()
    {
        _statsRepository.GetStat(Stats.SATURATION).Current.OnChanged += SaturationInfluence;

        _statsRepository.GetStat(Stats.JOY).Current.OnChanged += JoynInfluence;

        _statsRepository.GetStat(Stats.SLEEP_QUALITY).Current.OnChanged += SleepInfluence;
    }
    
    private void HealthChangeHandler(float oldVal, float newVal)
    {
        if (newVal == 0)
        {
            OnDeath?.Invoke();
            _movementDirection = Vector2.zero;
            _movementSpeed = 0;
        }
    }

    private void Move()
    {
        _transform.position += _movementDirection * _movementSpeed * Time.deltaTime;
    }
    private void Flip(float sign)
    {
        _root.localScale = new Vector3(Mathf.Sign(sign) * Mathf.Abs(_root.localScale.x), _root.localScale.y, _root.localScale.z);
    }
    private void GetHungry(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.SATURATION).Reduce(value * Time.deltaTime);
    }
    private void GetJoy(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.JOY).Add(value * Time.deltaTime);
    }
    private void GetTired(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.SLEEP_QUALITY).Reduce(value * Time.deltaTime);
    }
    private void GetSad(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.JOY).Reduce(value * Time.deltaTime);
    }
    private void GetRest(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.SLEEP_QUALITY).Add(value * Time.deltaTime);
    }
    private void GetDeath(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.PHYSICAL_HEALTH).Reduce(value * Time.deltaTime);
    }
    private void GetMad(float value = 0.001f)
    {
        _statsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Reduce(value * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out MushroomDrop mushroom))
        {
            switch (mushroom.EffectType)
            {
                case MushroomEffectType.Simple:
                    _statsRepository.GetStat(Stats.SATURATION).Add(0.1f);
                    break;
                case MushroomEffectType.Joyful:
                    _statsRepository.GetStat(Stats.SATURATION).Add(0.1f);
                    _statsRepository.GetStat(Stats.JOY).Add(0.25f);
                    break;
                case MushroomEffectType.Sad:
                    _statsRepository.GetStat(Stats.SATURATION).Add(0.1f);
                    _statsRepository.GetStat(Stats.JOY).Reduce(0.25f);
                    break;
            }
            Destroy(collision.gameObject);
        }
        if(collision.TryGetComponent(out Willson willson)){
            var impulse = (_movementDirection != Vector3.zero) ? _movementDirection : new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f));
            willson.SetImpulse(impulse.normalized * _impulseForce);
            _statsRepository.GetStat(Stats.JOY).Add(0.025f);
        }
        if(collision.TryGetComponent(out Bed bed))
        {
            OnSleep?.Invoke();
            StartCoroutine(SleepRoutine());
        }
        if(collision.TryGetComponent(out Water water))
        {
            _movementSpeed /= 2;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Water water))
        {
            _movementSpeed *= 2;
        }
    }
    private IEnumerator GetDeathRoutine()
    {
        var sr = _statsRepository;
        while (true)
        {
            var power = 0.004f + 0.0015f * (sr.IsOutOfRange(Stats.SLEEP_QUALITY) + sr.IsOutOfRange(Stats.SATURATION));
            GetDeath(power);
            yield return null;
        }
    }
    private IEnumerator GetMadRoutine()
    {
        var sr = _statsRepository;
        Debug.Log("mr_in");
        while (true)
        {
            var power = 0.004f + 0.0015f * (sr.IsOutOfRange(Stats.SLEEP_QUALITY) + sr.IsOutOfRange(Stats.JOY));
            GetMad(power);
            yield return null;
        }
    }
    private IEnumerator GetChangeRoutine(Stat stat, float value)
    {
        while (true)
        {
            stat.Change(value * Time.deltaTime);
            yield return null;
        }
    }
    private IEnumerator SleepRoutine()
    {
        var stat =_statsRepository.GetStat(Stats.SLEEP_QUALITY);
        var isClamping = stat.Current.Value < 0.9f;
        for(var i = 0f; i < 1; i += Time.deltaTime)
        {
            GetRest(0.25f);
            yield return null;
            if(isClamping && stat.Current.Value > 1)
            {
                stat.Current.Value = 1;
                break;
            } 

        }
        yield return new WaitForSeconds(2);

        OnRise?.Invoke();
    }
    private void SaturationInfluence(float oldValue, float newValue)
    {
        var diff = newValue - oldValue;

        if (_statsRepository.IsOutOfRange(Stats.SATURATION) == 1)
        {
            if (_death == null) _death = StartCoroutine(GetDeathRoutine());
            return;
        }
        else
        {
            if (_death != null)
            {
                StopCoroutine(_death);
                _death = null;
            }
        }

        _statsRepository.GetStat(Stats.PHYSICAL_HEALTH).Change(diff / 2);
    }
    private void JoynInfluence(float oldValue, float newValue)
    {
        var diff = newValue - oldValue;

        if (_statsRepository.IsOutOfRange(Stats.JOY) == 1)
        {
            if (_mad == null) _mad = StartCoroutine(GetMadRoutine());
            return;
        }
        else
        {
            if (_mad != null)
            {
                StopCoroutine(_mad);
                _mad = null;
            }
        }
        _statsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Change(diff / 2);
    }
    private void SleepInfluence(float oldValue, float newValue)
    {
        var diff = newValue - oldValue;

        if (_statsRepository.IsOutOfRange(Stats.SLEEP_QUALITY) == 1)
        {
            if (_death == null) _death = StartCoroutine(GetDeathRoutine());
            if (_mad == null)
            {
                _mad = StartCoroutine(GetMadRoutine());
                Debug.Log($"mr_start {_mad}");
            }
            return;
        }
        else
        {
            if (_death != null)
            {
                StopCoroutine(_death);
                _death = null;
            }
            if (_mad != null)
            {
                StopCoroutine(_mad);
                _mad = null;
                Debug.Log($"mr_end {_mad}");
            }
        }
        _statsRepository.GetStat(Stats.PSYCHICAL_HEALTH).Change(diff / 2);
        _statsRepository.GetStat(Stats.PHYSICAL_HEALTH).Change(diff / 2);
    }
    private void OnDisable()
    {
        _statsRepository.GetStat(Stats.SATURATION).Current.OnChanged -= SaturationInfluence;

        _statsRepository.GetStat(Stats.JOY).Current.OnChanged -= JoynInfluence;

        _statsRepository.GetStat(Stats.SLEEP_QUALITY).Current.OnChanged -= SleepInfluence;

        _statsRepository.GetStat(Stats.PHYSICAL_HEALTH).Current.OnChanged -= HealthChangeHandler;
    }
}