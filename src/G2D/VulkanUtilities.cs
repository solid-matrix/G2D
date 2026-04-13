using System.Runtime.InteropServices;
using System.Text;
using Vortice.Vulkan;

namespace G2D;

internal static unsafe class VulkanUtilities
{
    public static VkDescriptorPool CreateDescriptorPool(VulkanDevice device, uint maxSets, uint uniformCount, uint imageCount, uint samplerCount)
    {
        VkDescriptorPoolSize[] descriptorPoolSizes =
        [
            new() { type = VkDescriptorType.UniformBuffer, descriptorCount = uniformCount },
            new() { type = VkDescriptorType.SampledImage, descriptorCount = imageCount },
            new() { type = VkDescriptorType.Sampler, descriptorCount = samplerCount }
        ];
        fixed (VkDescriptorPoolSize* pDescriptorPoolSizes = descriptorPoolSizes)
        {
            var descriptorPoolInfo = new VkDescriptorPoolCreateInfo
            {
                flags = VkDescriptorPoolCreateFlags.UpdateAfterBind,
                poolSizeCount = (uint)descriptorPoolSizes.Length,
                pPoolSizes = pDescriptorPoolSizes,
                maxSets = maxSets
            };
            device.Api.vkCreateDescriptorPool(&descriptorPoolInfo, out var descriptorPool)
                .CheckResult("failed to create descriptor pool");

            return descriptorPool;
        }
    }

    public static void TransitionImageLayout(VulkanDevice device, VkCommandBuffer commandBuffer, VkImage image,
        VkImageLayout oldLayout, VkImageLayout newLayout,
        VkAccessFlags2 srcAccessMask, VkAccessFlags2 dstAccessMask,
        VkPipelineStageFlags2 srcStage, VkPipelineStageFlags2 dstStage)
    {
        // Initialize the VkImageMemoryBarrier2 structure
        var imageBarrier = new VkImageMemoryBarrier2
        {
            srcStageMask = srcStage, // Source pipeline stage mask
            dstStageMask = dstStage, // Destination pipeline stage mask

            srcAccessMask = srcAccessMask, // Source access mask
            dstAccessMask = dstAccessMask, // Destination access mask

            oldLayout = oldLayout, // Current layout of the image
            newLayout = newLayout, // Target layout of the image

            image = image,
            subresourceRange = new VkImageSubresourceRange(Vulkan.VK_IMAGE_ASPECT_COLOR_BIT, 0, 1, 0, 1)
        };

        var dependencyInfo = new VkDependencyInfo
        {
            dependencyFlags = VkDependencyFlags.ByRegion,
            // dependencyFlags = VkDependencyFlags.None,
            imageMemoryBarrierCount = 1,
            pImageMemoryBarriers = &imageBarrier
        };

        device.Api.vkCmdPipelineBarrier2(commandBuffer, &dependencyInfo);
    }

    public static VkPhysicalDevice SelectPhysicalDevice(VulkanInstance instance, VkPhysicalDevice[] physicalDevices, VkSurfaceKHR surface)
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

    public static int RankPhysicalDevice(VulkanInstance instance, VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        var (graphicsFamily, presentFamily, computeFamily) = instance.QueryGraphicsPresentQueueFamilies(device, surface);

        if (graphicsFamily == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;
        if (presentFamily == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;
        if (computeFamily == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;

        var formats = instance.GetPhysicalDeviceSurfaceFormats(device, surface);
        if (formats.Length == 0) return -1;

        var presentModes = instance.GetPhysicalDeviceSurfacePresentModes(device, surface);
        if (presentModes.Length == 0) return -1;

        VkPhysicalDeviceFeatures2 queryDeviceFeatures2 = new();
        VkPhysicalDeviceVulkan12Features vulkan12Features = new();
        VkPhysicalDeviceVulkan13Features vulkan13Features = new();

        queryDeviceFeatures2.pNext = &vulkan12Features;
        vulkan12Features.pNext = &vulkan13Features;

        instance.Api.vkGetPhysicalDeviceFeatures2(device, &queryDeviceFeatures2);

        if (!vulkan12Features.descriptorIndexing) return -1;
        if (!vulkan12Features.runtimeDescriptorArray) return -1;
        if (!vulkan12Features.shaderSampledImageArrayNonUniformIndexing) return -1;
        if (!vulkan13Features.dynamicRendering) return -1;
        if (!vulkan13Features.synchronization2) return -1;

        var rank = 0;
        VkPhysicalDeviceProperties props;
        VkPhysicalDeviceDescriptorIndexingProperties indexingProps = new();
        VkPhysicalDeviceProperties2 props2 = new() { pNext = &indexingProps };
        instance.Api.vkGetPhysicalDeviceProperties(device, &props);
        instance.Api.vkGetPhysicalDeviceProperties2(device, &props2);

        if (props.deviceType == VkPhysicalDeviceType.DiscreteGpu) rank += 1000;

        return rank;
    }

    public static VkUtf8String[] EnumerateInstanceLayerNames()
    {
        Vulkan.vkEnumerateInstanceLayerProperties(out var count)
            .CheckResult("failed to get instance layer properties");

        if (count == 0) return [];

        var props = new VkLayerProperties[count];

        Vulkan.vkEnumerateInstanceLayerProperties(props)
            .CheckResult("failed to get instance layer properties");

        var names = new VkUtf8String[count];
        for (var i = 0; i < count; i++)
            fixed (byte* pLayerName = props[i].layerName)
            {
                names[i] = new VkUtf8String(pLayerName);
            }

        return names;
    }

    public static VkUtf8String[] EnumerateInstanceExtensionNames()
    {
        Vulkan.vkEnumerateInstanceExtensionProperties(out var count)
            .CheckResult("failed to get instance layer properties");

        if (count == 0) return [];

        var props = new VkExtensionProperties[(int)count];

        Vulkan.vkEnumerateInstanceExtensionProperties(props)
            .CheckResult("failed to get instance layer properties");

        var names = new VkUtf8String[count];
        for (var i = 0; i < count; i++)
            fixed (byte* pExtensionName = props[i].extensionName)
            {
                names[i] = new VkUtf8String(pExtensionName);
            }

        return names;
    }

    public static bool CheckIsSupported(VkVersion apiVersion)
    {
        try
        {
            var res = Vulkan.vkInitialize();
            if (res != VkResult.Success) return false;

            uint propCount;
            res = Vulkan.vkEnumerateInstanceExtensionProperties(&propCount, null);
            if (res != VkResult.Success) return false;

            // We require Vulkan 1.3 or higher
            var version = Vulkan.vkEnumerateInstanceVersion();
            if (version < apiVersion)
                return false;

            // TODO: Enumerate physical devices and try to create instance.

            return true;
        }
        catch
        {
            return false;
        }
    }

    [UnmanagedCallersOnly]
    public static uint DebugMessengerCallback(VkDebugUtilsMessageSeverityFlagsEXT messageSeverity, VkDebugUtilsMessageTypeFlagsEXT messageTypes, VkDebugUtilsMessengerCallbackDataEXT* pCallbackData, void* userData)
    {
        var message = new VkUtf8String(pCallbackData->pMessage);
        Console.WriteLine($"[Vulkan][{messageTypes}][{messageSeverity}]: {message}");
        return Vulkan.VK_FALSE;
    }

    public static VkVersion ToVkVersion(this Version version)
    {
        return new VkVersion((uint)version.Major, (uint)version.Minor, (uint)version.Build);
    }

    public static VkUtf8String ToVkUtf8String(this string str)
    {
        return Encoding.UTF8.GetBytes(str);
    }
}