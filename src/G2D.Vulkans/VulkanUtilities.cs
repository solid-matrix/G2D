using Vortice.Vulkan;

namespace G2D;

public static unsafe class VulkanUtilities
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

    // TODO
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

    public static uint[] QueryQueueFamilies(VulkanInstance instance, VkPhysicalDevice device, VkSurfaceKHR surface)
    {
        var queueFamilies = new uint [QueueType.GetTypeCount()];
        Array.Fill(queueFamilies, Vulkan.VK_QUEUE_FAMILY_IGNORED);

        instance.Api.vkGetPhysicalDeviceQueueFamilyProperties(device, out var count);
        if (count == 0) return queueFamilies;

        var props = new VkQueueFamilyProperties[count];
        instance.Api.vkGetPhysicalDeviceQueueFamilyProperties(device, props);

        for (uint i = 0; i < props.Length; i++)
        {
            if (queueFamilies[QueueType.Graphics] == Vulkan.VK_QUEUE_FAMILY_IGNORED && (props[i].queueFlags & VkQueueFlags.Graphics) != 0)
            {
                var res = instance.Api.vkGetPhysicalDeviceSurfaceSupportKHR(device, i, surface, out var presentSupport);
                if (res == VkResult.Success && presentSupport)
                {
                    queueFamilies[QueueType.Graphics] = i;
                    continue;
                }
            }

            if (queueFamilies[QueueType.Compute] == Vulkan.VK_QUEUE_FAMILY_IGNORED && (props[i].queueFlags & VkQueueFlags.Compute) != 0)
            {
                queueFamilies[QueueType.Compute] = i;
                continue;
            }

            if (queueFamilies[QueueType.Transfer] == Vulkan.VK_QUEUE_FAMILY_IGNORED && (props[i].queueFlags & VkQueueFlags.Transfer) != 0) queueFamilies[QueueType.Transfer] = i;
        }

        if (queueFamilies[QueueType.Compute] == Vulkan.VK_QUEUE_FAMILY_IGNORED)
            queueFamilies[QueueType.Compute] = queueFamilies[QueueType.Graphics];

        if (queueFamilies[QueueType.Transfer] == Vulkan.VK_QUEUE_FAMILY_IGNORED)
            queueFamilies[QueueType.Transfer] = queueFamilies[QueueType.Graphics];

        return queueFamilies;
    }

    public static VkPhysicalDevice SelectGpu(VulkanInstance instance, VkPhysicalDevice[] gpus, VkSurfaceKHR surface)
    {
        var max = 0;
        var selected = VkPhysicalDevice.Null;

        foreach (var physicalDevice in gpus)
        {
            var rank = RankGpu(instance, physicalDevice, surface);
            if (rank > max)
            {
                max = rank;
                selected = physicalDevice;
            }
        }

        return selected;
    }

    public static int RankGpu(VulkanInstance instance, VkPhysicalDevice gpu, VkSurfaceKHR surface)
    {
        var queueFamilies = QueryQueueFamilies(instance, gpu, surface);

        if (queueFamilies[QueueType.Graphics] == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;
        if (queueFamilies[QueueType.Compute] == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;
        if (queueFamilies[QueueType.Transfer] == Vulkan.VK_QUEUE_FAMILY_IGNORED) return -1;

        var formats = instance.GetPhysicalDeviceSurfaceFormats(gpu, surface);
        if (formats.Length == 0) return -1;

        var presentModes = instance.GetPhysicalDeviceSurfacePresentModes(gpu, surface);
        if (presentModes.Length == 0) return -1;

        VkPhysicalDeviceFeatures2 queryDeviceFeatures2 = new();
        VkPhysicalDeviceVulkan12Features vulkan12Features = new();
        VkPhysicalDeviceVulkan13Features vulkan13Features = new();

        queryDeviceFeatures2.pNext = &vulkan12Features;
        vulkan12Features.pNext = &vulkan13Features;

        instance.Api.vkGetPhysicalDeviceFeatures2(gpu, &queryDeviceFeatures2);

        if (!vulkan12Features.descriptorIndexing) return -1;
        if (!vulkan12Features.runtimeDescriptorArray) return -1;
        if (!vulkan12Features.shaderSampledImageArrayNonUniformIndexing) return -1;
        if (!vulkan13Features.dynamicRendering) return -1;
        if (!vulkan13Features.synchronization2) return -1;

        var rank = 0;
        VkPhysicalDeviceProperties props;
        VkPhysicalDeviceDescriptorIndexingProperties indexingProps = new();
        VkPhysicalDeviceProperties2 props2 = new() { pNext = &indexingProps };
        instance.Api.vkGetPhysicalDeviceProperties(gpu, &props);
        instance.Api.vkGetPhysicalDeviceProperties2(gpu, &props2);

        if (props.deviceType == VkPhysicalDeviceType.DiscreteGpu) rank += 1000;

        return rank;
    }
}