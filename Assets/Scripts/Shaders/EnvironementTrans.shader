Shader "EnvironmentTransparency"
    {
        Properties
        {
            [NoScaleOffset]Texture2D_298e2d1c982d4a258981777827e0651b("BaseTexture", 2D) = "white" {}
            Vector1_f15c3ac65c844fd4813135c35339837f("Metallic", Float) = 0
            Vector1_439c8540ce3044f7b937f72b797852af("Smooth", Float) = 0
            Vector3_be9a2389ddea4e8f9ded27341aa1fbfe("LineStart", Vector) = (0, 0, 0, 0)
            Vector3_497cf597f45f4dd4a25ff67eff5dbed9("LineDir", Vector) = (0, 0, 0, 0)
            Vector1_2c8d4cfa56464ea8adaf68231170b93d("MinDist", Float) = 0
            Vector1_db3f2034e19149bcb22cc569bb7a0a91("MaxDist", Float) = 0
            [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
            [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
            [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
        }
        SubShader
        {
            Tags
            {
                "RenderPipeline"="UniversalPipeline"
                "RenderType"="Transparent"
                "UniversalMaterialType" = "Lit"
                "Queue"="Transparent"
            }
            Pass
            {
                Name "Universal Forward"
                Tags
                {
                    "LightMode" = "UniversalForward"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite On
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 4.5
                #pragma exclude_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma multi_compile_fog
                #pragma multi_compile _ DOTS_INSTANCING_ON
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
                #pragma multi_compile _ LIGHTMAP_ON
                #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
                #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
                #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
                #pragma multi_compile _ _SHADOWS_SOFT
                #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
                #pragma multi_compile _ SHADOWS_SHADOWMASK
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_NORMAL_WS
                #define VARYINGS_NEED_TANGENT_WS
                #define VARYINGS_NEED_TEXCOORD0
                #define VARYINGS_NEED_VIEWDIRECTION_WS
                #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_FORWARD
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    float4 uv1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float3 normalWS;
                    float4 tangentWS;
                    float4 texCoord0;
                    float3 viewDirectionWS;
                    #if defined(LIGHTMAP_ON)
                    float2 lightmapUV;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    float3 sh;
                    #endif
                    float4 fogFactorAndVertexLight;
                    float4 shadowCoord;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                    float3 WorldSpacePosition;
                    float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float3 interp1 : TEXCOORD1;
                    float4 interp2 : TEXCOORD2;
                    float4 interp3 : TEXCOORD3;
                    float3 interp4 : TEXCOORD4;
                    #if defined(LIGHTMAP_ON)
                    float2 interp5 : TEXCOORD5;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    float3 interp6 : TEXCOORD6;
                    #endif
                    float4 interp7 : TEXCOORD7;
                    float4 interp8 : TEXCOORD8;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    output.interp1.xyz =  input.normalWS;
                    output.interp2.xyzw =  input.tangentWS;
                    output.interp3.xyzw =  input.texCoord0;
                    output.interp4.xyz =  input.viewDirectionWS;
                    #if defined(LIGHTMAP_ON)
                    output.interp5.xy =  input.lightmapUV;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    output.interp6.xyz =  input.sh;
                    #endif
                    output.interp7.xyzw =  input.fogFactorAndVertexLight;
                    output.interp8.xyzw =  input.shadowCoord;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    output.normalWS = input.interp1.xyz;
                    output.tangentWS = input.interp2.xyzw;
                    output.texCoord0 = input.interp3.xyzw;
                    output.viewDirectionWS = input.interp4.xyz;
                    #if defined(LIGHTMAP_ON)
                    output.lightmapUV = input.interp5.xy;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    output.sh = input.interp6.xyz;
                    #endif
                    output.fogFactorAndVertexLight = input.interp7.xyzw;
                    output.shadowCoord = input.interp8.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_Sampler_3_Linear_Repeat);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float3 NormalTS;
                    float3 Emission;
                    float Metallic;
                    float Smoothness;
                    float Occlusion;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b, samplerTexture2D_298e2d1c982d4a258981777827e0651b, IN.uv0.xy);
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_R_4 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.r;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_G_5 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.g;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_B_6 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.b;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_A_7 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.a;
                    float _Property_cc953618e05e430787ee8d76f2e8f963_Out_0 = Vector1_f15c3ac65c844fd4813135c35339837f;
                    float _Property_90c12c52c854480a9eaecb63860ff333_Out_0 = Vector1_439c8540ce3044f7b937f72b797852af;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.BaseColor = (_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.xyz);
                    surface.NormalTS = IN.TangentSpaceNormal;
                    surface.Emission = float3(0, 0, 0);
                    surface.Metallic = _Property_cc953618e05e430787ee8d76f2e8f963_Out_0;
                    surface.Smoothness = _Property_90c12c52c854480a9eaecb63860ff333_Out_0;
                    surface.Occlusion = 1;
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                
                
                    output.WorldSpacePosition =          input.positionWS;
                    output.uv0 =                         input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "GBuffer"
                Tags
                {
                    "LightMode" = "UniversalGBuffer"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite Off
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 4.5
                #pragma exclude_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma multi_compile_fog
                #pragma multi_compile _ DOTS_INSTANCING_ON
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                #pragma multi_compile _ LIGHTMAP_ON
                #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
                #pragma multi_compile _ _SHADOWS_SOFT
                #pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
                #pragma multi_compile _ _GBUFFER_NORMALS_OCT
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_NORMAL_WS
                #define VARYINGS_NEED_TANGENT_WS
                #define VARYINGS_NEED_TEXCOORD0
                #define VARYINGS_NEED_VIEWDIRECTION_WS
                #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_GBUFFER
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    float4 uv1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float3 normalWS;
                    float4 tangentWS;
                    float4 texCoord0;
                    float3 viewDirectionWS;
                    #if defined(LIGHTMAP_ON)
                    float2 lightmapUV;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    float3 sh;
                    #endif
                    float4 fogFactorAndVertexLight;
                    float4 shadowCoord;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                    float3 WorldSpacePosition;
                    float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float3 interp1 : TEXCOORD1;
                    float4 interp2 : TEXCOORD2;
                    float4 interp3 : TEXCOORD3;
                    float3 interp4 : TEXCOORD4;
                    #if defined(LIGHTMAP_ON)
                    float2 interp5 : TEXCOORD5;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    float3 interp6 : TEXCOORD6;
                    #endif
                    float4 interp7 : TEXCOORD7;
                    float4 interp8 : TEXCOORD8;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    output.interp1.xyz =  input.normalWS;
                    output.interp2.xyzw =  input.tangentWS;
                    output.interp3.xyzw =  input.texCoord0;
                    output.interp4.xyz =  input.viewDirectionWS;
                    #if defined(LIGHTMAP_ON)
                    output.interp5.xy =  input.lightmapUV;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    output.interp6.xyz =  input.sh;
                    #endif
                    output.interp7.xyzw =  input.fogFactorAndVertexLight;
                    output.interp8.xyzw =  input.shadowCoord;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    output.normalWS = input.interp1.xyz;
                    output.tangentWS = input.interp2.xyzw;
                    output.texCoord0 = input.interp3.xyzw;
                    output.viewDirectionWS = input.interp4.xyz;
                    #if defined(LIGHTMAP_ON)
                    output.lightmapUV = input.interp5.xy;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    output.sh = input.interp6.xyz;
                    #endif
                    output.fogFactorAndVertexLight = input.interp7.xyzw;
                    output.shadowCoord = input.interp8.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_Sampler_3_Linear_Repeat);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float3 NormalTS;
                    float3 Emission;
                    float Metallic;
                    float Smoothness;
                    float Occlusion;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b, samplerTexture2D_298e2d1c982d4a258981777827e0651b, IN.uv0.xy);
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_R_4 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.r;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_G_5 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.g;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_B_6 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.b;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_A_7 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.a;
                    float _Property_cc953618e05e430787ee8d76f2e8f963_Out_0 = Vector1_f15c3ac65c844fd4813135c35339837f;
                    float _Property_90c12c52c854480a9eaecb63860ff333_Out_0 = Vector1_439c8540ce3044f7b937f72b797852af;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.BaseColor = (_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.xyz);
                    surface.NormalTS = IN.TangentSpaceNormal;
                    surface.Emission = float3(0, 0, 0);
                    surface.Metallic = _Property_cc953618e05e430787ee8d76f2e8f963_Out_0;
                    surface.Smoothness = _Property_90c12c52c854480a9eaecb63860ff333_Out_0;
                    surface.Occlusion = 1;
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                
                
                    output.WorldSpacePosition =          input.positionWS;
                    output.uv0 =                         input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/UnityGBuffer.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRGBufferPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "ShadowCaster"
                Tags
                {
                    "LightMode" = "ShadowCaster"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite On
                ColorMask 0
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 4.5
                #pragma exclude_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma multi_compile _ DOTS_INSTANCING_ON
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define VARYINGS_NEED_POSITION_WS
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_SHADOWCASTER
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 WorldSpacePosition;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                
                
                    output.WorldSpacePosition =          input.positionWS;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "DepthOnly"
                Tags
                {
                    "LightMode" = "DepthOnly"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite On
                ColorMask 0
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 4.5
                #pragma exclude_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma multi_compile _ DOTS_INSTANCING_ON
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define VARYINGS_NEED_POSITION_WS
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_DEPTHONLY
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 WorldSpacePosition;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                
                
                    output.WorldSpacePosition =          input.positionWS;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "DepthNormals"
                Tags
                {
                    "LightMode" = "DepthNormals"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite On
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 4.5
                #pragma exclude_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma multi_compile _ DOTS_INSTANCING_ON
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_NORMAL_WS
                #define VARYINGS_NEED_TANGENT_WS
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float3 normalWS;
                    float4 tangentWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                    float3 WorldSpacePosition;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float3 interp1 : TEXCOORD1;
                    float4 interp2 : TEXCOORD2;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    output.interp1.xyz =  input.normalWS;
                    output.interp2.xyzw =  input.tangentWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    output.normalWS = input.interp1.xyz;
                    output.tangentWS = input.interp2.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 NormalTS;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.NormalTS = IN.TangentSpaceNormal;
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                
                
                    output.WorldSpacePosition =          input.positionWS;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "Meta"
                Tags
                {
                    "LightMode" = "Meta"
                }
    
                // Render State
                Cull Off
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 4.5
                #pragma exclude_renderers gles gles3 glcore
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define ATTRIBUTES_NEED_TEXCOORD2
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_TEXCOORD0
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_META
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    float4 uv1 : TEXCOORD1;
                    float4 uv2 : TEXCOORD2;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float4 texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 WorldSpacePosition;
                    float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float4 interp1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    output.interp1.xyzw =  input.texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    output.texCoord0 = input.interp1.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_Sampler_3_Linear_Repeat);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float3 Emission;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b, samplerTexture2D_298e2d1c982d4a258981777827e0651b, IN.uv0.xy);
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_R_4 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.r;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_G_5 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.g;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_B_6 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.b;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_A_7 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.a;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.BaseColor = (_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.xyz);
                    surface.Emission = float3(0, 0, 0);
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                
                
                    output.WorldSpacePosition =          input.positionWS;
                    output.uv0 =                         input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                // Name: <None>
                Tags
                {
                    "LightMode" = "Universal2D"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite Off
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 4.5
                #pragma exclude_renderers gles gles3 glcore
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_TEXCOORD0
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_2D
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float4 texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 WorldSpacePosition;
                    float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float4 interp1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    output.interp1.xyzw =  input.texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    output.texCoord0 = input.interp1.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_Sampler_3_Linear_Repeat);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b, samplerTexture2D_298e2d1c982d4a258981777827e0651b, IN.uv0.xy);
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_R_4 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.r;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_G_5 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.g;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_B_6 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.b;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_A_7 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.a;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.BaseColor = (_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.xyz);
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                
                
                    output.WorldSpacePosition =          input.positionWS;
                    output.uv0 =                         input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"
    
                ENDHLSL
            }
        }
        SubShader
        {
            Tags
            {
                "RenderPipeline"="UniversalPipeline"
                "RenderType"="Transparent"
                "UniversalMaterialType" = "Lit"
                "Queue"="Transparent"
            }
            Pass
            {
                Name "Universal Forward"
                Tags
                {
                    "LightMode" = "UniversalForward"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite Off
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 2.0
                #pragma only_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma multi_compile_fog
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                #pragma multi_compile _ _SCREEN_SPACE_OCCLUSION
                #pragma multi_compile _ LIGHTMAP_ON
                #pragma multi_compile _ DIRLIGHTMAP_COMBINED
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
                #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
                #pragma multi_compile _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS _ADDITIONAL_OFF
                #pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
                #pragma multi_compile _ _SHADOWS_SOFT
                #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
                #pragma multi_compile _ SHADOWS_SHADOWMASK
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_NORMAL_WS
                #define VARYINGS_NEED_TANGENT_WS
                #define VARYINGS_NEED_TEXCOORD0
                #define VARYINGS_NEED_VIEWDIRECTION_WS
                #define VARYINGS_NEED_FOG_AND_VERTEX_LIGHT
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_FORWARD
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Shadows.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    float4 uv1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float3 normalWS;
                    float4 tangentWS;
                    float4 texCoord0;
                    float3 viewDirectionWS;
                    #if defined(LIGHTMAP_ON)
                    float2 lightmapUV;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    float3 sh;
                    #endif
                    float4 fogFactorAndVertexLight;
                    float4 shadowCoord;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                    float3 WorldSpacePosition;
                    float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float3 interp1 : TEXCOORD1;
                    float4 interp2 : TEXCOORD2;
                    float4 interp3 : TEXCOORD3;
                    float3 interp4 : TEXCOORD4;
                    #if defined(LIGHTMAP_ON)
                    float2 interp5 : TEXCOORD5;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    float3 interp6 : TEXCOORD6;
                    #endif
                    float4 interp7 : TEXCOORD7;
                    float4 interp8 : TEXCOORD8;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    output.interp1.xyz =  input.normalWS;
                    output.interp2.xyzw =  input.tangentWS;
                    output.interp3.xyzw =  input.texCoord0;
                    output.interp4.xyz =  input.viewDirectionWS;
                    #if defined(LIGHTMAP_ON)
                    output.interp5.xy =  input.lightmapUV;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    output.interp6.xyz =  input.sh;
                    #endif
                    output.interp7.xyzw =  input.fogFactorAndVertexLight;
                    output.interp8.xyzw =  input.shadowCoord;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    output.normalWS = input.interp1.xyz;
                    output.tangentWS = input.interp2.xyzw;
                    output.texCoord0 = input.interp3.xyzw;
                    output.viewDirectionWS = input.interp4.xyz;
                    #if defined(LIGHTMAP_ON)
                    output.lightmapUV = input.interp5.xy;
                    #endif
                    #if !defined(LIGHTMAP_ON)
                    output.sh = input.interp6.xyz;
                    #endif
                    output.fogFactorAndVertexLight = input.interp7.xyzw;
                    output.shadowCoord = input.interp8.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_Sampler_3_Linear_Repeat);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float3 NormalTS;
                    float3 Emission;
                    float Metallic;
                    float Smoothness;
                    float Occlusion;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b, samplerTexture2D_298e2d1c982d4a258981777827e0651b, IN.uv0.xy);
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_R_4 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.r;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_G_5 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.g;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_B_6 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.b;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_A_7 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.a;
                    float _Property_cc953618e05e430787ee8d76f2e8f963_Out_0 = Vector1_f15c3ac65c844fd4813135c35339837f;
                    float _Property_90c12c52c854480a9eaecb63860ff333_Out_0 = Vector1_439c8540ce3044f7b937f72b797852af;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.BaseColor = (_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.xyz);
                    surface.NormalTS = IN.TangentSpaceNormal;
                    surface.Emission = float3(0, 0, 0);
                    surface.Metallic = _Property_cc953618e05e430787ee8d76f2e8f963_Out_0;
                    surface.Smoothness = _Property_90c12c52c854480a9eaecb63860ff333_Out_0;
                    surface.Occlusion = 1;
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                
                
                    output.WorldSpacePosition =          input.positionWS;
                    output.uv0 =                         input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBRForwardPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "ShadowCaster"
                Tags
                {
                    "LightMode" = "ShadowCaster"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite On
                ColorMask 0
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 2.0
                #pragma only_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define VARYINGS_NEED_POSITION_WS
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_SHADOWCASTER
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 WorldSpacePosition;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                
                
                    output.WorldSpacePosition =          input.positionWS;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "DepthOnly"
                Tags
                {
                    "LightMode" = "DepthOnly"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite On
                ColorMask 0
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 2.0
                #pragma only_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define VARYINGS_NEED_POSITION_WS
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_DEPTHONLY
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 WorldSpacePosition;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                
                
                    output.WorldSpacePosition =          input.positionWS;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "DepthNormals"
                Tags
                {
                    "LightMode" = "DepthNormals"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite On
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 2.0
                #pragma only_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_NORMAL_WS
                #define VARYINGS_NEED_TANGENT_WS
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_DEPTHNORMALSONLY
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float3 normalWS;
                    float4 tangentWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 TangentSpaceNormal;
                    float3 WorldSpacePosition;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float3 interp1 : TEXCOORD1;
                    float4 interp2 : TEXCOORD2;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    output.interp1.xyz =  input.normalWS;
                    output.interp2.xyzw =  input.tangentWS;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    output.normalWS = input.interp1.xyz;
                    output.tangentWS = input.interp2.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 NormalTS;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.NormalTS = IN.TangentSpaceNormal;
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                    output.TangentSpaceNormal =          float3(0.0f, 0.0f, 1.0f);
                
                
                    output.WorldSpacePosition =          input.positionWS;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthNormalsOnlyPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                Name "Meta"
                Tags
                {
                    "LightMode" = "Meta"
                }
    
                // Render State
                Cull Off
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 2.0
                #pragma only_renderers gles gles3 glcore
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define ATTRIBUTES_NEED_TEXCOORD1
                #define ATTRIBUTES_NEED_TEXCOORD2
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_TEXCOORD0
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_META
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    float4 uv1 : TEXCOORD1;
                    float4 uv2 : TEXCOORD2;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float4 texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 WorldSpacePosition;
                    float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float4 interp1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    output.interp1.xyzw =  input.texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    output.texCoord0 = input.interp1.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_Sampler_3_Linear_Repeat);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float3 Emission;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b, samplerTexture2D_298e2d1c982d4a258981777827e0651b, IN.uv0.xy);
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_R_4 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.r;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_G_5 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.g;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_B_6 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.b;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_A_7 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.a;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.BaseColor = (_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.xyz);
                    surface.Emission = float3(0, 0, 0);
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                
                
                    output.WorldSpacePosition =          input.positionWS;
                    output.uv0 =                         input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/LightingMetaPass.hlsl"
    
                ENDHLSL
            }
            Pass
            {
                // Name: <None>
                Tags
                {
                    "LightMode" = "Universal2D"
                }
    
                // Render State
                Cull Back
                Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
                ZTest LEqual
                ZWrite Off
    
                // Debug
                // <None>
    
                // --------------------------------------------------
                // Pass
    
                HLSLPROGRAM
    
                // Pragmas
                #pragma target 2.0
                #pragma only_renderers gles gles3 glcore
                #pragma multi_compile_instancing
                #pragma vertex vert
                #pragma fragment frag
    
                // DotsInstancingOptions: <None>
                // HybridV1InjectedBuiltinProperties: <None>
    
                // Keywords
                // PassKeywords: <None>
                // GraphKeywords: <None>
    
                // Defines
                #define _SURFACE_TYPE_TRANSPARENT 1
                #define _NORMALMAP 1
                #define _NORMAL_DROPOFF_TS 1
                #define ATTRIBUTES_NEED_NORMAL
                #define ATTRIBUTES_NEED_TANGENT
                #define ATTRIBUTES_NEED_TEXCOORD0
                #define VARYINGS_NEED_POSITION_WS
                #define VARYINGS_NEED_TEXCOORD0
                #define FEATURES_GRAPH_VERTEX
                /* WARNING: $splice Could not find named fragment 'PassInstancing' */
                #define SHADERPASS SHADERPASS_2D
                /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */
    
                // Includes
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
                #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    
                // --------------------------------------------------
                // Structs and Packing
    
                struct Attributes
                {
                    float3 positionOS : POSITION;
                    float3 normalOS : NORMAL;
                    float4 tangentOS : TANGENT;
                    float4 uv0 : TEXCOORD0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : INSTANCEID_SEMANTIC;
                    #endif
                };
                struct Varyings
                {
                    float4 positionCS : SV_POSITION;
                    float3 positionWS;
                    float4 texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
                struct SurfaceDescriptionInputs
                {
                    float3 WorldSpacePosition;
                    float4 uv0;
                };
                struct VertexDescriptionInputs
                {
                    float3 ObjectSpaceNormal;
                    float3 ObjectSpaceTangent;
                    float3 ObjectSpacePosition;
                };
                struct PackedVaryings
                {
                    float4 positionCS : SV_POSITION;
                    float3 interp0 : TEXCOORD0;
                    float4 interp1 : TEXCOORD1;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    uint instanceID : CUSTOM_INSTANCE_ID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                    #endif
                };
    
                PackedVaryings PackVaryings (Varyings input)
                {
                    PackedVaryings output;
                    output.positionCS = input.positionCS;
                    output.interp0.xyz =  input.positionWS;
                    output.interp1.xyzw =  input.texCoord0;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
                Varyings UnpackVaryings (PackedVaryings input)
                {
                    Varyings output;
                    output.positionCS = input.positionCS;
                    output.positionWS = input.interp0.xyz;
                    output.texCoord0 = input.interp1.xyzw;
                    #if UNITY_ANY_INSTANCING_ENABLED
                    output.instanceID = input.instanceID;
                    #endif
                    #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                    output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                    #endif
                    #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                    output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                    #endif
                    #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                    output.cullFace = input.cullFace;
                    #endif
                    return output;
                }
    
                // --------------------------------------------------
                // Graph
    
                // Graph Properties
                CBUFFER_START(UnityPerMaterial)
                float4 Texture2D_298e2d1c982d4a258981777827e0651b_TexelSize;
                float Vector1_f15c3ac65c844fd4813135c35339837f;
                float Vector1_439c8540ce3044f7b937f72b797852af;
                float3 Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                float3 Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                float Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                float Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                CBUFFER_END
                
                // Object and Global properties
                TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(samplerTexture2D_298e2d1c982d4a258981777827e0651b);
                SAMPLER(_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_Sampler_3_Linear_Repeat);
    
                // Graph Functions
                
                // 33c4e9bd31e6c6a47a2434dda576cfa4
                #include "Assets/Scripts/Shaders/CustomFuncs.hlsl"
                
                void Unity_Distance_float3(float3 A, float3 B, out float Out)
                {
                    Out = distance(A, B);
                }
                
                void Unity_Comparison_Less_float(float A, float B, out float Out)
                {
                    Out = A < B ? 1 : 0;
                }
                
                void Unity_Comparison_Greater_float(float A, float B, out float Out)
                {
                    Out = A > B ? 1 : 0;
                }
                
                void Unity_Subtract_float(float A, float B, out float Out)
                {
                    Out = A - B;
                }
                
                void Unity_Divide_float(float A, float B, out float Out)
                {
                    Out = A / B;
                }
                
                void Unity_Branch_float(float Predicate, float True, float False, out float Out)
                {
                    Out = Predicate ? True : False;
                }
    
                // Graph Vertex
                struct VertexDescription
                {
                    float3 Position;
                    float3 Normal;
                    float3 Tangent;
                };
                
                VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
                {
                    VertexDescription description = (VertexDescription)0;
                    description.Position = IN.ObjectSpacePosition;
                    description.Normal = IN.ObjectSpaceNormal;
                    description.Tangent = IN.ObjectSpaceTangent;
                    return description;
                }
    
                // Graph Pixel
                struct SurfaceDescription
                {
                    float3 BaseColor;
                    float Alpha;
                };
                
                SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
                {
                    SurfaceDescription surface = (SurfaceDescription)0;
                    float4 _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0 = SAMPLE_TEXTURE2D(Texture2D_298e2d1c982d4a258981777827e0651b, samplerTexture2D_298e2d1c982d4a258981777827e0651b, IN.uv0.xy);
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_R_4 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.r;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_G_5 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.g;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_B_6 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.b;
                    float _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_A_7 = _SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.a;
                    float3 _Property_96304ab5e0294f04821f818bf7392860_Out_0 = Vector3_be9a2389ddea4e8f9ded27341aa1fbfe;
                    float3 _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0 = Vector3_497cf597f45f4dd4a25ff67eff5dbed9;
                    float3 _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2;
                    ClosestPointToLine_float(_Property_96304ab5e0294f04821f818bf7392860_Out_0, _Property_6f1b455e95ba48c5a52fc94388f27072_Out_0, IN.WorldSpacePosition, _CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2);
                    float _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2;
                    Unity_Distance_float3(_CustomFunction_599858691a434efa917097a0bc4c8aad_pntOnLine_2, IN.WorldSpacePosition, _Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2);
                    float _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2;
                    Unity_Comparison_Less_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_7c1c0fd3d769407891f0f4ac5ec487de_Out_0, _Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2);
                    float _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2;
                    Unity_Comparison_Greater_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Property_82f993afa4da4765b95ea05481bc3bb7_Out_0, _Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2);
                    float _Property_e057396f16db4015b9fdcde50d10fe8f_Out_0 = Vector1_db3f2034e19149bcb22cc569bb7a0a91;
                    float _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0 = Vector1_2c8d4cfa56464ea8adaf68231170b93d;
                    float _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2;
                    Unity_Subtract_float(_Property_e057396f16db4015b9fdcde50d10fe8f_Out_0, _Property_12fcb4810a1a42da93cf53cfef43f6c0_Out_0, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2);
                    float _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2;
                    Unity_Divide_float(_Distance_a4013584a0bc401da2efd95effa7c2c2_Out_2, _Subtract_4a96da37c12f407cbc602c785ebee65c_Out_2, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2);
                    float _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3;
                    Unity_Branch_float(_Comparison_044e8b98440e4f82ad04cbb83fe37547_Out_2, 1, _Divide_e4d3dca2c8964ee98aa5b8d2ac53fcae_Out_2, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3);
                    float _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    Unity_Branch_float(_Comparison_f4c7f9f7c7fc4f82bd198bd7b81e2ac4_Out_2, 0.4, _Branch_d2ad90d12ff648c1aa9b79ef51c206f5_Out_3, _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3);
                    surface.BaseColor = (_SampleTexture2D_5eeab90a2041405b84b9624b268fbdf7_RGBA_0.xyz);
                    surface.Alpha = _Branch_c8b91ae9d0be405db3f38ad3868ddc46_Out_3;
                    return surface;
                }
    
                // --------------------------------------------------
                // Build Graph Inputs
    
                VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
                {
                    VertexDescriptionInputs output;
                    ZERO_INITIALIZE(VertexDescriptionInputs, output);
                
                    output.ObjectSpaceNormal =           input.normalOS;
                    output.ObjectSpaceTangent =          input.tangentOS;
                    output.ObjectSpacePosition =         input.positionOS;
                
                    return output;
                }
                
                SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
                {
                    SurfaceDescriptionInputs output;
                    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
                
                
                
                
                
                    output.WorldSpacePosition =          input.positionWS;
                    output.uv0 =                         input.texCoord0;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
                #else
                #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                #endif
                #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
                
                    return output;
                }
                
    
                // --------------------------------------------------
                // Main
    
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
                #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/PBR2DPass.hlsl"
    
                ENDHLSL
            }
        }
        CustomEditor "ShaderGraph.PBRMasterGUI"
        FallBack "Hidden/Shader Graph/FallbackError"
    }
