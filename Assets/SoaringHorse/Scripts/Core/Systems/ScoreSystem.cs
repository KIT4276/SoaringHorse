using System;
using UnityEngine;

public class ScoreSystem
{
    private readonly IPlayerProgress _progress;
    private readonly ExperienceSystem _experienceSystem;

    private int _prevInteger;
    private int _newInteger;

    public float Score { get; private set; }
    
    public event Action<float> ChangeValue;
    public event Action ChangeIntegerValue;

    public ScoreSystem(IPlayerProgress progress, ExperienceSystem experienceSystem)
    {
        _progress = progress;
        _experienceSystem = experienceSystem;
    }

    public void Initialize()
    {
        _prevInteger = 0;

        Score = _progress.Score;
        ChangeValue?.Invoke(Score);

        _experienceSystem.ChangeValue += OnExpChanged;
    }

    public void ChangeScore(float value)
    {
        _prevInteger = Mathf.FloorToInt(Score);

        Score += value;
        _progress.AddScore(value);
        ChangeValue?.Invoke(Score);

        _newInteger = Mathf.FloorToInt(Score);

        if (_newInteger > _prevInteger)
            ChangeIntegerValue?.Invoke();
    }

    private void OnExpChanged(float exp) => 
        ChangeScore(exp);
}
