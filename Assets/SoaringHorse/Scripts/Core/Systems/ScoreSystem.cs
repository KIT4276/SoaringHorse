using System;
using UnityEngine;

public class ScoreSystem
{
    private readonly IPlayerProgress _progress;
    private readonly ExperienceSystem _experienceSystem;

    public float Score { get; private set; }
    
    public event Action<float> ChangeValue;

    public ScoreSystem(IPlayerProgress progress, ExperienceSystem experienceSystem)
    {
        _progress = progress;
        _experienceSystem = experienceSystem;
    }

    public void Initialize()
    {
        Score = _progress.Score;
        ChangeValue?.Invoke(Score);

        _experienceSystem.ChangeValue += OnExpChanged;
    }

    public void ChangeScore(float value)
    {
        Score += value;
        _progress.AddScore(value);
        ChangeValue?.Invoke(Score);
    }

    private void OnExpChanged(float exp)
    {
        ChangeScore(exp);
    }
}
