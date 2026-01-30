using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using Zenject;

public class InputHandler : IDisposable, ITickable
{
    public event Action<Vector2> OnGetScreenPosition;

    private InputSystem_Actions _inputActions;

    private bool _reset;
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
    public void Tick()
    {
        _reset = EventSystem.current.IsPointerOverGameObject();
    }
    private void PointPositionHandler(InputAction.CallbackContext context)
    {
        var point = _inputActions.Player.Position.ReadValue<Vector2>();

        //if (_reset) return;

        OnGetScreenPosition?.Invoke(point);
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) return;

        _inputActions.Disable();
        _inputActions.Enable();
    }

}
