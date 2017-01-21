Shader "[Fuse]/Arrow_v1"
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
		Pass
	{
		Tags{ "LightMode" = "ForwardBase" }
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"
#include "Lighting.cginc"

		// compile shader into multiple variants, with and without shadows
		// (we don't care about any lightmaps yet, so skip these variants)
#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
		// shadow helper functions and macros
#include "AutoLight.cginc"

		struct v2f
	{
		float2 uv : TEXCOORD0;
		SHADOW_COORDS(1) // put shadows data into TEXCOORD1
		fixed3 diff : COLOR0;
		fixed3 ambient : COLOR1;
		float4 pos : SV_POSITION;
	};
	v2f vert(appdata_base v)
	{
		v2f o;
		o.pos = UnityObjectToClipPos(v.vertex);
		o.uv = v.texcoord;
		half3 worldNormal = UnityObjectToWorldNormal(v.normal);
		half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
		o.diff = nl * _LightColor0.rgb;
		o.ambient = ShadeSH9(half4(worldNormal,1));
		// compute shadows data
		TRANSFER_SHADOW(o)
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
		//v = -0.5 + v * (3. / 2.);
		//v = Remap(-0.5, 1.0, 0.0, 1.0, v);
		//u = Remap(0.5 - (_Width / 2), 0.5 + (_Width / 2), 0.0, 1.0, u);
		v = Remap(0.0, 1.0, -0.5, 1.0, v);

		// Left
		float arrowLeftTop = 1 - (u * _Direction + v);
		arrowLeftTop = step(_Thickness, arrowLeftTop);
		float arrowLeftBot = (u * _Direction + v + 0.5);
		arrowLeftBot = step(_Thickness, arrowLeftBot);
		float arrowLeft = arrowLeftTop * arrowLeftBot;

		// Right
		float arrowRightTop = 1 - ((1 - u) * _Direction + v);
		arrowRightTop = step(_Thickness, arrowRightTop);
		float arrowRightBot = ((1 - u) * _Direction + v + 0.5);
		arrowRightBot = step(_Thickness, arrowRightBot);
		float arrowRight = arrowRightTop * arrowRightBot;

		// Split
		float arrowSplit = u;
		arrowSplit = step(0.5, arrowSplit);

		// Side mask (width)
		float sideMask = abs(u - 0.5) * 2;
		sideMask -= _Width / 2;
		sideMask = ceil(sideMask);
		//arrowSplit = step(0.5, arrowSplit);

		// Arrow
		float arrow = lerp(arrowLeft, arrowRight, arrowSplit);

		// Fuse
		return arrow - sideMask;
	}


	float4 GetDirectionalColor(float direction) {
		float remap = direction * 0.5 + 0.5;
		return lerp(_DirectionAColor, _DirectionBColor, remap);
	}

	fixed4 frag(v2f i) : SV_Target
	{
	fixed4 col = tex2D(_MainTex, i.uv);

	float x = i.uv.x;
	float y = i.uv.y;
	y += _AnimationTime;
	y *= _Slider;
	y = y - floor(y); // Repeat

	float arrow = DrawArrow(x, y);
	arrow = clamp(0, 1, arrow);


	col = lerp(GetDirectionalColor(_Direction), float4(1, 1, 1, 1), arrow);

	// compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
	fixed shadow = SHADOW_ATTENUATION(i);
	// darken light's illumination with shadow, keep ambient intact
	fixed3 lighting = i.diff * shadow + i.ambient;
	col.rgb *= lighting;
	return col;
	}


		ENDCG
	}

		// shadow casting support
		UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
	}
}