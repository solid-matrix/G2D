using G2D.Mathematics;
using SDL;

namespace G2D;

public readonly struct MouseButtonUpEvent
{
    private readonly SDL_Event _e;

    internal MouseButtonUpEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;

    public MouseButtons Button => (MouseButtons)(1 << (_e.button.button - 1));

    public int Clicks => _e.button.clicks;

    public Vec2 Location => new(_e.button.x, _e.button.y);
}