using UnityEngine.InputSystem.LowLevel;

public interface IGameState
{
    void Enter();
    void Exit();
}

public interface ITickableState
{
    void Tick();
}
