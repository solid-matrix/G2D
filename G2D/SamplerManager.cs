using Vortice.Vulkan;

namespace G2D;

internal unsafe class SamplerManager : IDisposable
{
    public const uint SamplerCount = 8;

    private readonly VulkanDevice _device;

    private readonly VkSampler[] _samplers;

    public SamplerManager(VulkanDevice device)
    {
        _device = device;
        _samplers = new VkSampler[SamplerCount];

        // NearestRepeat
        var samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Nearest,
            minFilter = VkFilter.Nearest,
            addressModeU = VkSamplerAddressMode.Repeat,
            addressModeV = VkSamplerAddressMode.Repeat
        };
        _device.Api.vkCreateSampler(&samplerInfo, out _samplers[(int)Sampler.NearestRepeat]);

        // NearestMirrorRepeat
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Nearest,
            minFilter = VkFilter.Nearest,
            addressModeU = VkSamplerAddressMode.MirroredRepeat,
            addressModeV = VkSamplerAddressMode.MirroredRepeat
        };
        _device.Api.vkCreateSampler(&samplerInfo, out _samplers[(int)Sampler.NearestMirrorRepeat]);

        // NearestClamp
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Nearest,
            minFilter = VkFilter.Nearest,
            addressModeU = VkSamplerAddressMode.ClampToEdge,
            addressModeV = VkSamplerAddressMode.ClampToEdge
        };
        _device.Api.vkCreateSampler(&samplerInfo, out _samplers[(int)Sampler.NearestClamp]);

        // NearestClampBorder
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Nearest,
            minFilter = VkFilter.Nearest,
            addressModeU = VkSamplerAddressMode.ClampToBorder,
            addressModeV = VkSamplerAddressMode.ClampToBorder
        };
        _device.Api.vkCreateSampler(&samplerInfo, out _samplers[(int)Sampler.NearestClampBorder]);

        // LinearRepeat
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Linear,
            minFilter = VkFilter.Linear,
            addressModeU = VkSamplerAddressMode.Repeat,
            addressModeV = VkSamplerAddressMode.Repeat
        };
        _device.Api.vkCreateSampler(&samplerInfo, out _samplers[(int)Sampler.LinearRepeat]);

        // LinearMirrorRepeat
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Linear,
            minFilter = VkFilter.Linear,
            addressModeU = VkSamplerAddressMode.MirroredRepeat,
            addressModeV = VkSamplerAddressMode.MirroredRepeat
        };
        _device.Api.vkCreateSampler(&samplerInfo, out _samplers[(int)Sampler.LinearMirrorRepeat]);

        // LinearClamp
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Linear,
            minFilter = VkFilter.Linear,
            addressModeU = VkSamplerAddressMode.ClampToEdge,
            addressModeV = VkSamplerAddressMode.ClampToEdge
        };
        _device.Api.vkCreateSampler(&samplerInfo, out _samplers[(int)Sampler.LinearClamp]);

        // LinearMirrorClamp
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Linear,
            minFilter = VkFilter.Linear,
            addressModeU = VkSamplerAddressMode.ClampToBorder,
            addressModeV = VkSamplerAddressMode.ClampToBorder
        };
        _device.Api.vkCreateSampler(&samplerInfo, out _samplers[(int)Sampler.LinearClampBorder]);
    }

    public VkSampler[] Samplers => _samplers;

    public void Dispose()
    {
        foreach (var sampler in _samplers) _device.Api.vkDestroySampler(sampler);
    }

    public VkSampler GetSampler(Sampler sampler)
    {
        return _samplers[(int)sampler];
    }
}