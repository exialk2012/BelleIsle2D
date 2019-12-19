// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "CKZ/02-MyShader"{
	Properties{
		_Color("Color",Color) = (1,1,1,1)
	}
	SubShader{
		pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			float4 _Color;
			// struct a2v{
			// 	float4 pos:POSITION;
			// 	float3 normal:NORMAL;
			// };

			struct v2f{
				float4 pos:SV_POSITION;
				float4 color:COLOR;
			};

			v2f vert(appdata_full o){
				v2f f;
				f.pos = UnityObjectToClipPos(o.vertex);
				f.color.xyz = o.normal*0.5+0.5;
				return f;
			}

			float4 frag(v2f f):SV_TARGET{
				return f.color+_Color;
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}