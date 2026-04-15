using Vortice.Vulkan;

namespace G2D;

internal unsafe class SamplerCollection : IDisposable
{
    private readonly GraphicsDevice _device;

    private readonly VkSampler[] _samplers;

    private readonly VkDescriptorSet _descriptorSet;

    public SamplerCollection(GraphicsDevice device, VkDescriptorSet descriptorSet)
    {
        _device = device;
        _descriptorSet = descriptorSet;

        _samplers = CreateStaticSamplers(device);

        UpdateDescriptorSet(device, _descriptorSet, _samplers);
    }

    public VkSampler[] Samplers => _samplers;

    public VkDescriptorSet DescriptorSet => _descriptorSet;

    public void Dispose()
    {
        foreach (var sampler in _samplers)
        {
            _device.Api.vkDestroySampler(sampler);
        }
    }

    private static VkSampler[] CreateStaticSamplers(GraphicsDevice device)
    {
        var samplers = new VkSampler[8];

        // NearestRepeat
        var samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Nearest,
            minFilter = VkFilter.Nearest,
            addressModeU = VkSamplerAddressMode.Repeat,
            addressModeV = VkSamplerAddressMode.Repeat
        };
        device.Api.vkCreateSampler(&samplerInfo, out samplers[(int)Sampler.NearestRepeat]);

        // NearestMirrorRepeat
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Nearest,
            minFilter = VkFilter.Nearest,
            addressModeU = VkSamplerAddressMode.MirroredRepeat,
            addressModeV = VkSamplerAddressMode.MirroredRepeat
        };
        device.Api.vkCreateSampler(&samplerInfo, out samplers[(int)Sampler.NearestMirrorRepeat]);

        // NearestClamp
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Nearest,
            minFilter = VkFilter.Nearest,
            addressModeU = VkSamplerAddressMode.ClampToEdge,
            addressModeV = VkSamplerAddressMode.ClampToEdge
        };
        device.Api.vkCreateSampler(&samplerInfo, out samplers[(int)Sampler.NearestClamp]);

        // NearestClampBorder
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Nearest,
            minFilter = VkFilter.Nearest,
            addressModeU = VkSamplerAddressMode.ClampToBorder,
            addressModeV = VkSamplerAddressMode.ClampToBorder
        };
        device.Api.vkCreateSampler(&samplerInfo, out samplers[(int)Sampler.NearestClampBorder]);

        // LinearRepeat
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Linear,
            minFilter = VkFilter.Linear,
            addressModeU = VkSamplerAddressMode.Repeat,
            addressModeV = VkSamplerAddressMode.Repeat
        };
        device.Api.vkCreateSampler(&samplerInfo, out samplers[(int)Sampler.LinearRepeat]);

        // LinearMirrorRepeat
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Linear,
            minFilter = VkFilter.Linear,
            addressModeU = VkSamplerAddressMode.MirroredRepeat,
            addressModeV = VkSamplerAddressMode.MirroredRepeat
        };
        device.Api.vkCreateSampler(&samplerInfo, out samplers[(int)Sampler.LinearMirrorRepeat]);

        // LinearClamp
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Linear,
            minFilter = VkFilter.Linear,
            addressModeU = VkSamplerAddressMode.ClampToEdge,
            addressModeV = VkSamplerAddressMode.ClampToEdge
        };
        device.Api.vkCreateSampler(&samplerInfo, out samplers[(int)Sampler.LinearClamp]);

        // LinearMirrorClamp
        samplerInfo = new VkSamplerCreateInfo
        {
            magFilter = VkFilter.Linear,
            minFilter = VkFilter.Linear,
            addressModeU = VkSamplerAddressMode.ClampToBorder,
            addressModeV = VkSamplerAddressMode.ClampToBorder
        };
        device.Api.vkCreateSampler(&samplerInfo, out samplers[(int)Sampler.LinearClampBorder]);

        return samplers;
    }

    private static void UpdateDescriptorSet(GraphicsDevice device, VkDescriptorSet descriptorSet, ReadOnlySpan<VkSampler> samplers)
    {
        var infos = new VkDescriptorImageInfo[(uint)samplers.Length];

        for (var i = 0; i < samplers.Length; i++)
        {
            infos[i] = new VkDescriptorImageInfo
            {
                sampler = samplers[i]
            };
        }

        VkWriteDescriptorSet writes;
        fixed (VkDescriptorImageInfo* pImage = infos)
        {
            writes = new VkWriteDescriptorSet
            {
                dstSet = descriptorSet,
                dstBinding = 0,
                dstArrayElement = 0, // offset
                descriptorType = VkDescriptorType.Sampler,
                descriptorCount = (uint)samplers.Length,
                pImageInfo = pImage
            };
        }

        device.Api.vkUpdateDescriptorSets(1, &writes, 0, null);
    }

    public VkSampler GetSampler(Sampler sampler)
    {
        return _samplers[(int)sampler];
    }
}