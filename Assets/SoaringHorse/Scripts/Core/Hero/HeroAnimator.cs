using UnityEngine;

public class HeroAnimator : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private InputManager _inputManager;

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
