using SDL;

namespace G2D;

public ref struct TextEditingEvent
{
    // TODO

    internal ref SDL_TextEditingEvent _event;

    internal TextEditingEvent(ref SDL_Event e)
    {
        _event = ref e.edit;
    }

    public EventType EventType => (EventType)_event.type;
}