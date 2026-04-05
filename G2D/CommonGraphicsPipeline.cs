using Vortice.Vulkan;

namespace G2D;

internal unsafe class CommonGraphicsPipeline : GraphicsPipeline, IDisposable
{
    private readonly VkDescriptorSetLayout _descriptorSetLayout;

    private readonly VulkanDevice _device;

    private readonly VkPipeline _pipeline;

    private readonly VkPipelineLayout _pipelineLayout;

    public CommonGraphicsPipeline(VulkanDevice device, VkFormat format)
    {
        _device = device;

        // Shader Stages
        VkShaderModule vertModule, fragModule;
        var vertCode = Game.InternalResource.GetBytes("Assets/Shaders/default.vert.spv");
        var fragCode = Game.InternalResource.GetBytes("Assets/Shaders/default.frag.spv");

        var vertEntry = (VkUtf8String)"main"u8;
        var fragEntry = (VkUtf8String)"main"u8;

        fixed (byte* pVertCode = vertCode)
        fixed (byte* pFragCode = fragCode)
        {
            _device.Api.vkCreateShaderModule((nuint)vertCode.Length, pVertCode, null, out vertModule)
                .CheckResult("failed to create shader module");
            _device.Api.vkCreateShaderModule((nuint)fragCode.Length, pFragCode, null, out fragModule)
                .CheckResult("failed to create shader module");
        }

        var vertShaderStageInfo = new VkPipelineShaderStageCreateInfo
        {
            stage = VkShaderStageFlags.Vertex,
            module = vertModule,
            pName = vertEntry
        };

        var fragShaderStageInfo = new VkPipelineShaderStageCreateInfo
        {
            stage = VkShaderStageFlags.Fragment,
            module = fragModule,
            pName = fragEntry
        };

        var shaderStageInfos = stackalloc VkPipelineShaderStageCreateInfo[2]
        {
            vertShaderStageInfo, fragShaderStageInfo
        };

        // Dynamic States
        VkDynamicState[] dynamicStates =
        [
            VkDynamicState.Viewport,
            VkDynamicState.Scissor
            // VkDynamicState.FrontFace
            // VkDynamicState.CullMode,
            // VkDynamicState.DepthTestEnable,
            // VkDynamicState.DepthWriteEnable,
            // VkDynamicState.StencilTestEnable,
            // VkDynamicState.DepthBias,
            // VkDynamicState.LineWidth
        ];
        VkPipelineDynamicStateCreateInfo dynamicStateInfo;
        fixed (VkDynamicState* pDynamicStates = dynamicStates)
        {
            dynamicStateInfo = new VkPipelineDynamicStateCreateInfo
            {
                dynamicStateCount = (uint)dynamicStates.Length,
                pDynamicStates = pDynamicStates
            };
        }

        // Vertex Input
        var vertexBindings = Common.GetBindingDescriptions();

        var vertexAttributes = Common.GetAttributeDescriptions();

        VkPipelineVertexInputStateCreateInfo vertexInputInfo;

        fixed (VkVertexInputBindingDescription* pBindings = vertexBindings)
        fixed (VkVertexInputAttributeDescription* pAttributes = vertexAttributes)
        {
            vertexInputInfo = new VkPipelineVertexInputStateCreateInfo
            {
                vertexBindingDescriptionCount = (uint)vertexBindings.Length,
                pVertexBindingDescriptions = pBindings,
                vertexAttributeDescriptionCount = (uint)vertexAttributes.Length,
                pVertexAttributeDescriptions = pAttributes
            };
        }

        // Input Assembly
        var inputAssemblyInfo = new VkPipelineInputAssemblyStateCreateInfo
        {
            topology = VkPrimitiveTopology.TriangleList,
            primitiveRestartEnable = false
        };

        // Viewports and scissors
        var viewportStateInfo = new VkPipelineViewportStateCreateInfo
        {
            viewportCount = 1,
            scissorCount = 1
        };

        // Rasterizer
        var rasterizationStateInfo = new VkPipelineRasterizationStateCreateInfo
        {
            depthClampEnable = false,
            rasterizerDiscardEnable = false,
            polygonMode = VkPolygonMode.Fill,
            lineWidth = 1.0f,
            cullMode = VkCullModeFlags.Back,
            frontFace = VkFrontFace.Clockwise,
            depthBiasEnable = false,
            depthBiasConstantFactor = 0,
            depthBiasClamp = 0,
            depthBiasSlopeFactor = 0
        };

        var multisampleStateInfo = new VkPipelineMultisampleStateCreateInfo
        {
            sampleShadingEnable = false,
            rasterizationSamples = VkSampleCountFlags.Count1,
            minSampleShading = 1,
            pSampleMask = null,
            alphaToCoverageEnable = false,
            alphaToOneEnable = false
        };

        // Color Blend
        var colorBlendAttachmentStateInfo = new VkPipelineColorBlendAttachmentState
        {
            blendEnable = true,
            srcColorBlendFactor = VkBlendFactor.One,
            dstColorBlendFactor = VkBlendFactor.OneMinusSrcAlpha,
            colorBlendOp = VkBlendOp.Add,
            srcAlphaBlendFactor = VkBlendFactor.One,
            dstAlphaBlendFactor = VkBlendFactor.OneMinusSrcAlpha,
            alphaBlendOp = VkBlendOp.Add,
            colorWriteMask = VkColorComponentFlags.All
        };

        var colorBlendStateInfo = new VkPipelineColorBlendStateCreateInfo
        {
            logicOpEnable = false,
            logicOp = VkLogicOp.And,
            attachmentCount = 1,
            pAttachments = &colorBlendAttachmentStateInfo
        };

        // Pipeline Layout
        VkDescriptorSetLayoutBinding[] descriptorSetLayoutBindings =
        [
            new()
            {
                binding = 0,
                descriptorType = VkDescriptorType.UniformBuffer,
                descriptorCount = 1,
                stageFlags = VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment
            }
        ];

        VkDescriptorSetLayoutCreateInfo descriptorSetLayoutInfo;
        fixed (VkDescriptorSetLayoutBinding* pBindings = descriptorSetLayoutBindings)
        {
            descriptorSetLayoutInfo = new VkDescriptorSetLayoutCreateInfo
            {
                bindingCount = (uint)descriptorSetLayoutBindings.Length,
                pBindings = pBindings
            };
        }

        _device.Api.vkCreateDescriptorSetLayout(&descriptorSetLayoutInfo, out _descriptorSetLayout);

        VkDescriptorSetLayout[] setLayouts =
        [
            _descriptorSetLayout
        ];

        VkPushConstantRange[] pushConstantRanges =
        [
            // new()
            // {
            //     stageFlags = VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment,
            //     offset = 0,
            //     size = 0
            // }
        ];

        VkPipelineLayoutCreateInfo pipelineLayoutInfo;
        fixed (VkDescriptorSetLayout* pDescriptorSetLayout = setLayouts)
        fixed (VkPushConstantRange* pPushConstantRanges = pushConstantRanges)
        {
            pipelineLayoutInfo = new VkPipelineLayoutCreateInfo
            {
                setLayoutCount = (uint)setLayouts.Length,
                pSetLayouts = pDescriptorSetLayout,
                pushConstantRangeCount = (uint)pushConstantRanges.Length,
                pPushConstantRanges = pPushConstantRanges
            };
        }

        _device.Api.vkCreatePipelineLayout(in pipelineLayoutInfo, out _pipelineLayout)
            .CheckResult("failed to create pipelineLayout");

        // Rendering
        var renderingInfo = new VkPipelineRenderingCreateInfo
        {
            colorAttachmentCount = 1,
            pColorAttachmentFormats = &format
        };

        // Pipeline

        var pipelineInfo = new VkGraphicsPipelineCreateInfo
        {
            stageCount = 2,
            pStages = shaderStageInfos,
            pVertexInputState = &vertexInputInfo,
            pInputAssemblyState = &inputAssemblyInfo,
            pTessellationState = null,
            pViewportState = &viewportStateInfo,
            pRasterizationState = &rasterizationStateInfo,
            pMultisampleState = &multisampleStateInfo,
            pDepthStencilState = null,
            pColorBlendState = &colorBlendStateInfo,
            pDynamicState = &dynamicStateInfo,
            layout = _pipelineLayout,
            pNext = &renderingInfo
        };

        _device.Api.vkCreateGraphicsPipeline(pipelineInfo, out _pipeline)
            .CheckResult("failed to create pipeline");

        _device.Api.vkDestroyShaderModule(vertModule);
        _device.Api.vkDestroyShaderModule(fragModule);
    }

