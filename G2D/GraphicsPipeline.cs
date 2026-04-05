using Vortice.Vulkan;

namespace G2D;

public abstract class GraphicsPipeline
{
    public abstract VkPipelineBindPoint BindPoint { get; }

    public abstract VkPipeline Pipeline { get; }

    public abstract VkPipelineLayout PipelineLayout { get; }
}