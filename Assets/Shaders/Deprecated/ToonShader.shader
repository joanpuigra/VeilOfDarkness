Shader "Custom/ToonShadowURP_MultiBand"
{
    Properties
    {
        _BaseColor ("Base Color", Color) = (1,1,1,1)
        _ShadowCutoff1 ("Shadow Cutoff 1", Range(0,1)) = 0.5
        _ShadowColor1 ("Shadow Color 1", Color) = (0.2,0.2,0.2,1)
        _ShadowColor2 ("Shadow Color 2", Color) = (0.0,0.0,0.0,1)
        _RampTex ("Ramp Texture", 2D) = "white" {}
        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Range(0.1,8.0)) = 4.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 200

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _SHADOWS_SOFT
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float3 normalWS : NORMAL;
                float4 shadowCoord : TEXCOORD0;
                float3 viewDirWS : TEXCOORD1;
            };

            float4 _BaseColor;
            float _ShadowCutoff1;
            float _ShadowCutoff2;
            float4 _ShadowColor1;
            float4 _ShadowColor2;
            float4 _RimColor;
            float _RimPower;
            sampler2D _RampTex;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS);
                OUT.normalWS = TransformObjectToWorldNormal(IN.normalOS);
                float3 worldPos = TransformObjectToWorld(IN.positionOS.xyz);
                OUT.shadowCoord = TransformWorldToShadowCoord(worldPos);
                OUT.viewDirWS = normalize(_WorldSpaceCameraPos - worldPos);
                return OUT;
            }

            half4 frag (Varyings IN) : SV_Target
            {
                Light mainLight = GetMainLight();
                float shadowAtten = MainLightRealtimeShadow(IN.shadowCoord);

                float NdotL = saturate(dot(IN.normalWS, mainLight.direction));
                float litValue = NdotL * shadowAtten;

                float rampSample = tex2D(_RampTex, float2(litValue, 0.5)).r;

                float3 finalColor = _BaseColor.rgb;

                if (rampSample < _ShadowCutoff1)
                {
                    finalColor = _ShadowColor2.rgb;
                }
                else if (rampSample < _ShadowCutoff2)
                {
                    finalColor = _ShadowColor1.rgb;
                }

                // Rim Light
                float rimFactor = pow(1.0 - saturate(dot(IN.normalWS, IN.viewDirWS)), _RimPower);
                finalColor += rimFactor * _RimColor.rgb;

                return float4(finalColor, 1.0);
            }

            ENDHLSL
        }
    }
}
