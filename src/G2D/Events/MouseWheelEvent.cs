using System.Numerics;
using SDL;
using Vortice.Mathematics;

namespace G2D;

public readonly ref struct MouseWheelEvent
{
    internal readonly ref SDL_MouseWheelEvent _event;

    public Vector2 Location => new(_event.mouse_x, _event.mouse_y);

    public MouseWheelDirection Direction => (MouseWheelDirection)_event.direction;

    public Vector2 Scroll => new(_event.x, _event.y);

    public Int2 Ticks => new(_event.integer_x, _event.integer_y);

    internal MouseWheelEvent(ref SDL_Event e)
    {
        _event = ref e.wheel;
    }

    public EventType EventType => (EventType)_event.type;
}