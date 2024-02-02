Shader "Unlit/AnimatedShader"
{
    Properties
    {
        _MainTexture("Main Texture", 2D) = "white" {}
        _AnimateXY("Animate X Y", Vector) = (0,0,0,0)
    }
    SubShader
    {

        Tags { "RenderType" = "Opaque" }

        LOD 100

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
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
             
            //putting the texture into the scope of the shader program
            sampler2D _MainTexture;
            //the unity material parameters are contained with unitycg.cginc
            //the unity prameteres all end in _ST
            float4 _MainTexture_ST;
            float4 _AnimateXY;
            //if you can move math to a vertex shader do it
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //allows for the unity default tiling and offset params to change values
                o.uv = TRANSFORM_TEX(v.uv, _MainTexture);
                //multiplies the offset by time which can be found in the unity built in shader variables
                //frac set the variable within its parameter back to 0 once it passes 1
                o.uv += _AnimateXY.xy * frac(float2(_Time.y / 15, 0));
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uvs = i.uv;
                

                //first parameter takes in the texture 
                //second parameter takes a coordinate and maps the color of the pixel to the uv
                //aka maps picture onto mesh
                fixed4 textureColor = tex2D(_MainTexture, uvs);
                return textureColor;

            }
            ENDCG
        }
    }
}
