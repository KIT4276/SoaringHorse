using System;

public interface IGameStateFactory
{
    TState Get<TState>() where TState : class, IGameState;
    IGameState Get(Type stateType);
}