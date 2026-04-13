using G2D.Mathematics;

namespace G2D;

public struct Texture
{
    private readonly TextureCollection _collection;

    public int Index;

    public Size2 Size => _collection._extents[Index];

    public float Width => _collection._extents[Index].Width;

    public float Height => _collection._extents[Index].Height;

    internal Texture(TextureCollection collection, int index)
    {
        _collection = collection;
        Index = index;
    }
}