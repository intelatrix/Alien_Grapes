using UnityEngine;
using System.Collections;

// Class that automatically resizes the Collider's box based on the Sprite's size
public class SpriteColliderResizer : MonoBehaviour 
{
	private Sprite sprite;
	private BoxCollider2D hitBox;

	// Use this for initialization
	void Start () 
	{
		sprite = GetComponent<Sprite> ();
		hitBox = GetComponent<BoxCollider2D> ();

		// Obtain the texture to start analyzing it
		Texture2D tex = sprite.texture;


	}

	private void analyzeTexture(Texture2D tex, bool imageCentered)
	{
		// Calculate the vertical mid point
		int vertMid = (int)(tex.height * 0.5);

		// Track the left and rights
		int left = 0;
		int right = tex.height - 1; 

		// Look through this row of pixels to get the first
		for (int x = 0; x < tex.width; ++x)
		{
			Color pixel = tex.GetPixel (x, vertMid);

			// If this is not transparent, meaning we are entering the effective sprite
			if (pixel.a > 0.0f)
			{
				// Found the left bound
				left = x;
				break;
			}
		}

		// If it is centered
		if (imageCentered)
		{
			// The right must be equal to the left
			right = left;
		}
		else
		{
			// Check the right side
			for (int x = tex.width - 1; x >= 0; --x)
			{
				Color pixel = tex.GetPixel (x, vertMid);

				// If this is not transparent, meaning we are entering the effective sprite
				if (pixel.a > 0.0f)
				{
					right = x;
					break;
				}
			}
		}

		// Calculate the pixel to scale ratio

		// Apply this ratio
	}
}
