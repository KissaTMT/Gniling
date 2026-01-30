using System.Collections.Generic;

public class StatsRepository
{
    private Dictionary<string, Stat> _statsMap;
    public StatsRepository()
    {
        _statsMap = new Dictionary<string, Stat>();
    }
    public void Regesrty(string statName, Stat stat)
    {
        _statsMap.Add(statName, stat);
    }
    public void Unregesrty(string statName)
    {
        _statsMap.Remove(statName);
    }
    public Stat GetStat(string statName)
    {
        if(_statsMap.TryGetValue(statName, out Stat stat)) return stat;
        else return null;
    }
    public int IsOutOfRange(string statName)
    {
        if (_statsMap.TryGetValue(statName, out Stat stat)) return stat.Current.Value <= 0 || stat.Current.Value > 1 ? 1 : 0;
        return -1;
    }
}
