using Vortice.Vulkan;

namespace G2D;

internal unsafe class ImageManager : IDisposable
{
    public const uint MaxImageCount = 65536;

    private readonly VulkanDevice _device;

    private readonly VkDescriptorSetLayout _descriptorSetLayout;

    private readonly VkDescriptorSet _descriptorSet;

    public ImageManager(VulkanDevice device, VkDescriptorPool pool)
    {
        _device = device;

        _descriptorSetLayout = CreateDescriptorSetLayout(device);

        _descriptorSet = AllocateDescriptorSet(device, _descriptorSetLayout, pool);

        UpdateDescriptorSet(device, _descriptorSet);
    }

    public VkDescriptorSetLayout DescriptorSetLayout => _descriptorSetLayout;

    public VkDescriptorSet DescriptorSet => _descriptorSet;

    public void Dispose()
    {
        _device.Api.vkDestroyDescriptorSetLayout(_descriptorSetLayout);
    }

    private static VkDescriptorSetLayout CreateDescriptorSetLayout(VulkanDevice device)
    {
        var binding = new VkDescriptorSetLayoutBinding
        {
            binding = 0,
            descriptorType = VkDescriptorType.SampledImage,
            descriptorCount = 0,
            stageFlags = VkShaderStageFlags.Fragment
        };
        var flags = VkDescriptorBindingFlags.PartiallyBound | VkDescriptorBindingFlags.UpdateAfterBind | VkDescriptorBindingFlags.VariableDescriptorCount;
        var bindingFlags = new VkDescriptorSetLayoutBindingFlagsCreateInfo
        {
            bindingCount = 1,
            pBindingFlags = &flags
        };
        var setLayoutInfo = new VkDescriptorSetLayoutCreateInfo
        {
            flags = VkDescriptorSetLayoutCreateFlags.UpdateAfterBindPool,
            bindingCount = 1,
            pBindings = &binding,
            pNext = &bindingFlags
        };
        device.Api.vkCreateDescriptorSetLayout(&setLayoutInfo, out var descriptorSetLayout);

        return descriptorSetLayout;
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

    private static void UpdateDescriptorSet(VulkanDevice device, VkDescriptorSet descriptorSet)
    {
        // TODO   
    }
}