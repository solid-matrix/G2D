using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

public sealed unsafe class Swapchain : IDisposable
{
    private const uint MaxFrameCountInFlight = 3;

    private readonly GraphicsDevice _device;

    private readonly VkSurfaceKHR _surface;

    private readonly Func<Size2I> _surfaceSizeProvider;


    private VkSwapchainKHR _swapchain;


    private VkExtent2D _extent;

    private VkFormat _format;

    private VkColorSpaceKHR _colorSpace;

    private VkPresentModeKHR _presentMode;


    private uint _imageCount;

    private VkImage[] _images = null!;

    private VkImageView[] _imageViews = null!;


    private bool _isValid;

    public Swapchain(GraphicsDevice device, VkSurfaceKHR surface, Func<Size2I> surfaceSizeProvider)
    {
        _device = device;
        _surface = surface;
        _surfaceSizeProvider = surfaceSizeProvider;

        _isValid = false;

        var extent = _surfaceSizeProvider();
        if (extent.Area == 0) return;

        var capabilities = _device.Instance.GetGpuSurfaceCapabilities(_device.Gpu, _surface);
        var formats = _device.Instance.GetGpuSurfaceFormats(_device.Gpu, _surface);
        var presentModes = _device.Instance.GetGpuSurfacePresentModes(_device.Gpu, _surface);
        var surfaceFormat = VulkanUtilities.ChooseSurfaceFormat(formats);

        _format = surfaceFormat.format;
        _colorSpace = surfaceFormat.colorSpace;
        _presentMode = VulkanUtilities.ChoosePresentMode(presentModes);
        _extent = VulkanUtilities.ChooseExtent(capabilities, new VkExtent2D(extent.Width, extent.Height));
        if (_extent.width == 0 || _extent.height == 0) return;
        _imageCount = capabilities.minImageCount + 1;
        if (capabilities.maxImageCount > 0 && _imageCount > capabilities.maxImageCount) _imageCount = capabilities.maxImageCount;

        VkSwapchainCreateInfoKHR createInfo = new()
        {
            surface = _surface,
            minImageCount = _imageCount,
            imageFormat = surfaceFormat.format,
            imageColorSpace = surfaceFormat.colorSpace,
            imageExtent = _extent,
            imageArrayLayers = 1,
            imageUsage = VkImageUsageFlags.ColorAttachment,
            imageSharingMode = VkSharingMode.Exclusive,
            preTransform = capabilities.currentTransform,
            compositeAlpha = VkCompositeAlphaFlagsKHR.Opaque,
            presentMode = _presentMode,
            clipped = true,
            oldSwapchain = VkSwapchainKHR.Null
        };

        _device.Api.vkCreateSwapchainKHR(&createInfo, out _swapchain)
            .CheckResult("failed to create swapchain");

        _device.Api.vkGetSwapchainImagesKHR(_swapchain, out _imageCount)
            .CheckResult("failed vulkan get swapchain images");

        _images = new VkImage[_imageCount];
        _device.Api.vkGetSwapchainImagesKHR(_swapchain, _images)
            .CheckResult("failed to get swapchain images");

        _imageViews = new VkImageView[_imageCount];
        for (var i = 0; i < _imageCount; i++)
        {
            var viewCreateInfo = new VkImageViewCreateInfo(
                _images[i],
                VkImageViewType.Image2D,
                _format,
                VkComponentMapping.Rgba,
                new VkImageSubresourceRange(VkImageAspectFlags.Color, 0, 1, 0, 1)
            );

            _device.Api.vkCreateImageView(&viewCreateInfo, out _imageViews[i])
                .CheckResult("failed vulkan create image view");
        }

        _isValid = true;
    }

    public VkImage[] Images => _images;

    public VkImageView[] ImageViews => _imageViews;


    public VkFormat Format => _format;

    public VkExtent2D Extent => _extent;

    public Size2I SurfaceSize => _surfaceSizeProvider();


    public void Dispose()
    {
        if (_isValid)
        {
            _device.WaitIdle();

            foreach (var imageView in _imageViews)
            {
                _device.Api.vkDestroyImageView(imageView);
            }

            if (_swapchain != VkSwapchainKHR.Null) _device.Api.vkDestroySwapchainKHR(_swapchain);
        }
    }

