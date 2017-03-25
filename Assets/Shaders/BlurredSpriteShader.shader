Shader "Custom/CustomCharacterSpriteShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_BlurAmount ("Blur Amount", Range(1,20)) = 1
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
				float4 worldPos : TEXCOORD1;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.worldPos = IN.vertex;
				OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPos);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;

				return OUT;
			}

			sampler2D _MainTex;
			float _BlurAmount;
			float4 _MainTex_TexelSize;

			float4 box(sampler2D tex, float2 uv, float4 size)
			{
				float4 c = tex2D(tex, uv + float2(-size.x, size.y) * _BlurAmount) + tex2D(tex, uv + float2(0, size.y)* _BlurAmount ) + tex2D(tex, uv + float2(size.x, size.y) * _BlurAmount )+
							tex2D(tex, uv + float2(-size.x, 0) * _BlurAmount )+ tex2D(tex, uv + float2(0, 0) * _BlurAmount) + tex2D(tex, uv + float2(size.x, 0) * _BlurAmount )+
							tex2D(tex, uv + float2(-size.x, -size.y)* _BlurAmount ) + tex2D(tex, uv + float2(0, -size.y) * _BlurAmount )+ tex2D(tex, uv + float2(size.x, -size.y) * _BlurAmount) ;

				return c / 9;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = box(_MainTex, IN.texcoord, _MainTex_TexelSize) * IN.color ;
				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
