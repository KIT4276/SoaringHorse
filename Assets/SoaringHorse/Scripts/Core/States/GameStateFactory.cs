using System;
using Zenject;

public sealed class GameStateFactory : IGameStateFactory
{
    private readonly LazyInject<BootstrapState> _bootstrapState;
    private readonly LazyInject<LoadSceneState> _loadSceneState;
    private readonly LazyInject<GameStartState> _gameStartState;
    private readonly LazyInject<GameplayState> _gameplayState;
    private readonly LazyInject<PauseState> _pauseState;

    public GameStateFactory(
        LazyInject<BootstrapState> bootstrapState,
        LazyInject<LoadSceneState> loadSceneState,
        LazyInject<GameStartState> gameStartState,
        LazyInject<GameplayState> gameplayState,
        LazyInject<PauseState> pauseState)
    {
        _bootstrapState = bootstrapState;
        _loadSceneState = loadSceneState;
        _gameStartState = gameStartState;
        _gameplayState = gameplayState;
        _pauseState = pauseState;
    }

    public TState Get<TState>() where TState : class, IGameState
    {
        return Get(typeof(TState)) as TState;
    }

    public IGameState Get(Type stateType)
    {
        if (stateType == typeof(BootstrapState))
            return _bootstrapState.Value;

        if (stateType == typeof(LoadSceneState))
            return _loadSceneState.Value;

        if (stateType == typeof(GameStartState))
            return _gameStartState.Value;

        if (stateType == typeof(GameplayState))
            return _gameplayState.Value;

        if (stateType == typeof(PauseState))
            return _pauseState.Value;

        throw new ArgumentException($"Unknown state type: {stateType}");
    }
}