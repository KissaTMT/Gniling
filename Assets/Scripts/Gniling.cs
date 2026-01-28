using System;
using System.Collections;
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

    private Transform _transform;
    private Transform _root;

    private Vector3 _movementDirection;

    private StatsRepository _statsRepository;
    public void Init()
    {
        _transform = GetComponent<Transform>();
        _root = _transform.GetChild(0);

        _statsRepository = new StatsRepository();

        _statsRepository.Regesrty(Stats.PHYSICAL_HELATH, new Stat01(0.5f));
        _statsRepository.Regesrty(Stats.PSYCHICAL_HELATH, new Stat01(0.5f));
        _statsRepository.Regesrty(Stats.SATURATION, new Stat(0.5f));
        _statsRepository.Regesrty(Stats.SLEEP_QUALITY, new Stat(0.5f));
        _statsRepository.Regesrty(Stats.JOY, new Stat(0.5f));

        _statsRepository.GetStat(Stats.PHYSICAL_HELATH).Current.OnChanged += HealthChangeHandler;

        SetupStatsInfluence();
    }

    
    public void Tick()
    {
        GetSad(0.004f);

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
        _statsRepository.GetStat(Stats.SATURATION).Current.OnChanged += (oldValue, newValue) =>
        {
            var diff = newValue - oldValue;

            if (oldValue > 1 && diff < 0) return;
            if (newValue > 1)
            {
                _statsRepository.GetStat(Stats.PHYSICAL_HELATH).Reduce(Mathf.Abs(diff) / 2);
                return;
            }

            if (diff > 0) _statsRepository.GetStat(Stats.PHYSICAL_HELATH).Add(Mathf.Abs(diff) / 2);
            else _statsRepository.GetStat(Stats.PHYSICAL_HELATH).Reduce(Mathf.Abs(diff) / 2);
        };

        _statsRepository.GetStat(Stats.JOY).Current.OnChanged += (oldValue, newValue) =>
        {
            var diff = newValue - oldValue;

            if (oldValue > 0.8f && diff < 0) return;
            if (newValue > 1)
            {
                _statsRepository.GetStat(Stats.PSYCHICAL_HELATH).Reduce(Mathf.Abs(diff) / 2);
                return;
            }
            if (diff > 0) _statsRepository.GetStat(Stats.PSYCHICAL_HELATH).Add(Mathf.Abs(diff) / 2);
            else _statsRepository.GetStat(Stats.PSYCHICAL_HELATH).Reduce(Mathf.Abs(diff) / 2);
        };
        _statsRepository.GetStat(Stats.SLEEP_QUALITY).Current.OnChanged += (oldValue, newValue) =>
        {
            var diff = newValue - oldValue;

            if (oldValue > 1 && diff < 0) return;

            if (newValue > 1)
            {
                _statsRepository.GetStat(Stats.PSYCHICAL_HELATH).Reduce(Mathf.Abs(diff) / 2);
                _statsRepository.GetStat(Stats.PHYSICAL_HELATH).Reduce(Mathf.Abs(diff) / 2);
                _statsRepository.GetStat(Stats.JOY).Reduce(Mathf.Abs(diff) / 2);
                _statsRepository.GetStat(Stats.SATURATION).Reduce(Mathf.Abs(diff) / 2);
                return; 
            }

            if (diff > 0)
            {
                _statsRepository.GetStat(Stats.PSYCHICAL_HELATH).Add(Mathf.Abs(diff)/2);
                _statsRepository.GetStat(Stats.PHYSICAL_HELATH).Add(Mathf.Abs(diff)/2);
            }
            else
            {
                _statsRepository.GetStat(Stats.PSYCHICAL_HELATH).Reduce(Mathf.Abs(diff)/2);
                _statsRepository.GetStat(Stats.PHYSICAL_HELATH).Reduce(Mathf.Abs(diff)/2);
            }
        };
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Mushroom mushroom))
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
            willson.AddForce(((_movementDirection != Vector3.zero) ? _movementDirection : new Vector3(Random.Range(0f,1f),Random.Range(0f,1f)).normalized));
            _statsRepository.GetStat(Stats.JOY).Add(0.025f);
        }
        if(collision.TryGetComponent(out Bed bed))
        {
            OnSleep?.Invoke();
            StartCoroutine(SleepRoutine());
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
    private void OnDisable()
    {
        _statsRepository.GetStat(Stats.PHYSICAL_HELATH).Current.OnChanged -= HealthChangeHandler;
    }
}