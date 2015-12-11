using UnityEngine;
using System.Collections;

public class CameraManager : MonoSingleton<CameraManager> {
	
	// Update is called once per frame
	void Update () 
	{
		Camera.main.transform.position = new Vector3(Player_Bear.Instance.transform.position.x, Player_Bear.Instance.transform.position.y, Camera.main.transform.position.z);
	}
}
