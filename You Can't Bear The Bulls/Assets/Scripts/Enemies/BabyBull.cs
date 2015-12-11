using UnityEngine;
using System.Collections;

public class BabyBull : BasicBull {

	bool IfGetHit = false;

	// Update is called once per frame
	void Update()
	{
		if(!IfGetHit)
		{
			if (IfFacingRight)
				transform.position += new Vector3(1,0,0) * MovementSpeed * TimeManager.Instance.GetGameDeltaTime();
			else
				transform.position += new Vector3(-1,0,0) * MovementSpeed * TimeManager.Instance.GetGameDeltaTime();
		}
	}

	public void GetHit()
	{
		IfGetHit = true;
	}
}
