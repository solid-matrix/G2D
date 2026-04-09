using SDL;

namespace G2D;

public ref struct TextInputEvent
{
    // TODO

    internal ref SDL_TextInputEvent _event;

    internal TextInputEvent(ref SDL_Event e)
    {
        _event = ref e.text;
    }

    public EventType EventType => (EventType)_event.type;
}