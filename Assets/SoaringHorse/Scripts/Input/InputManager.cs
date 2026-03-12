using System;
using UnityEngine.InputSystem;
using Zenject;

public class InputManager :  IInitializable, IDisposable
{
    public GameControls Controls { get; private set; }

    public event Action UpPressed;
    public event Action EscPressed;

    public void Initialize()
    {
        Controls = new GameControls();
        Controls.PlayerActionMap.Enable();

        Controls.PlayerActionMap.Up.performed += OnUpPerformed;
        Controls.PlayerActionMap.Esc.performed += OnEscPerformed;
    }

    public void Dispose()
    {
        Controls.PlayerActionMap.Up.performed -= OnUpPerformed;
        Controls.PlayerActionMap.Esc.performed -= OnEscPerformed;

        Controls.PlayerActionMap.Disable();
    }

    private void OnUpPerformed(InputAction.CallbackContext ctx) => UpPressed?.Invoke();
    private void OnEscPerformed(InputAction.CallbackContext ctx) => EscPressed?.Invoke();
}
