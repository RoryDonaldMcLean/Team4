Shader "Custom/TestReflection"
{
	Properties
	{
		_CubeMap("CubeMap", CUBE) = ""{}
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float3 worldNormal : TEXCOORD0;
				float3 worldRef : TEXCOORD1;
				float3 worldPos : TEXCOORD2;
				float3 worldViewDir : TEXCOORD3;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.worldNormal = mul(unity_ObjectToWorld, v.normal);
				o.worldViewDir = normalize(_WorldSpaceCameraPos.xyz - o.worldPos.xyz);
				o.worldRef = reflect(-o.worldViewDir,normalize(o.worldNormal));
				return o;
			}

			samplerCUBE _CubeMap;
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = texCUBE(_CubeMap, i.worldRef);
				return col;
			}
		ENDCG
		}
	}
}