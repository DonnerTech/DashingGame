Shader "FullScreen/Fullscreen Outline Effect"
{
    Properties
    {
        // This property is necessary to make the CommandBuffer.Blit bind the source texture to _MainTex
        _Scale("Scale", Float) = 1.0
        _DepthThreshold("Depth Threshold", Float) = 1.0
        _EdgeColor("Edge Color", Color) = (1.0, 1.0, 1.0, 1.0)
    }

    HLSLINCLUDE

    #pragma vertex Vert

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/RenderPipeline/RenderPass/CustomPass/CustomPassCommon.hlsl"

    // The PositionInputs struct allow you to retrieve a lot of useful information for your fullScreenShader:
    // struct PositionInputs
    // {
    //     float3 positionWS;  // World space position (could be camera-relative)
    //     float2 positionNDC; // Normalized screen coordinates within the viewport    : [0, 1) (with the half-pixel offset)
    //     uint2  positionSS;  // Screen space pixel coordinates                       : [0, NumPixels)
    //     uint2  tileCoord;   // Screen tile coordinates                              : [0, NumTiles)
    //     float  deviceDepth; // Depth from the depth buffer                          : [0, 1] (typically reversed)
    //     float  linearDepth; // View space Z coordinate                              : [Near, Far]
    // };

    // To sample custom buffers, you have access to these functions:
    // But be careful, on most platforms you can't sample to the bound color buffer. It means that you
    // can't use the SampleCustomColor when the pass color buffer is set to custom (and same for camera the buffer).
    // float4 CustomPassSampleCustomColor(float2 uv);
    // float4 CustomPassLoadCustomColor(uint2 pixelCoords);
    // float LoadCustomDepth(uint2 pixelCoords);
    // float SampleCustomDepth(float2 uv);

    // There are also a lot of utility function you can use inside Common.hlsl and Color.hlsl,
    // you can check them out in the source code of the core SRP package.

    float _Scale;
    float _DepthThreshold;
    float4 _EdgeColor;

    float4 FullScreenPass(Varyings varyings) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(varyings);
        float depth = LoadCameraDepth(varyings.positionCS.xy); // pixel coord to depth
        PositionInputs posInput = GetPositionInput(varyings.positionCS.xy, _ScreenSize.zw, depth, UNITY_MATRIX_I_VP, UNITY_MATRIX_V);
        float3 viewDirection = GetWorldSpaceNormalizeViewDir(posInput.positionWS);
        float4 color = float4(0.0, 0.0, 0.0, 0.0);

        // Load the camera color buffer at the mip 0 if we're not at the before rendering injection point
        if (_CustomPassInjectionPoint != CUSTOMPASSINJECTIONPOINT_BEFORE_RENDERING)
            color = float4(CustomPassLoadCameraColor(varyings.positionCS.xy, 0), 1);

        // Add your custom pass code here
        
        // info from learning:
        // color = float4(posInput.positionNDC, 0.0, 1.0); // raw uv coords
        // color = float4(posInput.positionSS * _ScreenSize.zw,0.0,1.0); // pixel to uv
        // color = float4(varyings.positionCS.xy  * _ScreenSize.zw, 0.0, 1.0); // pixel to uv

        float2 texelSize = float2(1.0, 1.0);

        // _Scale = 1;
        float halfScaleFloor = floor(_Scale * 0.5);
        float halfScaleCeil = ceil(_Scale * 0.5);

        // calculate the position of naboring pixels in an x patern
        float2 bottomLeftPixel = posInput.positionSS - float2(texelSize.x, texelSize.y) * halfScaleFloor;
        float2 topRightPixel = posInput.positionSS + float2(texelSize.x, texelSize.y) * halfScaleCeil;  
        float2 bottomRightPixel = posInput.positionSS + float2(texelSize.x * halfScaleCeil, -texelSize.y * halfScaleFloor);
        float2 topLeftPixel = posInput.positionSS + float2(-texelSize.x * halfScaleFloor, texelSize.y * halfScaleCeil);
        
        // load the depth data from the pixels
        float depth0 = LoadCameraDepth(bottomLeftPixel);
        float depth1 = LoadCameraDepth(topRightPixel);
        float depth2 = LoadCameraDepth(bottomRightPixel);
        float depth3 = LoadCameraDepth(topLeftPixel);
        
        //calculate the finite difference of the depth of the pixel accross from each other.
        float depthFiniteDifference0 = depth1 - depth0;
        float depthFiniteDifference1 = depth3 - depth2;

        // Roberts cross:
        // compute the sum of squares of the two differences
        float edgeDepth = sqrt(pow(depthFiniteDifference0, 2) + pow(depthFiniteDifference1, 2)) * 200;
        edgeDepth = edgeDepth > _DepthThreshold ? 1 : 0;
        return _EdgeColor * edgeDepth;
        // return float4(viewDirection, 1);

        
        // Fade value allow you to increase the strength of the effect while the camera gets closer to the custom pass volume
        //float f = 1 - abs(_FadeValue * 2 - 1);
        //return float4(color.rgb + f, color.a);
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "Custom Outline"

            ZWrite Off
            ZTest Always
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off

            HLSLPROGRAM
                #pragma fragment FullScreenPass
            ENDHLSL
        }
    }
    Fallback Off
}
