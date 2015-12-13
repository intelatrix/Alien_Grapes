using UnityEngine;
using System.Collections;

public class SplashMenuController : MonoBehaviour 
{
	public float WaitTime = 2.0f;

	// Update is called once per frame
	void Update () 
	{
		WaitTime -= Time.deltaTime;
		if (WaitTime < 0.0f)
		{
            Application.LoadLevel("MainScene");
		}
	}
}
