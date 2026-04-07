using Vortice.Vulkan;

namespace G2D;

internal unsafe class UniformBufferManager : IDisposable
{
    public const uint PerFrameUniformBufferCount = 1;

    private readonly VulkanDevice _device;

    private readonly VkDescriptorSetLayout _descriptorSetLayout;

    private readonly VulkanBufferSpanPool _uniformBufferSpanPool;

    private readonly VulkanBufferSpan[] _buffers;

    private readonly VkDescriptorSet[] _descriptorSets;

    public UniformBufferManager(VulkanDevice device, uint frameCount, VkDescriptorPool pool)
    {
        _device = device;

        _uniformBufferSpanPool = new VulkanBufferSpanPool(_device, VkBufferUsageFlags.UniformBuffer, VmaMemoryUsage.CpuToGpu);

        _buffers = new VulkanBufferSpan[frameCount];

        for (var i = 0; i < frameCount; i++)
            _buffers[i] = _uniformBufferSpanPool.Allocate((ulong)sizeof(Uniform));

        _descriptorSetLayout = CreateDescriptorSetLayout(device);

        _descriptorSets = AllocateDescriptorSets(device, _descriptorSetLayout, pool, frameCount);

        for (var i = 0; i < frameCount; i++)
            UpdateDescriptorSet(device, _descriptorSets[i], _buffers[i]);
    }

    public VkDescriptorSetLayout DescriptorSetLayout => _descriptorSetLayout;

    public VulkanBufferSpan[] Buffers => _buffers;

    public VkDescriptorSet[] DescriptorSets => _descriptorSets;

    public void Dispose()
    {
        _uniformBufferSpanPool.Dispose();
        _device.Api.vkDestroyDescriptorSetLayout(_descriptorSetLayout);
    }

    private static VkDescriptorSetLayout CreateDescriptorSetLayout(VulkanDevice device)
    {
        var uniformDescriptorSetLayoutBinding = new VkDescriptorSetLayoutBinding
        {
            binding = 0,
            descriptorType = VkDescriptorType.UniformBuffer,
            descriptorCount = 1,
            stageFlags = VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment
        };

        var uniformDescriptorSetLayoutInfo = new VkDescriptorSetLayoutCreateInfo
        {
            bindingCount = 1,
            pBindings = &uniformDescriptorSetLayoutBinding
        };

        device.Api.vkCreateDescriptorSetLayout(&uniformDescriptorSetLayoutInfo, out var ddescriptorSetLayout);

        return ddescriptorSetLayout;
    }

    private static VkDescriptorSet[] AllocateDescriptorSets(VulkanDevice device, VkDescriptorSetLayout layout, VkDescriptorPool pool, uint count)
    {
        var uniformDDescriptorSetLayouts = new VkDescriptorSetLayout[count];
        Array.Fill(uniformDDescriptorSetLayouts, layout);

        VkDescriptorSetAllocateInfo uniformDescriptorSetAllocateInfo;
        fixed (VkDescriptorSetLayout* pDescriptorSetLayouts = uniformDDescriptorSetLayouts)
        {
            uniformDescriptorSetAllocateInfo = new VkDescriptorSetAllocateInfo
            {
                descriptorPool = pool,
                descriptorSetCount = count,
                pSetLayouts = pDescriptorSetLayouts
            };
        }

        var descriptorSets = new VkDescriptorSet[count];
        fixed (VkDescriptorSet* pDescriptorSet = descriptorSets)
        {
            device.Api.vkAllocateDescriptorSets(&uniformDescriptorSetAllocateInfo, pDescriptorSet)
                .CheckResult("failed to allocate descriptor sets");
        }

        return descriptorSets;
    }

    private static void UpdateDescriptorSet(VulkanDevice device, VkDescriptorSet descriptorSet, VulkanBufferSpan buffer)
    {
        var uniformDescriptorBufferInfo = new VkDescriptorBufferInfo
        {
            buffer = buffer.Buffer,
            offset = buffer.Offset,
            range = buffer.Size
        };

        var uniformWriteDescriptorSet = new VkWriteDescriptorSet
        {
            dstSet = descriptorSet,
            dstBinding = 0,
            dstArrayElement = 0,
            descriptorType = VkDescriptorType.UniformBuffer,
            descriptorCount = 1,
            pBufferInfo = &uniformDescriptorBufferInfo
        };

        device.Api.vkUpdateDescriptorSets(1, &uniformWriteDescriptorSet, 0, null);
    }
}