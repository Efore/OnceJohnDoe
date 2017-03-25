Shader "Custom/BlurredSpriteShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_BlurAmount ("Blur amount",Range(0,1)) = 0
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

			float _BlurAmount;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (i.texcoord) * i.color;
				half4 texcol = half4(0,0,0,0);
			    float remaining=1.0f;
			    float coef=1.0;
			    float fI=0;
			    for (int j = 0; j < 3; j++) {
			    	fI++;
			    	coef*=0.32;
			    	texcol += tex2D(_MainTex, float2(i.texcoord.x, i.texcoord.y - fI * _BlurAmount)) * coef;
			    	texcol += tex2D(_MainTex, float2(i.texcoord.x - fI * _BlurAmount, i.texcoord.y)) * coef;
			    	texcol += tex2D(_MainTex, float2(i.texcoord.x + fI * _BlurAmount, i.texcoord.y)) * coef;
			    	texcol += tex2D(_MainTex, float2(i.texcoord.x, i.texcoord.y + fI * _BlurAmount)) * coef;
			    	
			    	remaining-=4*coef;
			    }
			    texcol += tex2D(_MainTex, float2(i.texcoord.x, i.texcoord.y)) * remaining;

				return texcol;
			}
		ENDCG
		}
	}
}
