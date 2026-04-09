using SDL;

namespace G2D;

public ref struct SensorEvent
{
    // TODO

    internal ref SDL_SensorEvent _event;

    internal SensorEvent(ref SDL_Event e)
    {
        _event = ref e.sensor;
    }

    public EventType EventType => (EventType)_event.type;
}