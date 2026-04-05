using Vortice.Vulkan;

namespace G2D;

internal unsafe class VulkanBuffer : IDisposable
{
    private readonly VmaAllocator _allocator;

    private readonly VmaAllocation _allocation;

    private readonly VmaAllocationInfo _allocationInfo;

    private readonly VkBuffer _buffer;

    private readonly ulong _size;


    public VulkanBuffer(VmaAllocator allocator, ulong size, VkBufferUsageFlags bufferUsage, VmaMemoryUsage memoryUsage)
    {
        _allocator = allocator;
        _size = size;

        var bufferInfo = new VkBufferCreateInfo
        {
            size = size,
            usage = bufferUsage,
            sharingMode = VkSharingMode.Exclusive
        };

        var allocInfo = new VmaAllocationCreateInfo
        {
            usage = memoryUsage
        };

        if (memoryUsage is VmaMemoryUsage.CpuToGpu or VmaMemoryUsage.CpuOnly)
            allocInfo.requiredFlags = VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent;

        Vma.vmaCreateBuffer(_allocator, &bufferInfo, &allocInfo, out _buffer, out _allocation, out _allocationInfo)
            .CheckResult("failed to create buffer");
    }

    public ulong Size => _size;

    public VkBuffer Buffer => _buffer;

    public void Dispose()
    {
        Vma.vmaDestroyBuffer(_allocator, _buffer, _allocation);
        GC.SuppressFinalize(this);
    }

    public void Upload<T>(ref T data) where T : unmanaged
    {
        void* dst = null;
        Vma.vmaMapMemory(_allocator, _allocation, &dst);

        var size = (ulong)sizeof(T);

        fixed (void* src = &data)
        {
            System.Buffer.MemoryCopy(src, dst, size, size);
        }

        Vma.vmaUnmapMemory(_allocator, _allocation);
    }

    public void Upload<T>(ReadOnlySpan<T> data) where T : unmanaged
    {
        void* dst = null;
        Vma.vmaMapMemory(_allocator, _allocation, &dst);

        var size = (ulong)(sizeof(T) * data.Length);

        fixed (void* src = data)
        {
            System.Buffer.MemoryCopy(src, dst, size, size);
        }

        Vma.vmaUnmapMemory(_allocator, _allocation);
    }
}