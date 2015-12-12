using UnityEngine;
using System.Collections;

public class CameraManager : MonoSingleton<CameraManager> 
{
	public GameObject Background1, Background2;

	private float camViewHeight;
	private float camViewWidth;
	private float backgroundHalfWidth = 9.6f;

	// Start is called once during instatiation
	void Start ()
	{
		camViewHeight = Camera.main.orthographicSize;
		camViewWidth = camViewHeight * Screen.width / Screen.height;
	}

	// Update is called once per frame
	void Update ()
	{
		Camera.main.transform.position = new Vector3 (Player_Bear.Instance.transform.position.x, Player_Bear.Instance.transform.position.y + 2.84f, Camera.main.transform.position.z);

		swapBackground (Background1, Background2);
		swapBackground (Background2, Background1);
	}

	private void swapBackground(GameObject bg1, GameObject bg2)
	{
		// If camLeft is in the bg
		if (getCameraLeft () > getBackgroundLeft (bg1) && getCameraLeft () < getBackgroundRight (bg1))
		{
			// If camRight is outside bg1 and bg2
			if (getCameraRight() > getBackgroundRight(bg1))
			{
				bg2.transform.position = new Vector3 (getBackgroundRight (bg1) + bg1.transform.lossyScale.x * backgroundHalfWidth, bg1.transform.position.y, bg1.transform.position.z);
			}
		}

		// If camRight is in the bg
		else if (getCameraRight () > getBackgroundLeft (bg1) && getCameraRight () < getBackgroundRight (bg1))
		{
			// If camLeft is outside bg1 and bg2
			if (getCameraLeft() < getBackgroundLeft(bg1))
			{
				bg2.transform.position = new Vector3 (getBackgroundLeft(bg1) - bg1.transform.lossyScale.x * backgroundHalfWidth, bg1.transform.position.y, bg1.transform.position.z);
			}
		}
	}

	private float getBackgroundLeft(GameObject bg)
	{
		return bg.transform.position.x - Background1.transform.lossyScale.x * backgroundHalfWidth;
	}

	private float getBackgroundRight(GameObject bg)
	{
		return bg.transform.position.x + Background1.transform.lossyScale.x * backgroundHalfWidth;
	}

	private float getCameraLeft()
	{
		return Camera.main.transform.position.x - camViewWidth;
	}

	private float getCameraRight()
	{
		return Camera.main.transform.position.x + camViewWidth;
	}
}