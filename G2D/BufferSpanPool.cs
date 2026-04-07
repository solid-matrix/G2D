using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class BufferSpanPool : IDisposable
{
    private readonly VulkanDevice _device;

    private readonly VkBufferUsageFlags _bufferUsage;

    private readonly VmaMemoryUsage _memoryUsage;

    private readonly List<VkBuffer> _buffers;

    private readonly List<ulong> _sizes;

    private readonly List<ulong> _occupied;

    private readonly List<VmaAllocation> _allocations;

    private ulong _sizeCount;

    public BufferSpanPool(VulkanDevice device, VkBufferUsageFlags bufferUsage, VmaMemoryUsage memoryUsage, ulong initialCapacity = 0)
    {
        _device = device;
        _bufferUsage = bufferUsage;
        _memoryUsage = memoryUsage;

        _buffers = [];
        _sizes = [];
        _occupied = [];
        _allocations = [];
        _sizeCount = 0;

        if (initialCapacity > 0) AddBuffer(initialCapacity);
    }

    public void Dispose()
    {
        for (var i = 0; i < _buffers.Count; i++) Vma.vmaDestroyBuffer(_device.Allocator, _buffers[i], _allocations[i]);
    }

    public BufferSpan Allocate(ulong size)
    {
        _sizeCount += size;

        for (var i = 0; i < _buffers.Count; i++)
            if (_sizes[i] - _occupied[i] >= size)
            {
                var span = new BufferSpan(this, _buffers[i], _allocations[i], _occupied[i], size);
                _occupied[i] += size;
                return span;
            }

        var bufferIndex = AddBuffer(size);
        _occupied[bufferIndex] += size;

        return new BufferSpan(this, _buffers[bufferIndex], _allocations[bufferIndex], 0, size);
    }

    public void Reset()
    {
        if (_buffers.Count == 0) return;

        if (_buffers.Count <= 1)
        {
            // if only one buffer exist, then reuse it.
            _occupied[0] = 0;
            _sizeCount = 0;
            return;
        }

        // if more than one buffer exist, then clear all and create a larger one
        for (var i = 0; i < _buffers.Count; i++) Vma.vmaDestroyBuffer(_device.Allocator, _buffers[i], _allocations[i]);

        _buffers.Clear();
        _allocations.Clear();
        _sizes.Clear();
        _occupied.Clear();

        AddBuffer(_sizeCount);

        _sizeCount = 0;
    }

    private int AddBuffer(ulong size)
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

        Vma.vmaCreateBuffer(_device.Allocator, &bufferInfo, &allocInfo, out var buffer, out var allocation, out _)
            .CheckResult("failed to create buffer");

        _buffers.Add(buffer);
        _allocations.Add(allocation);
        _occupied.Add(0);
        _sizes.Add(size);

        return _buffers.Count - 1;
    }

    public BufferSpan AllocateUpload<T>(ref T data) where T : unmanaged
    {
        var size = (ulong)sizeof(T);
        var span = Allocate(size);

        fixed (void* src = &data)
        {
            span.Upload(src, size);
        }

        return span;
    }

    public BufferSpan AllocateUpload<T>(ReadOnlySpan<T> data) where T : unmanaged
    {
        var size = (ulong)sizeof(T) * (ulong)data.Length;
        var span = Allocate(size);
        fixed (void* src = data)
        {
            span.Upload(src, size);
        }

        return span;
    }

    internal void Upload(BufferSpan bufferSpan, void* src, ulong size)
    {
        ArgumentOutOfRangeException.ThrowIfGreaterThan(size, bufferSpan._size);

        void* dst = null;

        Vma.vmaMapMemory(_device.Allocator, bufferSpan._allocation, &dst);

        dst = (void*)((ulong)dst + bufferSpan._offset);

        Buffer.MemoryCopy(src, dst, size, size);

        Vma.vmaUnmapMemory(_device.Allocator, bufferSpan._allocation);
    }
}