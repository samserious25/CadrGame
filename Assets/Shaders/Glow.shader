Shader "Custom/Glow"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		[PerRendererData]_MainTex("Sprite Texture", 2D) = "white" {}
		_Cutoff("Shadow alpha cutoff", Range(0,1)) = 0.5
	}

	SubShader
	{
		Tags
		{
			"Queue" = "Geometry"
			"RenderType" = "TransparentCutout"
		}

		Cull Off
		ZWrite On


		CGPROGRAM
		#pragma surface surf TileShading addshadow
		#pragma target 3.0

		// Custom lighting models, which aim to be simpler than the ones provided (these are for sprites anyway)
		// Flat shading, so light color is constant
		inline void LightingTileShading_GI(SurfaceOutput s, UnityGIInput data, inout UnityGI gi)
		{
			gi.light = data.light;
			gi.light.color = fixed3(1, 1, 1);
		}


		// Discretizes a continuous light color into several steps (not configurable yet)
		inline half3 stepAttenuation (half3 lightColor)
		{
			// Fiddle around with these values to check the effect in the editor
			half step1 = 0.05;
			half step2 = 0.2;
			half step3 = 0.45;

			half atten = length(lightColor);

			// Here I avoid using conditional structures, making the compiler decide
			half result = 0;
			result = step(step1, atten) * step2;
			result = max(result, step(step2, atten) * step3);
			result = max(result, step(step3, atten) * 1);
			return result * lightColor / atten;
		}

		// This #define is declared in the code generated by the surface shader
#ifdef UNITY_PASS_FORWARDADD
		// In the ForwardAdd pass, the light is stepped with our custom function
		inline fixed4 LightingTileShading(SurfaceOutput s, UnityGI gi)
		{
			fixed4 c;

			c.rgb = s.Albedo * stepAttenuation(gi.light.color);
			c.a = s.Alpha;
			return c;
		}
#else
		inline fixed4 LightingTileShading(SurfaceOutput s, UnityGI gi)
		{
			fixed4 c;

			c.rgb = s.Albedo * gi.light.color;
			c.a = s.Alpha;
			return c;
		}
#endif


		sampler2D _MainTex;
		fixed4 _Color;
		fixed _Cutoff;

		struct Input
		{
			float2 uv_MainTex;
			float4 color : COLOR;
		};

		
		void surf (Input IN, inout SurfaceOutput o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color * IN.color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			clip(o.Alpha - _Cutoff);
		}

		ENDCG
	}
		

	FallBack "Diffuse"
}