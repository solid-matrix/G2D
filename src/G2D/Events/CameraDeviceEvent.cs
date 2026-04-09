using SDL;

namespace G2D;

public ref struct CameraDeviceEvent
{
    // TODO

    internal ref SDL_CameraDeviceEvent _event;

    internal CameraDeviceEvent(ref SDL_Event e)
    {
        _event = ref e.cdevice;
    }

    public EventType EventType => (EventType)_event.type;
}