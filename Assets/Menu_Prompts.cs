using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class Menu_Prompts : MonoBehaviour
{
    [SerializeField]
    private Sprite _cancelGamepadSprite;

    [SerializeField]
    private Sprite _cancelKeyboardSprite;

    [SerializeField]
    private Image _cancelPromptImage;

    [SerializeField]
    private Sprite _selectGamepadSprite;

    [SerializeField]
    private Sprite _selectKeyboardSprite;

    [SerializeField]
    private Image _selectPromptImage;

    private void Awake()
    {
        Assert.IsNotNull(value: _selectGamepadSprite);
        Assert.IsNotNull(value: _selectKeyboardSprite);
        Assert.IsNotNull(value: _cancelGamepadSprite);
        Assert.IsNotNull(value: _cancelKeyboardSprite);
        Assert.IsNotNull(value: _selectPromptImage);
        Assert.IsNotNull(value: _cancelPromptImage);
    }

    private void OnEnable()
    {
        Menu_InputListener.OnInputDeviceChanged += RefreshPrompts;
    }

    private void OnDisable()
    {
        Menu_InputListener.OnInputDeviceChanged -= RefreshPrompts;
    }

    private void RefreshPrompts(InputDeviceType type)
    {
        _selectPromptImage.sprite = type == InputDeviceType.GAMEPAD ? _selectGamepadSprite : _selectKeyboardSprite;
        _cancelPromptImage.sprite = type == InputDeviceType.GAMEPAD ? _cancelGamepadSprite : _cancelKeyboardSprite;
    }
}