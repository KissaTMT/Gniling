using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public Transform Transform => _transform;
    public MushroomEffectType EffectType => _type;
    [SerializeField] private MushroomEffectType _type;
    private Transform _transform;

    public void Init()
    {
        _transform = GetComponent<Transform>();
    }
}
public enum MushroomEffectType
{
    Simple,
    Joyful,
    Sad
}
