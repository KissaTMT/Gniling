using UnityEngine;

public class MenuEntryPoint : MonoBehaviour
{
    [SerializeField] private Mushroom[] _mushrooms;
    [SerializeField] private Willson _willson;

    private void Awake()
    {
        for(var i = 0; i < _mushrooms.Length; i++)
        {
            _mushrooms[i].Init();
        }
        _willson.Init();
    }
    private void Start()
    {
        _willson.SetImpulse(Vector2.right * 5);
    }
}
