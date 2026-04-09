using Vortice.Vulkan;

namespace G2D;

internal struct DrawSessionState
{
    internal VkCommandBuffer _commandBuffer;

    internal VkExtent2D _extent;

    internal BufferSpan _uniformBuffer;

    internal BufferSpanPool _vertexBufferPool;

    internal BufferSpanPool _instanceBufferPool;

    internal BufferSpanPool _indexBufferPool;
}