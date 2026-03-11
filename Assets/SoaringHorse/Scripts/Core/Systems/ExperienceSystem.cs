using System;
using UnityEngine;
using Zenject;

public class ExperienceSystem : ITickable
{
    private readonly float _experienceIncrease;
    private readonly int _timeExperienceIncrease;
    private readonly ProgressSyncService _progressSyncService;
    private float _time = 0;
     
    public float Exp {  get; private set; }

    public event Action<float> ChangeValue;

    public ExperienceSystem(ProgressionConfig config, ProgressSyncService progressSyncService)
    {
        _experienceIncrease = config.ExperienceIncrease;
        _timeExperienceIncrease = config.TimeExperienceIncrease;
        _progressSyncService = progressSyncService;
    }

    public void Initialize()
    {
        Exp = _progressSyncService.ReadExperience();
        ChangeValue?.Invoke(Exp);
    }

    public void AddExp(float value)
    {
        Exp += value;

        _progressSyncService.SyncExperience(value);
        ChangeValue?.Invoke(Exp);
    }

    public void Tick()
    {
        _time += Time.deltaTime;

        if (_time < _timeExperienceIncrease)
            return;

        _time = 0f;
        AddExp(_experienceIncrease);
    }
}
