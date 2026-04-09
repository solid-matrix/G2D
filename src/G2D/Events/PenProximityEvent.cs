using SDL;

namespace G2D;

public ref struct PenProximityEvent
{
    // TODO

    internal ref SDL_PenProximityEvent _event;

    internal PenProximityEvent(ref SDL_Event e)
    {
        _event = ref e.pproximity;
    }

    public EventType EventType => (EventType)_event.type;
}