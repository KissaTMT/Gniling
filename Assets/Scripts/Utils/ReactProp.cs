using System;
using System.Collections.Generic;

public class ReactProp<T>
{
    public event Action<T,T> OnChanged;
    public T Value
    {
        get
        {
            return _value;
        }
        set {
            if(_comparer.Equals(_value, value) == false)
            {
                OnChanged?.Invoke(_value, value);
                _value = value;
            }
        }
    }
    private T _value;
    private IEqualityComparer<T> _comparer;

    public ReactProp() : this(default(T)) { }
    public ReactProp(T value) : this(value, EqualityComparer<T>.Default) { }
    public ReactProp(T value, IEqualityComparer<T> comparer)
    {
        _value = value;
        _comparer = comparer;
    }
}
