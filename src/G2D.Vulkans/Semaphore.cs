using Vortice.Vulkan;

namespace G2D;

public sealed class Semaphore : IDisposable
{
    private readonly Device _device;

    private readonly VkSemaphore _semaphore;

    internal Semaphore(Device device)
    {
        _device = device;
        _device.Api.vkCreateSemaphore(out _semaphore);
    }

    public void Dispose()
    {
        _device.Api.vkDestroySemaphore(_semaphore);
    }

    public static implicit operator VkSemaphore(Semaphore semaphore)
    {
        return semaphore._semaphore;
    }
}