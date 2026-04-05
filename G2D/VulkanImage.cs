using Vortice.Vulkan;

namespace G2D;

public unsafe class VulkanImage : IDisposable
{
    private readonly VmaAllocation _allocation;

    private readonly VmaAllocationInfo _allocationInfo;

    private readonly VmaAllocator _allocator;

    private readonly Extent3I _extent;

    private readonly VkImage _image;

    public VulkanImage(VmaAllocator allocator, Extent3I extent, VkImageUsageFlags imageUsage, VmaMemoryUsage memoryUsage, VkFormat format,
        VkImageType type = VkImageType.Image2D,
        VkImageTiling tiling = VkImageTiling.Optimal,
        VkSampleCountFlags samples = VkSampleCountFlags.Count1,
        VkSharingMode sharing = VkSharingMode.Exclusive,
        VkImageLayout initialLayout = VkImageLayout.Undefined,
        uint mipLevels = 1,
        uint arrayLayers = 1
    )
    {
        _allocator = allocator;
        _extent = extent;

        var imageInfo = new VkImageCreateInfo
        {
            imageType = type,
            extent = new VkExtent3D(extent.Width, extent.Height, extent.Depth),
            mipLevels = mipLevels,
            arrayLayers = arrayLayers,
            format = format,
            tiling = tiling,
            initialLayout = initialLayout,
            usage = imageUsage,
            sharingMode = sharing,
            queueFamilyIndexCount = 0,
            pQueueFamilyIndices = null,
            samples = samples
        };

        var allocInfo = new VmaAllocationCreateInfo
        {
            usage = memoryUsage
        };

        Vma.vmaCreateImage(_allocator, &imageInfo, &allocInfo, out _image, out _allocation, out _allocationInfo)
            .CheckResult("failed to create image");
    }

    public VkImage Image => _image;

    private Extent3I Extent => _extent;

    public void Dispose()
    {
        Vma.vmaDestroyImage(_allocator, _image, _allocation);
        GC.SuppressFinalize(this);
    }
}