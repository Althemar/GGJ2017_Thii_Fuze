Shader "[Fuse]/Fuse_v1"
{
	Properties
	{
		[NoScaleOffset] _MainTex("Texture", 2D) = "white" {}
		_Slider("Slider", Float) = 1
		_Direction("Direction", Range(-1,1)) = 1
		_AnimationTime("AnimationTime", Float) = 0

		_Width("Width", Float) = 1
		_Thickness("Thickness", Float) = 1
			// 
			_DirectionAColor("Direction A Color", Color) = (256,256,256,256)
			_DirectionBColor("Direction B Color", Color) = (256,256,256,256)
	}
		SubShader
		{
				Tags{ "RenderType" = "Opaque" }
				LOD 100
			Pass
		{
			Tags{ "LightMode" = "ForwardBase" }
			CGPROGRAM
	#pragma vertex vert
	#pragma fragment frag
	#pragma target 2.0
	#pragma multi_compile_fog

	#include "UnityCG.cginc"

			struct v2f
		{
			float2 uv : TEXCOORD0;
			float4 pos : SV_POSITION;
            fixed4 color : COLOR;
		};
		v2f vert(appdata_full v)
		{
			v2f o;
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;
			o.color = v.color;

			half3 worldNormal = UnityObjectToWorldNormal(v.normal);
			half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
			return o;
		}

		sampler2D _MainTex;
		float _Slider;
		float _Direction;
		float _AnimationTime;

		float _Width;
		float _Thickness;

		float4 _DirectionAColor;
		float4 _DirectionBColor;

		float Remap(float fromMin, float fromMax, float toMin, float toMax, float value) {
			//float fromLength = fromMax - fromMin;
			//float toLength = toMax - toMin;
			//return (((value - fromMin) * toLength) / fromLength) + toMin;
			return toMin + (value - fromMin) * (toMax - toMin) / (fromMax - fromMin);
		}

		float DrawArrow(float u, float v) {
			// Remap
			v = Remap(0.0, 1.0, -0.5, 1.0, v);
			
			// Side mask (width)
			float sideMask = abs(u - 0.5) * 2;
			sideMask -= _Width / 2;
			sideMask = ceil(sideMask);

			// Fuse
			return sideMask;
		}
		
		fixed4 frag(v2f i) : SV_Target
		{
		fixed4 col = tex2D(_MainTex, i.uv);

		float x = i.uv.y;
		float y = i.uv.x;
		y -= _Time.y;
		y *= _Slider;
		y = y - floor(y); // Repeat

		float arrow = DrawArrow(x, y);
		arrow = clamp(0, 1, arrow);


		col = lerp(_DirectionAColor, _DirectionBColor, arrow);

		col *= i.color;

		return col;
		}


			ENDCG
		}

			// shadow casting support
			UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		}
}