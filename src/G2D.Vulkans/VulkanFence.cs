using Vortice.Vulkan;

namespace G2D;

public sealed class VulkanFence : IDisposable
{
    private readonly VulkanDevice _device;

    private readonly VkFence _fence;

    internal VulkanFence(VulkanDevice device, VkFenceCreateFlags flags)
    {
        _device = device;
        _device.Api.vkCreateFence(flags, out _fence);
    }

    public void Dispose()
    {
        _device.Api.vkDestroyFence(_fence);
    }

    public bool IsSignaled()
    {
        var res = _device.Api.vkGetFenceStatus(_fence);
        return res == VkResult.Success;
    }

    public static implicit operator VkFence(VulkanFence fence)
    {
        return fence._fence;
    }
}