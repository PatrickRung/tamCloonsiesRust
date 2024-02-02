Shader "Unlit/TestShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
    }
    SubShader
    {
        //tag is the unity label for what type of shader 
        //opaque makes light not go through the object
        Tags { "RenderType"="Opaque" "RenderPipeline" = "UniversalPipeline"}

        Blend One Zero
        //shader code
        Pass
        {
            //starts HLSL program
            HLSLPROGRAM

            //kinda like declaring variable where we are saying vertex is refered to as vert
            #pragma vertex vert
            
            //specifies fragment to frag
            #pragma fragment frag
            
            //extra resources/no clue what this shit is
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            //takes in the parameters from the game
            struct App
            {
                float4 positionOS : POSITION;
                half3 normal      : NORMAL;
            };

            struct v2f
            {
                float4 positionHCS : SV_POSITION;
                half3 normal : TEXCOORD0;
                half3 worldPos : TEXCOORD1;
                half3 viewDir : TEXCOORD2;
            };
            v2f vert(App IN) {
                v2f OUT;

                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.normal = TransformObjectToWorldNormal(IN.normal);
                OUT.worldPos = mul(unity_ObjectToWorld, IN.positionOS);
                OUT.viewDir = normalize(GetWorldSpaceViewDir(OUT.worldPos));
                return OUT;
            }

            sampler2D _MainTex;

            half4 frag(v2f IN) : SV_TARGET{
                //finds the line that face away from the angle of the normal map
                float dotProduct = dot(IN.normal, IN.viewDir);

                //step says that if the second parameter is less that the first parameter then return 0
                dotProduct = step(0.5, dotProduct);
                
                half3 fillColor = IN.normal * 0.5 + 0.5;

                half3 finalColor = fillColor * dotProduct;
                [branch] if (finalColor.x) {
                    return tex2D(_MainTex, IN.normal);
                }
                return half4(finalColor, 1.0);
            }
            
            
            ENDHLSL
        }
    }
}
