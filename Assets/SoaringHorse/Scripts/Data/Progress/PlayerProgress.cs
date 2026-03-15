using System;

public sealed class PlayerProgress : IPlayerProgress
{
    public event Action Changed;

    private float _score;
    private int _lifes;

    private bool _suppress;

    public float Score => _score;
    public int Lifes => _lifes;

    public void ApplyFromSave(SaveData data)
    {
        if (data == null) return;

        _suppress = true;
        _score = data.score;
        _lifes = data.lifes;
        _suppress = false;
    }

    public void AddScore(float delta)
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
