using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public GameControls Controls { get; private set; }

    public event Action UpPressed;
    public event Action UpReleased;
    public event Action EscPressed;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Controls = new GameControls();
    }

    private void OnEnable()
    {
        Controls.PlayerActionMap.Enable();

        Controls.PlayerActionMap.Up.performed += OnUpPerformed;
        Controls.PlayerActionMap.Up.canceled += OnUpCanceled;

        Controls.PlayerActionMap.Esc.performed += OnEscPerformed;
    }

    private void OnDisable()
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
