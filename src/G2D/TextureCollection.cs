using G2D.Mathematics;
using SDL;
using Vortice.Vulkan;

namespace G2D;

internal unsafe class TextureCollection : IDisposable
{
    // public const uint MaxImageCount = 65536;

    private readonly VulkanDevice _device;

    private readonly VkDescriptorSet _descriptorSet;

    internal readonly List<VkImage> _images = [];

    internal readonly List<VkImageView> _imagesView = [];

    internal readonly List<VmaAllocation> _allocation = [];

    internal readonly List<Size2> _extents = [];

    internal readonly Queue<int> _recycledIndices = [];

    public TextureCollection(VulkanDevice device, VkDescriptorSet descriptorSet)
    {
        _device = device;
        _descriptorSet = descriptorSet;

        Create1PixelWhiteTexture();
    }

    public VkDescriptorSet DescriptorSet => _descriptorSet;

    public void Dispose()
    {
        for (var i = 0; i < _images.Count; i++)
        {
            if (_images[i] == VkImage.Null) continue;
            _device.Api.vkDestroyImageView(_imagesView[i]);
            Vma.vmaDestroyImage(_device.Allocator, _images[i], _allocation[i]);
        }
    }

    private void Create1PixelWhiteTexture()
    {
        byte[] data = [255, 255, 255, 255];
        var texture = CreateTextureFromRgba(data, 1, 1);
        if (texture.Index != 0) throw new Exception("failed to create 1 pixel white texture at 0");
    }

    internal Texture CreateTextureFromRaw(ReadOnlySpan<byte> raw)
    {
        var (image, allocation, extent) = InternalCreateTextureFromRawImage(raw);
        return CreateTexture(image, allocation, extent);
    }

    internal Texture CreateTextureFromRgba(ReadOnlySpan<byte> data, uint width, uint height)
    {
        var (image, allocation, extent) = InternalCreateTextureFromRgbaData(data, width, height);
        return CreateTexture(image, allocation, extent);
    }

    internal Texture CreateTexture(VkImage image, VmaAllocation allocation, Size2 extent)
    {
        var info = new VkImageViewCreateInfo
        {
            image = image,
            viewType = VkImageViewType.Image2D,
            format = VkFormat.R8G8B8A8Srgb,
            components = VkComponentMapping.Identity,
            subresourceRange = new VkImageSubresourceRange(VkImageAspectFlags.Color, 0, 1, 0, 1)
        };

        _device.Api.vkCreateImageView(&info, out var imageView);

        int i;

        if (_recycledIndices.Count == 0)
        {
            i = _images.Count;

            _images.Add(image);
            _imagesView.Add(imageView);
            _allocation.Add(allocation);
            _extents.Add(extent);
        }
        else
        {
            i = _recycledIndices.Dequeue();

            _images[i] = image;
            _imagesView[i] = imageView;
            _allocation[i] = allocation;
        }

        UpdateDescriptorSet(_device, _descriptorSet, (uint)i, imageView);

        return new Texture(this, i);
    }


    internal void DestroyTexture(Texture texture)
    {
        var i = texture.Index;
        _device.Api.vkDestroyImageView(_imagesView[i]);
        Vma.vmaDestroyImage(_device.Allocator, _images[i], _allocation[i]);

        _recycledIndices.Enqueue(texture.Index);
        _images[i] = VkImage.Null;
        _imagesView[i] = VkImageView.Null;
        _allocation[i] = VmaAllocation.Null;
    }

    private (VkImage, VmaAllocation, Size2) InternalCreateTextureFromRawImage(ReadOnlySpan<byte> raw)
    {
        SDL_Surface* surface;

        fixed (byte* rawPtr = raw)
        {
            var stream = SDL3.SDL_IOFromMem((nint)rawPtr, (uint)raw.Length);
            surface = SDL3_image.IMG_Load_IO(stream, true);
            if (surface == null) throw new Exception("failed to load image");
            if (surface->format != SDL3.SDL_PIXELFORMAT_RGBA32)
            {
                var converted = SDL3.SDL_ConvertSurface(surface, SDL3.SDL_PIXELFORMAT_RGBA32);
                SDL3.SDL_DestroySurface(surface);

                if (converted == null) throw new Exception("failed to convert pixel format");
                surface = converted;
            }
        }

        var (image, allocation, size) = InternalCreateTextureFromRgbaData(new ReadOnlySpan<byte>((void*)surface->pixels, surface->h * surface->pitch), (uint)surface->w, (uint)surface->h);

        SDL3.SDL_DestroySurface(surface);

        return (image, allocation, size);
    }


