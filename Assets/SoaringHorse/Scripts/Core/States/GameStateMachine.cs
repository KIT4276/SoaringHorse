using System;
using Zenject;

public sealed class GameStateMachine : IGameStateMachine, IInitializable, ITickable, IDisposable
{
    private readonly DiContainer _container;

    private IGameState _current;
    private Type _currentType;
    private Type _beforePauseType;

    public GameStateMachine(DiContainer container)
    {
        _container = container;
    }

    public void Initialize()
    {
        Enter<BootstrapState>();
    }

    public void Tick()
    {
        if (_current is ITickableState t)
            t.Tick();
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
        var type = typeof(TState);
        if (_currentType == type) return;

        _current?.Exit();

        _current = _container.Resolve<TState>();
        _currentType = type;

        _current.Enter();
    }

    public void Pause()
    {
        if (_currentType == typeof(PauseState)) return;

        _beforePauseType = _currentType;
        Enter<PauseState>();
    }

    public void Resume()
    {
        if (_currentType != typeof(PauseState)) return;

        var back = _beforePauseType ?? typeof(GameplayState);
        _beforePauseType = null;

        EnterByType(back);
    }

    private void EnterByType(Type type)
    {
        if (_currentType == type) return;

        _current?.Exit();

        _current = (IGameState)_container.Resolve(type);
        _currentType = type;

        _current.Enter();
    }
}
