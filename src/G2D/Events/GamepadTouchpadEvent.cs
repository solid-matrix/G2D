using SDL;

namespace G2D;

public ref struct GamepadTouchpadEvent
{
    // TODO

    internal ref SDL_GamepadTouchpadEvent _event;

    internal GamepadTouchpadEvent(ref SDL_Event e)
    {
        _event = ref e.gtouchpad;
    }

    public EventType EventType => (EventType)_event.type;
}