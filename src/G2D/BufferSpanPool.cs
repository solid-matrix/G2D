using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class BufferSpanPool : IDisposable
{
    private const uint UnitBufferSize = 1048576;

    private readonly VulkanDevice _device;

    private readonly VkBufferUsageFlags _bufferUsage;

    private readonly VmaMemoryUsage _memoryUsage;

    internal readonly List<VkBuffer> _buffer;

    internal readonly List<ulong> _size;

    internal readonly List<ulong> _occupy;

    internal readonly List<nint> _addr;

    internal readonly List<VmaAllocation> _allocation;

    private ulong _sizeCount;

    public BufferSpanPool(VulkanDevice device, VkBufferUsageFlags bufferUsage, VmaMemoryUsage memoryUsage)
    {
        _device = device;
        _bufferUsage = bufferUsage;
        _memoryUsage = memoryUsage;

        _buffer = [];
        _size = [];
        _occupy = [];
        _allocation = [];
        _addr = [];
        _sizeCount = 0;
    }

    public void Dispose()
    {
        for (var i = 0; i < _buffer.Count; i++) RemoveBuffer(i);
    }

    public BufferSpan Allocate(ulong size)
    {
        _sizeCount += size;
        ulong step;

        for (var i = 0; i < _buffer.Count; i++)
            if (_size[i] - _occupy[i] >= size)
            {
                var span = new BufferSpan(this, i, _occupy[i], size);

                step = _bufferUsage == VkBufferUsageFlags.UniformBuffer ? (size + 64 - 1) / 64 * 64 : size;
                _occupy[i] += step;

                return span;
            }

        var bufferIndex = AddBuffer(size > UnitBufferSize ? size : UnitBufferSize);

        step = _bufferUsage == VkBufferUsageFlags.UniformBuffer ? (size + 64 - 1) / 64 * 64 : size;
        _occupy[bufferIndex] += step;

        return new BufferSpan(this, bufferIndex, 0, size);
    }

    public void Reset()
    {
        if (_buffer.Count == 0) return;

        if (_buffer.Count == 1)
        {
            // if only one buffer exist, then reuse it.
            _occupy[0] = 0;
            _sizeCount = 0;
            return;
        }

        // if more than one buffer exist, then clear all and create a larger one
        for (var i = 0; i < _buffer.Count; i++) RemoveBuffer(i);

        _buffer.Clear();
        _allocation.Clear();
        _size.Clear();
        _occupy.Clear();
        _addr.Clear();

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

        if (_memoryUsage == VmaMemoryUsage.CpuToGpu)
        {
            allocInfo.requiredFlags |= VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent;

            if (_bufferUsage == VkBufferUsageFlags.UniformBuffer) allocInfo.preferredFlags |= VkMemoryPropertyFlags.HostCached;
        }


        Vma.vmaCreateBuffer(_device.Allocator, &bufferInfo, &allocInfo, out var buffer, out var allocation, out _)
            .CheckResult("failed to create buffer");

        void* addr = null;

        Vma.vmaMapMemory(_device.Allocator, allocation, &addr);

        _buffer.Add(buffer);
        _allocation.Add(allocation);
        _addr.Add((nint)addr);

        _occupy.Add(0);
        _size.Add(size);

        return _buffer.Count - 1;
    }

    private void RemoveBuffer(int i)
    {
        Vma.vmaUnmapMemory(_device.Allocator, _allocation[i]);
        Vma.vmaDestroyBuffer(_device.Allocator, _buffer[i], _allocation[i]);
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

        var dst = (void*)((ulong)_addr[bufferSpan._index] + bufferSpan._offset);

        Buffer.MemoryCopy(src, dst, size, size);
    }
}