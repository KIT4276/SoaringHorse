using System;
using UnityEngine;
using Zenject;

public sealed class LiveSystem : ITickable
{
    private readonly int _maxLives;
    private readonly float _invulnerabilityTime;
    private readonly RunProgressSyncService _progressSyncService;
    private float _invulnerabilityRemaining;
    private bool _isDead;

    public int CurrentLives { get; private set; }
    public bool IsInvulnerable => _invulnerabilityRemaining > 0f;
    public bool IsDead => _isDead;

    public event Action<int> ValueIncreased;
    public event Action<int> ValueDecreased;
    public event Action Death;

    public LiveSystem(RunProgressSyncService progressSyncService, HeroConfig config)
    {
        _progressSyncService = progressSyncService;
        _maxLives = config.MaxLives; 
        _invulnerabilityTime = config.InvulnerabilityTime;
    }

    public void Tick()
    {
        if (_invulnerabilityRemaining <= 0f)
            return;

        _invulnerabilityRemaining -= Time.deltaTime;

        if (_invulnerabilityRemaining < 0f)
            _invulnerabilityRemaining = 0f;
    }

    public void LoadFromProgress()
    {
        SetLivesInternal(_progressSyncService.ReadLifes(), resetInvulnerability: true);
        _isDead = CurrentLives <= 0;
        ValueIncreased?.Invoke(CurrentLives);
    }

    public void ResetForNewRun(int startLives)
    {
        SetLivesInternal(startLives, resetInvulnerability: true);
        _isDead = CurrentLives <= 0;
        ValueIncreased?.Invoke(CurrentLives);

        if (_isDead)
            Death?.Invoke();
    }

    public void Revive(int livesToAdd)
    {
        if (livesToAdd <= 0)
            return;

        SetLivesInternal(CurrentLives + livesToAdd, resetInvulnerability: false);
        _isDead = false;
        StartInvulnerability(_invulnerabilityTime);
        ValueIncreased?.Invoke(CurrentLives);
    }

    public void AddLives(int value)
    {
        if (value <= 0 || _isDead)
            return;

        int oldLives = CurrentLives;
        SetLivesInternal(CurrentLives + value, resetInvulnerability: false);

        if (CurrentLives != oldLives)
            ValueIncreased?.Invoke(CurrentLives);
    }

    public void SubtractLives(int value)
    {
        if (value <= 0)
            return;

        if (_isDead || IsInvulnerable)
            return;

        int oldLives = CurrentLives;
        SetLivesInternal(CurrentLives - value, resetInvulnerability: false);

        if (CurrentLives == oldLives)
            return;

        if (CurrentLives <= 0)
        {
            _isDead = true;
            _invulnerabilityRemaining = 0f;
            ValueDecreased?.Invoke(CurrentLives);  
            Death?.Invoke();
            return;
        }

        StartInvulnerability(_invulnerabilityTime);
        ValueDecreased?.Invoke(CurrentLives);
    }

    public void StartInvulnerability(float duration)
    {
        if (duration <= 0f)
            return;

        _invulnerabilityRemaining = duration;
    }

    public void ResetInvulnerability() => 
        _invulnerabilityRemaining = 0f;

    private void SetLivesInternal(int lives, bool resetInvulnerability)
    {
        CurrentLives = Mathf.Clamp(lives, 0, _maxLives);
        _progressSyncService.SetLifes(CurrentLives);
        if (resetInvulnerability)
            _invulnerabilityRemaining = 0f;
    }
}