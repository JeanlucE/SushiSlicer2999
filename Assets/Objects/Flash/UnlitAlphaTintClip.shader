﻿// Unlit alpha-blended shader.
// - no lighting
// - no lightmap support
// - no per-material color

Shader "Unlit/TransparentFadeClip" {
	Properties{
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
		_Fade("Fade", Range(0, 1)) = 1.0
		_Clip("Clip", Range(0, 1)) = 0.5
	}

		SubShader{
		Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent" }
		LOD 100

		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha

		Pass{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#pragma multi_compile_fog

#include "UnityCG.cginc"

		struct appdata_t {
		float4 vertex : POSITION;
		float2 texcoord : TEXCOORD0;
	};

	struct v2f {
		float4 vertex : SV_POSITION;
		half2 texcoord : TEXCOORD0;
		UNITY_FOG_COORDS(1)
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float _Fade;
	float _Clip;

	v2f vert(appdata_t v)
	{
		v2f o;
		o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
		o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
		UNITY_TRANSFER_FOG(o,o.vertex);
		return o;
	}

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = fixed4(0, 0, 0, 0);
		if (i.texcoord.x > _Clip)
		{
			col = tex2D(_MainTex, i.texcoord);
			col.a *= _Fade;
		}
	UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
	}
		ENDCG
	}
	}

}
