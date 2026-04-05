using System.Numerics;
using SDL;

namespace G2D;

public readonly ref struct MouseMotionEvent
{
    internal readonly ref SDL_MouseMotionEvent _event;

    public MouseButtons Buttons => (MouseButtons)_event.state;

    public Vector2 Position => new(_event.x, _event.y);

    public Vector2 Delta => new(_event.xrel, _event.yrel);

    internal MouseMotionEvent(ref SDL_Event e)
    {
        _event = ref e.motion;
    }

    public EventType EventType => (EventType)_event.type;
}