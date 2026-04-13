using G2D.Mathematics;
using SDL;

namespace G2D;

public readonly struct MouseMotionEvent
{
    private readonly SDL_Event _e;

    internal MouseMotionEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;

    public MouseButtons Buttons => (MouseButtons)_e.motion.state;

    public Vec2 Position => new(_e.motion.x, _e.motion.y);

    public Vec2 Delta => new(_e.motion.xrel, _e.motion.yrel);
}