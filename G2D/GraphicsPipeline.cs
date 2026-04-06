using Vortice.Vulkan;

namespace G2D;

internal unsafe class GraphicsPipeline : IDisposable
{
    private static readonly VkDynamicState[] DynamicStates =
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

    private static readonly VkVertexInputBindingDescription[] VertexBindings =
    [
        ..Vertex.GetBindingDescriptions(),
        ..InstanceData.GetBindingDescriptions()
    ];

    private static readonly VkVertexInputAttributeDescription[] VertexAttributes =
    [
        ..Vertex.GetAttributeDescriptions(),
        ..InstanceData.GetAttributeDescriptions()
    ];

    private static readonly VkPipelineInputAssemblyStateCreateInfo InputAssembly = new()
    {
        topology = VkPrimitiveTopology.TriangleList,
        primitiveRestartEnable = false
    };

    private static readonly VkPipelineMultisampleStateCreateInfo NoneMultisample = new()
    {
        rasterizationSamples = VkSampleCountFlags.Count1,
        sampleShadingEnable = false,
        minSampleShading = 0.0f,
        pSampleMask = null,
        alphaToCoverageEnable = false,
        alphaToOneEnable = false
    };

    private static readonly VkPipelineMultisampleStateCreateInfo Msaa2XMultisample = new()
    {
        rasterizationSamples = VkSampleCountFlags.Count2,
        sampleShadingEnable = false,
        minSampleShading = 0.0f,
        pSampleMask = null,
        alphaToCoverageEnable = false,
        alphaToOneEnable = false
    };

    private static readonly VkPipelineMultisampleStateCreateInfo Msaa4XMultisample = new()
    {
        rasterizationSamples = VkSampleCountFlags.Count4,
        sampleShadingEnable = false,
        minSampleShading = 0.0f,
        pSampleMask = null,
        alphaToCoverageEnable = false,
        alphaToOneEnable = false
    };


    private static readonly VkPipelineColorBlendAttachmentState OpaqueColorBlend = new()
    {
        blendEnable = false,
        colorWriteMask = VkColorComponentFlags.All
    };

    private static readonly VkPipelineColorBlendAttachmentState PremultipliedColorBlend = new()
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

    private static readonly VkPipelineColorBlendAttachmentState AlphaBlendColorBlend = new()
    {
        blendEnable = true,
        srcColorBlendFactor = VkBlendFactor.SrcAlpha,
        dstColorBlendFactor = VkBlendFactor.OneMinusSrcAlpha,
        colorBlendOp = VkBlendOp.Add,
        srcAlphaBlendFactor = VkBlendFactor.SrcAlpha,
        dstAlphaBlendFactor = VkBlendFactor.OneMinusSrcAlpha,
        alphaBlendOp = VkBlendOp.Add,
        colorWriteMask = VkColorComponentFlags.All
    };

    private static readonly VkPipelineColorBlendAttachmentState AddBlendColorBlend = new()
    {
        blendEnable = true,
        srcColorBlendFactor = VkBlendFactor.One,
        dstColorBlendFactor = VkBlendFactor.One,
        colorBlendOp = VkBlendOp.Add,
        srcAlphaBlendFactor = VkBlendFactor.One,
        dstAlphaBlendFactor = VkBlendFactor.One,
        alphaBlendOp = VkBlendOp.Add,
        colorWriteMask = VkColorComponentFlags.All
    };

    private static readonly VkPipelineColorBlendAttachmentState SoftAddBlendColorBlend = new()
    {
        blendEnable = true,
        srcColorBlendFactor = VkBlendFactor.SrcAlpha,
        dstColorBlendFactor = VkBlendFactor.One,
        colorBlendOp = VkBlendOp.Add,
        srcAlphaBlendFactor = VkBlendFactor.SrcAlpha,
        dstAlphaBlendFactor = VkBlendFactor.One,
        alphaBlendOp = VkBlendOp.Add,
        colorWriteMask = VkColorComponentFlags.All
    };

    private static readonly VkPipelineColorBlendAttachmentState MultiplyBlendColorBlend = new()
    {
        blendEnable = true,
        srcColorBlendFactor = VkBlendFactor.DstColor,
        dstColorBlendFactor = VkBlendFactor.Zero,
        colorBlendOp = VkBlendOp.Add,
        srcAlphaBlendFactor = VkBlendFactor.DstAlpha,
        dstAlphaBlendFactor = VkBlendFactor.Zero,
        alphaBlendOp = VkBlendOp.Add,
        colorWriteMask = VkColorComponentFlags.All
    };

