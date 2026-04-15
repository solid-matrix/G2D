using StbImageSharp;

namespace G2D;

public class AssetsManager
{
    private readonly TextureCollection _textureCollection;

    internal AssetsManager(TextureCollection textureCollection)
    {
        _textureCollection = textureCollection;
        Create1PixelWhiteTexture();
    }

    public Texture LoadTexture(byte[] raw)
    {
        var (data, width, height) = DecodeImageRawData(raw);
        return _textureCollection.CreateTexture(data, (uint)width, (uint)height);
    }

    public Texture LoadTexture(ReadOnlySpan<byte> data, int width, int height)
    {
        return _textureCollection.CreateTexture(data, (uint)width, (uint)height);
    }


    public Texture[] LoadTextureAtlas(byte[] atlas, byte[] imageRaw)
    {
        throw new NotImplementedException();
    }


    public void UnloadTexture(Texture texture)
    {
        _textureCollection.DestroyTexture(texture);
    }


    private static (byte[]data, int width, int height) DecodeImageRawData(byte[] raw)
    {
        var result = ImageResult.FromMemory(raw, ColorComponents.RedGreenBlueAlpha);

        return (result.Data, result.Width, result.Height);
    }

    private Texture Create1PixelWhiteTexture()
    {
        byte[] data = [255, 255, 255, 255];
        var texture = _textureCollection.CreateTexture(data, 1, 1);

        if (texture.Index != 0) throw new Exception("failed to create 1 pixel white texture at 0");

        return texture;
    }
}