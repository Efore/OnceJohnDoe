Shader "Custom/WavedScreenShader"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "black" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_DistortionSpreader("Distortion Spreader", Float) = 30
		_DistortionDamper("Distortion Damper", Float) = 200
		_WaveIndex("Wave Index", Range(0.0,1.57)) = 0
		_WaveColor("Wave Color", Color) = (1,1,1,1)
		_XPosOrigin("X Origin", Float) = 0.5
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
			sampler2D _NoiseTex;
			float _DistortionSpreader;
			float _DistortionDamper;
			float _WaveIndex;
			fixed4 _WaveColor;
			float _XPosOrigin;
			uniform float _VertsColor;
			uniform float _VertsColor2;

			float4 ctrParam(v2f i)
			{
				float2 ps = i.scr_pos.xy *_ScreenParams.xy / i.scr_pos.w;

				int pp = (int)ps.x % 3;
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
				float offsetchange = sin(_WaveIndex);

				if(IN.worldPos.x > _XPosOrigin)
					offsetchange = sin(_WaveIndex) * -1;				

				float2 offset = float2( tex2D(_NoiseTex, float2(IN.worldPos.x / _DistortionSpreader + offsetchange, 0)).g, 0);

				offset -= 0.5;
				float texOffset = offset/_DistortionDamper;
				float waveIntensity = (abs(IN.worldPos.x - _XPosOrigin)/(1 - _XPosOrigin));

				texOffset = texOffset * waveIntensity;
				_WaveColor = _WaveColor * waveIntensity;

				fixed4 c = tex2D(_MainTex,IN.uv + texOffset);
				c *= fixed4(1,1,1,1) - _WaveColor;

				return c * ctrParam(IN);
			}
			ENDCG
		}
	}
}