    private static readonly VkPipelineViewportStateCreateInfo ViewportState = new()
    {
        viewportCount = 1,
        scissorCount = 1
    };


    private static readonly VkPipelineRasterizationStateCreateInfo RasterizationState = new()
    {
        depthClampEnable = false,
        rasterizerDiscardEnable = false,
        polygonMode = VkPolygonMode.Fill,
        lineWidth = 1.0f,
        cullMode = VkCullModeFlags.None,
        frontFace = VkFrontFace.Clockwise,
        depthBiasEnable = false,
        depthBiasConstantFactor = 0,
        depthBiasClamp = 0,
        depthBiasSlopeFactor = 0
    };

    private static readonly VkDescriptorSetLayoutBinding[] DescriptorSetLayoutBindings =
    [
        new()
        {
            binding = 0,
            descriptorType = VkDescriptorType.UniformBuffer,
            descriptorCount = 1,
            stageFlags = VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment
        }
    ];

    private static readonly uint PushConstantRange = 128;

    protected readonly VulkanDevice _device;

    protected readonly VkDescriptorSetLayout _descriptorSetLayout;

    protected readonly VkPipelineLayout _pipelineLayout;

    protected readonly VkPipeline _pipeline;

    private GraphicsPipeline(VulkanDevice device, VkPipeline pipeline, VkDescriptorSetLayout descriptorSetLayout, VkPipelineLayout pipelineLayout)
    {
        _device = device;
        _pipeline = pipeline;
        _descriptorSetLayout = descriptorSetLayout;
        _pipelineLayout = pipelineLayout;
    }

    public VkPipelineBindPoint BindPoint => VkPipelineBindPoint.Graphics;

    public VkPipeline Pipeline => _pipeline;

    public VkPipelineLayout PipelineLayout => _pipelineLayout;

    public VkDescriptorSetLayout DescriptorSetLayout => _descriptorSetLayout;

    public void Dispose()
    {
        _device.Api.vkDestroyDescriptorSetLayout(_descriptorSetLayout);
        _device.Api.vkDestroyPipelineLayout(_pipelineLayout);
        _device.Api.vkDestroyPipeline(_pipeline);
    }

    public static VkShaderModule CreateShaderModule(VulkanDevice device, byte[] code)
    {
        fixed (byte* pCode = code)
        {
            device.Api.vkCreateShaderModule((nuint)code.Length, pCode, null, out var module)
                .CheckResult("failed to create shader module");

            return module;
        }
    }

