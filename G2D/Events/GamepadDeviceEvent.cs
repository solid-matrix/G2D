using SDL;

namespace G2D;

public ref struct GamepadDeviceEvent
{
    // TODO

    internal ref SDL_GamepadDeviceEvent _event;

    internal GamepadDeviceEvent(ref SDL_Event e)
    {
        _event = ref e.gdevice;
    }

    public EventType EventType => (EventType)_event.type;
}