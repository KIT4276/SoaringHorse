using System;
using UnityEngine;

public class ScoreSystem : IDisposable
{
    private readonly ProgressSyncService _progressSyncService;
    private readonly ExperienceSystem _experienceSystem;

    private int _prevInteger;
    private int _newInteger;

    public float Score { get; private set; }
    
    public event Action<float> ChangeValue;
    public event Action ChangeIntegerValue;

    public ScoreSystem(ProgressSyncService progressSyncService, ExperienceSystem experienceSystem)
    {
        _progressSyncService = progressSyncService;
        _experienceSystem = experienceSystem;
    }

    public void Initialize()
    {
        _prevInteger = 0;

        Score = _progressSyncService.ReadScore();
        ChangeValue?.Invoke(Score);

        _experienceSystem.ChangeValue += OnExpChanged;
    }


    public void Dispose() => 
        _experienceSystem.ChangeValue -= OnExpChanged;

    public void ChangeScore(float value)
    {
        _prevInteger = Mathf.FloorToInt(Score);

        Score += value;
        _progressSyncService.SyncScore(value);
        ChangeValue?.Invoke(Score);

        _newInteger = Mathf.FloorToInt(Score);

        if (_newInteger > _prevInteger)
            ChangeIntegerValue?.Invoke();
    }

    private void OnExpChanged(float exp) => 
        ChangeScore(exp);
}
