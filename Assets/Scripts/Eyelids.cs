using System.Collections;
using UnityEngine;
using Zenject;

public class Eyelids : MonoBehaviour
{
    [SerializeField] private Transform _top;
    [SerializeField] private Transform _bottom;

    [SerializeField] private float _speed;

    private PlayerGnilingBrian _player;
    private Gniling _gniling;

    [Inject]
    public void Construct(PlayerGnilingBrian player)
    {
        _player = player;
    }
    public void Init()
    {
        _gniling = _player.Gniling;
        _gniling.OnSleep += CloseProccess;
        _gniling.OnRise += OpenProccess;

        Close();
        OpenProccess();
    }
    public void OpenProccess()
    {
        StartCoroutine(Proccess(_top.position.y + _top.localScale.y, _bottom.position.y - _bottom.localScale.y));
    }
    public void CloseProccess()
    {
        StartCoroutine(Proccess(_top.position.y - _top.localScale.y, _bottom.position.y + _bottom.localScale.y));
    }
    private void Open()
    {
        _top.position = new Vector2(0,_top.position.y + _top.localScale.y);
        _bottom.position = new Vector2(0, _bottom.position.y - _bottom.localScale.y);
    }
    private void Close()
    {
        _top.position = new Vector2(0, _top.position.y - _top.localScale.y);
        _bottom.position = new Vector2(0, _bottom.position.y + _bottom.localScale.y);
    }
    private void OnDisable()
    {
        _gniling.OnSleep -= CloseProccess;
        _gniling.OnRise -= OpenProccess;
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
