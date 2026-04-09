using SDL;

namespace G2D;

public ref struct WindowEvent
{
    // TODO

    internal ref SDL_WindowEvent _event;

    internal WindowEvent(ref SDL_Event e)
    {
        _event = ref e.window;
    }

    public EventType EventType => (EventType)_event.type;
}