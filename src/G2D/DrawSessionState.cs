using Vortice.Vulkan;

namespace G2D;

internal struct DrawSessionState
{
    public VkCommandBuffer _commandBuffer;

    public VkExtent2D _extent;

    public UniformBuffer _uniformBuffer;

    public BufferSpanPool _vertexBufferPool;

    public BufferSpanPool _instanceBufferPool;
}