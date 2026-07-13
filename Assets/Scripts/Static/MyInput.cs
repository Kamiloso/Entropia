using UnityEngine.InputSystem;

public static class MyInput
{
    public static bool IsBack =>
        !IsEngine && !IsTurbo && Keyboard.current != null && Keyboard.current.leftAltKey.isPressed;

    public static bool IsEngine =>
        !IsTurbo && Keyboard.current != null && Keyboard.current.spaceKey.isPressed;

    public static bool IsTurbo =>
        Keyboard.current != null && Keyboard.current.leftShiftKey.isPressed;

    public static bool IsUp =>
        Keyboard.current != null && Keyboard.current.wKey.isPressed;

    public static bool IsDown =>
        Keyboard.current != null &&Keyboard.current.sKey.isPressed;

    public static bool IsLeft =>
        Keyboard.current != null && Keyboard.current.aKey.isPressed;

    public static bool IsRight =>
        Keyboard.current != null && Keyboard.current.dKey.isPressed;

    public static bool IsRollLeft =>
        Keyboard.current != null && Keyboard.current.qKey.isPressed;

    public static bool IsRollRight =>
        Keyboard.current != null && Keyboard.current.eKey.isPressed;
}