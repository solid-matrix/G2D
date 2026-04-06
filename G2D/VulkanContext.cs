using System.Numerics;
using System.Text;
using Vortice.Vulkan;

namespace G2D;

public sealed unsafe class VulkanContext : IDisposable
{
    internal const uint MaxFrameCountInFlight = 2;

    internal static readonly VkVersion VulkanVersion = VkVersion.Version_1_3;

    internal readonly Window _window;

    internal readonly VulkanInstance _instance;

    internal readonly VkSurfaceKHR _surface;

    internal readonly VulkanDevice _device;

    internal readonly VulkanSwapchain _swapchain;

    internal readonly VkDescriptorPool _descriptorPool;

    internal readonly VulkanBufferSpanPool _uniformBufferSpanPool;

    internal readonly GraphicsPipeline _commonGraphicsPipeline;


    private readonly uint _frameCountInFlight;

    private uint _currentFrame;


    internal readonly VkCommandBuffer[] _commandBuffers;

    internal readonly VulkanBufferSpan[] _uniformBuffers;

    internal readonly VkDescriptorSet[] _descriptorSets;

    internal readonly VulkanBufferSpanPool[] _vertexBufferPools;

    internal readonly VulkanBufferSpanPool[] _instanceBufferPools;

    internal readonly VulkanBufferSpanPool[] _indexBufferPools;

    internal readonly VkFence[] _submitFences;

    internal readonly VkSemaphore[] _acquireSemaphores;

    internal readonly VkSemaphore[] _releaseSemaphores;


