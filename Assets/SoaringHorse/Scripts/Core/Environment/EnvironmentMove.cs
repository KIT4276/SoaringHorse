using UnityEngine;
using Zenject;

public class EnvironmentMove : MonoBehaviour
{
    private float _startMoveSpeed;
   private float _currentMoveSpeed;
    private ExperienceSystem _experienceSystem;
    private Vector3 _moveVector = Vector3.zero;

    public float MoveSpeed => _currentMoveSpeed;

    [Inject]
    private void Construct(EnvironmentConfig gameConfig, ExperienceSystem experienceSystem)
    {
        _startMoveSpeed = gameConfig.EnvironmentMoveSpeed;
        _currentMoveSpeed = _startMoveSpeed;
        _experienceSystem = experienceSystem;
    }

    private void Start() => 
        _experienceSystem.ChangeValue += OnExpChanged;

    private void OnDestroy() => 
        _experienceSystem.ChangeValue -= OnExpChanged;

    private void OnExpChanged(float exp) => 
        ChangeSpeed(exp);

    private void Update() =>
        OnMoveEnvironment();

    public void ChangeSpeed(float increase) =>
        _currentMoveSpeed += increase;

    public void StopMove() =>
        _currentMoveSpeed = 0;

    public void MultiplySpeed(float multiplier) => 
        _currentMoveSpeed *= multiplier;

    public void ReduceSpeedByPercent(float percent)
    {
        float multiplier = 1f - percent;
        _currentMoveSpeed *= multiplier;
    }

    private void OnMoveEnvironment()
    {
        _moveVector.x = _currentMoveSpeed * Time.deltaTime;
        transform.position -= _moveVector;
    }
}
