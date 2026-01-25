using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public Transform Transform => _transform;
    public MushroomEffectType EffectType => _type;
    [SerializeField] private MushroomEffectType _type;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }
}
public enum MushroomEffectType
{
    Simple,
    Hallucinogenic
}