    public override VkPipelineBindPoint BindPoint => VkPipelineBindPoint.Graphics;

    public override VkPipeline Pipeline => _pipeline;

    public override VkPipelineLayout PipelineLayout => _pipelineLayout;

    public void Dispose()
    {
        _device.Api.vkDestroyDescriptorSetLayout(_descriptorSetLayout);
        _device.Api.vkDestroyPipelineLayout(_pipelineLayout);
        _device.Api.vkDestroyPipeline(_pipeline);
    }

    public VkDescriptorSet[] CreateDescriptorSets(VkDescriptorPool pool, ReadOnlySpan<VulkanBufferSpan> buffers)
    {
        var count = (uint)buffers.Length;

        var descriptorSetLayouts = new VkDescriptorSetLayout[count];
        Array.Fill(descriptorSetLayouts, _descriptorSetLayout);

        VkDescriptorSetAllocateInfo descriptorSetAllocateInfo;
        fixed (VkDescriptorSetLayout* pDescriptorSetLayouts = descriptorSetLayouts)
        {
            descriptorSetAllocateInfo = new VkDescriptorSetAllocateInfo
            {
                descriptorPool = pool,
                descriptorSetCount = count,
                pSetLayouts = pDescriptorSetLayouts
            };
        }

        var descriptorSets = new VkDescriptorSet[count];
        fixed (VkDescriptorSet* pDescriptorSet = descriptorSets)
        {
            _device.Api.vkAllocateDescriptorSets(&descriptorSetAllocateInfo, pDescriptorSet)
                .CheckResult("failed to allocate descriptor sets");
        }

        for (var i = 0; i < count; i++)
        {
            var bufferInfo = new VkDescriptorBufferInfo
            {
                buffer = buffers[i].Buffer,
                offset = buffers[i].Offset,
                range = buffers[i].Size
            };

            var descriptorWrites = new VkWriteDescriptorSet
            {
                dstSet = descriptorSets[i],
                dstBinding = 0,
                dstArrayElement = 0,
                descriptorType = VkDescriptorType.UniformBuffer,
                descriptorCount = 1,
                pBufferInfo = &bufferInfo
            };

            _device.Api.vkUpdateDescriptorSets(1, &descriptorWrites, 0, null);
        }

        return descriptorSets;
    }
}