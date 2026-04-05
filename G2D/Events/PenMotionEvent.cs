using SDL;

namespace G2D;

public ref struct PenMotionEvent
{
    // TODO

    internal ref SDL_PenMotionEvent _event;

    internal PenMotionEvent(ref SDL_Event e)
    {
        _event = ref e.pmotion;
    }

    public EventType EventType => (EventType)_event.type;
}