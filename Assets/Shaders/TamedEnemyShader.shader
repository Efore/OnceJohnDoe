﻿Shader "Custom/TamedEnemyShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0

		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_DistortionSpreader("Distortion Spreader", Float) = 30
		_DistortionDamper("Distortion Damper", Float) = 200
	}


	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 worldPosition : TEXCOORD1;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPosition = IN.vertex;
				OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPosition);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;


			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);

#if ETC1_EXTERNAL_ALPHA
				// get the color from an external texture (usecase: Alpha support for ETC1 on android)
				color.a = tex2D (_AlphaTex, uv).r;
#endif //ETC1_EXTERNAL_ALPHA

				return color;
			}

			sampler2D _NoiseTex;
			float _DistortionSpreader;
			float _DistortionDamper;

			fixed4 frag(v2f IN) : SV_Target
			{
				
				float2 offset = float2(
					tex2D(_NoiseTex, float2(IN.worldPosition.y / _DistortionSpreader + sin(_Time[1]), 0)).r,
					tex2D(_NoiseTex, float2(0, IN.worldPosition.x / _DistortionSpreader + sin(_Time[1]))).r
				);

				offset -= 0.5;

				fixed4 c = SampleSpriteTexture (IN.texcoord + offset/_DistortionDamper) * IN.color;
				c.rgb *= c.a;


				return c;
			}
		ENDCG
		}
	}
}
