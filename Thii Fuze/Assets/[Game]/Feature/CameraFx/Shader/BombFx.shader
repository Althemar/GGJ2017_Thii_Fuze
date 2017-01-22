Shader "[Fuse]/BombFx_v1"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}


		_Aperture("Aperture", Float) = 178.0
		_Implosion("Implosion", Float) = 1.0
		_EffectStrengh("EffectStrengh", Float) = 1.0
			_PosDevSpace("PosDevSpace", Vector) = (0,0,0,0)	
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			#define PI 3.1415926535

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			float _Aperture;
			float _Implosion;
			float _EffectStrengh;
			float4 _PosDevSpace;
#define EPSILON 0.000011


			fixed4 frag (v2f i) : SV_Target
			{
				//fixed4 col = tex2D(_MainTex, i.uv);
				//// just invert the colors
				//col = 1 - col;
				//return col;
/*
				float aperture = 178.0;
				float apertureHalf = 0.5 * aperture * (PI / 180.0);
				float maxFactor = sin(apertureHalf);

				float2 uv;
				float2 xy = 2.0 * i.uv.xy - 1.0;
				float d = length(xy);
				if (d < (2.0 - maxFactor))
				{
					d = length(xy * maxFactor);
					float z = sqrt(1.0 - d * d ) * _Implosion;
					float r = atan2(d, z) / PI;
					float phi = atan2(xy.y, xy.x);

					uv.x = r * cos(phi) + 0.5;
					uv.y = r * sin(phi) + 0.5;
				}
				else
				{
					uv = i.uv.xy;
				}
				fixed4 col = tex2D(_MainTex, uv);
				return col;

*/


				float2 p = float2(i.uv.x, i.uv.y);
				float prop = _ScreenParams.z;//screen proroption
				float2 m = float2(_PosDevSpace.x, _PosDevSpace.y / prop);//center coords
				float2 d = p - m;//vector from center to current fragment
				float r = sqrt(dot(d, d)); // distance of pixel from center



				float power = (2.0 * 3.141592 / (2.0 * sqrt(dot(m, m)))) * _EffectStrengh;

				float bind = sqrt(dot(m, m));//stick to corners

				float2 uv;
				if (power > 0.0)//fisheye
					uv = m + normalize(d) * tan(r * power) * bind / tan(bind * power);
				else if (power < 0.0)//antifisheye
					uv = m + normalize(d) * atan(r * -power * 10.0) * bind / atan(-power * bind * 10.0);
				else uv = p;//no effect for power = 1.0

				fixed4 col = tex2D(_MainTex, float2(uv.x, 1-uv.y * prop));;
				return col;
			}
			ENDCG
		}
	}
}
