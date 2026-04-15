using System.Runtime.CompilerServices;
using Vortice.Vulkan;

namespace G2D;

internal unsafe class UniformBuffer : IDisposable
{
    private readonly Device _device;

    private readonly VkDescriptorSet _descriptorSet;

    private readonly VkBuffer _buffer;

    private readonly VmaAllocation _allocation;

    private readonly void* _address;

    public UniformBuffer(Device device, VkDescriptorSet descriptorSet)
    {
        _device = device;

        _descriptorSet = descriptorSet;

        var bufferInfo = new VkBufferCreateInfo
        {
            size = (uint)sizeof(UniformStruct),
            usage = VkBufferUsageFlags.UniformBuffer,
            sharingMode = VkSharingMode.Exclusive
        };

        var allocInfo = new VmaAllocationCreateInfo
        {
            usage = VmaMemoryUsage.CpuToGpu,
            requiredFlags = VkMemoryPropertyFlags.HostVisible | VkMemoryPropertyFlags.HostCoherent,
            preferredFlags = VkMemoryPropertyFlags.HostCached
        };

        Vma.vmaCreateBuffer(_device.Allocator, &bufferInfo, &allocInfo, out _buffer, out _allocation, out _);

        UpdateDescriptorSet(device, _descriptorSet, _buffer);

        void* address;
        Vma.vmaMapMemory(_device.Allocator, _allocation, &address);
        _address = address;
    }

    public ref UniformStruct Data => ref Unsafe.AsRef<UniformStruct>(_address);

    public VkDescriptorSet DescriptorSet => _descriptorSet;

    public void Dispose()
    {
        Vma.vmaUnmapMemory(_device.Allocator, _allocation);
        Vma.vmaDestroyBuffer(_device.Allocator, _buffer, _allocation);
    }


    private static void UpdateDescriptorSet(Device device, VkDescriptorSet descriptorSet, VkBuffer buffer)
    {
        var uniformDescriptorBufferInfo = new VkDescriptorBufferInfo
        {
            buffer = buffer,
            offset = 0,
            range = (uint)sizeof(UniformStruct)
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