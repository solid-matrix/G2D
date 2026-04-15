using System.Runtime.InteropServices;
using Vortice.Vulkan;

namespace G2D;

public sealed unsafe class FrameManager : IDisposable
{
    private readonly GraphicsDevice _device;

    private readonly int _frameCount;

    private int _current;

    private readonly VkFence[] _fences;

    private readonly VkCommandBuffer[] _commandBuffers;

    private readonly List<VkSemaphore>[] _pendingSemaphoreLists;

    private readonly VkSemaphore[] _releaseSemaphores;

    private readonly VkSemaphore[] _acquireSemaphores;

    public FrameManager(GraphicsDevice device, int frameCount)
    {
        _device = device;
        _frameCount = frameCount;
        _current = 0;

        _fences = new VkFence[_frameCount];
        _commandBuffers = new VkCommandBuffer[_frameCount];
        _pendingSemaphoreLists = new List<VkSemaphore>[_frameCount];

        _releaseSemaphores = new VkSemaphore[_frameCount];
        _acquireSemaphores = new VkSemaphore[_frameCount];

        for (var i = 0; i < _frameCount; ++i)
        {
            _device.Api.vkAllocateCommandBuffer(_device.GraphicsCommandPool, out _commandBuffers[i])
                .CheckResult("Vulkan: failed to allocate command buffer");

            _pendingSemaphoreLists[i] = [];

            _releaseSemaphores[i] = _device.SemaphorePool.Acquire();
            _acquireSemaphores[i] = _device.SemaphorePool.Acquire();
            _fences[i] = _device.FencePool.Acquire();
        }
    }

    public void Dispose()
    {
        for (var i = 0; i < _frameCount; ++i)
        {
            _device.SemaphorePool.Release(_releaseSemaphores[i]);
            _device.SemaphorePool.Release(_acquireSemaphores[i]);
            _device.FencePool.Release(_fences[i]);
        }
    }

    public bool TryFetch(out VkCommandBuffer commandBuffer, out int frameIndex, out VkSemaphore acquireSemaphore)
    {
        commandBuffer = VkCommandBuffer.Null;
        frameIndex = _current;
        acquireSemaphore = _acquireSemaphores[_current];

        var res = _device.Api.vkGetFenceStatus(_fences[_current]);
        if (res != VkResult.Success) return false;

        _device.SemaphorePool.Release(CollectionsMarshal.AsSpan(_pendingSemaphoreLists[_current]));
        _pendingSemaphoreLists[_current].Clear();

        _device.Api.vkResetCommandBuffer(_commandBuffers[_current], VkCommandBufferResetFlags.None);
        commandBuffer = _commandBuffers[_current];

        return true;
    }

    public void Submit(VkCommandBuffer commandBuffer, ReadOnlySpan<VkSemaphore> waitSemaphores, out VkSemaphore signalSemaphore)
    {
        var waitStage = VkPipelineStageFlags.ColorAttachmentOutput;

        var releaseSemaphore = _releaseSemaphores[_current];

        _device.Api.vkResetFences(_fences[_current]);
        fixed (VkSemaphore* pWaitSemaphores = waitSemaphores)
        {
            var submitInfo = new VkSubmitInfo
            {
                commandBufferCount = 1u,
                pCommandBuffers = &commandBuffer,
                pWaitDstStageMask = &waitStage,

                waitSemaphoreCount = (uint)waitSemaphores.Length,
                pWaitSemaphores = pWaitSemaphores,

                signalSemaphoreCount = 1u,
                pSignalSemaphores = &releaseSemaphore
            };
            _device.Api.vkQueueSubmit(_device.GraphicsQueue, 1, &submitInfo, _fences[_current])
                .CheckResult("Vulkan: failed to queue submit");
        }

        _pendingSemaphoreLists[_current].AddRange(waitSemaphores);

        signalSemaphore = releaseSemaphore;
        _current = (_current + 1) % _frameCount;
    }

    public void Submit(VkCommandBuffer commandBuffer, VkSemaphore waitSemaphore, out VkSemaphore signalSemaphore)
    {
        Submit(commandBuffer, new ReadOnlySpan<VkSemaphore>(&waitSemaphore, 1), out signalSemaphore);
    }
}