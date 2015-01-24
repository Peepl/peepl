using UnityEngine;
using System.Collections;

public class PerlinNoise {

	float width;
	float height;
	float xorg;
	float yorg;
	float scale;
	public Texture2D noiseTex;
	Color[] pix;

	public PerlinNoise(float width, float height, float scale)
	{
		this.width = width;
		this.height = height;
		this.scale = scale;
		pix = new Color[(int)(width*height)];
		noiseTex = new Texture2D((int)width, (int)height);
	}
	
	private void calcNoise(){
		//TODO - is there need for multiple octaves of perlin noise?
		for(float y = 0; y < height; y++)
		{
			for(float x = 0; x < width; x++)
			{
				float xcoord = xorg + x / width * scale;
				float ycoord = yorg + y / width * scale;
				var sample = Mathf.PerlinNoise(xcoord, ycoord);
				pix[(int)(y*width+x)] = new Color(sample, sample, sample);
				//Add some additional noise to make it more even.
				sample = Mathf.PerlinNoise(xcoord/3.0f, ycoord/3.0f)*0.5f;
				pix[(int)(y*width+x)] -= new Color(sample, sample, sample);
			}
		}

		noiseTex.SetPixels(pix);
		noiseTex.Apply();
	}

	public void update(float offx, float offy)
	{
		xorg+=offx;
		yorg+=offy;
		this.calcNoise();
	}
}

