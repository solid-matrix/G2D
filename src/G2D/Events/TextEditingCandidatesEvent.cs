using SDL;

namespace G2D;

public ref struct TextEditingCandidatesEvent
{
    // TODO

    internal ref SDL_TextEditingCandidatesEvent _event;

    internal TextEditingCandidatesEvent(ref SDL_Event e)
    {
        _event = ref e.edit_candidates;
    }

    public EventType EventType => (EventType)_event.type;
}