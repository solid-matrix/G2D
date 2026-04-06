using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class VulkanBufferSpan
{
    private readonly VulkanBufferSpanPool _pool;

    private readonly VkBuffer _buffer;

    private readonly VmaAllocation _allocation;

    private readonly ulong _offset;

    private readonly ulong _size;

    internal VulkanBufferSpan(VulkanBufferSpanPool pool, VkBuffer buffer, VmaAllocation allocation, ulong offset, ulong size)
    {
        _pool = pool;
        _buffer = buffer;
        _allocation = allocation;
        _offset = offset;
        _size = size;
    }

    public VkBuffer Buffer => _buffer;

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
        ArgumentOutOfRangeException.ThrowIfGreaterThan(size, _size);

        void* dst = null;
        Vma.vmaMapMemory(_pool._device.Allocator, _allocation, &dst);
        dst = (void*)((ulong)dst + _offset);

        System.Buffer.MemoryCopy(src, dst, size, size);

        Vma.vmaUnmapMemory(_pool._device.Allocator, _allocation);
    }
}