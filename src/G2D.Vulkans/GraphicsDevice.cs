using Vortice.Vulkan;

namespace G2D;

public sealed unsafe class GraphicsDevice : IDisposable
{
    public const int MaxFrameCountInFlight = 3;

    private readonly VulkanInstance _instance;

    private readonly VkPhysicalDevice _gpu;

    private readonly VkDevice _device;

    private readonly VkDeviceApi _api;

    private readonly VmaAllocator _vmaAllocator;

    private readonly uint[] _queueFamilies;

    private readonly VkQueue[] _queues = new VkQueue[QueueType.GetTypeCount()];

    private readonly VkCommandPool[] _commandPools = new VkCommandPool[QueueType.GetTypeCount()];

    private readonly SemaphorePool _semaphorePool;

    private readonly FencePool _fencePool;

    private readonly FrameManager _frameManager;

    public GraphicsDevice(VulkanInstance instance, VkSurfaceKHR surface, VkUtf8String[] extensions)
        : this(instance, VulkanUtilities.SelectGpu(instance, instance.EnumerateGpus(), surface), surface, extensions)
    {
    }

    public GraphicsDevice(VulkanInstance instance, VkPhysicalDevice gpu, VkSurfaceKHR surface, VkUtf8String[] extensions)
    {
        _instance = instance;
        _gpu = gpu;

        HashSet<VkUtf8String> availableExtensionSet = [.. _instance.EnumerateDeviceExtensionNames(_gpu)];
        HashSet<VkUtf8String> requiredExtensionSet = [..extensions];

        if (!requiredExtensionSet.All(availableExtensionSet.Contains))
            throw new VkException("vulkan required device extension not supported");

        if (_instance.DebugEnabled)
            Console.WriteLine($"vulkan device extension enabled: {string.Join(", ", requiredExtensionSet)}");

        _queueFamilies = VulkanUtilities.QueryQueueFamilies(_instance, gpu, surface);

        if (instance.DebugEnabled)
            foreach (var i in QueueType.GetAllTypes())
            {
                if (_queueFamilies[i] != Vulkan.VK_QUEUE_FAMILY_IGNORED)
                    Console.WriteLine($"{i} Queue Family = {_queueFamilies[i]} ");
            }


        var counts = new Dictionary<uint, uint>();
        var offsets = new uint [QueueType.GetTypeCount()];

        foreach (var i in QueueType.GetAllTypes())
        {
            if (!counts.ContainsKey(_queueFamilies[i])) counts[_queueFamilies[i]] = 0;

            offsets[i] = counts[_queueFamilies[i]];
            counts[_queueFamilies[i]] += 1;
        }

        var priorities = stackalloc float[QueueType.GetTypeCount()];

        for (var i = 0; i < QueueType.GetTypeCount(); i++)
        {
            priorities[i] = 1;
        }

        var queueCreateInfos = stackalloc VkDeviceQueueCreateInfo[QueueType.GetTypeCount()];
        var queueFamilyCount = 0;

        foreach (var pair in counts)
        {
            if (pair.Value == 0) continue;

            queueCreateInfos[queueFamilyCount] = new VkDeviceQueueCreateInfo
            {
                queueFamilyIndex = pair.Key,
                queueCount = pair.Value,
                pQueuePriorities = priorities
            };

            queueFamilyCount++;
        }

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
            queueCreateInfoCount = (uint)queueFamilyCount,
            pQueueCreateInfos = queueCreateInfos,
            enabledExtensionCount = deviceExtensionNames.Length,
            ppEnabledExtensionNames = deviceExtensionNames,
            pEnabledFeatures = null,
            pNext = &features2
        };

        _instance.Api.vkCreateDevice(_gpu, &deviceCreateInfo, out _device)
            .CheckResult("failed to create vulkan device");

        _api = Vulkan.GetApi(_instance, _device);

        _api.vkGetDeviceQueue(_queueFamilies[QueueType.Graphics], offsets[QueueType.Graphics], out _queues[QueueType.Graphics]);
        _api.vkGetDeviceQueue(_queueFamilies[QueueType.Compute], offsets[QueueType.Compute], out _queues[QueueType.Compute]);
        _api.vkGetDeviceQueue(_queueFamilies[QueueType.Transfer], offsets[QueueType.Transfer], out _queues[QueueType.Transfer]);

        var vmaAllocatorInfo = new VmaAllocatorCreateInfo
        {
            vulkanApiVersion = _instance.ApiVersion,
            instance = _instance,
            physicalDevice = _gpu,
            device = _device
        };

        Vma.vmaCreateAllocator(in vmaAllocatorInfo, out _vmaAllocator)
            .CheckResult("failed to create vma allocator");

        foreach (var type in QueueType.GetAllTypes())
        {
            _api.vkCreateCommandPool(VkCommandPoolCreateFlags.ResetCommandBuffer, _queueFamilies[type], out _commandPools[type]);
        }

        _semaphorePool = new SemaphorePool(this);
        _fencePool = new FencePool(this);
        _frameManager = new FrameManager(this, MaxFrameCountInFlight);
    }


    public VulkanInstance Instance => _instance;

    public VkPhysicalDevice Gpu => _gpu;

    public VmaAllocator Allocator => _vmaAllocator;

    public VkDeviceApi Api => _api;

    public SemaphorePool SemaphorePool => _semaphorePool;

    public FencePool FencePool => _fencePool;

    public FrameManager FrameManager => _frameManager;

    public VkQueue GraphicsQueue => _queues[QueueType.Graphics];

    public VkQueue ComputeQueue => _queues[QueueType.Compute];

    public VkQueue TransferQueue => _queues[QueueType.Transfer];

    public VkCommandPool GraphicsCommandPool => _commandPools[QueueType.Graphics];

    public VkCommandPool ComputeCommandPool => _commandPools[QueueType.Compute];

    public VkCommandPool TransferCommandPool => _commandPools[QueueType.Transfer];


    public void Dispose()
    {
        _semaphorePool.Dispose();
        _fencePool.Dispose();
        _frameManager.Dispose();

        foreach (var i in QueueType.GetAllTypes())
        {
            _api.vkDestroyCommandPool(_commandPools[i]);
        }

        Vma.vmaDestroyAllocator(_vmaAllocator);
        _api.vkDestroyDevice();
    }

    public void WaitIdle()
    {
        _api.vkDeviceWaitIdle()
            .CheckResult("failed vulkan device wait idle");
    }

    public static implicit operator VkDevice(GraphicsDevice device)
    {
        return device._device;
    }
}