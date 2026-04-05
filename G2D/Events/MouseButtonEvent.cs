using System.Numerics;
using SDL;

namespace G2D;

public readonly ref struct MouseButtonEvent
{
    internal readonly ref SDL_MouseButtonEvent _event;

    public MouseButtons Button => (MouseButtons)(1 << (_event.button - 1));

    public bool IsDown => _event.down;

    public bool IsUp => !_event.down;

    public int Clicks => _event.clicks;

    public bool IsSingleClick => Clicks == 1;

    public bool IsDoubleClick => Clicks == 2;

    public Vector2 Location => new(_event.x, _event.y);

    internal MouseButtonEvent(ref SDL_Event e)
    {
        _event = ref e.button;
    }

    public EventType EventType => (EventType)_event.type;
}