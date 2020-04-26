using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class Menu_InputListener : MonoBehaviour
{

    public static InputDeviceType CurrentInputDevice = InputDeviceType.KEYBOARD;
    public static Action<InputDeviceType> OnInputDeviceChanged;
    public static Action<NavigateDirection> OnNavigate;

    public static Action<float> OnSecondaryNavigate;

    public static Action OnSubmit;

    public static Action OnCancel;

    public void Awake()
    {
        InputUser.onChange += OnInputUserChanged;
    }

    private void OnInputUserChanged(InputUser arg1, InputUserChange arg2, InputDevice arg3)
    {
        if (arg2 == InputUserChange.ControlsChanged)
        {
            CurrentInputDevice =  arg1.controlScheme.Value.name == "Gamepad" ? InputDeviceType.GAMEPAD : InputDeviceType.KEYBOARD;
            OnInputDeviceChanged?.Invoke(obj: CurrentInputDevice);
        }
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 val = context.ReadValue<Vector2>();
            if (val.y > 0)
                OnNavigate?.Invoke(obj: NavigateDirection.UP);
            else if (val.y < 0)
                OnNavigate?.Invoke(obj: NavigateDirection.DOWN);
            else if (val.x > 0)
                OnNavigate?.Invoke(obj: NavigateDirection.RIGHT);
            else
                OnNavigate?.Invoke(obj: NavigateDirection.LEFT);
        }
    }

    public void SecondaryNavigate(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float value = context.ReadValue<float>();
            OnSecondaryNavigate?.Invoke(obj: value);
        }
    }

    public void Submit(InputAction.CallbackContext context)
    {
        if (context.performed) OnSubmit?.Invoke();
    }

    public void Cancel(InputAction.CallbackContext context)
    {
        if (context.performed) OnCancel?.Invoke();
    }
}

public enum NavigateDirection
{
    UP,
    DOWN,
    LEFT,
    RIGHT
}

public enum InputDeviceType
{
    GAMEPAD,
    KEYBOARD
}