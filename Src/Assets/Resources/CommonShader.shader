Shader "Custom/CommonShader" {
                Properties {
                                _MainTex ("Base (RGB)", 2D) = "white" {}
                                //_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5  
                }
                SubShader {
                                Tags {"Queue"="Transparent+1"}
                                LOD 200

                                Blend SrcAlpha OneMinusSrcAlpha          
                                Cull Off
                                ZWrite Off
                                CGPROGRAM
                                #pragma surface surf Lambert

                                sampler2D _MainTex;

                                struct Input {
                                                float2 uv_MainTex;
                                };

                                void surf (Input IN, inout SurfaceOutput o) {
                                                half4 c = tex2D (_MainTex, IN.uv_MainTex);
                                                o.Albedo = c.rgb*4;
                                                o.Alpha = c.a;
                                }
                                ENDCG
                } 
                FallBack "Diffuse"
}
