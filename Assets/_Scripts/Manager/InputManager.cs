using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>, InputSystem_Actions.IPlayerActions, InputSystem_Actions.IUIActions
{
    private InputSystem_Actions _inputs;
    
    protected override void Awake()
    {
        base.Awake();

        if (_inputs == null)
        {
            _inputs = new InputSystem_Actions();

            _inputs.Player.SetCallbacks(this);
            _inputs.UI.SetCallbacks(this);
        }
    }

    private void OnEnable()
    {
        PlayerInputEnable();
    }

    private void OnDisable()
    {
        PlayerInputDisable();
        UiInputDisable();
    }

    public void UiInputEnable() => _inputs.UI.Enable();
    public void UiInputDisable() => _inputs.UI.Disable();
    public void PlayerInputEnable() => _inputs.Player.Enable();
    public void PlayerInputDisable() => _inputs.Player.Disable();

    public void SwitchToUI()
    {
        PlayerInputDisable();
        UiInputEnable();
    }

    public void SwitchToPlayer()
    {
        UiInputDisable();
        PlayerInputEnable();
    }

    public event Action<Vector2> MoveEvent;
    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public event Action InteractEvent;
    public void OnInteract(InputAction.CallbackContext context)
    {
        InteractEvent?.Invoke();
    }
    
    public Vector2 MousePosition;
    public void OnCameraMovement(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }

    public event Action CameraClickEvent;
    public void OnCameraClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            CameraClickEvent?.Invoke();
        }
    }

    public event Action PauseEvent;
    public void OnPause(InputAction.CallbackContext context)
    {
        PauseEvent?.Invoke();
    }
}