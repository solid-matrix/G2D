using Vortice.Vulkan;

namespace G2D;

internal unsafe class GraphicsPipelineFactory
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
        ..VertexStruct.GetBindingDescriptions(),
        ..InstanceStruct.GetBindingDescriptions()
    ];

    private static readonly VkVertexInputAttributeDescription[] VertexAttributes =
    [
        ..VertexStruct.GetAttributeDescriptions(),
        ..InstanceStruct.GetAttributeDescriptions()
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

    private readonly VulkanDevice _device;

    private readonly VkPipelineLayout _pipelineLayout;

    private readonly VkFormat _format;

    public GraphicsPipelineFactory(VulkanDevice device, VkPipelineLayout pipelineLayout, VkFormat format)
    {
        _device = device;
        _pipelineLayout = pipelineLayout;
        _format = format;
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

    public GraphicsPipeline Create(byte[] vertCode,
        byte[] fragCode,
        GraphicsPipelineMultisampleOption multisample = GraphicsPipelineMultisampleOption.None,
        GraphicsPipelineColorBlendOption colorBlend = GraphicsPipelineColorBlendOption.Premultiplied)
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

        return new GraphicsPipeline(_device, pipeline, _pipelineLayout);
    }
}