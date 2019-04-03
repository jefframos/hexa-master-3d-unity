Shader "GlowFX/GlowNoise" {
	Properties {
		_BaseColor ("Base Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_GlowColor ("Glow Color", Color) = (1,0,0,1)
		_GlowIntensity ("Glow Intensity", Range(0.1, 1.0)) = 0.45
		_GlowSize ("Glow Size", Range(0.25, 2.0)) = 1.0
		_AnimationSpeed ("Animation Speed", Range(0.0, 20.0)) = 1.0
		_NoiseAmmount ("Noise Ammount", Range(0.0, 1.0)) = 1.0
		_PulseFrequency ("Pulse Frequency", Range(0.0, 4.0)) = 1.0
		_PulseStrength ("Pulse Strength", Range(0.0, 0.5)) = 0.1		
	}

	SubShader {
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
		LOD 200

		// extra pass that renders to depth buffer only
		Pass {
			//ZWrite Off
			ColorMask 0
			Cull Front		
		}

		  CGPROGRAM
		  #pragma surface surf Lambert
		  struct Input {          
			  float2 uv_MainTex;
		  };

			half4 _BaseColor;	  

		  sampler2D _MainTex;
		  void surf (Input IN, inout SurfaceOutput o) {
			  o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb * _BaseColor.rgb;
		  }
		  ENDCG
	
	
		  CGPROGRAM
			#pragma target 3.0
			#pragma surface surf Glow alpha:fade vertex:vert keepalpha			
		
			#define GLOW_NOISE
		
			#include "GlowFX.cginc"
			  
			fixed4 LightingGlow(SurfaceOutput s, fixed3 lightDir, fixed atten)
			{
				return fixed4(s.Albedo, s.Alpha);
			}
	  	 
		   void surf (Input IN, inout SurfaceOutput o) {
				o.Albedo = _GlowColor;					
				o.Emission = _GlowColor;			
				o.Alpha = calculateGlow(IN, o.Normal);
		   }
		   ENDCG				   	  
	}	

}