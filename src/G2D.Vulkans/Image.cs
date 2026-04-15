using Vortice.Vulkan;

namespace G2D;

public sealed class Image
{
    private readonly VkImage _image;

    private readonly VkImageView _view;

    public Image(VkImage image, VkImageView view)
    {
        _image = image;
        _view = view;
    }

    public static implicit operator VkImage(Image image)
    {
        return image._image;
    }

    public static implicit operator VkImageView(Image image)
    {
        return image._view;
    }
}