//reference:https://blog.csdn.net/w9503/article/details/105322873?ops_request_misc=&request_id=&biz_id=102&utm_term=%E6%B0%B4%E6%B3%A2%20shader&utm_
//medium=distribute.pc_search_result.none-task-blog-2~all~sobaiduweb~default-8-105322873.142^v96^pc_search_result_base5&spm=1018.2226.3001.4187
Shader "Unlit/DesertWater"
{
    
    Properties
    {
        _WaterColor ("WaterColor", Color) = (1.0, 1.0, 1.0, 1.0) //WaterColor
        _WaterSpeed ("aterSpeed", Vector) = (1,1,1,1) //WaterSpeed

        _WaterNoise("WaterNoise",2D) = "black"{} // WaterNoise



        _RippleColor ("RippleColor", Color) = (1.0, 1.0, 1.0, 1.0) //RippleColor

        _RippleSpeed ("RippleSpeed", float) = 1.0 //RippleSpeed

        _RippleDepth ("RippleDepth", float) = 1.0 //RippleDepth

        _RippleSize ("RippleSize", float) = 1.0 //RippleSize

        _RippleTrans ("RippleTrans", range(0.0, 1.0)) = 1.0 //RippleTrans

        _RippleArti ("RippleArti", float) = 1.0 //RippleArti

        

        
    }

    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent" "IgnoreProjector" = "true"}
        LOD 100

        Pass
        {
           
            
            HLSLPROGRAM

          

            #include "UnityCG.cginc"
            
            #pragma vertex vert
            #pragma fragment frag



            float4 _WaterColor; //watercolor
            float4 _WaterSpeed; //waterspeed
            uniform sampler2D  _WaterNoise; //noise

            float4 _RippleColor;
            float _RippleSpeed;
            float _RippleDepth;
            float _RippleSize;
            float _RippleTrans;
            float _RippleArti;
            
            
            
            
           
            struct appdata
            {
                float4 vertex : POSITION;

                float2 uv : TEXCOORD0;

            };

            struct v2f
            {


                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD1; //1

            };



            v2f vert(appdata v)
            {
                v2f o;            
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {


                //Defined a float4 variable to receive the color from the properties.
                float4 col = _WaterColor;

                //Defined the floats to receive the result of the texture sampling

                float finalNoise0 = 0;
                float finalNoise1 = 0;
                //  Two sets of time-varying uv coordinate, sample and simulate the water ripple effect from the noise texture 
                half2 uv0 = ( _Time.x * _WaterSpeed.xy + i.uv * _RippleSize);
                half2 uv1 = ( _Time.x * _WaterSpeed.zw + i.uv * _RippleSize);

                // same noise texture _WaterNoise and produce noise effects
                finalNoise0 = tex2D(_WaterNoise,uv0);
                // produce a denser ripple effect
                finalNoise1 = tex2D(_WaterNoise,uv1 * 1.3);
 
                // New noise values for both noise effects.
                float LastNoise = finalNoise0 * finalNoise1;
                //Output of the step function is multiplied with _RippleTrans to control the visibility of water ripples
                float water = step(_RippleArti, LastNoise) * _RippleTrans;
                
                // make ripple area darker or brighter, simulating the interaction of light with the water surface.
                float4 finalcol = lerp(col, _RippleColor, water);
                
                return finalcol;
            }
            ENDHLSL
            

        }
    }
}
   