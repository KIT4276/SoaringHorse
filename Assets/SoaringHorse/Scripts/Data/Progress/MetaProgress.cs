using System;

public class MetaProgress : IMetaProgress
{
    private int _bestScore;
    private int _totalHorseshoes;
    private int _totalRevives;
    private float _bestRunTime;

    private bool _suppress;

    public int BestScore => _bestScore;
    public int TotalHorseshoes => _totalHorseshoes;
    public int TotalRevives => _totalRevives;
    public float BestRunTime => _bestRunTime;

    public event Action Changed;

    public void ApplyFromSave(MetaSaveData data)
    {
        if (data == null) return;

        _suppress = true;
        _bestScore = data.bestScore;
        _totalHorseshoes = data.totalHorseshoes;
        _totalRevives = data.totalRevives;
        _bestRunTime = data.bestRunTime;
        _suppress = false;
    }

    public void SetBestRunTime(float bestRunTime)
    {
        _bestRunTime = bestRunTime;
        Notify();
    }

    public void SetBestScore(int bestScore)
    {
        _bestScore = bestScore;
        Notify();
    }

    public void SetTotalHorseshoes(int totalHorseshoes)
    {
        _totalHorseshoes = totalHorseshoes;
        Notify();
    }

    public void SetTotalRevives(int revives)
    {
        _totalRevives = revives;
        Notify();
    }

    private void Notify()
    {
        if (_suppress) return;
        Changed?.Invoke();
    }
}