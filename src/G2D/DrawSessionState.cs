using Vortice.Vulkan;

namespace G2D;

internal struct DrawSessionState
{
    public VkCommandBuffer _commandBuffer;

    public VkExtent2D _extent;

    public UniformBuffer _uniformBuffer;

    public VertexInputManager VertexInputManager;
}