using System;
using UnityEngine;

public class LiveSystem
{
    private readonly IPlayerProgress _progress;
    private readonly int _maxLifes;

    public int CurrentLives { get; private set; }

    //public event Action<int> ChangeValue;

    public event Action<int> ValueIncreased;
    public event Action<int> ValueDecreased;

    public event Action Death;

    public LiveSystem(IPlayerProgress progress, GameConfig config)
    {
        _progress = progress;
        _maxLifes = config.MaxLifes;
    }

    public void Initialize()
    {
        CurrentLives = _progress.Lifes;
        ValueIncreased?.Invoke(CurrentLives);
       // Debug.Log("[LiveSystem] Initialize " + CurrentLives);
    }

    public void AddLives(int value)
    {
        CurrentLives += value;
        _progress.SetLifes(CurrentLives);

       if (CurrentLives > _maxLifes)
            CurrentLives = _maxLifes;

        ValueIncreased?.Invoke(CurrentLives);

        //Debug.Log("[LiveSystem] " + CurrentLives);
    }

    public void SubtractLives(int value)
    {
        CurrentLives -= value;
        if (CurrentLives < 0)
            CurrentLives = 0;

        _progress.SetLifes(CurrentLives);
        ValueDecreased?.Invoke(CurrentLives);
        //Debug.Log("[LiveSystem] "+ CurrentLives);

        if (CurrentLives == 0)
            Death?.Invoke();

    }
}
