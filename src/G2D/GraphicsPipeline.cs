using Vortice.Vulkan;

namespace G2D;

internal class GraphicsPipeline : IDisposable
{
    private readonly VulkanDevice _device;

    private readonly VkPipelineLayout _pipelineLayout;

    private readonly VkPipeline _pipeline;

    public GraphicsPipeline(VulkanDevice device, VkPipeline pipeline, VkPipelineLayout pipelineLayout)
    {
        _device = device;
        _pipeline = pipeline;
        _pipelineLayout = pipelineLayout;
    }

    public VkPipelineBindPoint BindPoint => VkPipelineBindPoint.Graphics;

    public VkPipeline Pipeline => _pipeline;

    public VkPipelineLayout PipelineLayout => _pipelineLayout;


    public void Dispose()
    {
        _device.Api.vkDestroyPipeline(_pipeline);
    }
}