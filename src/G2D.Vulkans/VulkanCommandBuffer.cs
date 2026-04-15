using Vortice.Vulkan;

namespace G2D;

public sealed class VulkanCommandBuffer
{
    private readonly VulkanDevice _device;

    private readonly VulkanCommandPool _commandPool;

    private readonly VkCommandBuffer _commandBuffer;

    internal VulkanCommandBuffer(VulkanDevice device, VulkanCommandPool pool, VkCommandBufferLevel level = VkCommandBufferLevel.Primary)
    {
        _device = device;
        _commandPool = pool;
        _device.Api.vkAllocateCommandBuffer(_commandPool, level, out _commandBuffer)
            .CheckResult("Vulkan: failed to allocate command buffer");
    }

    public VulkanCommandPool CommandPool => _commandPool;

    public void Reset()
    {
        _device.Api.vkResetCommandBuffer(_commandBuffer, VkCommandBufferResetFlags.None)
            .CheckResult("Vulkan: failed to reset command buffer");
    }

    // public void Dispose()
    // {
    //     _device.Api.vkFreeCommandBuffers(_commandPool, _commandBuffer);
    // }

    public static implicit operator VkCommandBuffer(VulkanCommandBuffer buffer)
    {
        return buffer._commandBuffer;
    }
}