using Vortice.Vulkan;

namespace G2D;

public sealed class VulkanCommandPool : IDisposable
{
    private readonly VulkanDevice _device;

    private readonly VkCommandPool _commandPool;

    private readonly HashSet<VulkanCommandBuffer> _commandBuffers;

    internal VulkanCommandPool(VulkanDevice device, VkCommandPoolCreateFlags flags, uint queueFamilyIndex)
    {
        _device = device;
        _device.Api.vkCreateCommandPool(flags, queueFamilyIndex, out _commandPool)
            .CheckResult("Vulkan: failed to create command pool");
        ;
        _commandBuffers = [];
    }

    public void Dispose()
    {
        _device.Api.vkDestroyCommandPool(_commandPool);
    }

    public VulkanCommandBuffer CreateCommandBuffer(VkCommandBufferLevel level = VkCommandBufferLevel.Primary)
    {
        var buffer = new VulkanCommandBuffer(_device, this, level);
        _commandBuffers.Add(buffer);
        return buffer;
    }

    public static implicit operator VkCommandPool(VulkanCommandPool pool)
    {
        return pool._commandPool;
    }
}