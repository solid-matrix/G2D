using SDL;

namespace G2D;

public ref struct JoyBatteryEvent
{
    // TODO

    internal ref SDL_JoyBatteryEvent _event;

    internal JoyBatteryEvent(ref SDL_Event e)
    {
        _event = ref e.jbattery;
    }

    public EventType EventType => (EventType)_event.type;
}