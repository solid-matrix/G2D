using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class VulkanDevice : IDisposable
{
    private readonly VkDeviceApi _api;

    private readonly uint _computeFamily;

    private readonly VkQueue _computeQueue;

    private readonly VkDevice _device;

    private readonly uint _graphicsFamily;

    private readonly VkQueue _graphicsQueue;

    private readonly VulkanInstance _instance;

    private readonly VkPhysicalDevice _physicalDevice;

    private readonly uint _presentFamily;

    private readonly VkQueue _presentQueue;

    public VulkanDevice(VulkanInstance instance, VkPhysicalDevice physicalDevice, VkSurfaceKHR surface, VkUtf8String[] requiredExtensions)
    {
        _instance = instance;
        _physicalDevice = physicalDevice;

        (_graphicsFamily, _presentFamily, _computeFamily) = instance.QueryGraphicsPresentQueueFamilies(physicalDevice, surface);

        HashSet<VkUtf8String> availableExtensionSet = [.. instance.EnumerateDeviceExtensionNames(_physicalDevice)];
        HashSet<VkUtf8String> requiredExtensionSet = [..requiredExtensions];

        if (!requiredExtensions.All(availableExtensionSet.Contains))
            throw new VkException("vulkan required device extension not supported");


        HashSet<uint> uniqueQueueFamilies = [_graphicsFamily, _presentFamily, _computeFamily];
        var priority = 1.0f;
        var queueCount = 0u;
        var queueCreateInfos = stackalloc VkDeviceQueueCreateInfo[3];

        // TODO 
        foreach (var queueFamily in uniqueQueueFamilies)
            queueCreateInfos[queueCount++] = new VkDeviceQueueCreateInfo
            {
                queueFamilyIndex = queueFamily,
                queueCount = 1,
                pQueuePriorities = &priority
            };

        VkPhysicalDeviceVulkan13Features deviceFeatures2 = new()
        {
            synchronization2 = true,
            dynamicRendering = true
        };

        VkPhysicalDeviceFeatures2 enableDeviceFeatures2 = new();
        enableDeviceFeatures2.pNext = &deviceFeatures2;

        using var deviceExtensionNames = new VkStringArray(requiredExtensionSet);

        VkDeviceCreateInfo deviceCreateInfo = new()
        {
            pNext = &enableDeviceFeatures2,
            queueCreateInfoCount = queueCount,
            pQueueCreateInfos = queueCreateInfos,
            enabledExtensionCount = deviceExtensionNames.Length,
            ppEnabledExtensionNames = deviceExtensionNames,
            pEnabledFeatures = null
        };

        instance.Api.vkCreateDevice(_physicalDevice, &deviceCreateInfo, out _device)
            .CheckResult("failed to create vulkan device");

        _api = Vulkan.GetApi(instance.Instance, _device);

        _api.vkGetDeviceQueue(_graphicsFamily, 0, out _graphicsQueue);
        _api.vkGetDeviceQueue(_presentFamily, 0, out _presentQueue);
        _api.vkGetDeviceQueue(_computeFamily, 0, out _computeQueue);
    }

    public VkDeviceApi Api => _api;

    public VulkanInstance Instance => _instance;

    public VkPhysicalDevice PhysicalDevice => _physicalDevice;

    public VkDevice Device => _device;

    public uint GraphicsFamily => _graphicsFamily;

    public uint PresentFamily => _presentFamily;

    public uint ComputeFamily => _computeFamily;

    public VkQueue GraphicsQueue => _graphicsQueue;

    public VkQueue PresentQueue => _presentQueue;

    public VkQueue ComputeQueue => _computeQueue;


    public void Dispose()
    {
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