using System;
using UnityEngine;
using Zenject;

public class EnvironmentMove : MonoBehaviour
{
    private float _startMoveSpeed;
    [SerializeField] private float _currentMoveSpeed;
    private float _speedTickTime;
    private float _speedIncreasePerTick;
    private Vector3 _moveVector = Vector3.zero;

    private float _time;

    public float MoveSpeed => _currentMoveSpeed;

    public event Action<float> SpeedChanged;

    [Inject]
    private void Construct(EnvironmentConfig envConfig)
    {
        _startMoveSpeed = envConfig.EnvironmentMoveSpeed;
        _currentMoveSpeed = _startMoveSpeed;
        _speedTickTime = envConfig.SpeedTickTime;
        _speedIncreasePerTick = envConfig.SpeedIncreasePerTick;
        SpeedChanged?.Invoke(_currentMoveSpeed);
    }

    private void Update()
    {
        OnSpeedChange();
        OnMoveEnvironment();
    }

    public void ReduceByPercent(float v)
    {
        _currentMoveSpeed *= 1 - v;
        if (_currentMoveSpeed < _startMoveSpeed)
            _currentMoveSpeed = _startMoveSpeed;

        SpeedChanged?.Invoke(_currentMoveSpeed);
    }

    private void OnSpeedChange()
    {
        _time += Time.deltaTime;

        if (_time < _speedTickTime)
            return;

        _time = 0f;
        AddSpeed(_speedIncreasePerTick);
    }

    private void AddSpeed(float speedIncreasePerTick)
    {
        _currentMoveSpeed += speedIncreasePerTick;
        SpeedChanged?.Invoke(_currentMoveSpeed);
    }

    private void OnMoveEnvironment()
    {
        _moveVector.x = _currentMoveSpeed * Time.deltaTime;
        transform.position -= _moveVector;
    }
}
