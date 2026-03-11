using UnityEngine;
using Zenject;

public class HeroMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rigidbody;

    private InputManager _inputManager;
    private HeroConfig _gameConfig;

    private bool _canMove;
    private bool _started;

    [Inject]
    private void Construct(HeroConfig gameConfig, InputManager inputManager)
    {
        _gameConfig = gameConfig;
        _inputManager = inputManager;
    }

    private void OnEnable()
    {
        _canMove = true; //temporary
        _started = false;
        _rigidbody.simulated = false;
        _rigidbody.linearVelocity = Vector2.zero;

        _inputManager.UpPressed += OnUpPressed;
    }

    private void OnDisable()
    {
        _inputManager.UpPressed -= OnUpPressed;
    }

    private void Update()
    {
        if (!_canMove)
        {
            if (transform.position != Vector3.zero)
                transform.position = Vector3.zero;

            _rigidbody.simulated = false;
            return;
        }
    }

    private void OnUpPressed()
    {
        if (!_canMove) return;

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
