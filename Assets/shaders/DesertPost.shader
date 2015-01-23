Shader "Hidden/DesertPost" {
	Properties {
		_MainTex ("Render Input", 2D) = "white" {}
		_Noise ("Noise texture", 2D) = "white" {}
		_Strength("Strength", Float) = 0.65
		_Color ("Glow Amount", Color) = (1,1,1,1)
	}
	SubShader {
		ZTest Always Cull Off ZWrite Off Fog { Mode Off }
		Pass {
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				
				sampler2D _MainTex;
				sampler2D _Noise;
				float4 _Color;
				float _Strength;
				float4 _Noise_TexelSize;
		
				uniform float3 _NoisePerChannel;
				uniform float3 _NoiseTilingPerChannel;
				uniform float3 _NoiseAmount;
				uniform float3 _MidGrey;	
		
				struct v2f 
				{
					float4 pos : SV_POSITION;
					float2 uv_screen : TEXCOORD0;
					float4 uvRg : TEXCOORD1;
					float2 uvB : TEXCOORD2;
				};
				
				struct appdata_img2 
				{
				    float4 vertex : POSITION;
				    float2 texcoord : TEXCOORD0;
				    float2 texcoord1 : TEXCOORD1;
				};		

				
				
				v2f vert (appdata_img2 v)
				{
					v2f o;
					
					o.pos = mul (UNITY_MATRIX_MVP, v.vertex);	

		       		o.uv_screen = v.vertex.xy;
					
					// different tiling for 3 channels
					o.uvRg = v.texcoord.xyxy + v.texcoord1.xyxy * _NoiseTilingPerChannel.rrgg * _Noise_TexelSize.xyxy;
					o.uvB = v.texcoord.xy + v.texcoord1.xy * _NoiseTilingPerChannel.bb * _Noise_TexelSize.xy;

					return o; 
				}

				
				float4 frag(v2f IN) : COLOR {
					half4 c = tex2D (_MainTex, IN.uv_screen);
					c.b = _Strength;
					
					
					// black & white intensities
					//Noise calculus from unity effects. Using only small amount of it.
					float2 blackWhiteCurve = Luminance(c.rgb) - _MidGrey.x; // maybe tweak middle grey
					blackWhiteCurve.xy = saturate(blackWhiteCurve.xy * _MidGrey.yz); //float2(1.0/0.8, -1.0/0.2));

					float finalIntensity = _NoiseAmount.x + max(0.0f, dot(_NoiseAmount.zy, blackWhiteCurve.xy));
					
					// fetching & scaling noise (COMPILER BUG WORKAROUND)
					float3 m = float3(0,0,0);
					m += (tex2D(_Noise, IN.uvRg.xy) * float4(1,0,0,0)).rgb;
					m += (tex2D(_Noise, IN.uvRg.zw) * float4(0,1,0,0)).rgb;
					m += (tex2D(_Noise, IN.uvB.xy) * float4(0,0,1,0)).rgb;

					m = saturate(lerp(float3(0.5,0.5,0.5), m, _NoisePerChannel.rgb * float3(finalIntensity,finalIntensity,finalIntensity) ));
					
					float4 no = float4(m.r, m.g, m.b, 1.0);
							
					return (c +no )*_Color;
				}
			ENDCG
		}
	}
}