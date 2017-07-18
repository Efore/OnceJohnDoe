Shader "Custom/LoadingScreenShader"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "black" {}
		_BlendingTex("Blending Texture", 2D) = "black" {}
		_Alpha ("Alpha Degree", Range(0.0,1.0)) = 0
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
				float4 scr_pos  : TEXCOORD1;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				o.scr_pos = ComputeScreenPos(o.vertex);
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _BlendingTex;
			bool _UseBlendingTex;
			float _Alpha;
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

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = 0;

				float2 temp = i.uv;
				temp.y = 1 - temp.y;
				col = tex2D (_BlendingTex, temp);	
						
				col.a = _Alpha;
				return col * ctrParam(i);
			}
			ENDCG
		}
	}
}