    public void Recreate()
    {
        Dispose();

        _isValid = false;
        var extent = _surfaceSizeProvider();
        if (extent.Area == 0) return;

        var capabilities = _device.Instance.GetGpuSurfaceCapabilities(_device.Gpu, _surface);
        var formats = _device.Instance.GetGpuSurfaceFormats(_device.Gpu, _surface);
        var presentModes = _device.Instance.GetGpuSurfacePresentModes(_device.Gpu, _surface);
        var surfaceFormat = VulkanUtilities.ChooseSurfaceFormat(formats);

        _format = surfaceFormat.format;
        _colorSpace = surfaceFormat.colorSpace;
        _presentMode = VulkanUtilities.ChoosePresentMode(presentModes);
        _extent = VulkanUtilities.ChooseExtent(capabilities, new VkExtent2D(extent.Width, extent.Height));
        if (_extent.width == 0 || _extent.height == 0) return;
        _imageCount = capabilities.minImageCount + 1;
        if (capabilities.maxImageCount > 0 && _imageCount > capabilities.maxImageCount) _imageCount = capabilities.maxImageCount;


        VkSwapchainCreateInfoKHR createInfo = new()
        {
            surface = _surface,
            minImageCount = _imageCount,
            imageFormat = surfaceFormat.format,
            imageColorSpace = surfaceFormat.colorSpace,
            imageExtent = _extent,
            imageArrayLayers = 1,
            imageUsage = VkImageUsageFlags.ColorAttachment,
            imageSharingMode = VkSharingMode.Exclusive,
            preTransform = capabilities.currentTransform,
            compositeAlpha = VkCompositeAlphaFlagsKHR.Opaque,
            presentMode = _presentMode,
            clipped = true,
            oldSwapchain = VkSwapchainKHR.Null
        };

        _device.Api.vkCreateSwapchainKHR(&createInfo, out _swapchain)
            .CheckResult("failed to create swapchain");

        _device.Api.vkGetSwapchainImagesKHR(_swapchain, out _imageCount)
            .CheckResult("failed vulkan get swapchain images");

        _images = new VkImage[_imageCount];
        _device.Api.vkGetSwapchainImagesKHR(_swapchain, _images)
            .CheckResult("failed to get swapchain images");

        _imageViews = new VkImageView[_imageCount];
        for (var i = 0; i < _imageCount; i++)
        {
            var viewCreateInfo = new VkImageViewCreateInfo(
                _images[i],
                VkImageViewType.Image2D,
                _format,
                VkComponentMapping.Rgba,
                new VkImageSubresourceRange(VkImageAspectFlags.Color, 0, 1, 0, 1)
            );

            _device.Api.vkCreateImageView(&viewCreateInfo, out _imageViews[i])
                .CheckResult("failed vulkan create image view");
        }

        _isValid = true;
    }

    public bool TryAcquire(out uint index, VkSemaphore semaphore)
    {
        index = 0;

        if (SurfaceSize.Area == 0) return false;
        if (!_isValid)
        {
            Recreate();
            return false;
        }

        var result = _device.Api.vkAcquireNextImageKHR(_swapchain, 0, semaphore, VkFence.Null, out index);

        switch (result)
        {
            case VkResult.Timeout:
                return false;
            case VkResult.ErrorOutOfDateKHR:
                Recreate();
                return false;
            case VkResult.Success:
            case VkResult.SuboptimalKHR:
                break;
            default:
                throw new VkException("failed to acquire swap chain image!");
        }

        return true;
    }

    public void Present(uint index, VkSemaphore semaphore)
    {
        var swapchain = _swapchain;

        var presentInfo = new VkPresentInfoKHR
        {
            swapchainCount = 1u,
            pSwapchains = &swapchain,
            pImageIndices = &index,
            pResults = null,

            waitSemaphoreCount = 1u,
            pWaitSemaphores = &semaphore
        };

        var result = _device.Api.vkQueuePresentKHR(_device.GraphicsQueue, &presentInfo);

        switch (result)
        {
            case VkResult.SuboptimalKHR:
            case VkResult.ErrorOutOfDateKHR:
                Recreate();
                break;
            case VkResult.Success:
                break;
            default:
                throw new VkException("failed to present swap chain image");
        }
    }
}