using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/DesertShader")]
public class DesertShader : ImageEffectBase {

	public float strength;
	public Color desertColor;
	public Texture noise;

	private PerlinNoise perlin;

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		perlin.update(0.2f,0.2f);
		material.SetFloat("_Strength", strength);
		material.SetColor("_Color", desertColor);
		material.SetTexture("_Perlin", perlin.noiseTex);
		material.SetTexture("_Noise", noise);
		Graphics.Blit (source, destination, material);
	}

	override protected void Start ()
	{
		base.Start();
		perlin = new PerlinNoise(512,512,6	);
		perlin.update(0,0);
	}
}
