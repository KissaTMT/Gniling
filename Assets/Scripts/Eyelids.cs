using System.Collections;
using UnityEngine;
using Zenject;

public class Eyelids : MonoBehaviour
{
    [SerializeField] private Transform _top;
    [SerializeField] private Transform _bottom;

    [SerializeField] private float _speed;

    private Gniling _gniling;

    [Inject]
    public void Construct(PlayerGnilingBrian player)
    {
        _gniling = player.Gniling;

        _gniling.OnSleep += Close;
        _gniling.OnRise += Open;
    }
    public void Open()
    {
        StartCoroutine(Proccess(_top.position.y + _top.localScale.y, _bottom.position.y - _bottom.localScale.y));
    }
    public void Close()
    {
        StartCoroutine(Proccess(_top.position.y - _top.localScale.y, _bottom.position.y + _bottom.localScale.y));
    }
    private void OnDisable()
    {
        _gniling.OnSleep -= Close;
        _gniling.OnRise -= Open;
    }
    private IEnumerator Proccess(float topTargetY, float bottomTargetY)
    {
        var targetTop = new Vector2(0, topTargetY);
        var targetBottom = new Vector2(0, bottomTargetY);

        var startTop = (Vector2)_top.position;
        var startBottom = (Vector2)_bottom.position;


        for(var i = 0f; i < 1f; i += _speed * Time.deltaTime)
        {
            _top.position = Vector2.Lerp(startTop, targetTop, i);
            _bottom.position = Vector2.Lerp(startBottom, targetBottom, i);
            yield return null;
        }

        _top.position = targetTop;
        _bottom.position = targetBottom;
    }
}
