Shader "Custom/ShockWaveShader"
{
	Properties
	{
		_MainTex ("Main Texture", 2D) = "black" {}
		_NoiseTex ("Noise Texture", 2D) = "black" {}
		_Ratio("Ratio", Range(0,1)) = 0
		_Thickness("Thickness", Range(0.01,1)) = 0
		_WaveColor("Wave Color", Color) = (1,1,1,1)
		_Center("Center", Vector) = (0,0,0,0)
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
			};

			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.worldPos = IN.vertex;
				OUT.vertex = mul(UNITY_MATRIX_MVP, OUT.worldPos);
				OUT.uv = IN.uv;
				return OUT;
			}
					
			sampler2D _MainTex;
			sampler2D _NoiseTex;
			fixed4 _WaveColor;
			float _Ratio;
			float2 _Center;
			float _Thickness; 

			float AspectRatioDistance(float2 pointA, float2 pointB)
			{
				float screenRatio = _ScreenParams.x/_ScreenParams.y;
				return sqrt(pow(pointA.x - pointB.x,2) + pow(pointA.y / screenRatio - pointB.y / screenRatio,2));
			}

			fixed4 frag (v2f IN) : SV_Target
			{
				float offsetchange = sin(1);
				float distorsionSpreader = 0.5;
				float distorsionDamper = 10;						

				float2 offset = float2( tex2D(_NoiseTex, float2(IN.worldPos.x / distorsionSpreader + offsetchange, 0)).g, 
				tex2D(_NoiseTex, float2(0, IN.worldPos.y / distorsionSpreader + offsetchange)).g);

				float dist = AspectRatioDistance(_Center.xy,IN.worldPos.xy);

				offset -= 0.5;
				float texOffset = offset/distorsionDamper;

				fixed4 waveColor = _WaveColor;

				float totalThickness = _Ratio + _Thickness;

				if(dist < _Ratio || dist > totalThickness)
				{
					waveColor = (1,1,1,1);
					texOffset = 0;
				}

				fixed4 c = tex2D(_MainTex,IN.uv + texOffset);
				c *= waveColor;

				return c;
			}
			ENDCG
		}
	}
}