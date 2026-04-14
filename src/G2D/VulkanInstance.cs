using Vortice.Vulkan;

namespace G2D;

internal sealed unsafe class VulkanInstance : IDisposable
{
    private readonly VkInstance _instance;

    private readonly VkInstanceApi _api;

    private readonly bool _debugEnabled;

    private readonly VkVersion _apiVersion;

    private readonly VkDebugUtilsMessengerEXT _debugMessenger = VkDebugUtilsMessengerEXT.Null;


    public VulkanInstance(VkUtf8String appName, VkVersion appVersion, VkUtf8String engineName, VkVersion engineVersion, VkVersion apiVersion, VkUtf8String[] requiredLayers, VkUtf8String[] requiredExtensions, bool debugEnabled = false)
    {
        _debugEnabled = debugEnabled;

        if (!VulkanUtilities.CheckIsSupported(apiVersion)) throw new VkException("vulkan not supported");
        _apiVersion = apiVersion;

        HashSet<VkUtf8String> availableLayerSet = [..VulkanUtilities.EnumerateInstanceLayerNames()];
        HashSet<VkUtf8String> availableExtensionSet = [..VulkanUtilities.EnumerateInstanceExtensionNames()];

        HashSet<VkUtf8String> requiredLayerSet = [..requiredLayers];
        HashSet<VkUtf8String> requiredExtensionSet = [.. requiredExtensions];

        if (_debugEnabled)
        {
            requiredLayerSet.Add(Vulkan.VK_LAYER_KHRONOS_VALIDATION_EXTENSION_NAME);
            requiredExtensionSet.Add(Vulkan.VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
        }

        if (!requiredLayerSet.All(availableLayerSet.Contains))
            throw new Exception("vulkan required instance layer not supported");

        if (!requiredExtensionSet.All(availableExtensionSet.Contains))
            throw new Exception("vulkan required instance extension not supported");

        VkApplicationInfo appInfo = new()
        {
            pApplicationName = appName,
            applicationVersion = appVersion,
            pEngineName = engineName,
            engineVersion = engineVersion,
            apiVersion = apiVersion
        };

        if (_debugEnabled)
        {
            Console.WriteLine($"vulkan instance layer enabled: {string.Join(", ", requiredLayerSet)}");
            Console.WriteLine($"vulkan instance extension enabled: {string.Join(", ", requiredExtensionSet)}");
        }

        using VkStringArray vkLayerNames = new(requiredLayerSet);
        using VkStringArray vkExtensionNames = new(requiredExtensionSet);

        VkInstanceCreateInfo instanceCreateInfo = new()
        {
            pApplicationInfo = &appInfo,
            enabledLayerCount = vkLayerNames.Length,
            ppEnabledLayerNames = vkLayerNames,
            enabledExtensionCount = vkExtensionNames.Length,
            ppEnabledExtensionNames = vkExtensionNames
        };

        VkDebugUtilsMessengerCreateInfoEXT debugUtilsCreateInfo = new();
        if (_debugEnabled)
        {
            debugUtilsCreateInfo.messageSeverity = VkDebugUtilsMessageSeverityFlagsEXT.Error | VkDebugUtilsMessageSeverityFlagsEXT.Warning;
            debugUtilsCreateInfo.messageType = VkDebugUtilsMessageTypeFlagsEXT.Validation | VkDebugUtilsMessageTypeFlagsEXT.Performance;
            debugUtilsCreateInfo.pfnUserCallback = &VulkanUtilities.DebugMessengerCallback;
            instanceCreateInfo.pNext = &debugUtilsCreateInfo;
        }

        Vulkan.vkCreateInstance(&instanceCreateInfo, out _instance)
            .CheckResult("vulkan failed to create instance");

        _api = Vulkan.GetApi(_instance);

        if (_debugEnabled)
            _api.vkCreateDebugUtilsMessengerEXT(&debugUtilsCreateInfo, null, out _debugMessenger)
                .CheckResult("vulkan failed to create debug utils messenger");
    }

    public VkInstance Instance => _instance;

    public bool DebugEnabled => _debugEnabled;

    public VkInstanceApi Api => _api;

    public VkVersion ApiVersion => _apiVersion;

    public void Dispose()
    {
        if (_debugEnabled)
            _api.vkDestroyDebugUtilsMessengerEXT(_debugMessenger);

        _api.vkDestroyInstance();
    }

    public VkPhysicalDevice[] EnumeratePhysicalDevices()
    {
        _api.vkEnumeratePhysicalDevices(out var count)
            .CheckResult("failed to enumerate physical devices");
        if (count == 0) return [];

        var physicalDevices = new VkPhysicalDevice[count];

        _api.vkEnumeratePhysicalDevices(physicalDevices)
            .CheckResult("failed to enumerate physical devices");

        return physicalDevices;
    }

    public VkSurfaceCapabilitiesKHR GetPhysicalDeviceSurfaceCapabilities(VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        _api.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(device, surface, out var capabilities)
            .CheckResult("failed to get physical device surface capabilities");
        return capabilities;
    }

    public VkSurfaceFormatKHR[] GetPhysicalDeviceSurfaceFormats(VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        _api.vkGetPhysicalDeviceSurfaceFormatsKHR(device, surface, out var count)
            .CheckResult("failed to  get physical device surface formats");
        if (count == 0) return [];

        var formats = new VkSurfaceFormatKHR[count];

        _api.vkGetPhysicalDeviceSurfaceFormatsKHR(device, surface, formats)
            .CheckResult("failed to  get physical device surface formats");

        return formats;
    }

    public VkPresentModeKHR[] GetPhysicalDeviceSurfacePresentModes(VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        _api.vkGetPhysicalDeviceSurfacePresentModesKHR(device, surface, out var count)
            .CheckResult("failed to   get physical device surface present modes");

        if (count == 0) return [];

        var presentModes = new VkPresentModeKHR[count];

        _api.vkGetPhysicalDeviceSurfacePresentModesKHR(device, surface, presentModes)
            .CheckResult("failed to   get physical device surface present modes");

        return presentModes;
    }

    public VkUtf8String[] EnumerateDeviceExtensionNames(VkPhysicalDevice device)
    {
        _api.vkEnumerateDeviceExtensionProperties(device, out var count)
            .CheckResult("failed to get device extension properties");

        if (count == 0) return [];

        var extensionProperties = new VkExtensionProperties[count];

        _api.vkEnumerateDeviceExtensionProperties(device, extensionProperties)
            .CheckResult("failed to get device extension properties");

        var names = new VkUtf8String[count];
        for (var i = 0; i < count; i++)
        {
            fixed (byte* pLayerName = extensionProperties[i].extensionName)
            {
                names[i] = new VkUtf8String(pLayerName);
            }
        }

        return names;
    }


    public static implicit operator VkInstance(VulkanInstance instance) => instance._instance;
}