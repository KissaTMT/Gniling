using System.Collections;
using UnityEngine;

public class Bed : MonoBehaviour
{
    public bool IsReadyToSleep => _isReset;
    [SerializeField] private ParticleSystem _emission;
    private Transform _transform;
    private Vector3 _initLocalScale;
    private Coroutine _popPup;
    private bool _isGetUp;
    private bool _isReset;
    public void Init()
    {
        _transform = GetComponent<Transform>();
        _initLocalScale = transform.localScale;
        StartCoroutine(PopPupRoutine());
    }
    private void Awake()
    {
        Init();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Gniling gniling))
        {
            if(_popPup == null) _popPup = StartCoroutine(PopPupRoutine());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Gniling gniling))
        {
            if (_popPup != null) _isGetUp = true;
        }
    }
    private IEnumerator PushRoutine()
    {
        var ls = _initLocalScale;
        for (var i = 0f; i < 1f; i += Time.deltaTime)
        {
            _transform.localScale = new Vector3(ls.x, ls.y - 0.1f * i, ls.z);
            yield return null;
        }
        _transform.localScale = _initLocalScale - Vector3.up * 0.1f;
    }
    private IEnumerator GetUpRoutine()
    {
        var ls = _initLocalScale - Vector3.up * 0.1f;
        for (var i = 0f; i < 1f; i += Time.deltaTime)
        {
            _transform.localScale = new Vector3(ls.x, ls.y + 0.1f * i, ls.z);
            yield return null;
        }
        _transform.localScale = _initLocalScale;
        _isGetUp = false;
    }
    private IEnumerator PopPupRoutine()
    {
        _isReset = false;
        _emission.Stop();
        yield return StartCoroutine(PushRoutine());
        yield return new WaitUntil(() => _isGetUp == true);
        yield return StartCoroutine(GetUpRoutine());

        yield return new WaitForSeconds(4);
        _emission.Play();
        _popPup = null;
        _isReset = true;
    }
}
