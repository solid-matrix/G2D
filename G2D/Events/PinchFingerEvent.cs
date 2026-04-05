using SDL;

namespace G2D;

public ref struct PinchFingerEvent
{
    // TODO

    internal ref SDL_PinchFingerEvent _event;

    internal PinchFingerEvent(ref SDL_Event e)
    {
        _event = ref e.pinch;
    }

    public EventType EventType => (EventType)_event.type;
}