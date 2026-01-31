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
    public event Action<Vector2> OnPointReset;

    [SerializeField] private float _movementSpeed = 5;
    [SerializeField] private float _impulseForce = 5; 

    private Transform _transform;
    private Transform _root;
    private Animator _animator;

    private Vector3 _movementDirection;
    private float _currentMovementSpeed;
    private bool _inWater;

    private StatsRepository _statsRepository;
    private GnilingStatsHandler _statsHandler;
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

        _statsHandler = new GnilingStatsHandler(this);
    }

    
    public void Tick()
    {
        _statsHandler.GetSad(0.004f);

        _animator.SetBool("IS_MOVE", _movementDirection.sqrMagnitude > 0.1f);

        if (_movementDirection == Vector3.zero) return;

        if (_inWater)
        {
            _currentMovementSpeed = _movementSpeed / 2;
            _statsHandler.GetSad(0.02f);
            _statsHandler.GetHungry(0.01f);
            _statsHandler.GetTired(0.01f);
        }
        else _currentMovementSpeed = _movementSpeed;

            Move();
        Flip(_movementDirection.x);

        _statsHandler.GetHungry(0.005f);
        _statsHandler.GetTired(0.005f);
    }
    public void SetMovementDirection(Vector3 direction)
    {
        _movementDirection = direction;
    }
    
    
    private void HealthChangeHandler(float oldVal, float newVal)
    {
        if (newVal == 0)
        {
            OnDeath?.Invoke();
            OnPointReset?.Invoke(_transform.position);
            _movementDirection = Vector2.zero;
            _currentMovementSpeed = 0;
        }
    }

    private void Move()
    {
        _transform.position += _movementDirection * _currentMovementSpeed * Time.deltaTime;
    }
    private void Flip(float sign)
    {
        _root.localScale = new Vector3(Mathf.Sign(sign) * Mathf.Abs(_root.localScale.x), _root.localScale.y, _root.localScale.z);
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
            OnPointReset?.Invoke(_transform.position);
            var impulse = (_movementDirection != Vector3.zero) ? _movementDirection : new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f));
            willson.SetImpulse(impulse.normalized * _impulseForce);
            _statsRepository.GetStat(Stats.JOY).Add(0.025f);
        }
        if(collision.TryGetComponent(out Bed bed))
        {
            OnSleep?.Invoke();
            StartCoroutine(SleepRoutine());
        }
        if (collision.TryGetComponent(out Water water)) _inWater = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Water water)) _inWater = false;
    }
    
    private IEnumerator SleepRoutine()
    {
        var stat =_statsRepository.GetStat(Stats.SLEEP_QUALITY);
        var isClamping = stat.Current.Value < 0.9f;
        for(var i = 0f; i < 1; i += Time.deltaTime)
        {
            _statsHandler.GetRest(0.25f);
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
        _statsRepository.GetStat(Stats.PHYSICAL_HEALTH).Current.OnChanged -= HealthChangeHandler;
        _statsHandler.Dispose();
    }
}