    internal VulkanContext(Window window, string appName, Version appVersion, string engineName, Version engineVersion, bool debugEnabled = false)
    {
        _window = window;

        // Create Instance
        var vkAppVersion = new VkVersion((uint)appVersion.Major, (uint)appVersion.Minor, (uint)appVersion.Build);
        var vkEngineVersion = new VkVersion((uint)engineVersion.Major, (uint)engineVersion.Minor, (uint)engineVersion.Build);
        _instance = new VulkanInstance(
            Encoding.UTF8.GetBytes(appName), vkAppVersion,
            Encoding.UTF8.GetBytes(engineName), vkEngineVersion,
            VulkanVersion,
            [],
            [..Window.GetVulkanInstanceExtensions().Select(s => Encoding.UTF8.GetBytes(s))],
            debugEnabled);

        // Create Surface
        _surface = new VkSurfaceKHR((ulong)window.CreateSurface(_instance.Instance));

        // Select Physical Device 
        var physicalDevices = _instance.EnumeratePhysicalDevices();
        var physicalDevice = SelectPhysicalDevice(_instance, physicalDevices, _surface);
        if (physicalDevice == VkPhysicalDevice.Null) throw new Exception("failed to find a suitable physical device!");

        // Create Device
        _device = new VulkanDevice(_instance, physicalDevice, _surface, [Vulkan.VK_KHR_SWAPCHAIN_EXTENSION_NAME]);

        // Create Swapchain
        _swapchain = new VulkanSwapchain(_device, _surface, _window);
        if (!_swapchain.IsValid)
            throw new Exception("failed to create swapchain");
        _frameCountInFlight = Math.Min(_swapchain.ImageCount, MaxFrameCountInFlight);

        // Create DescriptorPool
        var descriptorPoolSize = new VkDescriptorPoolSize
        {
            type = VkDescriptorType.UniformBuffer,
            descriptorCount = _frameCountInFlight
        };
        var descriptorPoolInfo = new VkDescriptorPoolCreateInfo
        {
            poolSizeCount = 1,
            pPoolSizes = &descriptorPoolSize,
            maxSets = _frameCountInFlight
        };
        _device.Api.vkCreateDescriptorPool(&descriptorPoolInfo, out _descriptorPool)
            .CheckResult("failed to create descriptor pool");

        // Create Uniform Buffer Span Pool
        _uniformBufferSpanPool = new VulkanBufferSpanPool(_device, VkBufferUsageFlags.UniformBuffer, VmaMemoryUsage.CpuToGpu);

        // Create Common Graphics Pipeline
        _commonGraphicsPipeline = GraphicsPipeline.Create(
            _device,
            _swapchain.Format,
            Game.InternalResource.GetBytes("Assets/Shaders/default.vert.spv"),
            Game.InternalResource.GetBytes("Assets/Shaders/default.frag.spv")
        );

        // Create UniformBuffers
        _uniformBuffers = new VulkanBufferSpan[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++)
            _uniformBuffers[i] = _uniformBufferSpanPool.Allocate((ulong)sizeof(Uniform));

        // Create VertexBuffer List
        _vertexBufferPools = new VulkanBufferSpanPool[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++) _vertexBufferPools[i] = new VulkanBufferSpanPool(_device, VkBufferUsageFlags.VertexBuffer, VmaMemoryUsage.CpuToGpu);

        // Create InstanceBuffer List
        _instanceBufferPools = new VulkanBufferSpanPool[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++) _instanceBufferPools[i] = new VulkanBufferSpanPool(_device, VkBufferUsageFlags.VertexBuffer, VmaMemoryUsage.CpuToGpu);

        // Create IndexBuffer List
        _indexBufferPools = new VulkanBufferSpanPool[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++) _indexBufferPools[i] = new VulkanBufferSpanPool(_device, VkBufferUsageFlags.IndexBuffer, VmaMemoryUsage.CpuToGpu);


        // Set DescriptorSets
        _descriptorSets = _commonGraphicsPipeline.CreateDescriptorSets(_descriptorPool, _uniformBuffers);

        // Create CommandBuffers 
        _commandBuffers = new VkCommandBuffer[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++)
            _device.Api.vkAllocateCommandBuffer(_device.GraphicsCommandPool, out _commandBuffers[i])
                .CheckResult("failed to allocate command buffer");

        // Create SyncObjects
        _submitFences = new VkFence[_frameCountInFlight];
        _acquireSemaphores = new VkSemaphore[_frameCountInFlight];
        _releaseSemaphores = new VkSemaphore[_frameCountInFlight];
        for (var i = 0; i < _frameCountInFlight; i++)
        {
            _device.Api.vkCreateFence(VkFenceCreateFlags.Signaled, out _submitFences[i])
                .CheckResult("failed to create fence");
            _device.Api.vkCreateSemaphore(out _acquireSemaphores[i])
                .CheckResult("failed to create semaphore");
            _device.Api.vkCreateSemaphore(out _releaseSemaphores[i])
                .CheckResult("failed to create semaphore");
        }
    }

    internal VkDeviceApi Api => _device.Api;

    void IDisposable.Dispose()
    {
        _device.WaitIdle();

        _swapchain.Dispose();

        _commonGraphicsPipeline.Dispose();

        for (var i = 0; i < _frameCountInFlight; i++)
        {
            _vertexBufferPools[i].Dispose();
            _instanceBufferPools[i].Dispose();
            _indexBufferPools[i].Dispose();
        }

        _uniformBufferSpanPool.Dispose();
        _device.Api.vkDestroyDescriptorPool(_descriptorPool);

        for (var i = 0; i < _frameCountInFlight; i++)
        {
            _device.Api.vkDestroyFence(_submitFences[i]);
            _device.Api.vkDestroySemaphore(_acquireSemaphores[i]);
            _device.Api.vkDestroySemaphore(_releaseSemaphores[i]);
        }


        _device.Dispose();
        Window.DestroySurface(_instance.Instance, _surface);
        _instance.Dispose();
    }

