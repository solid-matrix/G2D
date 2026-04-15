using Vortice.Vulkan;

namespace G2D;

public sealed class CommandPool : IDisposable
{
    private readonly Device _device;

    private readonly VkCommandPool _commandPool;

    private readonly HashSet<CommandBuffer> _commandBuffers;

    internal CommandPool(Device device, VkCommandPoolCreateFlags flags, uint queueFamilyIndex)
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

    public CommandBuffer CreateCommandBuffer(VkCommandBufferLevel level = VkCommandBufferLevel.Primary)
    {
        var buffer = new CommandBuffer(_device, this, level);
        _commandBuffers.Add(buffer);
        return buffer;
    }

    public static implicit operator VkCommandPool(CommandPool pool)
    {
        return pool._commandPool;
    }
}