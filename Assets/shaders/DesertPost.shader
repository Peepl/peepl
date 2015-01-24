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
float _Strength;

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
	float4 p = tex2D(_Perlin, i.uv);
	p.r = p.r * n.x*_Strength;
	depthValue =clamp(depthValue+depthValue*p.r, 0.0, 1.0);
   	depth.r = depthValue;
  	depth.g = depthValue;
   	depth.b = depthValue;
   	
   	float4 c = tex2D(_MainTex, i.uv);
	
	
	c.r =c.r*max(0.01,(1-depthValue)) + depthValue*_Color.r ;
	c.g =c.g*max(0.01,(1-depthValue)) + depthValue*_Color.g ; 
	c.b =c.b*max(0.01,(1-depthValue)) + depthValue*_Color.b ;

   depth.a = 1;
   return c;
}
ENDCG
}
}
FallBack "Diffuse"
}