using System.Numerics;
using SDL;

namespace G2D;

public unsafe class Mouse
{
    internal Mouse()
    {
    }

    public bool IsAvailable()
    {
        return SDL3.SDL_HasMouse();
    }

    public MouseButtons GetButtons()
    {
        return (MouseButtons)SDL3.SDL_GetMouseState(null, null);
    }

    public Vector2 GetPosition()
    {
        float x = 0, y = 0;
        SDL3.SDL_GetMouseState(&x, &y);
        return new Vector2(x, y);
    }

    public bool IsButtonDown(MouseButtons button)
    {
        return (GetButtons() & button) == button;
    }

    public bool IsButtonUp(MouseButtons button)
    {
        return (GetButtons() & button) == 0;
    }
}