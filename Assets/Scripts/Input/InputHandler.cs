using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InputHandler : IDisposable
{
    public event Action<Vector2> OnGetPosition;

    private InputSystem_Actions _inputActions;
    private Camera _main;
    public InputHandler()
    {
        _inputActions = new InputSystem_Actions();
        _main = Camera.main;
        _inputActions.Enable();

        _inputActions.Player.GetPosition.performed += PointPositionHandler;
    }
    public void Dispose()
    {
        _inputActions.Player.GetPosition.performed -= PointPositionHandler;

        _inputActions.Disable();
        _inputActions.Dispose();
    }
    private void PointPositionHandler(InputAction.CallbackContext context)
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        var pointWorldPosition = _main.ScreenToWorldPoint(_inputActions.Player.Position.ReadValue<Vector2>());

        OnGetPosition?.Invoke(pointWorldPosition);
    }
}
