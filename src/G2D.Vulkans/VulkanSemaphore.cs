using Vortice.Vulkan;

namespace G2D;

public sealed class VulkanSemaphore : IDisposable
{
    private readonly VulkanDevice _device;

    private readonly VkSemaphore _semaphore;

    internal VulkanSemaphore(VulkanDevice device)
    {
        _device = device;
        _device.Api.vkCreateSemaphore(out _semaphore);
    }

    public void Dispose()
    {
        _device.Api.vkDestroySemaphore(_semaphore);
    }

    public static implicit operator VkSemaphore(VulkanSemaphore semaphore)
    {
        return semaphore._semaphore;
    }
}