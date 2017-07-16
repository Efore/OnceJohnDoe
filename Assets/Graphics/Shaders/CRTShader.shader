﻿Shader "Custom/CRTShader" 
{
    Properties {
        _MainTex ("Base (RGB)", 2D) = "white" {}
    }

    SubShader {
        Pass {
            ZTest Always Cull Off ZWrite Off Fog { Mode off }

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #pragma fragmentoption ARB_precision_hint_fastest
            #include "UnityCG.cginc"
            #pragma target 3.0

            struct v2f 
            {
                float4 pos      : POSITION;
                float2 uv       : TEXCOORD0;
            };

            uniform sampler2D _MainTex;

            v2f vert(appdata_img v)
            {
                v2f o;
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
                return o;
            }

            half4 frag(v2f i): COLOR
            {
                half4 color = tex2D(_MainTex, i.uv);
                return color;
            }

            ENDCG
        }
    }
    FallBack "Diffuse"
}