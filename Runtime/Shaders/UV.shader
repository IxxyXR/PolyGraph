﻿Shader "Hidden/Mixture/UV"
{	
	Properties
	{
		[MixtureVector3]_Scale("UV Scale", Vector) = (1.0,1.0,1.0,0.0)
		[MixtureVector3]_Bias("UV Bias", Vector) = (0.0,0.0,0.0,0.0)
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
			#pragma fragment mixture
			#pragma target 3.0

			#pragma multi_compile CRT_2D CRT_3D CRT_CUBE

			float4 _Scale;
			float4 _Bias;

			float4 mixture (v2f_customrendertexture IN) : SV_Target
			{
				return float4(IN.globalTexcoord.xyz * _Scale + _Bias, 1) ;
			}
			ENDCG
		}
	}
}
