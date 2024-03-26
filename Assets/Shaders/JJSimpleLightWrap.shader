Shader "Custom/JJSimpleLightWrap"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Lightwrap ("Lightwrap", 2D) = "white" {}
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        [NoScaleOffset] _NormalMap("Normal Map", 2D) = "bump" {}

        _ShadowColor("Shadow Color", Color) = (0.1,0.1,0.1,1)

        _DiffuseFalloff("Diffuse Fallof", float) = 0.15
        _DiffuseWrap("Diffuse Wrap", Range (0, 1)) = 0

        _Gloss("Gloss", float) = 100
        _SpecularThreshold("Specular Threshold", Range (0, 1)) = 0.85
        _SpecularFalloff("Specular Falloff", Range (0, 1)) = 0.1
        _SpecularColor("Specular Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags {"Queue"="Geometry" "RenderType"="Transparent" "LightMode"="ForwardBase"}
        Blend SrcAlpha OneMinusSrcAlpha
        Cull front 
        LOD 100
        Lighting On

        Pass
        {
            Cull Off
            CGPROGRAM
            // Physically based Standard lighting model, and enable shadows on all light types <- said the dipshit
            #pragma vertex vert alpha
            #pragma fragment frag alpha
            
            #pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight

            #include "UnityCG.cginc"
            #include "UnityLightingCommon.cginc"
            #include "AutoLight.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            sampler2D _NormalMap;

            float4 _ShadowColor;

            float4 _Color;
            sampler2D _Lightwrap;

            float _DiffuseFalloff;
            float _DiffuseWrap;
            
            float _Gloss;
            float4 _SpecularColor;
            float _SpecularFalloff;
            float _SpecularThreshold;
            float _SpecularIntensity;

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
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                float3 tangent : TEXCOORD4;
                float3 biTangent : TEXCOORD5;
                UNITY_FOG_COORDS(1)
                float4 pos : SV_POSITION;
                float3 worldPos : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                SHADOW_COORDS(6)
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.worldPos = mul(unity_ObjectToWorld, v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.tangent = UnityObjectToWorldDir(v.tangent);
                o.biTangent = cross(v.normal, v.tangent) * (v.tangent.w * unity_WorldTransformParams.w);

                o.screenPos = o.pos;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_SHADOW(o)
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 c = tex2D (_MainTex, i.uv) * _Color;
                float2 screenPos = i.screenPos.xy / i.screenPos.w;

                float3 normalMap = UnpackNormal(tex2D(_NormalMap, i.uv));

                float3x3 mtxTanToWorld = {
                    i.tangent.x, i.biTangent.x, i.normal.x,
                    i.tangent.y, i.biTangent.y, i.normal.y,
                    i.tangent.z, i.biTangent.z, i.normal.z,
                };

                float3 N = normalize(i.normal);
                //return float4 (N, 1);
                float3 forward = mul((float3x3)unity_CameraToWorld, float3(0, 0, 1));
                float3 viewVec = get_view_vector_from_screen_uv(i.screenPos);
                //float3 forward = mul((float3x3)unity_CameraToWorld, float3(0,0,1));
                float2 matcapUv = matcap_uv_compute(normalize(i.worldPos - _WorldSpaceCameraPos), i.normal, false);

                float3 light = float3(0, 0, 0);
                float3 finalSpec = float3(0,0,0);

                UNITY_LIGHT_ATTENUATION(atten, i, i.worldPos);

                // Diffuse
                float3 lightDir = _WorldSpaceLightPos0.xyz;
                
                float NdotL = lerp(-1 + _DiffuseWrap, 1, (dot(N, lightDir) + 1) / 2);
                float lightAffect = pow(saturate(NdotL), _DiffuseFalloff);

                light += _LightColor0.xyz * lightAffect * atten;

                // Specular
                float3 V = normalize(_WorldSpaceCameraPos - i.worldPos);
                //float3 R = reflect(-_WorldSpaceLightPos0.xyz, N);
                float3 H = normalize(_WorldSpaceLightPos0.xyz + V);
                float specularLight = saturate(dot(H, N));
                specularLight = pow(specularLight, _Gloss);
                specularLight = lerp(specularLight > _SpecularThreshold ? 1 : 0, specularLight, _SpecularFalloff);
                
                finalSpec += _LightColor0.xyz * _SpecularColor * specularLight * atten;
                light += _ShadowColor.xyz;

                fixed4 col = tex2D(_Lightwrap, matcapUv);
                return (c * col) * float4(light, 1) + float4(finalSpec, 0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
