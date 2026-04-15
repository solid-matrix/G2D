using Vortice.Vulkan;

namespace G2D;

internal unsafe class GraphicsLayout : IDisposable
{
    public const uint MaxUniformCount = 1;

    public const uint MaxSamplerCount = 8;

    public const uint MaxImageCount = 65536;

    public const uint PushConstantRange = 128;

    private static readonly VkVertexInputBindingDescription[] VertexBindings =
    [
        new(sizeof(VertexStruct), VkVertexInputRate.Vertex, 0),
        new(sizeof(InstanceStruct), VkVertexInputRate.Instance, 1)
    ];

    private static readonly VkVertexInputAttributeDescription[] VertexAttributes =
    [
        // Vertex Struct
        new(0, VkFormat.R32G32Sfloat, 0, 0), // Position
        new(1, VkFormat.R32G32Sfloat, 8, 0), // TexCoord 
        new(2, VkFormat.R32G32B32A32Sfloat, 16, 0), // Color

        // Instance Struct
        new(3, VkFormat.R32G32Sfloat, 0, 1), // ModelTransform Col0
        new(4, VkFormat.R32G32Sfloat, 8, 1), //
        new(5, VkFormat.R32G32Sfloat, 16, 1), // 
        new(6, VkFormat.R32G32B32A32Sfloat, 24, 1), // Color
        new(7, VkFormat.R32G32Sfloat, 40, 1), // TextureOffset
        new(8, VkFormat.R32G32Sfloat, 48, 1), // TextureScale
        new(9, VkFormat.R32Sfloat, 56, 1), // Layer
        new(10, VkFormat.R32Uint, 60, 1) // TextureSamplerIndex
    ];

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

    private static readonly VkPipelineColorBlendAttachmentState StraightAlphaBlendColorBlend = new()
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

    private readonly Device _device;

    private readonly VkFormat _format;

    private readonly VkDescriptorSetLayout _uniformDescriptorSetLayout;

    private readonly VkDescriptorSetLayout _textureDescriptorSetLayout;

    private readonly VkDescriptorSetLayout _samplerDescriptorSetLayout;

    private readonly VkPipelineLayout _pipelineLayout;

    private readonly List<VkPipeline> _pipelines;

    public GraphicsLayout(Device device, VkFormat swapchainFormat)
    {
        _device = device;
        _format = swapchainFormat;

        _uniformDescriptorSetLayout = CreateUniformDescriptorSetLayout(_device);
        _textureDescriptorSetLayout = CreateTextureDescriptorSetLayout(_device);
        _samplerDescriptorSetLayout = CreateSamplerDescriptorSetLayout(_device);

        _pipelineLayout = CreatePipelineLayout(_device, [
            _uniformDescriptorSetLayout,
            _textureDescriptorSetLayout,
            _samplerDescriptorSetLayout
        ]);

        _pipelines = [];
    }

    public VkPipelineLayout PipelineLayout => _pipelineLayout;

    public VkDescriptorSetLayout UniformDescriptorSetLayout => _uniformDescriptorSetLayout;

    public VkDescriptorSetLayout TextureDescriptorSetLayout => _textureDescriptorSetLayout;

    public VkDescriptorSetLayout SamplerDescriptorSetLayout => _samplerDescriptorSetLayout;

    public IReadOnlyList<VkPipeline> Pipelines => _pipelines;

    public void Dispose()
    {
        foreach (var pipeline in _pipelines)
        {
            _device.Api.vkDestroyPipeline(pipeline);
        }

        _device.Api.vkDestroyPipelineLayout(_pipelineLayout);
        _device.Api.vkDestroyDescriptorSetLayout(_uniformDescriptorSetLayout);
        _device.Api.vkDestroyDescriptorSetLayout(_textureDescriptorSetLayout);
        _device.Api.vkDestroyDescriptorSetLayout(_samplerDescriptorSetLayout);
    }

