using System.Runtime.InteropServices;
using SDL;

namespace G2D;

public readonly unsafe struct TextEditingCandidatesEvent
{
    private readonly SDL_Event _e;

    internal TextEditingCandidatesEvent(SDL_Event e)
    {
        _e = e;
    }

    public EventType EventType => (EventType)_e.type;

    public string[] Candidates
    {
        get
        {
            if (_e.edit_candidates.candidates == null) return [];
            var res = new string[_e.edit_candidates.num_candidates];
            for (var i = 0; i < _e.edit_candidates.num_candidates; i++)
                res[i] = Marshal.PtrToStringUTF8((nint)_e.edit_candidates.candidates[i]) ?? string.Empty;
            return res;
        }
    }

    public int NumCandidates => _e.edit_candidates.num_candidates;

    public int SelectedCandidate => _e.edit_candidates.selected_candidate;

    public bool Horizontal => _e.edit_candidates.horizontal;
}