using G2D.Mathematics;
using SDL;

namespace G2D;

public readonly struct WindowResizedEvent
{
    private readonly SDL_Event _e;

    internal WindowResizedEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;

    public int Width => _e.window.data1;

    public int Height => _e.window.data2;

    public Size2I Size => new(_e.window.data1, _e.window.data2);
}