namespace G2D;

public class AssetsManager
{
    private readonly TextureCollection _textureCollection;

    internal AssetsManager(TextureCollection textureCollection)
    {
        _textureCollection = textureCollection;
    }

    public Texture LoadTexture(byte[] raw) => _textureCollection.CreateTextureFromRaw(raw);

    public void UnloadTexture(Texture texture)
    {
        _textureCollection.DestroyTexture(texture);
    }

    public Texture[] LoadTextureAtlas(byte[] atlas, byte[] imageRaw) => throw new NotImplementedException();

    private void DecodeImageRawData(byte[] raw)
    {
        // new StbImageSharp.StbImage.stbi__context()
    }

    public Texture LoadTextureFromRgba(ReadOnlySpan<byte> data, int width, int height) => throw new NotImplementedException();
}