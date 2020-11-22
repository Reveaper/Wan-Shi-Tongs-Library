// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Particles/Alpha Blended Clipsafe" {
	Properties{
		_TintColor("Tint Color", Color) = (0.5,0.5,0.5,0.5)
		_MainTex("Particle Texture", 2D) = "white" {}
		_FadeDistance("Fade Start Distance", float) = 0.5
		_InvFade("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	}

		SubShader{
			Tags { "Queue" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaTest Greater .01
			ColorMask RGB
			Lighting Off
			ZWrite Off
			Fog { Color(0,0,0,0) }
			Pass {
				CGPROGRAM
					#pragma vertex vert
					#pragma fragment frag
					#pragma target 2.0
					#pragma shader_feature SOFTPARTICLES_ON
					#pragma fragmentoption ARB_fog_exp2
					#pragma fragmentoption ARB_precision_hint_fastest
					#include "UnityCG.cginc"

					uniform float4	_MainTex_ST,
									_TintColor;
					uniform float _FadeDistance;

					struct appdata_vert {
						float4 vertex : POSITION;
						float4 texcoord : TEXCOORD0;
						float4 color : COLOR;
					};

					uniform sampler2D _MainTex;
					uniform sampler2D_float _CameraDepthTexture;
					uniform float _InvFade;

					struct v2f {
						float4 pos : SV_POSITION;
						float2	uv : TEXCOORD0;
						float4 color : COLOR;
						#ifdef SOFTPARTICLES_ON
						float4 projPos : TEXCOORD2;
						#endif
					};

					v2f vert(appdata_vert v) {
						v2f o;
						o.pos = UnityObjectToClipPos(v.vertex);
						o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
						float4 viewPos = mul(UNITY_MATRIX_MV, v.vertex);
						float alpha = (-viewPos.z - _ProjectionParams.y) / _FadeDistance;
						alpha = min(alpha, 1);
						o.color = float4(v.color.rgb, v.color.a * alpha);
						o.color *= _TintColor * 2;
						#ifdef SOFTPARTICLES_ON
						o.projPos = ComputeScreenPos(o.pos);
						COMPUTE_EYEDEPTH(o.projPos.z);
						#endif
						return o;
					}

					float4 frag(v2f i) : COLOR {
						half4 texcol = tex2D(_MainTex, i.uv);

						#ifdef SOFTPARTICLES_ON
						float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						float fade = saturate(_InvFade * (sceneZ - partZ));
						i.color.a *= fade;
						#endif

						return texcol * i.color;
					}
				ENDCG
			}
		}

			Fallback "Particles/Alpha Blended"
}