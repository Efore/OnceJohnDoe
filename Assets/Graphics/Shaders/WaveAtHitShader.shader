Shader "Custom/WaveAtHitShader"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "black" {}
		_RenderedTex ("Rendered Texture", 2D) = "black" {}
		_NoiseTex ("Noise Texture", 2D) = "black" {}
		_WaveColor("Wave Color", Color) = (1,1,1,1)
        _VertsColor("Verts fill color", Range(0.0,1.0)) = 0
		_VertsColor2("Verts fill color 2", Range(0.0,1.0)) = 0	
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
				float4 worldPos : TEXCOORD1;
				float4 scr_pos  : TEXCOORD2;
			};

			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.worldPos = IN.vertex;
				OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPos);
				OUT.uv = IN.uv;
				OUT.scr_pos = ComputeScreenPos(OUT.vertex);
				return OUT;
			}
					
			sampler2D _MainTex;
			sampler2D _RenderedTex;
			sampler2D _NoiseTex;
			fixed4 _WaveColor;
			float _Thickness; 
			uniform float _VertsColor;
			uniform float _VertsColor2;

			float4 ctrParam(v2f i)
			{
				float2 ps = i.scr_pos.xy *_ScreenParams.xy / i.scr_pos.w;

				uint pp = (uint)ps.x % 3;
				float4 muls = float4(0, 0, 0, 1);

		        if (pp == 1) { 
		        	muls.r = 1; 
		        	muls.b = _VertsColor; 
		        	muls.g = _VertsColor2; 
	        	}
				else if (pp == 2) { 
					muls.r = _VertsColor; 
					muls.g = 1; 
					muls.b = _VertsColor2; 
				}
        		else { 
        			muls.r = _VertsColor2; 
        			muls.b = 1; 
        			muls.g = _VertsColor; 
        		}

        		return muls;
			}

			fixed4 frag (v2f IN) : SV_Target
			{
				float offsetchange = sin(1);
				float distorsionSpreader = 0.5;
				float distorsionDamper = 10;						

				float2 offset = float2( tex2D(_NoiseTex, float2(IN.worldPos.x / distorsionSpreader + offsetchange, 0)).g, 
				tex2D(_NoiseTex, float2(0, IN.worldPos.y / distorsionSpreader + offsetchange)).g);

				offset -= 0.5;
				float texOffset = offset/distorsionDamper;

				fixed4 waveColor = _WaveColor;

				if(tex2D(_RenderedTex, float2(IN.worldPos.x, IN.worldPos.y)).r > 0)
				{
					waveColor = 1;
					texOffset = 0;
				}

				fixed4 c = tex2D(_MainTex,IN.uv + texOffset);
				c *= waveColor;

				return c * ctrParam(IN);
			}
			ENDCG
		}
	}
}