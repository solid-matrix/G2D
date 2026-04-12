using G2D.Mathematics;

namespace G2D;

public struct Texture
{
    internal TextureCollection Collection;

    public int Index;

    public Size2 Size => Collection._extents[Index];

    public float Width => Collection._extents[Index].Width;

    public float Height => Collection._extents[Index].Height;

    internal Texture(TextureCollection collection, int index)
    {
        Collection = collection;
        Index = index;
    }
}