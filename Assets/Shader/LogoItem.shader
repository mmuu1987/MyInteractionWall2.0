Shader "Unlit/LogoItem"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

         _ShowTex ("ShowTex", 2D) = "white" {}

         _Alpha("Alpha",Range(0,1)) = 1

        _Color("Color",Color)=(1,1,1,1)

    }
    SubShader
    {
       Tags{"Queue" = "Transparent"  "IgnoreProjection" = "True" "RenderType" = "Transparent" } // 透明的Shader
        LOD 100

        Pass
        {

            ZWrite Off // 关闭深度写入
            Blend SrcAlpha OneMinusSrcAlpha // 混合的参数

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
                float2 uv2:TEXCOORD2;
              
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _ShowTex;
            float4 _ShowTex_ST;
            float _Alpha;
            sampler2D _Show2Tex;

            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv2 = TRANSFORM_TEX(v.uv, _ShowTex);
                
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);


                fixed4 col3 = tex2D(_ShowTex, i.uv);//用uv读取_ShowTex的贴图

                fixed4 col2 = tex2D(_ShowTex,i.uv2);//调节UV得到的
                
              

                //fixed4 col4 = lerp(col,col3,_Alpha);

                
                if(i.uv.x>0.078 &&i.uv.x<0.92 &&i.uv.y>0.08&&i.uv.y<0.92)
                {
                     if(col.a<0.9f)
                      {
                       col = col2;
                      }
                }
                
               

                return col*_Color;
            }
            ENDCG
        }
    }
}