    public GraphicsShader CreateShader(
        byte[] vertCode,
        byte[] fragCode,
        GraphicsPipelineMultisampleOption multisample = GraphicsPipelineMultisampleOption.None,
        GraphicsPipelineColorBlendOption colorBlend = GraphicsPipelineColorBlendOption.Premultiplied
    )
    {
        // Shader Stages
        var entryName = (VkUtf8String)"main"u8;
        var vertModule = CreateShaderModule(_device, vertCode);
        var fragModule = CreateShaderModule(_device, fragCode);

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
        fixed (VkPipelineColorBlendAttachmentState* pStraightAlphaBlendColorBlend = &StraightAlphaBlendColorBlend)
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
                    GraphicsPipelineColorBlendOption.StraightAlpha => pStraightAlphaBlendColorBlend,
                    GraphicsPipelineColorBlendOption.Add => pAddBlendColorBlend,
                    GraphicsPipelineColorBlendOption.SoftAdd => pSoftAddBlendColorBlend,
                    GraphicsPipelineColorBlendOption.Multiply => pMultiplyBlendColorBlend,
                    _ => throw new ArgumentOutOfRangeException(nameof(colorBlend), colorBlend, null)
                }
            };
        }

        // Rendering
        var format = _format;
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
                layout = _pipelineLayout,
                pNext = &renderingInfo
            };
        }

        _device.Api.vkCreateGraphicsPipeline(pipelineInfo, out var pipeline)
            .CheckResult("failed to create pipeline");

        _device.Api.vkDestroyShaderModule(vertModule);

        _device.Api.vkDestroyShaderModule(fragModule);

        var shader = new GraphicsShader(this, _pipelines.Count);

        _pipelines.Add(pipeline);

        return shader;
    }

    public VkDescriptorSet[] AllocateUniformDescriptorSets(VkDescriptorPool pool, uint count)
    {
        var uniformDDescriptorSetLayouts = new VkDescriptorSetLayout[count];
        Array.Fill(uniformDDescriptorSetLayouts, _uniformDescriptorSetLayout);

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
            _device.Api.vkAllocateDescriptorSets(&uniformDescriptorSetAllocateInfo, pDescriptorSet)
                .CheckResult("failed to allocate descriptor sets");
        }

        return descriptorSets;
    }

    public VkDescriptorSet AllocateTextureDescriptorSet(VkDescriptorPool pool)
    {
        var layout = _textureDescriptorSetLayout;
        var allocate = new VkDescriptorSetAllocateInfo
        {
            descriptorPool = pool,
            descriptorSetCount = 1,
            pSetLayouts = &layout
        };

        VkDescriptorSet descriptorSet;

        _device.Api.vkAllocateDescriptorSets(&allocate, &descriptorSet);

        return descriptorSet;
    }

    public VkDescriptorSet AllocateSamplerDescriptorSet(VkDescriptorPool pool)
    {
        var layout = _samplerDescriptorSetLayout;

        var allocate = new VkDescriptorSetAllocateInfo
        {
            descriptorPool = pool,
            descriptorSetCount = 1,
            pSetLayouts = &layout
        };

        VkDescriptorSet descriptorSet;

        _device.Api.vkAllocateDescriptorSets(&allocate, &descriptorSet);

        return descriptorSet;
    }

    public static VkShaderModule CreateShaderModule(Device device, byte[] code)
    {
        fixed (byte* pCode = code)
        {
            device.Api.vkCreateShaderModule((nuint)code.Length, pCode, null, out var module)
                .CheckResult("failed to create shader module");

            return module;
        }
    }

    private static VkPipelineLayout CreatePipelineLayout(Device device, VkDescriptorSetLayout[] descriptorSetLayouts)
    {
        VkPushConstantRange[] pushConstantRanges =
        [
            new() { stageFlags = VkShaderStageFlags.Vertex | VkShaderStageFlags.Fragment, offset = 0, size = PushConstantRange }
        ];

        VkPipelineLayoutCreateInfo pipelineLayoutInfo;

        fixed (VkDescriptorSetLayout* pDescriptorSetLayout = descriptorSetLayouts)
        fixed (VkPushConstantRange* pPushConstantRanges = pushConstantRanges)
        {
            pipelineLayoutInfo = new VkPipelineLayoutCreateInfo
            {
                setLayoutCount = (uint)descriptorSetLayouts.Length,
                pSetLayouts = pDescriptorSetLayout,
                pushConstantRangeCount = (uint)pushConstantRanges.Length,
                pPushConstantRanges = pPushConstantRanges
            };
        }

        device.Api.vkCreatePipelineLayout(in pipelineLayoutInfo, out var pipelineLayout)
            .CheckResult("failed to create pipeline layout");

        return pipelineLayout;
    }

    private static VkDescriptorSetLayout CreateUniformDescriptorSetLayout(Device device)
    {
        var uniformDescriptorSetLayoutBinding = new VkDescriptorSetLayoutBinding
        {
            binding = 0,
            descriptorType = VkDescriptorType.UniformBuffer,
            descriptorCount = MaxUniformCount,
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

    private static VkDescriptorSetLayout CreateSamplerDescriptorSetLayout(Device device)
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

    private static VkDescriptorSetLayout CreateTextureDescriptorSetLayout(Device device)
    {
        var binding = new VkDescriptorSetLayoutBinding
        {
            binding = 0,
            descriptorType = VkDescriptorType.SampledImage,
            descriptorCount = MaxImageCount,
            stageFlags = VkShaderStageFlags.Fragment
        };
        var flags = VkDescriptorBindingFlags.PartiallyBound | VkDescriptorBindingFlags.UpdateAfterBind;
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
}