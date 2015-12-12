using UnityEngine;
using System.Collections;

public class CameraManager : MonoSingleton<CameraManager> {

	public GameObject Background1, Background2;
	// Update is called once per frame
	void Update () 
	{
		Camera.main.transform.position = new Vector3(Player_Bear.Instance.transform.position.x, Player_Bear.Instance.transform.position.y + 2.84f, Camera.main.transform.position.z);

		float XPosition = Camera.main.transform.position.x;

		float LeftX, RightX;

		LeftX = XPosition - 9.6f;

		int TimesToMove = Mathf.FloorToInt(XPosition/19.2f);
		int TimesToMoveLeft = Mathf.FloorToInt(LeftX/19.2f);


		Background1.transform.position = new Vector3(TimesToMove*-19.2f,0,0);
		if(TimesToMove == TimesToMoveLeft)
		{
			Background2.transform.position = new Vector3((TimesToMove-1)*-19.2f,0,0);
		}
		else
		{
			Background2.transform.position = new Vector3((TimesToMove)*-19.2f,0,0);
		}

	}
}
