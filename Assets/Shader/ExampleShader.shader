Shader "Unlit/ExampleShader"
//folder and name
{
    //properties are the variables that can affect the shader ex texture, color.
    Properties
    {
        _Color("Test Color", color) = (1,1,1,1)
    }
    //shader code goes in sub shader
    SubShader
    {
        //properties of shader
        Tags { "RenderType"="Opaque" }
        //level of detail
        LOD 100

        Pass
        {
            CGPROGRAM
            //the actual shader code
            //declaring vertex shader
            #pragma vertex vert //vertex shader runs on every vert
            #pragma fragment frag //runs on every pixel

            #include "UnityCG.cginc"

            //object data or mesh data contains data like vertex or uv
            //all the data coming from mesh
            struct appdata
            {
                //position of each vertex
                float4 vertex : POSITION;
            };
            //vertex to fragment shader
            //frag shader is what actually drawn on screen
            struct v2f
            {
          
                float4 vertex : SV_POSITION; 
            };
    // declares the color variable that we had in the properties into the hlsl code
            fixed4 _Color; 

            //takes in the data from apdata
            v2f vert (appdata v)
            {
                //says that the output is going to output
                v2f o;
                //model view projection matrix
                //moving from object space to world space
                o.vertex = UnityObjectToClipPos(v.vertex); 
                return o;
            }

            //takes in v2f data as i
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = _Color;
                return col;
                //return col returns the color of at each pixel
            }
            ENDCG
        }
    }
}
