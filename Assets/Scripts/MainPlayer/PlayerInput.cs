using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerInput : MonoBehaviour
{
    public bool IsTurbo =>
        enabled && Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;

    public bool IsEngine =>
        enabled && Keyboard.current != null && !Keyboard.current.leftShiftKey.isPressed && Keyboard.current.spaceKey.isPressed;

    public bool IsBrake =>
        enabled && Keyboard.current != null && !Keyboard.current.leftShiftKey.isPressed && !Keyboard.current.spaceKey.isPressed && Keyboard.current.leftAltKey.isPressed;

    public bool IsUp =>
        enabled && Keyboard.current != null && Keyboard.current.wKey.isPressed;

    public bool IsDown =>
        enabled && Keyboard.current != null && Keyboard.current.sKey.isPressed;

    public bool IsRight =>
        enabled && Keyboard.current != null && Keyboard.current.dKey.isPressed;

    public bool IsLeft =>
        enabled && Keyboard.current != null && Keyboard.current.aKey.isPressed;

    public bool IsRollRight =>
        enabled && Keyboard.current != null && Keyboard.current.eKey.isPressed;

    public bool IsRollLeft =>
        enabled && Keyboard.current != null && Keyboard.current.qKey.isPressed;
}
