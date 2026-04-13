using G2D.Mathematics;
using SDL;

namespace G2D;

public readonly struct MouseWheelEvent
{
    private readonly SDL_Event _e;

    internal MouseWheelEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;

    public Vec2 Position => new(_e.wheel.mouse_x, _e.wheel.mouse_y);

    public Vec2 Scroll => new(_e.wheel.x, _e.wheel.y);

    public Vec2I Ticks => new(_e.wheel.integer_x, _e.wheel.integer_y);

    public MouseWheelDirection Direction => (MouseWheelDirection)_e.wheel.direction;
}