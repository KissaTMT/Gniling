using UnityEngine;

public class Gniling : MonoBehaviour
{
    public StatsRepository StatsRepository => _statsRepository;
    public Transform Transform => _transform;
    [SerializeField] private float _movementSpeed = 6;

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

        SetupStatsInfluence();
    }
    public void Tick()
    {
        GetHungry();
        GetTired();
        GetSad();

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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out Mushroom mushroom))
        {
            switch (mushroom.EffectType)
            {
                case MushroomEffectType.Simple:
                    _statsRepository.GetStat(Stats.SATURATION).Add(0.1f);
                    break;
                case MushroomEffectType.Hallucinogenic:
                    _statsRepository.GetStat(Stats.SATURATION).Add(0.1f);
                    _statsRepository.GetStat(Stats.JOY).Add(0.25f);
                    break;
            }
            Destroy(collision.gameObject);
        }
        if(collision.TryGetComponent(out Willson willson)){
            willson.AddForce(((_movementDirection != Vector3.zero) ? _movementDirection : new Vector3(Random.Range(0f,1f),Random.Range(0f,1f)).normalized));
            _statsRepository.GetStat(Stats.JOY).Add(0.05f);
        }
    }
}