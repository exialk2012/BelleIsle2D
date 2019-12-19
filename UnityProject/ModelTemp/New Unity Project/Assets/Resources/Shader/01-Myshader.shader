Shader	"CKZ/Shader01"{
	Properties{
		_Color("Color",Color) = (1,1,1,1)
		_Vector("Vector",Vector) = (1,2,3,4)
		_Int("Int",int) = 452
		_Float("Float",float) = 4.1
		_Range("Range",Range(0,1)) = 0
		_2D("2D",2D) = "White"{}
		_Cube("Cube",Cube) = "White"{}
		_3D("3D",3D) = "White"{}
	}
	SubShader{
		pass{
			CGPROGRAM
			float4 _Color;
			float4 _Vector;
			float _Int;
			float _Float;
			float _Range;
			sampler2D _2D;
			samplerCUBE _Cube;
			sampler3D _3D;
			ENDCG
		}
	}
	
	Fallback "Diffuse"
}