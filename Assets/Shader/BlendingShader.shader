Shader "Unlit/BlendingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", color) = (1,1,1,1)
        [Enum(UnityEngine.Rendering.BlendMode)]
        _SrcFactor("Src Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)]
        _DstFactor("Dst Factor", Float) = 10
        [Enum(UnityEngine.Rendering.BlendOp)]
        _Opp("Operation", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        //blend formula
        //source = whatever this shader outpus
        //destination = whatever is in the background

        //color = (0,0,0,0.7)
        //source * fsource + destination * fdestination
        //White * 0.7 + Background Color * (1-0.7)
        //we see 78% white and 30% of the background 

        //hard code example:  Blend One One
        
            Blend [_SrcFactor] [_DstFactor] 
            BlendOp [_Opp]
    
        //fix for sending me straight to heaven is changing the render order to transparent
        //Blend One One allows me to reach heaven earlier
        //SrcAlpha OneMinusSrcAlpha makes a trippy snake thing

        //additive
        //source * 1 + destination * 1
        //grass + back ground
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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                return col;
            }
            ENDCG
        }
    }
}
