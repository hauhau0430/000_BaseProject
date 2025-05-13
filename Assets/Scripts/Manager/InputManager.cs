using UnityEngine;

public static class InputManager
{
    public static bool GetTouch()
    {
        return Input.GetMouseButton(0);
    }

    public static bool GetTouchDown()
    {
        return Input.GetMouseButtonDown(0);
    }

    public static bool GetTouchUp()
    {
        return Input.GetMouseButtonUp(0);
    }

    public static Vector2? GetTouchPosition()
    {
        if (0 < Input.touchCount)
        {
            return Input.mousePosition;
        }

        return null;
    }
}
