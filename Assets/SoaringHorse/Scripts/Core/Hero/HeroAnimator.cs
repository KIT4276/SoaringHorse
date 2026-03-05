using UnityEngine;
using Zenject;

public class HeroAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private InputManager _inputManager;

    [Inject]
    public void Constrtuct(InputManager inputManager)
        => _inputManager = inputManager;

    private void OnEnable()
    {
        _inputManager.UpPressed += OnUpPressed;
    }

    private void OnUpPressed()
    {
        _animator.SetTrigger("Up");
    }

    private void OnDisable()
    {
        _inputManager.UpPressed -= OnUpPressed;
    }
}
