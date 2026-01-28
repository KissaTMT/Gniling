using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Bar01 : MonoBehaviour
{
    [SerializeField] private Image _bar;
    [SerializeField] private float _changeSpeed = 0.5f;

    private ReactProp<float> _currentValue;
    private float _oldValue;
    public void Init(ReactProp<float> react)
    {
        _currentValue = react;
        _currentValue.OnChanged += ChangeBar;
        _oldValue = _currentValue.Value;

        _bar.fillAmount = _currentValue.Value;
    }
    private void OnDisable()
    {
        if (_currentValue == null) return;
        _currentValue.OnChanged -= ChangeBar;
    }
    private void ChangeBar(float oldValue, float newValue)
    {
        //if (Mathf.Abs(_oldValue - newValue) < 0.05f) return;
        //StartCoroutine(ChangeBarRoutine(oldValue,newValue));
        //_oldValue = newValue;

        _bar.fillAmount = newValue;

        if (newValue > 1) _bar.color = Color.red;
        else _bar.color = Color.white;
    }
    private IEnumerator ChangeBarRoutine(float oldValue, float newValue)
    {
        for (var i = 0f; i < 1; i += _changeSpeed * Time.deltaTime)
        {
            _bar.fillAmount = Mathf.Lerp(_oldValue, newValue, i);
            yield return null;
        }
        _bar.fillAmount = newValue;

        if (newValue > 1) _bar.color = Color.red;
        else _bar.color = Color.white;
    }
}