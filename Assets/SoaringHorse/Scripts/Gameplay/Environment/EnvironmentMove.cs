using UnityEngine;
using Zenject;

public class EnvironmentMove : MonoBehaviour
{
    private SpeedSystem _speedSystem;
    
    private Vector3 _moveVector = Vector3.zero;

    [Inject]
    private void Construct(SpeedSystem speedSystem) => 
        _speedSystem = speedSystem;

    private void Update()
    {
        _moveVector.x = _speedSystem.CurrentSpeed * Time.deltaTime;
        transform.position -= _moveVector;
    }
}
