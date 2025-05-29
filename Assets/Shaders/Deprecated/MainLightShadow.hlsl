#ifndef MAIN_LIGHT_SHADOW_INCLUDED
#define MAIN_LIGHT_SHADOW_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

void SampleMainLightShadow_float(float3 worldPos, out float shadow)
{
    float4 shadowCoord = TransformWorldToShadowCoord(worldPos);
    shadow = MainLightRealtimeShadow(shadowCoord);
}

#endif
