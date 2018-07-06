Shader "Custom/ElecParticle"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_CentreColor("Centre Color", Color) = (1,1,1,1)
		_EdgeColor("Edge Color", Color) = (1,1,1,1)
		_EdgeFactor("Edge Factor", Range(0,1)) = 0.1
	}
	SubShader
	{
		Tags { "Queue" = "Geometry+200" "RenderType"="Opaque" }
		LOD 100

		//Pass
		//{
		//	Cull Front

		//	CGPROGRAM
		//	#include "UnityCG.cginc"  
		//	fixed4 _EdgeColor;
		//	float _EdgeFactor;

		//	struct v2f
		//	{
		//		float4 pos : SV_POSITION;
		//	};

		//	v2f vert(appdata_full v)
		//	{
		//		v2f o;
		//		//v.vertex.xyz += v.normal * _OutlineFactor;  
		//		o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
		//		float3 vnormal = mul((float3x3)UNITY_MATRIX_IT_MV, v.normal);
		//		float2 offset = TransformViewToProjection(vnormal.xy);
		//		o.pos.xy += offset * _EdgeFactor;
		//		return o;
		//	}

		//	fixed4 frag(v2f i) : SV_Target
		//	{
		//		return _EdgeColor;
		//	}

		//	#pragma vertex vert  
		//	#pragma fragment frag  
		//	ENDCG
		//}

		Pass
		{
			ZTest LEqual
			ZWrite On
			Cull Back
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
		
			#include "UnityCG.cginc"

			//struct appdata
			//{
			//	float4 vertex : POSITION;
			//	float2 uv : TEXCOORD0;
			//	float3 normal : NORMAL;
			//};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
				float3 viewDir : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _CentreColor;
			fixed4 _EdgeColor;
		
			v2f vert (appdata_base v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
				o.normal = v.normal;
				o.viewDir = ObjSpaceViewDir(v.vertex);
			
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				float3 normal = normalize(i.normal);
				float3 viewDir = normalize(i.viewDir);
				float rim = 1 - max(0, dot(normal, viewDir));
				
				//fixed4 col = tex2D(_MainTex, i.uv) * lerp(_CentreColor, _EdgeColor, rim);
				fixed4 col = tex2D(_MainTex, i.uv) * lerp(_CentreColor, _EdgeColor, 0.5f);
				return col;
			}

			fixed4 lerp(fixed a, fixed b, float alpha)
			{
				return a * (1 - alpha) + b * alpha;
			}
			ENDCG
		}
	}

	FallBack "Diffuse"
}
