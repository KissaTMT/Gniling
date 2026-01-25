using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Stat
{
    public ReactProp<float> Current = new();

    public Stat() : this(1) { }
    public Stat(float initValue)
    {
        Current.Value = initValue;
    }
    public void Add(float value) => Change(value);
    public void Reduce(float value) => Change(-value);
    protected virtual void Change(float value)
    {
        Current.Value = Mathf.Max(0, Current.Value + value);
    }
}
