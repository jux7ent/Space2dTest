Shader "PlanetShader"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _Radius("Radius", float) = 0

        [IntRange] _StencilRef ("Stencil Value", Range(0, 255)) = 0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Stencil
        {
            Ref [_StencilRef]
            Comp Equal
            Pass Keep
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                UNITY_VERTEX_OUTPUT_STEREO
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float centerX = i.texcoord.x - 0.5f;
                float centerY = i.texcoord.y - 0.5f;

                if (centerX * centerX + centerY * centerY > _Radius * _Radius) discard;

                fixed4 col = tex2D(_MainTex, i.texcoord);
                UNITY_APPLY_FOG(i.fogCoord, col);
                UNITY_OPAQUE_ALPHA(col.a);
                return col;
            }
            ENDCG
        }
    }

}
/*
Shader "PlanetShader"
{
Properties
{
_TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
_MainTex("Particle Texture", 2D) = "white" {}
_Radius("Radius", float) = 0

[IntRange] _StencilRef ("Stencil Value", Range(0, 255)) = 0
}

CGINCLUDE
#include "UnityCG.cginc"

sampler2D _MainTex;
float4 _MainTex_ST;
fixed4 _TintColor;
float _Radius;

struct appdata_t
{
    float4 position : POSITION;
    float4 texcoord : TEXCOORD0;
    fixed4 color : COLOR;
};

struct v2f
{
    float4 position : SV_POSITION;
    float2 texcoord : TEXCOORD0;
    fixed4 color : COLOR;
    UNITY_FOG_COORDS(1)
};

v2f vert(appdata_t v)
{
    v2f o;
    o.position = UnityObjectToClipPos(v.position);
    o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
    o.color = v.color;
    UNITY_TRANSFER_FOG(o, o.vertex);
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
    float centerX = i.texcoord.x - 0.5f;
    float centerY = i.texcoord.y - 0.5f;

    if (centerX * centerX + centerY * centerY > _Radius * _Radius) discard;

    fixed4 texColor = tex2D(_MainTex, i.texcoord);
    fixed4 col = 2.0f * i.color * _TintColor * texColor;
    UNITY_APPLY_FOG_COLOR(i.fogCoord, col, (fixed4)0);
    return col;
}
ENDCG

SubShader
{
Tags
{
"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"
}

Stencil
{
Ref [_StencilRef]
Comp Equal
Pass Keep
}

// Blend One OneMinusSrcAlpha
Cull Off Lighting Off ZWrite Off Fog
{
Mode Off
}

Pass
{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_particles
#pragma multi_compile_fog
ENDCG
}
}
}*/