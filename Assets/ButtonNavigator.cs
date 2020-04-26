using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

public class ButtonNavigator : MonoBehaviour
{
    [SerializeField]
    private Button _cancelButton;

    private Selectable _currentlySelected;

    [SerializeField]
    private Color _defaultColor = Color.white;

    private bool _navigationEnabled;

    [SerializeField]
    private Selectable[] _selectables;

    #region UnityEvents
    private void Awake()
    {
        Assert.IsTrue(condition: _selectables.Length > 0);
        Assert.IsNotNull(value: _cancelButton);
    }

    private void OnEnable()
    {
        Menu_InputListener.OnInputDeviceChanged += OnInputDeviceChanged;
        Menu_InputListener.OnNavigate += OnNavigate;
        Menu_InputListener.OnSecondaryNavigate += OnSecondaryNavigate;
        Menu_InputListener.OnSubmit += OnSubmit;
        Menu_InputListener.OnCancel += OnCancel;

        if (Menu_InputListener.CurrentInputDevice == InputDeviceType.GAMEPAD)
        {
            _navigationEnabled = true;
            Select(_selectables[0]);
        }
    }

    private void OnDisable()
    {
        Menu_InputListener.OnNavigate -= OnNavigate;
        Menu_InputListener.OnSecondaryNavigate -= OnSecondaryNavigate;
        Menu_InputListener.OnSubmit -= OnSubmit;
        Menu_InputListener.OnCancel -= OnCancel;
        
        if (Menu_InputListener.CurrentInputDevice == InputDeviceType.GAMEPAD && _currentlySelected)
        {
            _currentlySelected.targetGraphic.color = _currentlySelected.colors.normalColor;
            _currentlySelected = null;
        }
    }
#endregion

    private void Select(Selectable obj)
    {
        _currentlySelected = obj;
        if (_navigationEnabled)
        {
            _currentlySelected.targetGraphic.color = _currentlySelected.colors.highlightedColor;

            EventTrigger trigger = _currentlySelected.GetComponent<EventTrigger>();
            if (trigger) trigger.OnPointerEnter(eventData: new PointerEventData(eventSystem: EventSystem.current));
        }
    }

    private void OnInputDeviceChanged(InputDeviceType type)
    {
        if (type == InputDeviceType.GAMEPAD)
        {
            _navigationEnabled = true;
            Select(obj: _selectables[0]);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            _navigationEnabled = false;
            if (_currentlySelected != null)
            {
                _currentlySelected.targetGraphic.color = _currentlySelected.colors.normalColor;
                _currentlySelected = null;
            }

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnCancel()
    {
        if (_navigationEnabled) _cancelButton.onClick?.Invoke();
    }

    private void OnSubmit()
    {
        if (_navigationEnabled) _currentlySelected.OnPointerDown(eventData: new PointerEventData(eventSystem: EventSystem.current));
    }

    private void OnSecondaryNavigate(float obj)
    {
        if (_navigationEnabled)
        {
            AxisEventData axisEventData = new AxisEventData(eventSystem: EventSystem.current);
            MoveDirection moveDir = obj > 0 ? MoveDirection.Right : MoveDirection.Left;
            axisEventData.moveDir = moveDir;
            axisEventData.moveVector = Vector2.right * obj;
            _currentlySelected.OnMove(eventData: axisEventData);
        }
    }

    private void OnNavigate(NavigateDirection obj)
    {
        if (_navigationEnabled)
        {
            Selectable newObj = FindSelectableInDirection(dir: obj);
            if (newObj)
            {
                _currentlySelected.targetGraphic.color = _currentlySelected.colors.normalColor;
                Select(obj: newObj);
            }
        }
    }

    private Selectable FindSelectableInDirection(NavigateDirection dir)
    {
        Selectable[] dirSelectables;
        Selectable nextSelectable = null;
        switch (dir)
        {
            case NavigateDirection.UP:
                dirSelectables = _selectables.Where(predicate: button => button.transform.position.y > _currentlySelected.transform.position.y && Mathf.Abs(
                                                 f: button
                                                    .transform.position.x - _currentlySelected.transform.position.x) < 100)
                                             .ToArray();
                if (dirSelectables.Length > 0)
                    nextSelectable = dirSelectables.OrderBy(keySelector: button => button.transform.position.y).ToArray()[0];
                else
                    nextSelectable = _currentlySelected.FindSelectableOnUp();

                break;
            case NavigateDirection.DOWN:
                dirSelectables = _selectables.Where(predicate: button => button.transform.position.y < _currentlySelected.transform.position.y && Mathf.Abs(
                                                 f: button
                                                    .transform.position.x - _currentlySelected.transform.position.x) < 100)
                                             .ToArray();
                if (dirSelectables.Length > 0)
                    nextSelectable = dirSelectables.OrderByDescending(keySelector: button => button.transform.position.y).ToArray()[0];
                else
                    nextSelectable = _currentlySelected.FindSelectableOnDown();

                break;
            case NavigateDirection.LEFT:
                dirSelectables = _selectables.Where(predicate: button => button.transform.position.x < _currentlySelected.transform.position.x && Mathf.Abs(
                                                 f: button
                                                    .transform.position.y - _currentlySelected.transform.position.y) < 100)
                                             .ToArray();
                if (dirSelectables.Length > 0)
                    nextSelectable = dirSelectables.OrderByDescending(keySelector: button => button.transform.position.x).ToArray()[0];
                else
                    nextSelectable = _currentlySelected.FindSelectableOnLeft();

                break;
            case NavigateDirection.RIGHT:
                dirSelectables = _selectables.Where(predicate: button => button.transform.position.x > _currentlySelected.transform.position.x && Mathf.Abs(
                                                 f: button
                                                    .transform.position.y - _currentlySelected.transform.position.y) < 100)
                                             .ToArray();
                if (dirSelectables.Length > 0)
                    nextSelectable = dirSelectables.OrderBy(keySelector: button => button.transform.position.x).ToArray()[0];
                else
                    nextSelectable = _currentlySelected.FindSelectableOnRight();

                break;
            default:
                nextSelectable = _currentlySelected;
                break;
        }

        return nextSelectable;
    }
}