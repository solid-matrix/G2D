using SDL;

namespace G2D;

public ref struct UserEvent
{
    // TODO

    internal ref SDL_UserEvent _event;

    internal UserEvent(ref SDL_Event e)
    {
        _event = ref e.user;
    }

    public EventType EventType => (EventType)_event.type;
}