using Vortice.Vulkan;

namespace G2D;

public sealed class FencePool : IDisposable
{
    private readonly GraphicsDevice _device;

    private readonly List<VkFence> _fences;

    private readonly Queue<VkFence> _available;

    public FencePool(GraphicsDevice device)
    {
        _device = device;
        _fences = [];
        _available = [];
    }

    public void Dispose()
    {
        foreach (var fence in _fences)
        {
            _device.Api.vkDestroyFence(fence);
        }
    }

    public VkFence Acquire()
    {
        if (_available.Count == 0)
        {
            _device.Api.vkCreateFence(VkFenceCreateFlags.Signaled, out var semaphore);
            _fences.Add(semaphore);
            _available.Enqueue(semaphore);
        }

        return _available.Dequeue();
    }

    public void Release(VkFence fence)
    {
        if (fence == VkFence.Null) throw new Exception("Vulkan: failed to release a null fence");
        _available.Enqueue(fence);
    }

    public void Release(ReadOnlySpan<VkFence> fences)
    {
        foreach (var fence in fences)
        {
            _available.Enqueue(fence);
        }
    }
}