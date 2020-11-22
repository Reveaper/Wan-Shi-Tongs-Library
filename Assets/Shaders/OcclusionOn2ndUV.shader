Shader "Custom/OcclusionOn2ndUV" {
	Properties{

		_MainTex("Base (RGB)", 2D) = "white" {}
		_AOTex("AO", 2D) = "white" {}
	}
		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			CGPROGRAM

			#pragma surface surf Standard
			#pragma target 3.0

			sampler2D _MainTex;
			sampler2D _AOTex;

			struct Input
			{
				float2 uv_MainTex : TEXCOORD0;
				float2 uv2_AOTex :  TEXCOORD1;
			};


			void surf(Input IN, inout SurfaceOutputStandard o)
			{
				half3 c = tex2D(_MainTex, IN.uv_MainTex.xy).rgb;
				half ao = tex2D(_AOTex, IN.uv2_AOTex.xy).r;
				o.Albedo = c;
				o.Occlusion = ao;
			}
			ENDCG
	}
		FallBack "Diffuse"
}