    internal void StartDrawSession(Graphics g)
    {
        g.Reset();

        if (!_swapchain.IsValid)
        {
            if (_window.GetClientExtent().Area == 0) return;
            if (!_swapchain.Recreate()) return;
        }


        Api.vkWaitForFences(_submitFences[_currentFrame], VkBool32.True, ulong.MaxValue);

        var result = _device.Api.vkAcquireNextImageKHR(_swapchain.Swapchain, ulong.MaxValue, _acquireSemaphores[_currentFrame], VkFence.Null, out var imageIndex);
        if (result is VkResult.ErrorOutOfDateKHR)
        {
            _swapchain.Recreate();
            return;
        }

        if (result != VkResult.Success && result != VkResult.SuboptimalKHR) throw new VkException("failed to acquire swap chain image!");

        Api.vkResetFences(_submitFences[_currentFrame]);
        Api.vkResetCommandBuffer(_commandBuffers[_currentFrame], VkCommandBufferResetFlags.None).CheckResult();

        _vertexBufferPools[_currentFrame].Reset();
        _instanceBufferPools[_currentFrame].Reset();
        _indexBufferPools[_currentFrame].Reset();


        VkCommandBufferBeginInfo beginInfo = new()
        {
            flags = VkCommandBufferUsageFlags.OneTimeSubmit
        };
        Api.vkBeginCommandBuffer(_commandBuffers[_currentFrame], &beginInfo)
            .CheckResult("failed to create command buffer");

        TransitionImageLayout(_commandBuffers[_currentFrame], _swapchain.Images[imageIndex],
            Vulkan.VK_IMAGE_LAYOUT_UNDEFINED, Vulkan.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL,
            0, Vulkan.VK_ACCESS_2_COLOR_ATTACHMENT_WRITE_BIT,
            Vulkan.VK_PIPELINE_STAGE_2_TOP_OF_PIPE_BIT, Vulkan.VK_PIPELINE_STAGE_2_COLOR_ATTACHMENT_OUTPUT_BIT);

        g._commandBuffer = _commandBuffers[_currentFrame];
        g._uniformBuffer = _uniformBuffers[_currentFrame];
        g._descriptorSet = _descriptorSets[_currentFrame];
        g._vertexBufferPool = _vertexBufferPools[_currentFrame];
        g._instanceBufferPool = _instanceBufferPools[_currentFrame];
        g._indexBufferPool = _indexBufferPools[_currentFrame];
        g._submitFence = _submitFences[_currentFrame];
        g._acquireSemaphore = _acquireSemaphores[_currentFrame];
        g._releaseSemaphore = _releaseSemaphores[_currentFrame];
        g._imageIndex = imageIndex;
        g._image = _swapchain.Images[imageIndex];
        g._imageView = _swapchain.ImageViews[imageIndex];
        g._requireDraw = true;
        g._extent = _swapchain.Extent;
        g.Uniform.Resolution = new Vector2(g._extent.width, g._extent.height);
    }

    internal void EndDrawSession(Graphics g)
    {
        g.EnsureEndRender();

        TransitionImageLayout(g._commandBuffer, g._image,
            Vulkan.VK_IMAGE_LAYOUT_COLOR_ATTACHMENT_OPTIMAL, Vulkan.VK_IMAGE_LAYOUT_PRESENT_SRC_KHR,
            Vulkan.VK_ACCESS_2_COLOR_ATTACHMENT_WRITE_BIT, 0,
            Vulkan.VK_PIPELINE_STAGE_2_COLOR_ATTACHMENT_OUTPUT_BIT, Vulkan.VK_PIPELINE_STAGE_2_BOTTOM_OF_PIPE_BIT);

        Api.vkEndCommandBuffer(g._commandBuffer).CheckResult();

        var waitStage = VkPipelineStageFlags.ColorAttachmentOutput;
        var waitSemaphore = g._acquireSemaphore;
        var signalSemaphore = g._releaseSemaphore;
        var commandBuffer = g._commandBuffer;

        VkSubmitInfo submitInfo = new()
        {
            commandBufferCount = 1u,
            pCommandBuffers = &commandBuffer,
            waitSemaphoreCount = 1u,
            pWaitSemaphores = &waitSemaphore,
            pWaitDstStageMask = &waitStage,
            signalSemaphoreCount = 1u,
            pSignalSemaphores = &signalSemaphore
        };
        Api.vkQueueSubmit(_device.GraphicsQueue, submitInfo, g._submitFence);

        var result = _device.Api.vkQueuePresentKHR(_device.PresentQueue, g._releaseSemaphore, _swapchain.Swapchain, g._imageIndex);

        if (result is VkResult.SuboptimalKHR or VkResult.ErrorOutOfDateKHR)
            _swapchain.Recreate();
        else if (result != VkResult.Success)
            throw new VkException("failed to present swap chain image");

        g._requireDraw = false;
        _currentFrame = (_currentFrame + 1) % _frameCountInFlight;
    }

