using System;
using UnityEngine;
using Zenject;

public class SpeedSystem : ITickable, IInitializable
{
    private readonly float _minSpeed;
    private readonly ProgressSyncService _progressSyncService;

    private float _speedTickTime;
    private float _speedIncreasePerTick;

    private float _time;

    public float CurrentSpeed { get; private set; }

    public event Action<float> CurrentSpeedChanged;

    public SpeedSystem(ProgressSyncService progressSyncService, EnvironmentConfig config)
    {
        _progressSyncService = progressSyncService;
        _minSpeed = config.EnvironmentMoveSpeed;
        _speedTickTime = config.SpeedTickTime;
        _speedIncreasePerTick = config.SpeedIncreasePerTick;

    }

    public void Initialize()
    {
        CurrentSpeed = _progressSyncService.ReadSpeed();
        CurrentSpeedChanged?.Invoke(CurrentSpeed);
    }

    public void Tick()
    {
        _time += Time.deltaTime;

        if (_time < _speedTickTime)
            return;

        _time = 0f;
        AddSpeed(_speedIncreasePerTick);
    }

    public void ReduceByPercent(float value)
    {
        CurrentSpeed *= 1 - value;
        if (CurrentSpeed < _minSpeed)
            CurrentSpeed = _minSpeed;

        SetProgressAndNotify();
    }

    private void AddSpeed(float speedIncreasePerTick)
    {
        CurrentSpeed += speedIncreasePerTick;
        SetProgressAndNotify();
    }

    private void SetProgressAndNotify()
    {
        _progressSyncService.SetSpeed(CurrentSpeed);
        CurrentSpeedChanged?.Invoke(CurrentSpeed);
    }
}
