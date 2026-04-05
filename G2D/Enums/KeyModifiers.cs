namespace G2D;

[Flags]
public enum KeyModifiers : ushort
{
    None = 0,
    Lshift = 1,
    Rshift = 2,
    Level5 = 4,
    Lctrl = 0x40,
    Rctrl = 0x80,
    Lalt = 0x100,
    Ralt = 0x200,
    Lgui = 0x400,
    Rgui = 0x800,
    Num = 0x1000,
    Caps = 0x2000,
    Mode = 0x4000,
    Scroll = 0x8000,
    Ctrl = 0xC0,
    Shift = 3,
    Alt = 0x300,
    Gui = 0xC00
}