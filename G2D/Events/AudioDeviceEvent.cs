using SDL;

namespace G2D;

public ref struct AudioDeviceEvent
{
    // TODO

    internal ref SDL_AudioDeviceEvent _event;

    internal AudioDeviceEvent(ref SDL_Event e)
    {
        _event = ref e.adevice;
    }

    public EventType EventType => (EventType)_event.type;
}