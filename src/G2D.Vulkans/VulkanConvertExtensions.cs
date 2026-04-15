using System.Text;
using Vortice.Vulkan;

namespace G2D;

public static class VulkanConvertExtensions
{
    public static VkVersion ToVkVersion(this Version? version)
    {
        return version == null ? new VkVersion(0) : new VkVersion((uint)version.Major, (uint)version.Minor, (uint)version.Build, (uint)version.Revision);
    }

    public static VkUtf8String ToVkUtf8String(this string value)
    {
        return Encoding.UTF8.GetBytes(value);
    }

    public static VkUtf8String[] ToVkUtf8StringArray(this string[] values)
    {
        return values.Select(s => s.ToVkUtf8String()).ToArray();
    }
}