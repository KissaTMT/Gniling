using UnityEngine;

public class Stat01 : Stat
{
    public Stat01(float init) : base(init) { }
    protected override void Change(float value)
    {
        Current.Value = Mathf.Clamp01(Current.Value + value);
    }
}
