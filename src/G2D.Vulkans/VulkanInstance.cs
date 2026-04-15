using System.Runtime.InteropServices;
using Vortice.Vulkan;

namespace G2D;

public sealed unsafe class VulkanInstance : IDisposable
{
    private const string DefaultApplicationName = "G2D Application";

    private const string DefaultEngineName = "Custom";


    private readonly VkInstance _instance;

    private readonly VkInstanceApi _api;

    private readonly bool _debugEnabled;

    private readonly VkVersion _apiVersion;

    private readonly VkDebugUtilsMessengerEXT _debugMessenger = VkDebugUtilsMessengerEXT.Null;

    public VulkanInstance(string[] extensions, VkVersion apiVersion, string appName = DefaultApplicationName, Version? appVersion = null, string engineName = DefaultEngineName, Version? engineVersion = null, bool debugEnabled = false)
    {
        _debugEnabled = debugEnabled;
        _apiVersion = apiVersion;

        if (!CheckIsSupported(_apiVersion))
            throw new VkException("vulkan not supported");

        var appInfo = new VkApplicationInfo
        {
            pApplicationName = appName.ToVkUtf8String(),
            applicationVersion = appVersion.ToVkVersion(),
            pEngineName = engineName.ToVkUtf8String(),
            engineVersion = engineVersion.ToVkVersion(),
            apiVersion = _apiVersion
        };


        HashSet<VkUtf8String> availableLayers = [..EnumerateInstanceLayers()];
        HashSet<VkUtf8String> availableExtensions = [..EnumerateInstanceExtensions()];

        HashSet<VkUtf8String> requiredLayers = [];
        HashSet<VkUtf8String> requiredExtensions = [.. extensions.ToVkUtf8StringArray()];

        if (_debugEnabled)
        {
            requiredLayers.Add(Vulkan.VK_LAYER_KHRONOS_VALIDATION_EXTENSION_NAME);
            requiredExtensions.Add(Vulkan.VK_EXT_DEBUG_UTILS_EXTENSION_NAME);
        }

        if (!requiredLayers.All(availableLayers.Contains))
            throw new Exception("vulkan required instance layer not supported");

        if (!requiredExtensions.All(availableExtensions.Contains))
            throw new Exception("vulkan required instance extension not supported");


        if (_debugEnabled)
        {
            Console.WriteLine($"vulkan instance layer enabled: {string.Join(", ", requiredLayers)}");
            Console.WriteLine($"vulkan instance extension enabled: {string.Join(", ", requiredExtensions)}");
        }

        using VkStringArray vkLayerNames = new(requiredLayers);
        using VkStringArray vkExtensionNames = new(requiredExtensions);

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
            debugUtilsCreateInfo.pfnUserCallback = &DebugMessengerCallback;
            instanceCreateInfo.pNext = &debugUtilsCreateInfo;
        }

        Vulkan.vkCreateInstance(&instanceCreateInfo, out _instance)
            .CheckResult("vulkan failed to create instance");

        _api = Vulkan.GetApi(_instance);

        if (_debugEnabled)
            _api.vkCreateDebugUtilsMessengerEXT(&debugUtilsCreateInfo, null, out _debugMessenger)
                .CheckResult("vulkan failed to create debug utils messenger");
    }

    public VkInstanceApi Api => _api;

    public bool DebugEnabled => _debugEnabled;

    public VkVersion ApiVersion => _apiVersion;

    public void Dispose()
    {
        if (_debugEnabled)
            _api.vkDestroyDebugUtilsMessengerEXT(_debugMessenger);

        _api.vkDestroyInstance();
    }


    public VkPhysicalDevice[] EnumerateGpus()
    {
        _api.vkEnumeratePhysicalDevices(out var count)
            .CheckResult("failed to enumerate physical devices");
        if (count == 0) return [];

        var physicalDevices = new VkPhysicalDevice[count];

        _api.vkEnumeratePhysicalDevices(physicalDevices)
            .CheckResult("failed to enumerate physical devices");

        return physicalDevices;
    }

    public VkSurfaceCapabilitiesKHR GetGpuSurfaceCapabilities(VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        _api.vkGetPhysicalDeviceSurfaceCapabilitiesKHR(device, surface, out var capabilities)
            .CheckResult("failed to get physical device surface capabilities");
        return capabilities;
    }

    public VkSurfaceFormatKHR[] GetGpuSurfaceFormats(VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        _api.vkGetPhysicalDeviceSurfaceFormatsKHR(device, surface, out var count)
            .CheckResult("failed to  get physical device surface formats");
        if (count == 0) return [];

        var formats = new VkSurfaceFormatKHR[count];

        _api.vkGetPhysicalDeviceSurfaceFormatsKHR(device, surface, formats)
            .CheckResult("failed to  get physical device surface formats");

        return formats;
    }

    public VkPresentModeKHR[] GetGpuSurfacePresentModes(VkPhysicalDevice device, VkSurfaceKHR surface)
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

    public static bool CheckIsSupported(VkVersion requiredApiVersion)
    {
        try
        {
            var res = Vulkan.vkInitialize();
            if (res != VkResult.Success) return false;

            var version = Vulkan.vkEnumerateInstanceVersion();
            return version >= requiredApiVersion;
        }
        catch
        {
            return false;
        }
    }

    public static VkUtf8String[] EnumerateInstanceLayers()
    {
        Vulkan.vkEnumerateInstanceLayerProperties(out var count)
            .CheckResult("failed to get instance layer properties");

        if (count == 0) return [];

        var props = new VkLayerProperties[count];

        Vulkan.vkEnumerateInstanceLayerProperties(props)
            .CheckResult("failed to get instance layer properties");

        var names = new VkUtf8String[count];
        for (var i = 0; i < count; i++)
        {
            fixed (byte* pLayerName = props[i].layerName)
            {
                names[i] = new VkUtf8String(pLayerName);
            }
        }

        return names;
    }

    public static VkUtf8String[] EnumerateInstanceExtensions()
    {
        Vulkan.vkEnumerateInstanceExtensionProperties(out var count)
            .CheckResult("failed to get instance layer properties");

        if (count == 0) return [];

        var props = new VkExtensionProperties[(int)count];

        Vulkan.vkEnumerateInstanceExtensionProperties(props)
            .CheckResult("failed to get instance layer properties");

        var names = new VkUtf8String[count];
        for (var i = 0; i < count; i++)
        {
            fixed (byte* pExtensionName = props[i].extensionName)
            {
                names[i] = new VkUtf8String(pExtensionName);
            }
        }

        return names;
    }

    [UnmanagedCallersOnly]
    public static uint DebugMessengerCallback(VkDebugUtilsMessageSeverityFlagsEXT messageSeverity, VkDebugUtilsMessageTypeFlagsEXT messageTypes, VkDebugUtilsMessengerCallbackDataEXT* pCallbackData, void* userData)
    {
        var message = new VkUtf8String(pCallbackData->pMessage);
        Console.WriteLine($"[Vulkan][{messageTypes}][{messageSeverity}]: {message}");
        return Vulkan.VK_FALSE;
    }

    public static implicit operator VkInstance(VulkanInstance instance)
    {
        return instance._instance;
    }
}