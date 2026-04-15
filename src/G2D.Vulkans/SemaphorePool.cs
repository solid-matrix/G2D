using Vortice.Vulkan;

namespace G2D;

public sealed class SemaphorePool : IDisposable
{
    private readonly GraphicsDevice _device;

    private readonly List<VkSemaphore> _semaphores;

    private readonly Queue<VkSemaphore> _available;

    public SemaphorePool(GraphicsDevice device)
    {
        _device = device;
        _semaphores = [];
        _available = [];
    }

    public void Dispose()
    {
        foreach (var semaphore in _semaphores)
        {
            _device.Api.vkDestroySemaphore(semaphore);
        }
    }

    public VkSemaphore Acquire()
    {
        if (_available.Count == 0)
        {
            _device.Api.vkCreateSemaphore(out var semaphore);
            _semaphores.Add(semaphore);
            _available.Enqueue(semaphore);
        }

        return _available.Dequeue();
    }

    public void Release(VkSemaphore semaphore)
    {
        if (semaphore == VkSemaphore.Null) throw new Exception("Vulkan: failed to release a null semaphore");
        _available.Enqueue(semaphore);
    }

    public void Release(ReadOnlySpan<VkSemaphore> semaphores)
    {
        foreach (var semaphore in semaphores)
        {
            _available.Enqueue(semaphore);
        }
    }
}