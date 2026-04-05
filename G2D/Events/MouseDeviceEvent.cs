using SDL;

namespace G2D;

public ref struct MouseDeviceEvent
{
    internal ref SDL_MouseDeviceEvent _event;

    internal MouseDeviceEvent(ref SDL_Event e)
    {
        _event = ref e.mdevice;
    }

    public EventType EventType => (EventType)_event.type;
}