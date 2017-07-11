﻿Shader "Custom/LoadingScreenShader"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "black" {}
		_BlendingTex("Blending Texture", 2D) = "black" {}
		_Alpha ("Alpha Degree", Range(0.0,1.0)) = 0
	}
	SubShader
	{
		// No culling or depth
	    Tags{"Queue"="Transparent"}
    	Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BlendingTex;
			bool _UseBlendingTex;
			float _Alpha;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = 0;

				float2 temp = i.uv;
				temp.y = 1 - temp.y;
				col = tex2D (_BlendingTex, temp);	
						
				col.a = _Alpha;
				return col;
			}
			ENDCG
		}
	}
}