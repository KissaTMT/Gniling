using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    [SerializeField] private MushroomDrop _drop;
    private Transform _transform;
    public void Init()
    {
        _transform = GetComponent<Transform>();
    }
    public void Drop()
    {
        Instantiate(_drop, _transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 0.5f)), Quaternion.identity).Init();
    }
    public float GetDropProbability() => (0.6f + (int) _drop.EffectType * 0.1f);
    private void Awake()
    {
        Init();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent(out Willson willson))
        {
            StartCoroutine(PumpRoutine());
            if (Random.value > GetDropProbability()) Drop();
        }
    }
    private IEnumerator PumpRoutine()
    {
        var ls = _transform.localScale;
        for (var i = 0f; i < Mathf.PI; i += 4 * Time.deltaTime)
        {
            _transform.localScale = new Vector3(ls.x, ls.y - 0.1f * Mathf.Sin(i), ls.z);
            yield return null;
        }
        _transform.localScale = ls;
    }
}