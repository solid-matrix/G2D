using SDL;

namespace G2D;

public ref struct JoyBallEvent
{
    // TODO

    internal ref SDL_JoyBallEvent _event;

    internal JoyBallEvent(ref SDL_Event e)
    {
        _event = ref e.jball;
    }

    public EventType EventType => (EventType)_event.type;
}