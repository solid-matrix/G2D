namespace G2D;

public class Assets
{
    private readonly TextureManager _textureManager;

    internal Assets(TextureManager textureManager)
    {
        _textureManager = textureManager;
    }

    public Texture LoadTexture(byte[] raw)
    {
        return _textureManager.CreateTextureFromRaw(raw);
    }

    public Texture[] LoadTextureAtlas(byte[] atlas, byte[] imageRaw)
    {
        throw new NotImplementedException();
    }
}