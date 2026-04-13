using Vortice.Vulkan;

namespace G2D;

public readonly struct GraphicsShader : IEquatable<GraphicsShader>
{
    private readonly GraphicsLayout _layout;

    private readonly int _index;

    internal GraphicsShader(GraphicsLayout layout, int index)
    {
        _layout = layout;
        _index = index;
    }

    internal VkPipeline Pipeline => _layout.Pipelines[_index];

    public static bool operator ==(GraphicsShader left, GraphicsShader right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(GraphicsShader left, GraphicsShader right)
    {
        return !(left == right);
    }

    public bool Equals(GraphicsShader other)
    {
        return _layout == other._layout && _index == other._index;
    }

    public override bool Equals(object? obj)
    {
        return obj is GraphicsShader other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(_layout, _index);
    }
}