using SDL;

namespace G2D;

public ref struct GamepadAxisEvent
{
    // TODO

    internal ref SDL_GamepadAxisEvent _event;

    internal GamepadAxisEvent(ref SDL_Event e)
    {
        _event = ref e.gaxis;
    }

    public EventType EventType => (EventType)_event.type;
}