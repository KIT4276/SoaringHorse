using System;
using UnityEngine.InputSystem;
using Zenject;

public class InputManager :  IInitializable, IDisposable
{
    public GameControls Controls { get; private set; }

    public event Action UpPressed;
    public event Action UpReleased;
    public event Action EscPressed;

    public void Initialize()
    {
        Controls = new GameControls();
        Controls.PlayerActionMap.Enable();

        Controls.PlayerActionMap.Up.performed += OnUpPerformed;
        Controls.PlayerActionMap.Up.canceled += OnUpCanceled;

        Controls.PlayerActionMap.Esc.performed += OnEscPerformed;
    }

    public void Dispose()
    {
        Controls.PlayerActionMap.Up.performed -= OnUpPerformed;
        Controls.PlayerActionMap.Up.canceled -= OnUpCanceled;

        Controls.PlayerActionMap.Esc.performed -= OnEscPerformed;

        Controls.PlayerActionMap.Disable();
    }


    private void OnUpPerformed(InputAction.CallbackContext ctx) => UpPressed?.Invoke();
    private void OnUpCanceled(InputAction.CallbackContext ctx) => UpReleased?.Invoke();
    private void OnEscPerformed(InputAction.CallbackContext ctx) => EscPressed?.Invoke();



    // Пример переключения карт (если добавишь UI map)
    public void EnableGameplay()
    {
        // Controls.UI.Disable();
        Controls.PlayerActionMap.Enable();
    }

    public void EnableUI()
    {
        Controls.PlayerActionMap.Disable();
        // Controls.UI.Enable();
    }
}
