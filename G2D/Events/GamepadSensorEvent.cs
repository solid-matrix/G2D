using SDL;

namespace G2D;

public ref struct GamepadSensorEvent
{
    // TODO

    internal ref SDL_GamepadSensorEvent _event;

    internal GamepadSensorEvent(ref SDL_Event e)
    {
        _event = ref e.gsensor;
    }

    public EventType EventType => (EventType)_event.type;
}