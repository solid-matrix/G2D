using G2D.Mathematics;
using SDL;

namespace G2D;

public unsafe class MouseManager
{
    private float _x;

    private float _y;

    private MouseButtons _buttons;

    internal MouseManager()
    {
        Update();
    }

    public float X => _x;

    public float Y => _y;

    public MouseButtons Buttons => _buttons;

    public Vec2 Position => new(_x, _y);

    internal void Update()
    {
        float x, y;
        _buttons = (MouseButtons)SDL3.SDL_GetMouseState(&x, &y);
    }

    public bool IsButtonDown(MouseButtons button)
    {
        return (_buttons & button) == button;
    }

    public bool IsButtonUp(MouseButtons button)
    {
        return (_buttons & button) == 0;
    }
}