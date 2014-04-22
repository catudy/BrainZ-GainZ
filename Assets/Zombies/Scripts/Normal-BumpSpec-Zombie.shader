Shader "Custom/Bumped Specular Zombie" {
Properties {
	_Color ("Main Color", Color) = (1,1,1,1)
	_SpecColor ("Specular Color", Color) = (0.5, 0.5, 0.5, 1)
	_Shininess ("Shininess", Range (0.03, 1)) = 0.078125
	_MainTex ("Base (RGB) Gloss (A)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
}
SubShader { 
	Tags { "RenderType"="Opaque" }
	LOD 400
	
CGPROGRAM
#pragma surface surf BlinnPhong
#pragma exclude_renderers flash

sampler2D _MainTex;
sampler2D _BumpMap;
fixed4 _Color;
half _Shininess;

struct Input {
	float2 uv_MainTex;
	float2 uv_BumpMap;
    float4 color : COLOR;
};

void surf (Input IN, inout SurfaceOutput o) {

	float2 painSkinOffset = {0.75, 0.25};

	fixed4 tex = (tex2D(_MainTex, IN.uv_MainTex) * (1.0f - IN.color.a)) +
				 (tex2D(_MainTex, IN.uv_MainTex + IN.color.rg + painSkinOffset) * IN.color.a);

	o.Albedo = tex.rgb;
	o.Gloss = tex.a;
	o.Alpha = tex.a;
	o.Specular = _Shininess;

	o.Normal = UnpackNormal (tex2D(_BumpMap, IN.uv_BumpMap) * (1.0f - IN.color.a) +
 						     tex2D(_BumpMap, IN.uv_BumpMap + IN.color.rg + painSkinOffset) * IN.color.a);
 						
}
ENDCG
}

FallBack "Specular"
}
