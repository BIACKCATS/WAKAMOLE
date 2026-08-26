Shader "Custom/URP_OccludedOuterOutline"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OutlineColor ("Outline Color", Color) = (1, 1, 0, 1)
        _OutlineWidth ("Outline Width", Range(0.0, 0.05)) = 0.01
        _OutlineTex ("Outline Texture", 2D) = "white" {}
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            "RenderPipeline"="UniversalPipeline" 
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            Name "OuterOutlinePass"

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_OutlineTex);
            SAMPLER(sampler_OutlineTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _OutlineTex_ST; // Tiling & Offset 계산을 위해 추가
                float4 _OutlineColor;
                float _OutlineWidth;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionCS = TransformObjectToHClip(input.positionOS.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                half alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv).a;

                // 8방향 UV 오프셋으로 알파 영역 확장 (Outer Outline)
                half maxAlpha = alpha;
                float2 offsets[8] = {
                    float2(-1, 0), float2(1, 0), float2(0, -1), float2(0, 1),
                    float2(-0.707, -0.707), float2(0.707, -0.707),
                    float2(-0.707, 0.707), float2(0.707, 0.707)
                };

                for (int i = 0; i < 8; i++)
                {
                    float2 sampleUV = input.uv + offsets[i] * _OutlineWidth;
                    maxAlpha = max(maxAlpha, SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, sampleUV).a);
                }

                // 외곽선 영역(확장 영역 - 원본 영역) 알파 추출
                half outlineAlpha = saturate(maxAlpha - alpha);

                // 빗금 텍스처 UV에 Tiling 및 Offset 적용
                float2 outlineUV = input.uv * _OutlineTex_ST.xy + _OutlineTex_ST.zw;
                half4 fillTex = SAMPLE_TEXTURE2D(_OutlineTex, sampler_OutlineTex, outlineUV);

                // 텍스처 알파(A) 채널만 패턴 마스크로 사용 (선=1, 배경투명=0)
                half patternMask = fillTex.a;

                // 텍스처 검은색을 무시하고 _OutlineColor 지정 색상으로 채우기
                half4 finalColor = _OutlineColor;
                finalColor.a *= outlineAlpha * patternMask * input.color.a;

                return finalColor;
            }
            ENDHLSL
        }
    }
}