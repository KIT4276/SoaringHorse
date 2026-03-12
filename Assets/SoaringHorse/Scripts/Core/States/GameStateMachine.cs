using System;
using UnityEngine;
using Zenject;

public sealed class GameStateMachine : IGameStateMachine, IInitializable, ITickable, IDisposable
{
    private readonly IGameStateFactory _stateFactory;

    private IGameState _current;
    private Type _currentType;
    private Type _beforePauseType;

    public event Action<IGameState> ChangeState;

    public GameStateMachine(IGameStateFactory stateFactory) => 
        _stateFactory = stateFactory;

    public void Initialize() => 
        Enter<BootstrapState>();

    public void Tick()
    {
        if (_current is ITickableState tickableState)
            tickableState.Tick();
    }

    public void Dispose()
    {
        _current?.Exit();
        _current = null;
        _currentType = null;
        _beforePauseType = null;
    }

    public void Enter<TState>() where TState : class, IGameState
    {
        ChangeStateTo(typeof(TState));
    }

    public void Pause()
    {
        if (_currentType == typeof(PauseState))
            return;
        _beforePauseType = _currentType;
        ChangeStateTo(typeof(PauseState));
    }

    public void Resume()
    {
        if (_currentType != typeof(PauseState))
            return;

        var backStateType = _beforePauseType ?? typeof(GameplayState);
        _beforePauseType = null;
        ChangeStateTo(backStateType);
    }

    private void ChangeStateTo(Type stateType)
    {
        if (_currentType == stateType)
            return;

        _current?.Exit();

        _current = _stateFactory.Get(stateType);
        _currentType = stateType;

        _current.Enter();
        ChangeState?.Invoke(_current);
        Debug.Log(_current);
    }
}