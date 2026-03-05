using System;

public sealed class PlayerProgress : IPlayerProgress
{
    public event Action Changed;

    private int _exp;
    private int _score;
    private int _lifes;

    private bool _suppress;

    public int Exp => _exp;
    public int Score => _score;
    public int Lifes => _lifes;

    public void ApplyFromSave(SaveData data)
    {
        if (data == null) return;

        _suppress = true;
        _exp = data.exp;
        _score = data.score;
        _lifes = data.lifes;
        _suppress = false;
    }

    public void AddExp(int delta)
    {
        if (delta == 0) return;
        _exp += delta;
        if (_exp < 0) _exp = 0;
        Notify();
    }

    public void AddScore(int delta)
    {
        if (delta == 0) return;
        _score += delta;
        if (_score < 0) _score = 0;
        Notify();
    }

    public void SetLifes(int value)
    {
        if (_lifes == value) return;
        _lifes = value;
        if (_lifes < 0) _lifes = 0;
        Notify();
    }

    private void Notify()
    {
        if (_suppress) return;
        Changed?.Invoke();
    }
}
