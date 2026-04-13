using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class VulkanDevice : IDisposable
{
    private readonly VulkanInstance _instance;

    private readonly VkPhysicalDevice _physicalDevice;

    private readonly VkDevice _device;

    private readonly VkDeviceApi _api;

    private readonly VmaAllocator _vmaAllocator;


    private readonly uint _graphicsFamily;

    private readonly uint _presentFamily;

    private readonly uint _computeFamily;


    private readonly VkQueue _graphicsQueue;

    private readonly VkQueue _presentQueue;

    private readonly VkQueue _computeQueue;


    private readonly VkCommandPool _graphicsCommandPool;

    private readonly VkCommandPool _computeCommandPool;


    public VulkanDevice(VulkanInstance instance, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkUtf8String[] requiredExtensions)
    {
        _instance = instance;
        _physicalDevice = physicalDevice;

        (_graphicsFamily, _presentFamily, _computeFamily) = instance.QueryGraphicsPresentQueueFamilies(physicalDevice, surface);

        HashSet<VkUtf8String> availableExtensionSet = [.. instance.EnumerateDeviceExtensionNames(_physicalDevice)];
        HashSet<VkUtf8String> requiredExtensionSet = [..requiredExtensions];

        if (!requiredExtensionSet.All(availableExtensionSet.Contains))
            throw new VkException("vulkan required device extension not supported");

        if (_instance.DebugEnabled)
        {
            Console.WriteLine("vulkan device extension enabled:");
            foreach (var extension in requiredExtensionSet) Console.WriteLine($" - {extension}");
        }


        HashSet<uint> uniqueQueueFamilies = [_graphicsFamily, _presentFamily, _computeFamily];
        var priority = 1.0f;
        var queueCount = 0u;
        var queueCreateInfos = stackalloc VkDeviceQueueCreateInfo[3];

        foreach (var queueFamily in uniqueQueueFamilies)
            queueCreateInfos[queueCount++] = new VkDeviceQueueCreateInfo
            {
                queueFamilyIndex = queueFamily,
                queueCount = 1,
                pQueuePriorities = &priority
            };

        VkPhysicalDeviceFeatures2 features2 = new();
        VkPhysicalDeviceVulkan12Features vulkan12Features = new()
        {
            runtimeDescriptorArray = true,
            shaderSampledImageArrayNonUniformIndexing = true,
            descriptorBindingSampledImageUpdateAfterBind = true,
            descriptorBindingPartiallyBound = true
            // descriptorBindingVariableDescriptorCount = true
        };
        VkPhysicalDeviceVulkan13Features vulkan13Features = new()
        {
            synchronization2 = true,
            dynamicRendering = true
        };
        features2.pNext = &vulkan12Features;
        vulkan12Features.pNext = &vulkan13Features;

        using var deviceExtensionNames = new VkStringArray(requiredExtensionSet);

        VkDeviceCreateInfo deviceCreateInfo = new()
        {
            queueCreateInfoCount = queueCount,
            pQueueCreateInfos = queueCreateInfos,
            enabledExtensionCount = deviceExtensionNames.Length,
            ppEnabledExtensionNames = deviceExtensionNames,
            pEnabledFeatures = null,
            pNext = &features2
        };

        instance.Api.vkCreateDevice(_physicalDevice, &deviceCreateInfo, out _device)
            .CheckResult("failed to create vulkan device");

        _api = Vulkan.GetApi(instance.Instance, _device);

        _api.vkGetDeviceQueue(_graphicsFamily, 0, out _graphicsQueue);
        _api.vkGetDeviceQueue(_presentFamily, 0, out _presentQueue);
        _api.vkGetDeviceQueue(_computeFamily, 0, out _computeQueue);

        var vmaAllocatorInfo = new VmaAllocatorCreateInfo
        {
            vulkanApiVersion = _instance.ApiVersion,
            instance = _instance.Instance,
            physicalDevice = _physicalDevice,
            device = _device
        };

        Vma.vmaCreateAllocator(in vmaAllocatorInfo, out _vmaAllocator)
            .CheckResult("failed to create vma allocator");

        _api.vkCreateCommandPool(VkCommandPoolCreateFlags.ResetCommandBuffer | VkCommandPoolCreateFlags.Transient, _graphicsFamily, out _graphicsCommandPool)
            .CheckResult("vulkan failed to create command pool");

        if (_computeFamily != _graphicsFamily)
            _api.vkCreateCommandPool(VkCommandPoolCreateFlags.ResetCommandBuffer | VkCommandPoolCreateFlags.Transient, _computeFamily, out _computeCommandPool)
                .CheckResult("vulkan failed to create command pool");
    }

    public VulkanInstance Instance => _instance;

    public VkPhysicalDevice PhysicalDevice => _physicalDevice;

    public VkDevice Device => _device;

    public VmaAllocator Allocator => _vmaAllocator;

    public VkDeviceApi Api => _api;

    public uint GraphicsFamily => _graphicsFamily;

    public uint PresentFamily => _presentFamily;

    public uint ComputeFamily => _computeFamily;

    public VkQueue GraphicsQueue => _graphicsQueue;

    public VkQueue PresentQueue => _presentQueue;

    public VkQueue ComputeQueue => _computeQueue;

    public VkCommandPool GraphicsCommandPool => _graphicsCommandPool;

    public VkCommandPool ComputeCommandPool => _computeCommandPool;

    public void Dispose()
    {
        _api.vkDestroyCommandPool(_graphicsCommandPool);
        if (_computeFamily != _graphicsFamily)
            _api.vkDestroyCommandPool(_computeCommandPool);

        Vma.vmaDestroyAllocator(_vmaAllocator);

        _api.vkDestroyDevice();
    }

    public void WaitIdle()
    {
        _api.vkDeviceWaitIdle()
            .CheckResult("failed vulkan device wait idle");
    }

    public static implicit operator VkDevice(VulkanDevice device)
    {
        return device._device;
    }
}