    public static GraphicsPipeline Create(
        VulkanDevice device,
        VkFormat format,
        byte[] vertCode,
        byte[] fragCode,
        GraphicsPipelineMultisampleOption multisample = GraphicsPipelineMultisampleOption.None,
        GraphicsPipelineColorBlendOption colorBlend = GraphicsPipelineColorBlendOption.Premultiplied
    )
    {
        // Shader Stages
        var entryName = (VkUtf8String)"main"u8;
        var vertModule = CreateShaderModule(device, vertCode);
        var fragModule = CreateShaderModule(device, fragCode);

        var vertShaderStage = new VkPipelineShaderStageCreateInfo
        {
            stage = VkShaderStageFlags.Vertex,
            module = vertModule,
            pName = entryName
        };
        var fragShaderStage = new VkPipelineShaderStageCreateInfo
        {
            stage = VkShaderStageFlags.Fragment,
            module = fragModule,
            pName = entryName
        };
        var shaderStages = new[] { vertShaderStage, fragShaderStage };

        // Dynamic State
        VkPipelineDynamicStateCreateInfo dynamicState;
        fixed (VkDynamicState* pDynamicStates = DynamicStates)
        {
            dynamicState = new VkPipelineDynamicStateCreateInfo
            {
                dynamicStateCount = (uint)DynamicStates.Length,
                pDynamicStates = pDynamicStates
            };
        }

        // Vertex Input State
        VkPipelineVertexInputStateCreateInfo vertexInputState;

        fixed (VkVertexInputBindingDescription* pBindings = VertexBindings)
        fixed (VkVertexInputAttributeDescription* pAttributes = VertexAttributes)
        {
            vertexInputState = new VkPipelineVertexInputStateCreateInfo
            {
                vertexBindingDescriptionCount = (uint)VertexBindings.Length,
                pVertexBindingDescriptions = pBindings,
                vertexAttributeDescriptionCount = (uint)VertexAttributes.Length,
                pVertexAttributeDescriptions = pAttributes
            };
        }

        // Input Assembly State
        var inputAssemblyState = InputAssembly;

        // Viewport State
        var viewportState = ViewportState;

        // Rasterization State
        var rasterizationState = RasterizationState;

        // Multisample State
        var multisampleState = multisample switch
        {
            GraphicsPipelineMultisampleOption.None => NoneMultisample,
            GraphicsPipelineMultisampleOption.Msaa2X => Msaa2XMultisample,
            GraphicsPipelineMultisampleOption.Msaa4X => Msaa4XMultisample,
            _ => throw new ArgumentOutOfRangeException(nameof(multisample), multisample, null)
        };

        // Color Blend State
        VkPipelineColorBlendStateCreateInfo colorBlendStateInfo;
        fixed (VkPipelineColorBlendAttachmentState* pOpaqueColorBlend = &OpaqueColorBlend)
        fixed (VkPipelineColorBlendAttachmentState* pPremultipliedColorBlend = &PremultipliedColorBlend)
        fixed (VkPipelineColorBlendAttachmentState* pAlphaBlendColorBlend = &AlphaBlendColorBlend)
        fixed (VkPipelineColorBlendAttachmentState* pAddBlendColorBlend = &AddBlendColorBlend)
        fixed (VkPipelineColorBlendAttachmentState* pSoftAddBlendColorBlend = &SoftAddBlendColorBlend)
        fixed (VkPipelineColorBlendAttachmentState* pMultiplyBlendColorBlend = &MultiplyBlendColorBlend)
        {
            colorBlendStateInfo = new VkPipelineColorBlendStateCreateInfo
            {
                logicOpEnable = false,
                logicOp = VkLogicOp.Clear,
                attachmentCount = 1,
                pAttachments = colorBlend switch
                {
                    GraphicsPipelineColorBlendOption.Opaque => pOpaqueColorBlend,
                    GraphicsPipelineColorBlendOption.Premultiplied => pPremultipliedColorBlend,
                    GraphicsPipelineColorBlendOption.Alpha => pAlphaBlendColorBlend,
                    GraphicsPipelineColorBlendOption.Add => pAddBlendColorBlend,
                    GraphicsPipelineColorBlendOption.SoftAdd => pSoftAddBlendColorBlend,
                    GraphicsPipelineColorBlendOption.Multiply => pMultiplyBlendColorBlend,
                    _ => throw new ArgumentOutOfRangeException(nameof(colorBlend), colorBlend, null)
                }
            };
        }

        // Descriptor Layout
        VkDescriptorSetLayoutCreateInfo descriptorSetLayoutInfo;
        fixed (VkDescriptorSetLayoutBinding* pBindings = DescriptorSetLayoutBindings)
        {
            descriptorSetLayoutInfo = new VkDescriptorSetLayoutCreateInfo
            {
                bindingCount = (uint)DescriptorSetLayoutBindings.Length,
                pBindings = pBindings
            };
        }

        device.Api.vkCreateDescriptorSetLayout(&descriptorSetLayoutInfo, out var descriptorSetLayout);

        // Pipeline Layout
        VkDescriptorSetLayout[] setLayouts = [descriptorSetLayout];

        VkPushConstantRange[] pushConstantRanges =
        [
            new()
            {
                stageFlags = VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment,
                offset = 0,
                size = PushConstantRange
            }
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

        device.Api.vkCreatePipelineLayout(in pipelineLayoutInfo, out var pipelineLayout)
            .CheckResult("failed to create pipeline layout");

        // Rendering
        var renderingInfo = new VkPipelineRenderingCreateInfo
        {
            colorAttachmentCount = 1,
            pColorAttachmentFormats = &format
        };

        // Pipeline
        VkGraphicsPipelineCreateInfo pipelineInfo;
        fixed (VkPipelineShaderStageCreateInfo* pStageInfos = shaderStages)
        {
            pipelineInfo = new VkGraphicsPipelineCreateInfo
            {
                stageCount = (uint)shaderStages.Length,
                pStages = pStageInfos,
                pVertexInputState = &vertexInputState,
                pInputAssemblyState = &inputAssemblyState,
                pTessellationState = null,
                pViewportState = &viewportState,
                pRasterizationState = &rasterizationState,
                pMultisampleState = &multisampleState,
                pDepthStencilState = null,
                pColorBlendState = &colorBlendStateInfo,
                pDynamicState = &dynamicState,
                layout = pipelineLayout,
                pNext = &renderingInfo
            };
        }

        device.Api.vkCreateGraphicsPipeline(pipelineInfo, out var pipeline)
            .CheckResult("failed to create pipeline");

        device.Api.vkDestroyShaderModule(vertModule);

        device.Api.vkDestroyShaderModule(fragModule);

        return new GraphicsPipeline(device, pipeline, descriptorSetLayout, pipelineLayout);
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