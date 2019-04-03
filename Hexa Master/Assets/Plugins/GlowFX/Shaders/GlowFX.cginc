// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

		half4 _GlowColor;		
		half _GlowIntensity;
		float _AnimationSpeed;
		float _GlowSize;
		half _PulseStrength;
		half _PulseFrequency;

#ifdef GLOW_NOISE
		half _NoiseAmmount;
#endif

	   struct Input {
			float3 viewDir;		

#ifdef GLOW_NOISE
			half noise;
#endif
	   };
	   	   
		
#ifdef GLOW_NOISE
		float noise(float3 x)
		{
			float3 p = floor(x);
			float3 f = frac(x);
			f = f*f*(3.0-2.0*f);
			
			float n = p.x + p.y*157.0 + 113.0*p.z;
			
			float4 v1 = frac(753.5453123*sin(n + float4(0.0, 1.0, 157.0, 158.0)));
			float4 v2 = frac(753.5453123*sin(n + float4(113.0, 114.0, 270.0, 271.0)));
			float4 v3 = lerp(v1, v2, f.z);
			float2 v4 = lerp(v3.xy, v3.zw, f.y);
			return lerp(v4.x, v4.y, f.x);
		}
#endif

	float4 getPixelPos(float4 pos)
	{
		float4 temp = UnityObjectToClipPos(pos);
		
		temp = ComputeScreenPos(temp);
		
		return temp;
	}
		
	void vert(inout appdata_full v, out Input o) {
		UNITY_INITIALIZE_OUTPUT(Input, o);
		
		#ifdef GLOW_NOISE
		float3 noisePos = v.vertex.xyz;
		noisePos += v.normal * _Time.x * _AnimationSpeed;
		o.noise = noise(noisePos * 10) * _NoiseAmmount;
		#endif
		
					
		//half ammount = abs(cos(_Time.x));
		
		half ammount = 1.0;

		#ifdef GLOW_NOISE
		ammount = (ammount + o.noise) * 0.5;
		#else
		ammount = (ammount + 0.5) * 0.5;
		#endif
		
		float radius = _GlowSize + abs(sin(_Time.w * _PulseFrequency)) * _PulseStrength;
		ammount = clamp(ammount, 0, 1) * radius;
				
		half3 ofs = v.normal * ammount; 
		v.vertex.xyz += ofs;
		
		//o.outNormal =  normalize( mul( UNITY_MATRIX_IT_MV, v.normal));		
	}
	
	half calculateGlow(Input IN, half3 normal)
	{	
		half dist = 1  - saturate(dot (normalize(IN.viewDir), normal));
		//float  _RimPower = 0.7;	
		//dist =  pow (dist, _RimPower);
		
		half cut = 0.6;
		
			
		half limit = 0.5;

		#ifdef GLOW_NOISE
		//limit += IN.noise * 0.5;
		#endif
		
		dist = smoothstep( limit, 0.0, dist);
		
		//dist = 1 - dist;			
		
	//	dist = length(IN.base_pos.xy - IN.ext_pos.xy);
		
		//dist = 1;
		
		return dist * _GlowIntensity;		
	}
	