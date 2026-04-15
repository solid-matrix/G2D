using Vortice.Vulkan;

namespace G2D;

public sealed class SwapchainFrame
{
    private readonly Swapchain _swapchain;

    private readonly uint _imageIndex;

    private readonly uint _index;

    internal SwapchainFrame(Swapchain swapchain, uint imageIndex, uint index)
    {
        _swapchain = swapchain;
        _imageIndex = imageIndex;
        _index = index;
    }

    internal uint ImageIndex => _imageIndex;

    public uint Index => _index;

    public CommandBuffer CommandBuffer => _swapchain.CommandBuffers[_index];

    public static implicit operator VkImage(SwapchainFrame frame)
    {
        return frame._swapchain.Images[frame._imageIndex];
    }

    public static implicit operator VkImageView(SwapchainFrame frame)
    {
        return frame._swapchain.ImageViews[frame._imageIndex];
    }
}