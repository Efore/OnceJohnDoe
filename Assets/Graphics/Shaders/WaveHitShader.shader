// Shader created with Shader Forge v1.30 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.30;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False;n:type:ShaderForge.SFN_Final,id:3138,x:35029,y:32859,varname:node_3138,prsc:2|emission-1831-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:34202,y:32769,ptovrint:False,ptlb:WaveColor,ptin:_WaveColor,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.9647059,c2:0.9176471,c3:0.3058824,c4:0.6;n:type:ShaderForge.SFN_Tex2d,id:5248,x:33268,y:32860,ptovrint:False,ptlb:Noise Tex,ptin:_NoiseTex,varname:node_5248,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0042e3e09b395dc49be772196fa56268,ntxv:0,isnm:False|UVIN-3670-OUT;n:type:ShaderForge.SFN_Tex2d,id:5008,x:34621,y:32996,ptovrint:False,ptlb:Main Tex,ptin:_MainTex,varname:node_5008,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:ad272c145bb277e4b9df245201e63e96,ntxv:0,isnm:False|UVIN-221-OUT;n:type:ShaderForge.SFN_Vector1,id:2620,x:32555,y:32845,cmnt:Spreader,varname:node_2620,prsc:2,v1:0.89;n:type:ShaderForge.SFN_Vector1,id:2462,x:33583,y:33206,cmnt:Damper,varname:node_2462,prsc:2,v1:4.06;n:type:ShaderForge.SFN_Slider,id:6547,x:32281,y:32997,ptovrint:False,ptlb:WaveIndex,ptin:_WaveIndex,varname:node_6547,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1.57;n:type:ShaderForge.SFN_Sin,id:7001,x:32768,y:32962,varname:node_7001,prsc:2|IN-6547-OUT;n:type:ShaderForge.SFN_FragmentPosition,id:3580,x:32555,y:32701,varname:node_3580,prsc:2;n:type:ShaderForge.SFN_Divide,id:9760,x:32768,y:32811,varname:node_9760,prsc:2|A-3580-X,B-2620-OUT;n:type:ShaderForge.SFN_Add,id:763,x:32955,y:32852,varname:node_763,prsc:2|A-9760-OUT,B-7001-OUT;n:type:ShaderForge.SFN_Append,id:3670,x:33103,y:32919,varname:node_3670,prsc:2|A-763-OUT,B-1951-OUT;n:type:ShaderForge.SFN_Vector1,id:1951,x:32924,y:33153,varname:node_1951,prsc:2,v1:0;n:type:ShaderForge.SFN_Append,id:7779,x:33434,y:32933,varname:node_7779,prsc:2|A-5248-G,B-1951-OUT;n:type:ShaderForge.SFN_Vector1,id:3873,x:33306,y:33149,varname:node_3873,prsc:2,v1:1;n:type:ShaderForge.SFN_Subtract,id:6681,x:33614,y:33034,varname:node_6681,prsc:2|A-7779-OUT,B-3873-OUT;n:type:ShaderForge.SFN_Divide,id:3518,x:33843,y:33074,varname:node_3518,prsc:2|A-6681-OUT,B-2462-OUT;n:type:ShaderForge.SFN_Add,id:221,x:34301,y:33087,varname:node_221,prsc:2|A-5075-OUT,B-8538-UVOUT;n:type:ShaderForge.SFN_TexCoord,id:8538,x:33760,y:33237,varname:node_8538,prsc:2,uv:0;n:type:ShaderForge.SFN_ValueProperty,id:1768,x:33301,y:32679,ptovrint:False,ptlb:xOrigin,ptin:_xOrigin,varname:node_1768,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.5;n:type:ShaderForge.SFN_Subtract,id:6673,x:33542,y:32572,varname:node_6673,prsc:2|A-3580-X,B-1768-OUT;n:type:ShaderForge.SFN_Abs,id:6145,x:33756,y:32687,varname:node_6145,prsc:2|IN-6673-OUT;n:type:ShaderForge.SFN_Divide,id:1477,x:33982,y:32722,varname:node_1477,prsc:2|A-6145-OUT,B-2930-OUT;n:type:ShaderForge.SFN_Subtract,id:2930,x:33767,y:32834,varname:node_2930,prsc:2|A-6465-OUT,B-1768-OUT;n:type:ShaderForge.SFN_Vector1,id:6465,x:33537,y:32880,varname:node_6465,prsc:2,v1:1;n:type:ShaderForge.SFN_Multiply,id:5075,x:34033,y:32929,varname:node_5075,prsc:2|A-1477-OUT,B-3518-OUT;n:type:ShaderForge.SFN_Multiply,id:7317,x:34466,y:32706,varname:node_7317,prsc:2|A-1477-OUT,B-7241-RGB;n:type:ShaderForge.SFN_Subtract,id:8925,x:34741,y:32711,varname:node_8925,prsc:2|A-3301-OUT,B-7317-OUT;n:type:ShaderForge.SFN_Vector4,id:3301,x:34466,y:32552,varname:node_3301,prsc:2,v1:1,v2:1,v3:1,v4:1;n:type:ShaderForge.SFN_Multiply,id:1831,x:34850,y:32917,varname:node_1831,prsc:2|A-8925-OUT,B-5008-RGB;proporder:7241-5008-5248-6547-1768;pass:END;sub:END;*/

Shader "Shader Forge/WaveHitShader" {
    Properties {
        _WaveColor ("WaveColor", Color) = (0.9647059,0.9176471,0.3058824,0.6)
        _MainTex ("Main Tex", 2D) = "white" {}
        _NoiseTex ("Noise Tex", 2D) = "white" {}
        _WaveIndex ("WaveIndex", Range(0, 1.57)) = 0
        _xOrigin ("xOrigin", Float ) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma exclude_renderers gles3 metal d3d11_9x xbox360 xboxone ps3 ps4 psp2 
            #pragma target 3.0
            uniform float4 _WaveColor;
            uniform sampler2D _NoiseTex; uniform float4 _NoiseTex_ST;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float _WaveIndex;
            uniform float _xOrigin;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.posWorld = mul(_Object2World, v.vertex);
                o.pos = mul(UNITY_MATRIX_MVP, v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
////// Lighting:
////// Emissive:
                float node_1477 = (abs((i.posWorld.r-_xOrigin))/(1.0-_xOrigin));
                float node_1951 = 0.0;
                float2 node_3670 = float2(((i.posWorld.r/0.89)+sin(_WaveIndex)),node_1951);
                float4 _NoiseTex_var = tex2D(_NoiseTex,TRANSFORM_TEX(node_3670, _NoiseTex));
                float2 node_221 = ((node_1477*((float2(_NoiseTex_var.g,node_1951)-1.0)/4.06))+i.uv0);
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_221, _MainTex));
                float3 emissive = ((float4(1,1,1,1)-(node_1477*_WaveColor.rgb))*_MainTex_var.rgb).rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}
