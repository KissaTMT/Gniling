using System.Collections;
using UnityEngine;

public class Bed : MonoBehaviour
{
    private Transform _transform;
    private Vector3 _initLocalScale;
    public void Init()
    {
        _transform = GetComponent<Transform>();
        _initLocalScale = transform.localScale;
    }
    private void Awake()
    {
        Init();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Gniling gniling))
        {
            StartCoroutine(PushRoitine());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Gniling gniling))
        {
            StartCoroutine(GetUpRoutine());
        }
    }
    private IEnumerator PushRoitine()
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
    }
}
