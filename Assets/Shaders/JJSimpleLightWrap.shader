Shader "Custom/JJSimpleLightWrap"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Lightwrap ("Lightwrap", 2D) = "white" {}
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100 

        Pass
        {
            Cull Off
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types <- said the dipshit
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;
            sampler2D _Lightwrap;
            

            float2 matcap_uv_compute(float3 I, float3 N, bool flipped)
            {
              /* Quick creation of an orthonormal basis */
              float a = 1.0 / (1.0 + I.z);
              float b = -I.x * I.y * a;
              float3 b1 = float3(1.0 - I.x * I.x * a, b, -I.x);
              float3 b2 = float3(b, 1.0 - I.y * I.y * a, -I.y);
              float2 matcap_uv = float2(dot(b1, N), dot(b2, N));
              if (flipped) {
                matcap_uv.x = -matcap_uv.x;
              }
              return matcap_uv * 0.496 + 0.5;
            }

            float3 get_view_vector_from_screen_uv(float2 uvcoords)
            {
                if (UNITY_MATRIX_P[3][3] == 0.0) {
                    float2 ndc = float2(uvcoords * 2.0 - 1.0);
                    /* This is the manual inversion of the ProjectionMatrix. */
                    float3 vV = float3((-ndc - UNITY_MATRIX_P[2].xy) /
                                    float2(UNITY_MATRIX_P[0][0], UNITY_MATRIX_P[1][1]),
                                -UNITY_MATRIX_P[2][2] - UNITY_MATRIX_P[3][2]);
                    return normalize(vV);
                }
                /* Orthographic case. */
                return float3(0.0, 0.0, 1.0);
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.screenPos = o.vertex;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = tex2D (_MainTex, i.uv) * _Color;
                float2 screenPos = i.screenPos.xy / i.screenPos.w;

                float3 forward = mul((float3x3)unity_CameraToWorld, float3(0, 0, 1));
                float3 viewVec = get_view_vector_from_screen_uv(i.screenPos);
                //float3 forward = mul((float3x3)unity_CameraToWorld, float3(0,0,1));
                float2 matcapUv = matcap_uv_compute(normalize(i.worldPos - _WorldSpaceCameraPos), i.normal, false);

                fixed4 col = tex2D(_Lightwrap, matcapUv);
                return c * col;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
