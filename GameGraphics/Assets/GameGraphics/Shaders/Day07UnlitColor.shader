Shader "GameGraphics/Day07 Unlit Sun"
{
    Properties
    {
        _DarkColor ("Dark Sun Color", Color) = (0.3, 0.01, 0.0, 1.0)
        _MidColor ("Mid Sun Color", Color) = (1.0, 0.15, 0.0, 1.0)
        _BrightColor ("Bright Sun Color", Color) = (1.0, 0.9, 0.2, 1.0)

        _NoiseScale ("Noise Scale", Float) = 5.0
        _FlowSpeed ("Flow Speed", Float) = 0.3
        _Contrast ("Contrast", Float) = 2.0

        _PulseSpeed ("Pulse Speed", Float) = 2.0
        _PulseAmount ("Pulse Amount", Float) = 0.03
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        Pass
        {
            Name "ForwardUnlit"

            Tags
            {
                "LightMode" = "UniversalForward"
            }

            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            CBUFFER_START(UnityPerMaterial)

                half4 _DarkColor;
                half4 _MidColor;
                half4 _BrightColor;

                float _NoiseScale;
                float _FlowSpeed;
                float _Contrast;

                float _PulseSpeed;
                float _PulseAmount;

            CBUFFER_END


            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };


            struct Varyings
            {
                float4 positionCS : SV_POSITION;

                float3 positionOS : TEXCOORD0;
                float3 normalOS : TEXCOORD1;
            };


            float Random(float3 p)
            {
                return frac(
                    sin(
                        dot(
                            p,
                            float3(12.9898, 78.233, 37.719)
                        )
                    )
                    * 43758.5453
                );
            }


            float Noise(float3 p)
            {
                float3 cell = floor(p);
                float3 local = frac(p);

                local =
                    local * local * (3.0 - 2.0 * local);

                float n000 = Random(cell + float3(0, 0, 0));
                float n100 = Random(cell + float3(1, 0, 0));
                float n010 = Random(cell + float3(0, 1, 0));
                float n110 = Random(cell + float3(1, 1, 0));

                float n001 = Random(cell + float3(0, 0, 1));
                float n101 = Random(cell + float3(1, 0, 1));
                float n011 = Random(cell + float3(0, 1, 1));
                float n111 = Random(cell + float3(1, 1, 1));

                float x00 = lerp(n000, n100, local.x);
                float x10 = lerp(n010, n110, local.x);
                float x01 = lerp(n001, n101, local.x);
                float x11 = lerp(n011, n111, local.x);

                float y0 = lerp(x00, x10, local.y);
                float y1 = lerp(x01, x11, local.y);

                return lerp(y0, y1, local.z);
            }


            Varyings vert(Attributes input)
            {
                Varyings output;

                float3 position = input.positionOS.xyz;

                float3 noisePos =
                    normalize(input.positionOS.xyz) * _NoiseScale;

                float surfaceNoise =
                    Noise(
                        noisePos
                        + float3(
                            _Time.y * _FlowSpeed,
                            _Time.y * (_FlowSpeed * 0.7),
                            _Time.y * (_FlowSpeed * 0.5)
                        )
                    );

                // 0~1 → -0.5~0.5
                surfaceNoise -= 0.5;

                // 표면 일부만 불규칙하게 돌출
                position +=
                    normalize(input.normalOS)
                    * surfaceNoise
                    * _PulseAmount;

                output.positionCS =
                    TransformObjectToHClip(position);

                output.positionOS =
                    input.positionOS.xyz;

                output.normalOS =
                    input.normalOS;

                return output;
            }


            half4 frag(Varyings input) : SV_Target
            {
                float3 pos =
                    normalize(input.positionOS)
                    * _NoiseScale;


                // 시간에 따라 패턴 이동
                float3 flow =
                    float3(
                        _Time.y * _FlowSpeed,
                        _Time.y * (_FlowSpeed * 0.7),
                        _Time.y * (_FlowSpeed * 0.5)
                    );


                // 큰 무늬
                float noise1 =
                    Noise(pos + flow);


                // 작은 무늬
                float noise2 =
                    Noise(
                        pos * 2.0
                        - flow * 1.5
                    );


                // 두 패턴 섞기
                float sunPattern =
                    noise1 * 0.65
                    +
                    noise2 * 0.35;


                // 대비 강화
                sunPattern =
                    saturate(
                        (sunPattern - 0.5)
                        * _Contrast
                        +
                        0.5
                    );


                // 어두운 빨강 → 주황
                half4 color1 =
                    lerp(
                        _DarkColor,
                        _MidColor,
                        sunPattern
                    );


                // 밝은 부분을 추가
                float hotArea =
                    smoothstep(
                        0.55,
                        0.85,
                        sunPattern
                    );


                half4 finalColor =
                    lerp(
                        color1,
                        _BrightColor,
                        hotArea
                    );


                return finalColor;
            }

            ENDHLSL
        }
    }
}