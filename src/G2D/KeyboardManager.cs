using SDL;

namespace G2D;

public unsafe class KeyboardManager
{
    private readonly int _keyCount;
    private readonly SDLBool* _state;

    internal KeyboardManager()
    {
        var keyCount = 0;
        _state = SDL3.SDL_GetKeyboardState(&keyCount);
        _keyCount = keyCount;
    }

    public bool IsKeyDown(Keys key)
    {
        var code = (int)key;
        if (code >= _keyCount) return false;
        return _state[code];
    }

    public bool IsKeyUp(Keys key)
    {
        return !IsKeyDown(key);
    }

    // public int GetPressedKeyCount()
    // {
    //     var sum = 0;
    //     for (var i = 0; i < _keyCount; i++)
    //         if (_state[i])
    //             sum++;
    //     return sum;
    // }
    //
    // public Keys[] GetPressedKeys()
    // {
    //     var keys = new List<Keys>((int)Keys.Count);
    //
    //     for (var i = 0; i < _keyCount; i++)
    //         if (_state[i])
    //             keys.Add((Keys)i);
    //     return [.. keys];
    // }
}