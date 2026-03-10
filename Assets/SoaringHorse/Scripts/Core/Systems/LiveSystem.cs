using System;
using UnityEngine;
using Zenject;

public class LiveSystem : ITickable
{
    private readonly IPlayerProgress _progress;
    private readonly int _maxLifes;
    private readonly float _invulnerabilityTime;

    private float _invulnerabilityRemaining;
    private bool _isDead;

    public int CurrentLives { get; private set; }
    public bool IsInvulnerable => _invulnerabilityRemaining > 0f;
    public bool IsDead => _isDead;

    public event Action<int> ValueIncreased;
    public event Action<int> ValueDecreased;
    public event Action Death;

    public LiveSystem(IPlayerProgress progress, GameConfig config)
    {
        _progress = progress;
        _maxLifes = config.MaxLifes;
        _invulnerabilityTime = config.InvulnerabilityTime;
    }

    public void Initialize()
    {
        CurrentLives = Mathf.Clamp(_progress.Lifes, 0, _maxLifes);
        _progress.SetLifes(CurrentLives);

        _invulnerabilityRemaining = 0f;
        _isDead = CurrentLives <= 0;

        ValueIncreased?.Invoke(CurrentLives);
    }

    public void Tick()
    {
        if (_invulnerabilityRemaining <= 0f)
            return;

        _invulnerabilityRemaining -= Time.deltaTime;

        if (_invulnerabilityRemaining < 0f)
            _invulnerabilityRemaining = 0f;
    }

    public void StartNewGame(int startLives)
    {
        CurrentLives = Mathf.Clamp(startLives, 0, _maxLifes);
        _progress.SetLifes(CurrentLives);

        _invulnerabilityRemaining = 0f;
        _isDead = false;

        ValueIncreased?.Invoke(CurrentLives);
    }

    public void Revive(int livesToAdd)
    {
        CurrentLives = Mathf.Clamp(CurrentLives + livesToAdd, 0, _maxLifes);
        _progress.SetLifes(CurrentLives);

        _isDead = false;
        StartInvulnerability(_invulnerabilityTime);

        ValueIncreased?.Invoke(CurrentLives);
    }

    public void AddLives(int value)
    {
        if (value <= 0)
            return;

        CurrentLives = Mathf.Clamp(CurrentLives + value, 0, _maxLifes);
        _progress.SetLifes(CurrentLives);

        ValueIncreased?.Invoke(CurrentLives);
    }

    public void SubtractLives(int value)
    {
        if (value <= 0)
            return;

        if (_isDead)
            return;

        if (IsInvulnerable)
            return;

        CurrentLives -= value;

        if (CurrentLives <= 0)
        {
            CurrentLives = 0;
            _progress.SetLifes(CurrentLives);

            _invulnerabilityRemaining = 0f;
            _isDead = true;

            ValueDecreased?.Invoke(CurrentLives);
            Death?.Invoke();
            return;
        }

        _progress.SetLifes(CurrentLives);
        StartInvulnerability(_invulnerabilityTime);
        ValueDecreased?.Invoke(CurrentLives);
    }

    public void StartInvulnerability(float duration)
    {
        if (duration <= 0f)
            return;

        _invulnerabilityRemaining = duration;
    }

    public void ResetInvulnerability()
    {
        _invulnerabilityRemaining = 0f;
    }

    public void StartNewGame()
    {
        CurrentLives = Mathf.Clamp(_progress.Lifes, 0, _maxLifes);
        _progress.SetLifes(CurrentLives);

        _invulnerabilityRemaining = 0f;
        _isDead = false;

        ValueIncreased?.Invoke(CurrentLives);
    }
}

