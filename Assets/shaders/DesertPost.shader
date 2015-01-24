Shader "Custom/DepthGrayscale" {

Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	

SubShader {
Tags { "RenderType"="Opaque" }

Pass{
CGPROGRAM
#pragma vertex vert
#pragma fragment frag
#include "UnityCG.cginc"

sampler2D _CameraDepthTexture;
sampler2D _MainTex;
sampler2D _Perlin;
sampler2D _Noise;
float4 _Color;
float _PerlinStrength;
float _FogStrength;
float2 _Center;
float _Angle;
float _Day;
float _POffX;
float _POffY;

struct v2f {
   float4 pos : SV_POSITION;
   float4 scrPos:TEXCOORD1;
   float2 uv : TEXCOORD0;
};

//Vertex Shader
v2f vert (appdata_base v){
   v2f o;
   o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
   o.scrPos=ComputeScreenPos(o.pos);
   //for some reason, the y position of the depth texture comes out inverted
   //o.scrPos.y = 1 - o.scrPos.y;
   o.uv = v.vertex.xy;
   return o;
}

//Fragment Shader
half4 frag (v2f i) : COLOR{
   float depthValue = Linear01Depth (tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos)).r);
   half4 depth;
	float4 n = tex2D(_Noise, i.uv*2.0);
	float4 p = tex2D(_Perlin, i.uv*0.5+float2(_POffX, _POffY)*0.005);
	p.r = p.r * n.x*_PerlinStrength;
	//depthValue =clamp(depthValue+depthValue*p.r, 0.0, 1.0);
   	depth.r = depthValue;
  	depth.g = depthValue;
   	depth.b = depthValue;
   	
   	float4 c = tex2D(_MainTex, i.uv);
	
	float aspect = _ScreenParams.x/_ScreenParams.y;
	
	float dx = _Center.x - i.uv.x;
	float dy = (_Center.y - i.uv.y)/aspect*2;
	float dist = sqrt(dx*dx + dy*dy);
	
//	_FogStrength = clamp(0,1,_FogStrength-(1-dist/3.0));
	
//	_FogStrength = dist>0.1? smoothstep(0.0,_FogStrength, (dist-0.1)*2)+_FogStrength/6.0:_FogStrength/6.0;
	//_FogStrength = 0;
	float smooth = 1.0-smoothstep(0.0,1.0, (dist-0.12)*(4.25+p.r*2));
	//float border = smooth > 0.54 && smooth < 0.57 ? 0.1 : 0.0;
	//smooth = smooth > 0.30 && smooth < 0.38 ? 73.0 : smooth;
	_FogStrength *= 1-smooth*0.31;
	_FogStrength+=p.r;
	float darken = dist>0.12? smooth :1.0;
	darken +=_Day;
	darken = clamp(darken,0, 1);
	
	c*= darken;
	c.r = lerp(c.r, c.r*max(0.01,(1-depthValue)) + depthValue*_Color.r , _FogStrength);
	c.g =lerp(c.g,c.g*max(0.01,(1-depthValue)) + depthValue*_Color.g , _FogStrength); 
	c.b =lerp(c.b,c.b*max(0.01,(1-depthValue)) + depthValue*_Color.b , _FogStrength);
	c*= clamp(darken*p.r,0.8,1.0);
//	c+=float4(border,border,border,border);
    depth.a = 1;
    return c;
}
ENDCG
}
}
FallBack "Diffuse"
}