﻿Shader "Hidden/Mixture/UVDistort"
{	
	Properties
	{
		[InlineTexture] _Texture_2D("Distort Map", 2D) = "black" {}
		[InlineTexture]_UV_2D("UV", 2D) = "black" {}

		[InlineTexture]_Texture_3D("Distort Map", 3D) = "black" {}
		[InlineTexture]_UV_3D("UV", 3D) = "black" {}

		[InlineTexture]_Texture_Cube("Distort Map", Cube) = "black" {}
		[InlineTexture]_UV_Cube("Direction", Cube) = "black" {}

		[MixtureVector3]_Scale("Distort Scale", Vector) = (1.0,1.0,1.0,0.0)
		[MixtureVector3]_Bias("Distort Bias", Vector) = (0.0,0.0,0.0,0.0)
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#include "MixtureFixed.cginc"
			#pragma vertex CustomRenderTextureVertexShader
			#pragma fragment MixtureFragment
			#pragma target 3.0

			#pragma shader_feature CRT_2D CRT_3D CRT_CUBE
			#pragma shader_feature _ USE_CUSTOM_UV

			TEXTURE_SAMPLER_X(_Texture);
			TEXTURE_SAMPLER_X(_UV);
			float4 _Scale;
			float4 _Bias;

			float4 mixture (v2f_customrendertexture IN) : SV_Target
			{
#ifdef USE_CUSTOM_UV
				float3 uv = SAMPLE_X(_UV, IN.localTexcoord.xyz, IN.direction).rgb;
#else
				float3 uv = GetDefaultUVs(IN);
#endif

				// Scale and Bias does not works on cubemap
				uv += ScaleBias(SAMPLE_X(_Texture, IN.localTexcoord.xyz, IN.direction).rgb, _Scale.xyz, _Bias.xyz);

				return float4(uv.xy, 1, 1);

			}
			ENDCG
		}
	}
}
