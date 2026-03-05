public interface IGameStateMachine
{
    void Enter<TState>() where TState : class, IGameState;
    void Pause();   
    void Resume();  
}