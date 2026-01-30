using System.Collections;
using UnityEngine;

public class MushroomDrop : MonoBehaviour
{
    public Transform Transform => _transform;
    public MushroomEffectType EffectType => _type;
    [SerializeField] private MushroomEffectType _type;
    private Transform _transform;
    private Transform _root;
    private float _time;

    public void Init()
    {
        _transform = GetComponent<Transform>();
        _root = _transform.GetChild(0);
    }
    private void Update()
    {
        _time += 2 * Time.deltaTime;
        _root.localPosition = new Vector3(0, 0.25f + 0.2f * Mathf.Sin(_time), 0);
    }
}
public enum MushroomEffectType
{
    Sad,
    Joyful,
    Simple
}
