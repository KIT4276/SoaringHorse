using System;
using UnityEngine;
using Zenject;

public class ExperienceSystem : ITickable
{
    private readonly IPlayerProgress _progress;
    private readonly float _experienceIncrease;
    private readonly int _timeExperienceIncrease;

    private float _time = 0;
     
    public float Exp {  get; private set; }

    public event Action<float> ChangeValue;

    public ExperienceSystem(IPlayerProgress progress, ProgressionConfig config)
    {
        _progress = progress;
        _experienceIncrease = config.ExperienceIncrease;
        _timeExperienceIncrease = config.TimeExperienceIncrease;
        //Debug.Log(_experienceIncrease);
    }

    public void Initialize()
    {
        Exp = _progress.Exp;
        ChangeValue?.Invoke(Exp);
       // Debug.Log($"[ExperienceSystem] Initialize {Exp:F2}");
    }

    public void AddExp(float value)
    {
        Exp += value;
        _progress.AddExp(value);
        ChangeValue?.Invoke(Exp);
        //Debug.Log($"[ExperienceSystem {Exp:F2}] ");
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
