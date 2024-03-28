﻿Shader "Custom/ToonGlossinessRotated" {
	Properties{
		_Color("Color", Color) = (1, 1, 1, 1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_MinLightIntesity("Min Light Intesity", Range(0, 1)) = 0
		_MinLightIntesityValue("Min Light Intesity Value", Range(0, 1)) = 0
		_MinAttenuation("Min Attenuation", Range(0, 1)) = 0
		_MinAttenuationValue("Min Attenuation Value", Range(0, 1)) = 0
		_Glossiness("Glossiness", Range(0,1)) = 0.5
		_GlossinessIntensivity("GlossinessIntensivity", Range(0,3)) = 0.5
		_GlossinessEdge("GlossinessEdge", Range(0,1)) = 0.5
		_RotateSpeed("Rotate speed", Range(0,2)) = 0.0
	}
		SubShader{
			Tags {
				"RenderType" = "Opaque"
			}
			LOD 200
			CGPROGRAM
			#pragma surface surf CelShaded vertex:vert
			#pragma target 3.0

			float _MinLightIntesity;
			float _MinLightIntesityValue;
			float _MinAttenuation;
			float _MinAttenuationValue;
			half _Glossiness;
			half _GlossinessIntensivity;
			half _GlossinessEdge;
			half _RotateSpeed;

			struct SurfaceOutputCelShaded
			{
				fixed3 Albedo;
				fixed3 Normal;
				float Smoothness;
				half3 Emission;
				fixed Alpha;
			};

			half4 LightingCelShaded(SurfaceOutputCelShaded s, half3 lightDir, half3 viewDir, half atten) {
				half NdotL = dot(s.Normal, lightDir);

				float m = 1 / _MinLightIntesity;
				float lightIntensity = NdotL > _MinLightIntesity ? 1 : lerp(_MinLightIntesityValue, 1, NdotL * m);
				float l = 1 / _MinAttenuation;
				float attenuation = atten > _MinAttenuation ? 1 : lerp(_MinAttenuationValue, 1, atten * l);

				float3 refl = reflect(normalize(lightDir), normalize(s.Normal));
				float vDotRefl = dot(viewDir, -refl);
				float kk = 1 / _Glossiness;
				float gloss = _Glossiness * 30;
				float3 specular = vDotRefl > _GlossinessEdge ? pow(vDotRefl * lightIntensity, gloss * gloss) : 0;

				//float clr = (_LightColor0.r + _LightColor0.g + _LightColor0.b) / 3;
				//half4 midColor = float4(clr, clr, clr, 1);

				half4 c;

				c.rgb = (s.Albedo + specular * _GlossinessIntensivity) * (lightIntensity * attenuation);
				c.a = s.Alpha;
				return c;
			}

			sampler2D _MainTex;
			fixed4 _Color;
			struct Input {
				float2 uv_MainTex;
			};

			float4 RotateAroundYInDegrees(float4 vertex, float degrees)
			{
				float alpha = degrees;
				float sina, cosa;
				sincos(alpha, sina, cosa);
				float2x2 m = float2x2(cosa, -sina, sina, cosa);

				return float4(mul(m, vertex.xz), vertex.yw).xzyw;
			}

			void vert(inout appdata_full v)
			{
				float angle = _Time.y * _RotateSpeed;
				v.vertex = RotateAroundYInDegrees(v.vertex, angle);
				float4 normal = float4(v.normal, 0);
				v.normal = RotateAroundYInDegrees(normal, angle).xyz;
			}

			void surf(Input IN, inout SurfaceOutputCelShaded o) {
				fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
				o.Albedo = c.rgb;
				o.Alpha = c.a;
				o.Smoothness = _Glossiness;
			}
			ENDCG
		}
			FallBack "Diffuse"
}