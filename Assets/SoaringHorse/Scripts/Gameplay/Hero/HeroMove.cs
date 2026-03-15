using UnityEngine;
using Zenject;

public class HeroMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;

    private InputManager _inputManager;
    private HeroConfig _gameConfig;

    private bool _started;

    [Inject]
    private void Construct(HeroConfig gameConfig, InputManager inputManager)
    {
        _gameConfig = gameConfig;
        _inputManager = inputManager;
        //_rigidbody.gravityScale = gameConfig.Gravity;
    }

    private void OnEnable()
    {
        transform.position = Vector3.zero;
        _started = false;
        _rigidbody.simulated = false;
        _rigidbody.linearVelocity = Vector2.zero;

        _inputManager.UpPressed += OnUpPressed;
    }

    private void OnDisable() => 
        _inputManager.UpPressed -= OnUpPressed;

    private void OnUpPressed()
    {
        if (!_started)
        {
            _started = true;
            _rigidbody.simulated = true;
            _rigidbody.linearVelocity = Vector2.zero;
            return;
        }

        var v = _rigidbody.linearVelocity;
        v.y = 0f;
        _rigidbody.linearVelocity = v;

        _rigidbody.AddForce(transform.up * _gameConfig.HeroUpForce, ForceMode2D.Impulse);
    }
}
