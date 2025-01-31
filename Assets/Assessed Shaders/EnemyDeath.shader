// https://www.shadertoy.com/view/td2yDm
Shader "Unlit/EnemyDeath"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _DissolveTex ("DissolveTexture", 2D) = "white" {}
        _DissolveThreshold ("DissolveThreshold", Range(0,1)) = 0
        _Color ("Color", Color) = (0,0,0,0)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Tags { "Queue" = "Transparent"}
        LOD 100
        //Blend One One

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"    // lots of built-in functions

            sampler2D _MainTex;
            sampler2D _DissolveTex;
            float4 _MainTex_ST;
            float _DissolveThreshold;
            float4 _Color;
            float _EdgeSize;


            // data for mesh
            struct appdata 
            {
                float4 vertex : POSITION;   // vertex position
                float2 uv : TEXCOORD0;  // uv0 coordinates
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;  // output
                o.vertex = UnityObjectToClipPos(v.vertex); // converts local space to clip space
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);

                // get the edge of dissolve texture
                float4 dissolve = tex2D(_DissolveTex, i.uv);
                float dx = tex2D(_DissolveTex, i.uv) - tex2D(_DissolveTex, i.uv) * 0.4;
                float dy = dx;
                // get the distance between pixel and another pixel that's transparency * 0.5
                float edge = sqrt(dx * dx + dy * dy);
                
                if (edge < _DissolveThreshold) 
                {
                    col = lerp(col, _Color , edge * 4); // make the col color to be selected colour near the edge
                }

                // compare uv coord of dissolve texture with threshold, if less than, then clip
                clip(dissolve - _DissolveThreshold); 

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
