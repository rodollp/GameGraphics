// Inspector에서 보일 Shader 메뉴 이름입니다.
Shader "GameGraphics/Day07 Outline Shell"
{
    // Material Inspector에 노출할 값입니다.
    Properties
    {
        // 외곽선 색상
        _OutlineColor ("Outline Color", Color) = (0.05, 0.02, 0.08, 1.0)

        // 메시를 Normal 방향으로 얼마나 부풀릴지 결정합니다.
        _OutlineWidth ("Outline Width", Float) = 0.03
    }


    SubShader
    {
        // URP에서 사용하는 불투명 Shader입니다.
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }


        Pass
        {
            Name "OutlineShell"

            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // 앞면을 그리지 않습니다.
            //
            // 부풀린 Mesh의 뒷면만 보이게 해서
            // 원본 Mesh 바깥으로 튀어나온 부분을
            // 외곽선처럼 사용합니다.
            Cull Front


            HLSLPROGRAM


            // Vertex Shader
            #pragma vertex vert

            // Fragment Shader
            #pragma fragment frag


            // TransformObjectToHClip 등의
            // URP 기본 함수를 사용하기 위해 포함합니다.
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"


            // Material마다 달라지는 값입니다.
            CBUFFER_START(UnityPerMaterial)

                half4 _OutlineColor;

                float _OutlineWidth;

            CBUFFER_END


            // =========================
            // Vertex Input
            // =========================

            struct Attributes
            {
                // Object Space의 정점 위치
                float4 positionOS : POSITION;

                // Object Space의 Normal 방향
                float3 normalOS : NORMAL;
            };


            // =========================
            // Vertex → Fragment
            // =========================

            struct Varyings
            {
                // 화면에 그릴 최종 Clip Space 위치
                float4 positionCS : SV_POSITION;
            };


            // =========================
            // Vertex Shader
            // =========================

            Varyings vert(Attributes input)
            {
                Varyings output;


                // 1.
                // Normal 방향 × Outline 두께
                //
                // Shader Graph의
                //
                // Normal Vector(Object)
                //          ↓
                // Multiply ← OutlineWidth
                //
                // 와 같은 계산입니다.

                float3 outlineOffset =
                    input.normalOS * _OutlineWidth;


                // 2.
                // 원래 정점 위치에 Offset을 더합니다.
                //
                // Shader Graph의
                //
                // Position(Object)
                //       +
                // outlineOffset
                //
                // 과 같습니다.

                float3 positionOS =
                    input.positionOS.xyz
                    + outlineOffset;


                // 3.
                // 이동된 Object Space 위치를
                // 화면에 그릴 Clip Space 위치로 변환합니다.

                output.positionCS =
                    TransformObjectToHClip(positionOS);


                return output;
            }


            // =========================
            // Fragment Shader
            // =========================

            half4 frag(Varyings input) : SV_Target
            {
                // 외곽선은 별도의 조명 계산 없이
                // Material에서 지정한 색을 그대로 사용합니다.

                return _OutlineColor;
            }


            ENDHLSL
        }
    }
}