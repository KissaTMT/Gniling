using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Image _bar;

    private ReactProp<float> _currentValue;
    private Color _colorBase;

    public void Init(ReactProp<float> react)
    {
        _currentValue = react;
        _currentValue.OnChanged += ChangeBar;
        _colorBase = _bar.color;

        _bar.fillAmount = _currentValue.Value;
    }
    private void OnDisable()
    {
        if (_currentValue == null) return;
        _currentValue.OnChanged -= ChangeBar;
    }
    private void ChangeBar(float oldValue, float newValue)
    {
        _bar.fillAmount = newValue;

        if (newValue > 1) _bar.color = _colorBase * Color.red;
        else _bar.color = _colorBase;
    }
}