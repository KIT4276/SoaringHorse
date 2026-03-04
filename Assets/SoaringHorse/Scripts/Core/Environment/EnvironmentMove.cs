using UnityEngine;
using Zenject;

public class EnvironmentMove : MonoBehaviour
{
    private float _startMoveSpeed;
    private float _currentMoveSpeed;
    private Vector3 _moveVector = Vector3.zero;

    public float MoveSpeed { get => _currentMoveSpeed; }

    [Inject]
    private void Construct(GameConfig gameConfig)
    {
        _startMoveSpeed = gameConfig.EnvironmentMoveSpeed;
        _currentMoveSpeed = _startMoveSpeed;
    }

    private void Update() =>
        OnMoveEnvironment();

    public void ChangeSpeed(float increase) => //increase = 0.2f
        _currentMoveSpeed += increase;

    public void StopMove() =>
        _currentMoveSpeed = 0;

    private void OnMoveEnvironment()
    {
        _moveVector.x = _currentMoveSpeed * Time.deltaTime;
        transform.position -= _moveVector;
    }
}
