using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/DesertShader")]
public class DesertShader : ImageEffectBase {

	public float strength;
	public Texture noise;
	public Color desertColor;


	public Vector4 noiseTexelSize;
	public Vector3 noisePerChannel;
	public Vector3 noiseTilingPerChannel;
	public Vector3 noiseAmount;
	public Vector3 midGrey;


	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		material.SetFloat("_Strength", strength);
		material.SetColor("_DesertColor", desertColor);
		material.SetTexture("_Noise", noise);
		material.SetVector("_NoiseTexelSize", noiseTexelSize);
		material.SetVector("_NoisePerChannel",noisePerChannel);
		material.SetVector("_NoiseTilingPerChannel",noiseTilingPerChannel);
		material.SetVector("_NoiseAmount",noiseAmount);
		material.SetVector("_MidGrey",midGrey);
		Graphics.Blit (source, destination, material);
	}
}