    private (VkImage, VmaAllocation, Size2) InternalCreateTextureFromRgbaData(ReadOnlySpan<byte> data, uint width, uint height)
    {
        // allocate staging buffer
        var stagingBufferInfo = new VkBufferCreateInfo
        {
            size = (uint)data.Length,
            usage = VkBufferUsageFlags.TransferSrc,
            sharingMode = VkSharingMode.Exclusive
        };
        var stagingAllocationInfo = new VmaAllocationCreateInfo
        {
            usage = VmaMemoryUsage.CpuToGpu,
            flags = VmaAllocationCreateFlags.Mapped,
            requiredFlags = VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent
        };

        Vma.vmaCreateBuffer(_device.Allocator, &stagingBufferInfo, &stagingAllocationInfo, out var stagingBuffer, out var stagingAllocation, out _)
            .CheckResult("failed to create buffer");

        // Upload data
        fixed (void* pData = data)
        {
            Vma.vmaCopyMemoryToAllocation(_device.Allocator, pData, stagingAllocation, 0, (uint)data.Length);
        }

        // Create Image
        var imageInfo = new VkImageCreateInfo
        {
            imageType = VkImageType.Image2D,
            format = VkFormat.R8G8B8A8Srgb,
            extent = new VkExtent3D(width, height, 1),
            mipLevels = 1,
            arrayLayers = 1,
            samples = VkSampleCountFlags.Count1,
            tiling = VkImageTiling.Optimal,
            usage = VkImageUsageFlags.TransferDst | VkImageUsageFlags.Sampled,
            sharingMode = VkSharingMode.Exclusive,
            initialLayout = VkImageLayout.Undefined
        };

        var imageAllocationInfo = new VmaAllocationCreateInfo
        {
            usage = VmaMemoryUsage.GpuOnly,
            flags = VmaAllocationCreateFlags.DedicatedMemory
        };

        Vma.vmaCreateImage(_device.Allocator, &imageInfo, &imageAllocationInfo, out var image, out var imageAllocation, out _);

        // Create Temporary Command Buffer
        _device.Api.vkAllocateCommandBuffer(_device.GraphicsCommandPool, out var commandBuffer);

        _device.Api.vkBeginCommandBuffer(commandBuffer, VkCommandBufferUsageFlags.OneTimeSubmit);

        // Copy From Staging Buffer To Image
        VulkanUtilities.TransitionImageLayout(_device, commandBuffer, image,
            VkImageLayout.Undefined, VkImageLayout.TransferDstOptimal,
            VkAccessFlags2.None, VkAccessFlags2.TransferWrite,
            VkPipelineStageFlags2.TopOfPipe, VkPipelineStageFlags2.Transfer);


        var copyRegion = new VkBufferImageCopy2
        {
            bufferOffset = 0,
            bufferRowLength = 0,
            bufferImageHeight = 0,
            imageSubresource = new VkImageSubresourceLayers
            {
                aspectMask = VkImageAspectFlags.Color,
                mipLevel = 0,
                baseArrayLayer = 0,
                layerCount = 1
            },
            imageOffset = new VkOffset3D(0, 0, 0),
            imageExtent = new VkExtent3D(width, height, 1)
        };

        var copyInfo = new VkCopyBufferToImageInfo2
        {
            srcBuffer = stagingBuffer,
            dstImage = image,
            dstImageLayout = VkImageLayout.TransferDstOptimal,
            regionCount = 1,
            pRegions = &copyRegion
        };

        _device.Api.vkCmdCopyBufferToImage2(commandBuffer, &copyInfo);

        VulkanUtilities.TransitionImageLayout(_device, commandBuffer, image,
            VkImageLayout.TransferDstOptimal, VkImageLayout.ShaderReadOnlyOptimal,
            VkAccessFlags2.TransferWrite, VkAccessFlags2.ShaderRead,
            VkPipelineStageFlags2.Transfer, VkPipelineStageFlags2.FragmentShader);

        _device.Api.vkEndCommandBuffer(commandBuffer);

        _device.Api.vkCreateFence(VkFenceCreateFlags.Signaled, out var fence);
        _device.Api.vkResetFences(fence);

        var submitInfo = new VkSubmitInfo
        {
            commandBufferCount = 1u,
            pCommandBuffers = &commandBuffer
        };

        _device.Api.vkQueueSubmit(_device.GraphicsQueue, 1, &submitInfo, fence);
        _device.Api.vkWaitForFences(fence, true, ulong.MaxValue);

        // cleanup
        _device.Api.vkDestroyFence(fence);
        _device.Api.vkFreeCommandBuffers(_device.GraphicsCommandPool, commandBuffer);
        Vma.vmaDestroyBuffer(_device.Allocator, stagingBuffer, stagingAllocation);

        return (image, imageAllocation, new Size2((int)width, (int)height));
    }

    private static void UpdateDescriptorSet(VulkanDevice device, VkDescriptorSet descriptorSet, uint index, VkImageView imageView)
    {
        var info = new VkDescriptorImageInfo
        {
            imageLayout = VkImageLayout.ShaderReadOnlyOptimal,
            imageView = imageView
        };

        var write = new VkWriteDescriptorSet
        {
            dstSet = descriptorSet,
            dstBinding = 0,
            dstArrayElement = index, // offset
            descriptorCount = 1,
            descriptorType = VkDescriptorType.SampledImage,
            pImageInfo = &info
        };

        device.Api.vkUpdateDescriptorSets(1, &write, 0, null);
    }
}