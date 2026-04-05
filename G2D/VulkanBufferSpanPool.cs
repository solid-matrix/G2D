using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class VulkanBufferSpanPool : IDisposable
{
    internal readonly VmaAllocator _allocator;

    internal readonly VkBufferUsageFlags _bufferUsage;

    internal readonly VmaMemoryUsage _memoryUsage;

    internal readonly List<VkBuffer> _buffers;

    internal readonly List<VmaAllocation> _allocations;

    public VulkanBufferSpanPool(VmaAllocator allocator, VkBufferUsageFlags bufferUsage, VmaMemoryUsage memoryUsage)
    {
        _allocator = allocator;
        _bufferUsage = bufferUsage;
        _memoryUsage = memoryUsage;

        _buffers = [];
        _allocations = [];
    }

    public void Dispose()
    {
        Reset();
    }

    public VulkanBufferSpan Allocate(ulong size)
    {
        var bufferInfo = new VkBufferCreateInfo
        {
            size = size,
            usage = _bufferUsage,
            sharingMode = VkSharingMode.Exclusive
        };

        var allocInfo = new VmaAllocationCreateInfo
        {
            usage = _memoryUsage
        };

        if (_memoryUsage is VmaMemoryUsage.CpuToGpu or VmaMemoryUsage.CpuOnly)
            allocInfo.requiredFlags = VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent;

        Vma.vmaCreateBuffer(_allocator, &bufferInfo, &allocInfo, out var buffer, out var allocation, out _)
            .CheckResult("failed to create buffer");

        _buffers.Add(buffer);
        _allocations.Add(allocation);

        return new VulkanBufferSpan(this, buffer, allocation, 0, size);
    }

    public VulkanBufferSpan AllocateUpload<T>(ref T data) where T : unmanaged
    {
        var size = (ulong)sizeof(T);
        var span = Allocate(size);

        fixed (void* src = &data)
        {
            span.Upload(src, size);
        }

        return span;
    }

    public VulkanBufferSpan AllocateUpload<T>(ReadOnlySpan<T> data) where T : unmanaged
    {
        var size = (ulong)sizeof(T) * (ulong)data.Length;
        var span = Allocate(size);
        fixed (void* src = data)
        {
            span.Upload(src, size);
        }

        return span;
    }


    public void Reset()
    {
        for (var i = 0; i < _buffers.Count; i++) Vma.vmaDestroyBuffer(_allocator, _buffers[i], _allocations[i]);

        _buffers.Clear();
        _allocations.Clear();
    }
}