    internal void TransitionImageLayout(VkCommandBuffer commandBuffer, VkImage image,
        VkImageLayout oldLayout, VkImageLayout newLayout,
        VkAccessFlags2 srcAccessMask, VkAccessFlags2 dstAccessMask,
        VkPipelineStageFlags2 srcStage, VkPipelineStageFlags2 dstStage)
    {
        // Initialize the VkImageMemoryBarrier2 structure
        var imageBarrier = new VkImageMemoryBarrier2
        {
            srcStageMask = srcStage, // Source pipeline stage mask
            srcAccessMask = srcAccessMask, // Source access mask
            dstStageMask = dstStage, // Destination pipeline stage mask
            dstAccessMask = dstAccessMask, // Destination access mask
            oldLayout = oldLayout, // Current layout of the image
            newLayout = newLayout, // Target layout of the image
            srcQueueFamilyIndex = Vulkan.VK_QUEUE_FAMILY_IGNORED,
            dstQueueFamilyIndex = Vulkan.VK_QUEUE_FAMILY_IGNORED,
            image = image,
            subresourceRange = new VkImageSubresourceRange(Vulkan.VK_IMAGE_ASPECT_COLOR_BIT, 0, 1, 0, 1)
        };

        // Initialize the VkDependencyInfo structure
        VkDependencyInfo dependencyInfo = new()
        {
            dependencyFlags = 0, // No special dependency flags
            imageMemoryBarrierCount = 1, // Number of image memory barriers
            pImageMemoryBarriers = &imageBarrier // Pointer to the image memory barrier(s)
        };

        // Record the pipeline barrier into the command buffer
        _device.Api.vkCmdPipelineBarrier2(commandBuffer, &dependencyInfo);
    }

    private static VkPhysicalDevice SelectPhysicalDevice(VulkanInstance instance, VkPhysicalDevice[] physicalDevices, VkSurfaceKHR surface)
    {
        var max = 0;
        var selected = VkPhysicalDevice.Null;

        foreach (var physicalDevice in physicalDevices)
        {
            var rank = RankPhysicalDevice(instance, physicalDevice, surface);
            if (rank > max)
            {
                max = rank;
                selected = physicalDevice;
            }
        }

        return selected;
    }

    private static int RankPhysicalDevice(VulkanInstance instance, VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        var (graphicsFamily, presentFamily, computeFamily) = instance.QueryGraphicsPresentQueueFamilies(device, surface);

        if (graphicsFamily == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;
        if (presentFamily == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;
        if (computeFamily == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;

        var formats = instance.GetPhysicalDeviceSurfaceFormats(device, surface);
        if (formats.Length == 0) return -1;

        var presentModes = instance.GetPhysicalDeviceSurfacePresentModes(device, surface);
        if (presentModes.Length == 0) return -1;

        VkPhysicalDeviceVulkan13Features queryVulkan13Features = new();
        VkPhysicalDeviceFeatures2 queryDeviceFeatures2 = new() { pNext = &queryVulkan13Features };

        instance.Api.vkGetPhysicalDeviceFeatures2(device, &queryDeviceFeatures2);

        if (!queryVulkan13Features.dynamicRendering) return -1;
        if (!queryVulkan13Features.synchronization2) return -1;

        var rank = 0;
        instance.Api.vkGetPhysicalDeviceProperties(device, out var properties);

        if (properties.deviceType == VkPhysicalDeviceType.DiscreteGpu) rank += 1000;

        return rank;
    }
}