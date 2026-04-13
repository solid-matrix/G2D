using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class VulkanSwapchain : IDisposable
{
    private readonly VulkanDevice _device;

    private readonly VkSurfaceKHR _surface;

    private VkExtent2D _extent;

    private VkFormat _format;

    private uint _imageCount;

    private VkImage[] _images;

    private VkImageView[] _imageViews;

    private bool _isValid;

    private VkPresentModeKHR _presentMode;

    private VkSwapchainKHR _swapchain;

    private readonly Func<Size2I> _surfaceSizeProvider;


    public VulkanSwapchain(VulkanDevice device, VkSurfaceKHR surface, Func<Size2I> surfaceSizeProvider)
    {
        _device = device;
        _surface = surface;
        _images = [];
        _imageViews = [];
        _surfaceSizeProvider = surfaceSizeProvider;
        Create();
    }

    public VkSwapchainKHR Swapchain => _swapchain;

    public uint ImageCount => _imageCount;

    public VkFormat Format => _format;

    public VkExtent2D Extent => _extent;

    public VkImage[] Images => _images;

    public VkImageView[] ImageViews => _imageViews;

    public bool IsValid => _isValid;

    public Size2I SurfaceSize => _surfaceSizeProvider();

    public void Dispose()
    {
        if (_isValid)
            Destroy();
    }

    private void Create()
    {
        _imageCount = 0;
        _format = VkFormat.Undefined;
        _presentMode = VkPresentModeKHR.Immediate;
        _extent = VkExtent2D.Zero;
        _isValid = false;
        _images = [];
        _imageViews = [];

        var size = _surfaceSizeProvider();

        if (size.Area == 0) return;

        var capabilities = _device.Instance.GetPhysicalDeviceSurfaceCapabilities(_device.PhysicalDevice, _surface);
        var formats = _device.Instance.GetPhysicalDeviceSurfaceFormats(_device.PhysicalDevice, _surface);
        var presentModes = _device.Instance.GetPhysicalDeviceSurfacePresentModes(_device.PhysicalDevice, _surface);

        var surfaceFormat = ChooseSurfaceFormat(formats);

        _format = surfaceFormat.format;

        _presentMode = ChoosePresentMode(presentModes);

        _extent = ChooseExtent(capabilities, new VkExtent2D(size.Width, size.Height));
        if (_extent.width == 0 || _extent.height == 0) return;


        _imageCount = capabilities.minImageCount;
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
                surfaceFormat.format,
                VkComponentMapping.Rgba,
                new VkImageSubresourceRange(VkImageAspectFlags.Color, 0, 1, 0, 1)
            );

            _device.Api.vkCreateImageView(&viewCreateInfo, out _imageViews[i])
                .CheckResult("failed vulkan create image view");
        }

        _isValid = true;
    }

    private void Destroy()
    {
        foreach (var imageView in _imageViews) _device.Api.vkDestroyImageView(imageView);

        if (_swapchain != VkSwapchainKHR.Null) _device.Api.vkDestroySwapchainKHR(_swapchain);

        _isValid = false;
    }

    public bool Recreate()
    {
        _device.WaitIdle();

        if (_isValid) Destroy();

        Create();

        return _isValid;
    }

    private static VkExtent2D ChooseExtent(VkSurfaceCapabilitiesKHR capabilities, VkExtent2D actualExtent)
    {
        if (capabilities.currentExtent.width > 0) return capabilities.currentExtent;

        actualExtent = new VkExtent2D(
            Math.Clamp(actualExtent.width, capabilities.minImageExtent.width, capabilities.maxImageExtent.width),
            Math.Clamp(actualExtent.height, capabilities.minImageExtent.height, capabilities.maxImageExtent.height)
        );

        return actualExtent;
    }

    private static VkSurfaceFormatKHR ChooseSurfaceFormat(ReadOnlySpan<VkSurfaceFormatKHR> availableFormats)
    {
        // If the surface format list only includes one entry with VK_FORMAT_UNDEFINED,
        // there is no preferred format, so we assume VK_FORMAT_B8G8R8A8_SRGB
        if (availableFormats.Length == 1 && availableFormats[0].format == VkFormat.Undefined)
            return new VkSurfaceFormatKHR(VkFormat.B8G8R8A8Srgb, availableFormats[0].colorSpace);

        foreach (var availableFormat in availableFormats)
            if (availableFormat is { format: VkFormat.B8G8R8A8Srgb, colorSpace: VkColorSpaceKHR.SrgbNonLinear })
                return availableFormat;

        return availableFormats[0];
    }

    private static VkPresentModeKHR ChoosePresentMode(ReadOnlySpan<VkPresentModeKHR> availablePresentModes)
    {
        foreach (var availablePresentMode in availablePresentModes)
            if (availablePresentMode == VkPresentModeKHR.Mailbox)
                return availablePresentMode;

        return VkPresentModeKHR.Fifo;
    }
}