using System;

public sealed class RunProgress : IRunProgress
{
    public event Action Changed;

    private int _score;
    private int _lifes;
    private float _speed;
    private float _runTime;

    private bool _suppress;

    public int Score => _score;
    public int Lifes => _lifes;

    public float Speed => _speed;

    public float RunTime => _runTime;

    public void ApplyFromSave(RunSaveData data)
    {
        if (data == null) return;

        _suppress = true;
        _score = data.score;
        _lifes = data.lifes;
        _speed = data.speed;
        _runTime = data.runTime;

        _suppress = false;
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

    public void SetSpeed(float value)
    {
        if(_speed == value) return;
        _speed = value;
        Notify();
    }

    public void SetRunTime(float value)
    {
       if(RunTime == value) return;
       _runTime = value;
        Notify();
    }

    private void Notify()
    {
        if (_suppress) return;
        Changed?.Invoke();
    }

}

