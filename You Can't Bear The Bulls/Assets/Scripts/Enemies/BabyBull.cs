using UnityEngine;
using System.Collections;

public class BabyBull : BasicBull {

	bool IfGetHit = false;

	public void GetHit()
	{
		IfGetHit = true;
	}
}
