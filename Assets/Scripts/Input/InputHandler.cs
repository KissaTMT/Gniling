using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public class InputHandler : IDisposable
{
    public event Action<Vector2> OnGetScreenPosition;

    private InputSystem_Actions _inputActions;
    public InputHandler()
    {
        _inputActions = new InputSystem_Actions();
        _inputActions.Enable();
        Application.focusChanged += OnApplicationFocus;
        _inputActions.Player.GetPosition.performed += PointPositionHandler;
    }
    public void Dispose()
    {
        _inputActions.Player.GetPosition.performed -= PointPositionHandler;
        Application.focusChanged -= OnApplicationFocus;
        _inputActions.Disable();
        _inputActions.Dispose();
    }
    public void Reset()
    {
        _inputActions.Disable();
        _inputActions.Enable();
    }
    private void PointPositionHandler(InputAction.CallbackContext context)
    {
        var point = _inputActions.Player.Position.ReadValue<Vector2>();

        OnGetScreenPosition?.Invoke(point);
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) return;

        Reset();
    }

}
