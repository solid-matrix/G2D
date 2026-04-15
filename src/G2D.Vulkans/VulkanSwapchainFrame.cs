using Vortice.Vulkan;

namespace G2D;

public sealed class VulkanSwapchainFrame
{
    private readonly VulkanSwapchain _swapchain;

    private readonly uint _imageIndex;

    private readonly uint _index;

    internal VulkanSwapchainFrame(VulkanSwapchain swapchain, uint imageIndex, uint index)
    {
        _swapchain = swapchain;
        _imageIndex = imageIndex;
        _index = index;
    }

    public uint ImageIndex => _imageIndex;

    public uint Index => _index;

    public VulkanCommandBuffer CommandBuffer => _swapchain.CommandBuffers[_index];

    public static implicit operator VkImage(VulkanSwapchainFrame frame)
    {
        return frame._swapchain.Images[frame._imageIndex];
    }

    public static implicit operator VkImageView(VulkanSwapchainFrame frame)
    {
        return frame._swapchain.ImageViews[frame._imageIndex];
    }
}