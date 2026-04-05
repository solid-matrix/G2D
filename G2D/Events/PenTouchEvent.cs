using SDL;

namespace G2D;

public ref struct PenTouchEvent
{
    // TODO

    internal ref SDL_PenTouchEvent _event;

    internal PenTouchEvent(ref SDL_Event e)
    {
        _event = ref e.ptouch;
    }

    public EventType EventType => (EventType)_event.type;
}