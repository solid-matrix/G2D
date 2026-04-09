using SDL;

namespace G2D;

public ref struct RenderEvent
{
    // TODO

    internal ref SDL_RenderEvent _event;

    internal RenderEvent(ref SDL_Event e)
    {
        _event = ref e.render;
    }

    public EventType EventType => (EventType)_event.type;
}