using UnityEngine;
using Zenject;

public class HeroAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private ParticleSystem _particleSystem;

    private const string damage = "Damage";
    private const string up = "Up";

    private InputManager _inputManager;

    [Inject]
    public void Construct(InputManager inputManager) => 
        _inputManager = inputManager;

    private void OnEnable()
    {
        if (_inputManager != null)
            _inputManager.UpPressed += OnUpPressed;
    }

    public void PlayDamage(Vector2 contact)
    {
        _animator.SetTrigger(damage);
        _particleSystem.transform.position = contact;
        _particleSystem.Play();
    }

    private void OnUpPressed()
    {
        _animator.SetTrigger(up);
    }

    private void OnDisable()
    {
        _inputManager.UpPressed -= OnUpPressed;
    }
}
