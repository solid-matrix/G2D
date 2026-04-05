using SDL;

namespace G2D;

public ref struct JoyDeviceEvent
{
    // TODO

    internal ref SDL_JoyDeviceEvent _event;

    internal JoyDeviceEvent(ref SDL_Event e)
    {
        _event = ref e.jdevice;
    }

    public EventType EventType => (EventType)_event.type;
}