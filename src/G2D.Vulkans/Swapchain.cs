using G2D.Mathematics;
using Vortice.Vulkan;

namespace G2D;

public sealed unsafe class Swapchain : IDisposable
{
    private const uint MaxFrameCountInFlight = 3;

    private readonly Device _device;

    private readonly VkSurfaceKHR _surface;

    private VkSwapchainKHR _swapchain;

    private readonly Func<Size2I> _surfaceSizeProvider;


    private VkExtent2D _extent;

    private VkFormat _format;

    private VkPresentModeKHR _presentMode;


    private uint _imageCount;

    private VkImage[] _images = null!;

    private VkImageView[] _imageViews = null!;


    private bool _isValid;

    private uint _frameCountInFlight;

    private uint _currentFrame;

    private readonly CommandBuffer[] _commandBuffers;

    private readonly Fence[] _submitFences;

    private readonly Semaphore[] _acquireSemaphores;

    private readonly Semaphore[] _releaseSemaphores;

    public Swapchain(Device device, VkSurfaceKHR surface, Func<Size2I> surfaceSizeProvider)
    {
        _device = device;
        _surface = surface;
        _surfaceSizeProvider = surfaceSizeProvider;

        _commandBuffers = new CommandBuffer[MaxFrameCountInFlight];
        _submitFences = new Fence[MaxFrameCountInFlight];
        _acquireSemaphores = new Semaphore[MaxFrameCountInFlight];
        _releaseSemaphores = new Semaphore[MaxFrameCountInFlight];

        for (var i = 0; i < MaxFrameCountInFlight; i++)
        {
            _commandBuffers[i] = _device.GetCommandPool(QueueType.Graphics).CreateCommandBuffer();
            _submitFences[i] = _device.CreateFence(VkFenceCreateFlags.Signaled);
            _acquireSemaphores[i] = _device.CreateSemaphore();
            _releaseSemaphores[i] = _device.CreateSemaphore();
        }

        _isValid = false;

        Create();

        if (!_isValid) throw new Exception("Vulkan: failed to create swapchain");
    }

    internal VkImage[] Images => _images;

    internal VkImageView[] ImageViews => _imageViews;

    internal CommandBuffer[] CommandBuffers => _commandBuffers;

    public VkFormat Format => _format;

    public VkExtent2D Extent => _extent;

    public Size2I SurfaceSize => _surfaceSizeProvider();

    public uint FrameCountInFlight => _frameCountInFlight;

    public void Dispose()
    {
        for (var i = 0; i < MaxFrameCountInFlight; i++)
        {
            _submitFences[i].Dispose();
            _acquireSemaphores[i].Dispose();
            _releaseSemaphores[i].Dispose();
        }

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

        var capabilities = _device.Instance.GetPhysicalDeviceSurfaceCapabilities(_device.Gpu, _surface);
        var formats = _device.Instance.GetPhysicalDeviceSurfaceFormats(_device.Gpu, _surface);
        var presentModes = _device.Instance.GetPhysicalDeviceSurfacePresentModes(_device.Gpu, _surface);

        var surfaceFormat = VulkanUtilities.ChooseSurfaceFormat(formats);

        _format = surfaceFormat.format;

        _presentMode = VulkanUtilities.ChoosePresentMode(presentModes);

        _extent = VulkanUtilities.ChooseExtent(capabilities, new VkExtent2D(size.Width, size.Height));
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
                surfaceFormat.format,
                VkComponentMapping.Rgba,
                new VkImageSubresourceRange(VkImageAspectFlags.Color, 0, 1, 0, 1)
            );

            _device.Api.vkCreateImageView(&viewCreateInfo, out _imageViews[i])
                .CheckResult("failed vulkan create image view");
        }

        _frameCountInFlight = Math.Min(_imageCount, MaxFrameCountInFlight);
        _currentFrame = 0;
        _isValid = true;
    }

    private void Destroy()
    {
        foreach (var imageView in _imageViews)
        {
            _device.Api.vkDestroyImageView(imageView);
        }

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

    public SwapchainFrame? Acquire()
    {
        if (SurfaceSize.Area == 0) return null;
        if (!_isValid)
        {
            Recreate();
            return null;
        }

        if (!_submitFences[_currentFrame].IsSignaled())
            return null;

        var result = _device.Api.vkAcquireNextImageKHR(_swapchain, 0, _acquireSemaphores[_currentFrame], VkFence.Null, out var imageIndex);

        switch (result)
        {
            case VkResult.Timeout:
                return null;
            case VkResult.ErrorOutOfDateKHR:
                Recreate();
                return null;
            case VkResult.Success:
            case VkResult.SuboptimalKHR:
                break;
            default:
                throw new VkException("failed to acquire swap chain image!");
        }

        _commandBuffers[_currentFrame].Reset();

        return new SwapchainFrame(this, imageIndex, _currentFrame);
    }

    public void SubmitPresent(SwapchainFrame frame)
    {
        if (frame.Index != _currentFrame)
            throw new Exception("Vulkan: frame index skipped");

        VkCommandBuffer commandBuffer = _commandBuffers[_currentFrame];
        var swapchain = _swapchain;
        var waitStage = VkPipelineStageFlags.ColorAttachmentOutput;
        VkSemaphore acquireSemaphore = _acquireSemaphores[_currentFrame];
        VkSemaphore releaseSemaphore = _releaseSemaphores[_currentFrame];
        VkFence submitFence = _submitFences[_currentFrame];
        var imageIndex = frame.ImageIndex;

        _device.Api.vkResetFences(submitFence);

        var submitInfo = new VkSubmitInfo
        {
            commandBufferCount = 1u,
            pCommandBuffers = &commandBuffer,
            pWaitDstStageMask = &waitStage,

            waitSemaphoreCount = 1u,
            pWaitSemaphores = &acquireSemaphore,
            signalSemaphoreCount = 1u,
            pSignalSemaphores = &releaseSemaphore
        };
        _device.Api.vkQueueSubmit(_device.GetQueue(QueueType.Graphics), 1, &submitInfo, _submitFences[_currentFrame])
            .CheckResult("failed to queue submit");

        // present
        var presentInfo = new VkPresentInfoKHR
        {
            swapchainCount = 1u,
            pSwapchains = &swapchain,
            pImageIndices = &imageIndex,

            waitSemaphoreCount = 1u,
            pWaitSemaphores = &releaseSemaphore
        };
        var result = _device.Api.vkQueuePresentKHR(_device.GetQueue(QueueType.Graphics), &presentInfo);

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

        _currentFrame = (_currentFrame + 1) % _frameCountInFlight;
    }
}