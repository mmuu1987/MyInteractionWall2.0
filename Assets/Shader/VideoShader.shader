Shader "Unlit/VideoShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Mask ("Mask", 2D) = "white" {}
        _Rnage("Range",Range(0,1)) =0.5
         [Enum(UnityEngine.Rendering.BlendMode)]
         MySrcMode ("SrcMode", Float) = 0
         [Enum(UnityEngine.Rendering.BlendMode)]
         MyDstMode ("DstMode", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}

		 ZWrite off
		
		Blend [MySrcMode] [MyDstMode]
        //Blend SrcAlpha  OneMinusSrcAlpha
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
                float2 uv : TEXCOORD0;
              
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _Mask;
            float _Rnage;
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
                fixed4 col2 = tex2D(_Mask,i.uv);

                if(col2.a>0)
                {
                  col.a=0;
                }


               // col.a = 0.45;

                if(col.x+col.y+col.z <=_Rnage)col.a = 0; 

                if(col.a>0)
                {
                    col.a=0.25;
                }
               
             
              
               
                return col;
            }
            ENDCG
        }
    }
}
