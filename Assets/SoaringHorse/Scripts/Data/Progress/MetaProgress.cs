using System;

public class MetaProgress : IMetaProgress
{
    private int _bestScore;
    private int _totalHorseshoes;
    private int _bestRevives;
    private float _bestRunTime;

    private bool _suppress;

    public int BestScore => _bestScore;
    public int TotalHorseshoes => _totalHorseshoes;
    public int BestRevives => _bestRevives;
    public float BestRunTime => _bestRunTime;

    public event Action Changed;

    public void ApplyFromSave(MetaSaveData data)
    {
        if (data == null) return;

        _suppress = true;
        _bestScore = data.bestScore;
        _totalHorseshoes = data.totalHorseshoes;
        _bestRevives = data.totalRevives;
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

    public void SetBestRevives(int revives)
    {
        _bestRevives = revives;
        Notify();
    }

    private void Notify()
    {
        if (_suppress) return;
        Changed?.Invoke();
    }
}
