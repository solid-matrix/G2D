using SDL;

namespace G2D;

public ref struct ClipboardEvent
{
    // TODO

    internal ref SDL_ClipboardEvent _event;

    internal ClipboardEvent(ref SDL_Event e)
    {
        _event = ref e.clipboard;
    }

    public EventType EventType => (EventType)_event.type;
}