Shader "Custom/BlurredSpriteShader"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_blurSizeXY("BlurSizeXY", Range(0,20)) = 0
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

			sampler2D _GrabTexture : register(s0);
            float _blurSizeXY;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (i.texcoord) * i.color;
				float2 screenPos = i.vertex.xy;
				float depth= _blurSizeXY*0.0005;

			    screenPos.x = (screenPos.x + 1) * 0.5;

			    screenPos.y = 1-(screenPos.y + 1) * 0.5;

			    half4 sum = half4(0.0h,0.0h,0.0h,0.0h);   
			    sum += tex2D( _GrabTexture, float2(screenPos.x-5.0 * depth, screenPos.y+5.0 * depth)) * 0.025;    
			    sum += tex2D( _GrabTexture, float2(screenPos.x+5.0 * depth, screenPos.y-5.0 * depth)) * 0.025;
			    
			    sum += tex2D( _GrabTexture, float2(screenPos.x-4.0 * depth, screenPos.y+4.0 * depth)) * 0.05;
			    sum += tex2D( _GrabTexture, float2(screenPos.x+4.0 * depth, screenPos.y-4.0 * depth)) * 0.05;

			    
			    sum += tex2D( _GrabTexture, float2(screenPos.x-3.0 * depth, screenPos.y+3.0 * depth)) * 0.09;
			    sum += tex2D( _GrabTexture, float2(screenPos.x+3.0 * depth, screenPos.y-3.0 * depth)) * 0.09;
			    
			    sum += tex2D( _GrabTexture, float2(screenPos.x-2.0 * depth, screenPos.y+2.0 * depth)) * 0.12;
			    sum += tex2D( _GrabTexture, float2(screenPos.x+2.0 * depth, screenPos.y-2.0 * depth)) * 0.12;
			    
			    sum += tex2D( _GrabTexture, float2(screenPos.x-1.0 * depth, screenPos.y+1.0 * depth)) *  0.15;
			    sum += tex2D( _GrabTexture, float2(screenPos.x+1.0 * depth, screenPos.y-1.0 * depth)) *  0.15;
			    
				

			    sum += tex2D( _GrabTexture, screenPos-5.0 * depth) * 0.025;    
			    sum += tex2D( _GrabTexture, screenPos-4.0 * depth) * 0.05;
			    sum += tex2D( _GrabTexture, screenPos-3.0 * depth) * 0.09;
			    sum += tex2D( _GrabTexture, screenPos-2.0 * depth) * 0.12;
			    sum += tex2D( _GrabTexture, screenPos-1.0 * depth) * 0.15;    
			    sum += tex2D( _GrabTexture, screenPos) * 0.16; 
			    sum += tex2D( _GrabTexture, screenPos+5.0 * depth) * 0.15;
			    sum += tex2D( _GrabTexture, screenPos+4.0 * depth) * 0.12;
			    sum += tex2D( _GrabTexture, screenPos+3.0 * depth) * 0.09;
			    sum += tex2D( _GrabTexture, screenPos+2.0 * depth) * 0.05;
			    sum += tex2D( _GrabTexture, screenPos+1.0 * depth) * 0.025;
			       
				return sum/2;
			}
		ENDCG
		}
	}
}
