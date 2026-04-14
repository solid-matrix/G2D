using G2D.Mathematics;
using SDL;

namespace G2D;

public unsafe class MouseManager
{
    private volatile float _x;

    private volatile float _y;

    private volatile MouseButtons _buttons;

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

    public bool IsButtonDown(MouseButtons button) => (_buttons & button) == button;

    public bool IsButtonUp(MouseButtons button) => (_buttons & button) == 0;
}