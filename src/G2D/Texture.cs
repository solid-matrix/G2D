using Vortice.Mathematics;

namespace G2D;

public struct Texture
{
    internal TextureManager Manager;

    public int Index;

    public Size Size => Manager._extents[Index];

    public float Width => Manager._extents[Index].Width;

    public float Height => Manager._extents[Index].Height;

    internal Texture(TextureManager manager, int index)
    {
        Manager = manager;
        Index = index;
    }
}