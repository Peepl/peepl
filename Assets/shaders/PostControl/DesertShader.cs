using UnityEngine;

[ExecuteInEditMode]
[AddComponentMenu("Image Effects/DesertShader")]
public class DesertShader : ImageEffectBase {

	public float perlinStrength;
	public Color desertColor;
	public Texture noise;
	public float speedX;
	public float speedY;
	public float fogStrength;
	public float day;

	private PerlinNoise perlin;

	// Called by camera to apply image effect
	void OnRenderImage (RenderTexture source, RenderTexture destination) {
		perlin.update(speedX,speedY);
		material.SetFloat("_PerlinStrength", perlinStrength);
		material.SetFloat("_FogStrength", fogStrength);
		material.SetFloat("_Day", day);
		//		material.SetFloat("_SpeedY", speedY);
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
		material.SetVector("_Center", new Vector2(0.5f, 0.5f));
	}
}
