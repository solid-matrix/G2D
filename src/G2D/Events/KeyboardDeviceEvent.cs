using SDL;

namespace G2D;

public ref struct KeyboardDeviceEvent
{
    // TODO

    internal ref SDL_KeyboardDeviceEvent _event;

    internal KeyboardDeviceEvent(ref SDL_Event e)
    {
        _event = ref e.kdevice;
    }

    public EventType EventType => (EventType)_event.type;
}