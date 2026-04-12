namespace G2D;

public class Assets
{
    private readonly TextureCollection _textureCollection;

    internal Assets(TextureCollection textureCollection)
    {
        _textureCollection = textureCollection;
    }

    public Texture LoadTexture(byte[] raw)
    {
        return _textureCollection.CreateTextureFromRaw(raw);
    }

    public void UnloadTexture(Texture texture)
    {
        _textureCollection.DestroyTexture(texture);
    }

    public Texture[] LoadTextureAtlas(byte[] atlas, byte[] imageRaw)
    {
        throw new NotImplementedException();
    }
}