using System.Reflection;

namespace G2D;

public class EmbeddedResource
{
    private readonly Assembly _assembly;

    public EmbeddedResource(Assembly assembly)
    {
        _assembly = assembly;
    }

    public Stream GetStream(string path)
    {
        var name = $"{_assembly.GetName().Name}.{path.Replace('/', '.').Replace('\\', '.')}";
        return _assembly.GetManifestResourceStream(name) ?? throw new FileNotFoundException();
    }

    public string GetString(string path)
    {
        using var stream = GetStream(path);
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }

    public byte[] GetBytes(string path)
    {
        using var stream = GetStream(path);
        using var ms = new MemoryStream();
        stream.CopyTo(ms);

        return ms.ToArray();
    }
}