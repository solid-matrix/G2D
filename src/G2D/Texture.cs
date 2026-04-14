using G2D.Mathematics;

namespace G2D;

public struct Texture
{
    private readonly TextureCollection _collection;

    public int Index;

    public Size2 Size => _collection.Extents[Index];

    public float Width => _collection.Extents[Index].Width;

    public float Height => _collection.Extents[Index].Height;

    internal Texture(TextureCollection collection, int index)
    {
        _collection = collection;
        Index = index;
    }
}