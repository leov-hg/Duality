Shader "Unlit/TexturePanningTransparent"
{
    Properties
    {
        _MainTex ("MainTex", 2D) = "white" {}
        _MainTexPan ("MainTex Pan", Vector) = (0, 0, 0, 0)
        _Noise ("Noise", 2D) = "white" {}
        _NoisePan ("Noise Pan", Vector) = (0, 0, 0, 0)
        _DisplaceStrength ("Displace Strength", float) = 1
    }
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
        }
        LOD 100
        
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back

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
                float2 uv1 : TEXCOORD1;
                float4 color : COLOR;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex, _Noise;
            float4 _MainTex_ST, _Noise_ST;
            float2 _MainTexPan, _NoisePan;
            float _DisplaceStrength;
            uniform float GLOBAL_Alpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv1 = TRANSFORM_TEX(v.uv1, _Noise);
                o.color = v.color;

                float4 displaceMask = tex2Dlod(_MainTex, float4(o.uv.x + _Time.y * _MainTexPan.x, o.uv.y + _Time.y * _MainTexPan.y, 0, 0)).a;

                v.vertex.xyz += v.normal * displaceMask.a * _DisplaceStrength;

                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, float2(i.uv.x + _Time.y * _MainTexPan.x, i.uv.y + _Time.y * _MainTexPan.y));
                col.a *= tex2D(_Noise, float2(i.uv1.x + _Time.y * _NoisePan.x, i.uv1.y + _Time.y * _NoisePan.y));
                col.a *= i.color.r * GLOBAL_Alpha;

                return col;
                //return i.color.r;
            }
            ENDCG
        }
    }
}
