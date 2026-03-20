using System;
using UnityEngine;
using Zenject;

public class ScoreSystem : ITickable
{
    private readonly ProgressSyncService _progressSyncService;
    private readonly int _scoreIncreasePerTick;
    private readonly float _scoreTickTime;

    private float _time;

    public float Score { get; private set; }

    public event Action<float> ChangeValue;
    public event Action ChangeIntegerValue;

    public ScoreSystem(ProgressSyncService progressSyncService, ProgressionConfig config)
    {
        _progressSyncService = progressSyncService;
        _scoreIncreasePerTick = config.ScoreIncreasePerTick;
        _scoreTickTime = config.ScoreTickTime;
    }

    public void Initialize()
    {
        Score = _progressSyncService.ReadScore();
        ChangeValue?.Invoke(Score);
    }

    public void Tick()
    {
        _time += Time.deltaTime;

        if (_time < _scoreTickTime)
            return;

        _time = 0f;
        AddScore(_scoreIncreasePerTick);
    }

    public void AddScore(int value)
    {
        int prevInteger = Mathf.FloorToInt(Score);

        Score += value;
        _progressSyncService.SyncScore(value);
        ChangeValue?.Invoke(Score);

        int newInteger = Mathf.FloorToInt(Score);

        if (newInteger > prevInteger)
            ChangeIntegerValue?.Invoke();
    }
}