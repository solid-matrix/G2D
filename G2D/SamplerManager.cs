using Vortice.Vulkan;

namespace G2D;

internal unsafe class SamplerManager : IDisposable
{
    public const uint MaxSamplerCount = 8;

    private readonly VulkanDevice _device;

    private readonly VkSampler[] _samplers;

    private readonly VkDescriptorSetLayout _descriptorSetLayout;

    private readonly VkDescriptorSet _descriptorSet;

    public SamplerManager(VulkanDevice device, VkDescriptorPool pool)
    {
        _device = device;

        _descriptorSetLayout = CreateDescriptorSetLayout(device);

        _descriptorSet = AllocateDescriptorSet(device, _descriptorSetLayout, pool);

        _samplers = CreateStaticSamplers(device);

        UpdateDescriptorSet(device, _descriptorSet, _samplers);
    }

    public VkSampler[] Samplers => _samplers;

    public VkDescriptorSetLayout DescriptorSetLayout => _descriptorSetLayout;

    public VkDescriptorSet DescriptorSet => _descriptorSet;

    public void Dispose()
    {
        foreach (var sampler in _samplers) _device.Api.vkDestroySampler(sampler);

        _device.Api.vkDestroyDescriptorSetLayout(_descriptorSetLayout);
    }

    private static VkSampler[] CreateStaticSamplers(VulkanDevice device)
    {
        var samplers = new VkSampler[MaxSamplerCount];

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

    private static VkDescriptorSetLayout CreateDescriptorSetLayout(VulkanDevice device)
    {
        var binding = new VkDescriptorSetLayoutBinding
        {
            binding = 0,
            descriptorType = VkDescriptorType.Sampler,
            descriptorCount = MaxSamplerCount,
            stageFlags = VkShaderStageFlags.Fragment
        };
        var setLayoutInfo = new VkDescriptorSetLayoutCreateInfo
        {
            bindingCount = 1,
            pBindings = &binding
        };

        device.Api.vkCreateDescriptorSetLayout(&setLayoutInfo, out var samplerDescriptorSetLayout);

        return samplerDescriptorSetLayout;
    }

    private static VkDescriptorSet AllocateDescriptorSet(VulkanDevice device, VkDescriptorSetLayout layout, VkDescriptorPool pool)
    {
        var allocate = new VkDescriptorSetAllocateInfo
        {
            descriptorPool = pool,
            descriptorSetCount = 1,
            pSetLayouts = &layout
        };

        VkDescriptorSet descriptorSet;

        device.Api.vkAllocateDescriptorSets(&allocate, &descriptorSet);

        return descriptorSet;
    }

    private static void UpdateDescriptorSet(VulkanDevice device, VkDescriptorSet descriptorSet, VkSampler[] samplers)
    {
        var images = new VkDescriptorImageInfo[MaxSamplerCount];

        for (var i = 0; i < MaxSamplerCount; i++)
            images[i] = new VkDescriptorImageInfo
            {
                sampler = samplers[i]
            };

        VkWriteDescriptorSet writes;
        fixed (VkDescriptorImageInfo* pImage = images)
        {
            writes = new VkWriteDescriptorSet
            {
                dstSet = descriptorSet,
                dstBinding = 0,
                dstArrayElement = 0, // offset
                descriptorType = VkDescriptorType.Sampler,
                descriptorCount = MaxSamplerCount,
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