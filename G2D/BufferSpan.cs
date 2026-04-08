using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class BufferSpan
{
    private readonly BufferSpanPool _pool;

    internal int _index;

    internal readonly ulong _offset;

    internal readonly ulong _size;

    internal BufferSpan(BufferSpanPool pool, int index, ulong offset, ulong size)
    {
        _pool = pool;
        _index = index;
        _offset = offset;
        _size = size;
    }

    public VkBuffer Buffer => _pool._buffer[_index];

    public ulong Offset => _offset;

    public ulong Size => _size;

    public void Upload<T>(ref T data) where T : unmanaged
    {
        var size = (ulong)sizeof(T);

        fixed (void* src = &data)
        {
            Upload(src, size);
        }
    }

    public void Upload<T>(ReadOnlySpan<T> data) where T : unmanaged
    {
        var size = (ulong)sizeof(T) * (ulong)data.Length;
        fixed (void* src = data)
        {
            Upload(src, size);
        }
    }

    public void Upload(void* src, ulong size)
    {
        _pool.Upload(this, src, size);
    }
}