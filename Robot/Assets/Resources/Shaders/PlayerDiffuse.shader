

Shader "Custom/PlayerDiffuse"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Base 2D", 2D) = "white"{}
		_XRayColor("XRay Color", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags{ "Queue" = "Geometry+100" "RenderType" = "Opaque" }

		//X ray Pass  
		Pass
		{
			Blend SrcAlpha One
			ZWrite Off
			ZTest Greater

			CGPROGRAM
			#include "Lighting.cginc"  
			fixed4 _XRayColor;
			
			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 normal : normal;
				float3 viewDir : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.viewDir = ObjSpaceViewDir(v.vertex);
				o.normal = v.normal;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 normal = normalize(i.normal);
				float3 viewDir = normalize(i.viewDir);
				float rim = 1 - dot(normal, viewDir);
				return _XRayColor * rim;
			}
			#pragma vertex vert  
			#pragma fragment frag  
			ENDCG
		}

		//Default Pass  
		Pass
		{
			ZWrite On
			CGPROGRAM
			#include "Lighting.cginc" 
			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _Color;

			struct v2f
			{
				float4 pos : SV_POSITION;
				float3 normal : normal;
				float2 uv : TEXCOORD0;
			};

			v2f vert(appdata_base v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.normal = v.normal;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float lightSensitive;
				fixed4 color;
				fixed4 textureColor;

				color = UNITY_LIGHTMODEL_AMBIENT;

				float3 normal = normalize(UnityObjectToWorldNormal(i.normal));
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);

				lightSensitive = saturate(dot(lightDir, normal));
				color += (_LightColor0 * lightSensitive);
				textureColor = tex2D(_MainTex, i.uv) * _Color;

				return textureColor * color;
			}

			#pragma vertex vert  
			#pragma fragment frag     
			ENDCG
		}
	}

		FallBack "Diffuse"
}