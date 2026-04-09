using SDL;

namespace G2D;

public ref struct GamepadButtonEvent
{
    // TODO

    internal ref SDL_GamepadButtonEvent _event;

    internal GamepadButtonEvent(ref SDL_Event e)
    {
        _event = ref e.gbutton;
    }

    public EventType EventType => (EventType)_event.type;
}